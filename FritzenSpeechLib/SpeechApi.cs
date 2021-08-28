using FritzenSpeech.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FritzenSpeech
{
    public class SpeechApi
    {
        private readonly LanguageDetection languageDetection = new LanguageDetection();
        private readonly FrmMainTray trayAgent = new FrmMainTray();
        private readonly VolumeMixer mixer = new VolumeMixer();
        private readonly ClipboardBackup clipboardBackup = new ClipboardBackup();
        private readonly SpeechSynthesizer speech = new SpeechSynthesizer();
        private List<VoiceRate> voices = null;
        private int hotkeyIndex = 0;
        private string md5 = "";
        private const int DEFAULT_SPEECH_RATE = 4; // Ranges from -10 to 10 
        public delegate void FritzenTrayEventHandler(SpeechEventArgs e);
        public event FritzenTrayEventHandler EventFired;
        public List<VoiceRate> Voices => voices;
        public SpeechSynthesizer api => speech;
        public int Volume => speech.Volume;
        
        private SoundPlayer sound;


        public void SetAudioAuto(bool value)
        {
            mixer.SetAdjustAudio(value);
        }
        private string CleanUp(string text)
        {
            text = text.Replace(System.Environment.NewLine, " ");
            return text;
        }

        private VoiceRate FindVoiceRate(string id)
        {
            return voices.Find(x => x.Voice.Id.Equals(id));
        }

        public void AdjustVoiceRate(string id, int value)
        {
            VoiceRate vr = FindVoiceRate(id);
            vr.Rate = value;

            Dictionary<string, int> VoiceRateDictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(Settings.Default.VoiceRate);

            if (!VoiceRateDictionary.ContainsKey(id))
            {
                VoiceRateDictionary.Add(id, value);
            }
            else
            {
                VoiceRateDictionary[id] = value;
            }


            Settings.Default.VoiceRate = JsonConvert.SerializeObject(VoiceRateDictionary);
            Settings.Default.Save();
        }

        public void SpeakStartOrResume(string text)
        {
            SynthesizerState state = speech.State;

            if (state == SynthesizerState.Ready)
            {
                SpeakStart(text, languageDetection.Detect(text));
            }
            else
            {
                SpeakResume();
            }
        }

        public string SpeakStart(string text, string lang)
        {
            SpeakStop();

            text = CleanUp(text);

            bool voiceSelected = false;
            foreach (VoiceRate vi in voices)
            {
                if (vi.Voice.Culture.Name.Equals(lang) || (vi.Voice.Culture.Parent != null && vi.Voice.Culture.Parent.Name.Equals(lang))) //Parent: pt-BR ~ pt
                {
                    speech.SelectVoice(vi.Voice.Name);
                    speech.Rate = vi.Rate;
                    voiceSelected = true;
                    break;
                }
            }

            if (!voiceSelected)
            {
                speech.SelectVoice(voices[0].Voice.Name);
            }

            mixer.AdjustVolume();

            speech.SpeakAsync(text);

            if (speech.State == SynthesizerState.Paused)
            {
                speech.Resume();
            }

            return text;
        }


        public void SpeakStop()
        {
            mixer.RestoreVolume();
            Prompt p1 = speech.GetCurrentlySpokenPrompt();
            if (p1 != null)
            {
                speech.SpeakAsyncCancel(p1);
            }

            Util.Delay(10);
            speech.SpeakAsyncCancelAll();
            speech.Resume();

        }

        public void SpeakPause()
        {
            mixer.RestoreVolume();
            speech.Pause();
        }

        private void SpeakResume()
        {
            mixer.AdjustVolume(); //Range from 0 to 100.
            speech.Resume();
        }
        public void SpeakVolume(int volume)
        {
            speech.Volume = volume;
            Settings.Default.Volume = volume;
            Settings.Default.Save();
        }

        public void ShowBalloonTip()
        {
            trayAgent.ShowBalloonTip();
        }

        public SpeechApi()
        {
            sound = new SoundPlayer( Resources.blop );

            speech.SpeakCompleted += Speech_SpeakCompleted;
            speech.SpeakProgress += Speech_SpeakProgress;

            trayAgent.SpeechActionFired += TrayAgent_SpeechActionFired;

            voices = new List<VoiceRate>();

            string voicesOrder = Settings.Default.Voices;

            Dictionary<string, int> VoiceRateDictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(Settings.Default.VoiceRate);

            if (VoiceRateDictionary == null)
            {
                VoiceRateDictionary = new Dictionary<string, int>();
            }

            foreach (InstalledVoice iv in speech.GetInstalledVoices())
            {
                if (!VoiceRateDictionary.ContainsKey(iv.VoiceInfo.Id))
                {
                    voices.Add(new VoiceRate()
                    {
                        Voice = iv.VoiceInfo,
                        Rate = DEFAULT_SPEECH_RATE
                    });

                    VoiceRateDictionary.Add(iv.VoiceInfo.Id, DEFAULT_SPEECH_RATE);
                }
                else
                {
                    voices.Add(new VoiceRate()
                    {
                        Voice = iv.VoiceInfo,
                        Rate = VoiceRateDictionary[iv.VoiceInfo.Id]
                    });
                }

                string vid = iv.VoiceInfo.Id;

                if (!voicesOrder.Contains(vid))
                {
                    voicesOrder += vid + ";";
                }
            }


            Settings.Default.VoiceRate = JsonConvert.SerializeObject(VoiceRateDictionary);


            voicesOrder = voicesOrder.TrimEnd(';');

            Settings.Default.Voices = voicesOrder;
            Settings.Default.Save();

            List<string> docIds = voicesOrder.Split(';').ToList<String>();
            voices = voices.OrderBy(d => docIds.IndexOf(d.Voice.Id)).ToList();
            speech.Volume = Settings.Default.Volume;

            trayAgent.Show();
            trayAgent.Hide();
        }



        private void TrayAgent_SpeechActionFired(SpeechEventArgs e)
        {
            SynthesizerState state = speech.State;

            if (e.Actions == Actions.ABOUT || e.Actions == Actions.RESTORE || e.Actions == Actions.EXIT)
            {
                EventFired?.Invoke(e);
                return;
            }


            if (e.Actions == Actions.HOTKEY_FIRED)
            {

                mixer.RestoreVolume();

                string lang = "";
                string newMD5 = "";

                string textBeforeCopy = Clipboard.GetText(TextDataFormat.Text);
                Util.Delay(10);
                clipboardBackup.Backup();

                Util.Delay(5);
                Util.PressKey(Keys.ControlKey, false);
                Util.Delay(5);
                Util.PressKey(Keys.C, false);
                Util.Delay(20);
                Util.PressKey(Keys.C, true);
                Util.Delay(5);
                Util.PressKey(Keys.ControlKey, true);
                Util.Delay(400);

                string text = Clipboard.GetText(TextDataFormat.Text);

                clipboardBackup.Restore();



                if (string.IsNullOrEmpty(text) || textBeforeCopy.Equals(text))
                {

                    sound.Play();

                    if (state == SynthesizerState.Speaking)
                    {
                        SpeakPause();
                        return;
                    }
                    if (state == SynthesizerState.Paused)
                    {
                        SpeakResume();
                        return;
                    }
                }

                Parallel.Invoke(() =>
                {
                    Debug.WriteLine("Begin GetMd5Hash task...");
                    newMD5 = Util.GetMd5Hash(text);
                    Debug.WriteLine("End GetMd5Hash task...");
                }, () =>
                {
                    Debug.WriteLine("Begin languageDetection task...");
                    lang = languageDetection.Detect(text);
                    Debug.WriteLine("End languageDetection task...");
                }, () =>
                {
                    Debug.WriteLine("Begin Beep task...");
                    sound.Play();
                    Debug.WriteLine("End Beep task...");
                }
                    );

                //if the text is the same
                if (newMD5.Equals(md5))
                {
                    if (speech.State == SynthesizerState.Ready)
                    {
                        md5 = "";
                    }

                    if (state == SynthesizerState.Speaking)
                    {
                        SpeakPause();
                    }
                    if (state == SynthesizerState.Paused)
                    {
                        SpeakResume();
                    }
                }
                //if the text has changed
                if (!newMD5.Equals(md5))
                {
                    SpeakStop();
                    SpeakStart(text, lang);
                    md5 = newMD5;
                }
            }
        }


        public void SetHotkeys(int index)
        {
            hotkeyIndex = index;
            trayAgent.RegisterHotkey(index);
        }



        private void Speech_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {

        }

        private void Speech_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            mixer.RestoreVolume();

        }

        public void SpeechVoicesReorder(List<VoiceRate> newOrder)
        {

            voices = newOrder;

            string voicesOrder = "";

            foreach (VoiceRate vi in newOrder)
            {
                voicesOrder += vi.Voice.Id + ";";
            }

            voicesOrder = voicesOrder.TrimEnd(';');

            Settings.Default.Voices = voicesOrder;
            Settings.Default.Save();
        }



        public string SpeechExport(string path, string text)
        {
            text = CleanUp(text);
            return text;
        }

    }
}
