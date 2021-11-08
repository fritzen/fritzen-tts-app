using AudioSwitcher.AudioApi.CoreAudio;
using AudioSwitcher.AudioApi.Session;
using System.Collections.Generic;
using System.Reflection;

namespace FritzenSpeech
{
    public class VolumeMixer
    {

        private CoreAudioController cac = null;
        private readonly Dictionary<string, double> volume = new Dictionary<string, double>();
        private bool adjustAudio = true;

        public void SetAdjustAudio(bool value)
        {
            adjustAudio = value;
        }

        public VolumeMixer()
        {
            cac = new CoreAudioController();            
        }

        public void AdjustVolume()
        {
            CoreAudioDevice ca = cac.DefaultPlaybackDevice;

            if (adjustAudio)
            {
                string appName = Assembly.GetEntryAssembly().GetName().Name;

                if (ca.State == AudioSwitcher.AudioApi.DeviceState.Active)
                {
                    IAudioSessionController controller = ca.SessionController;
                    if (controller == null)
                    {
                        cac = new CoreAudioController();
                        controller = cac.DefaultPlaybackDevice.SessionController;
                    }
                    IEnumerable<IAudioSession> sessions = controller.ActiveSessions();
                    foreach (IAudioSession session in sessions)
                    {
                        if (!session.DisplayName.Equals(appName))
                        {
                            AddOrUpdateKV(session.Id, session.Volume);
                            session.Volume = 10;
                        }
                    }
                }
            }
        }

        public void RestoreVolume()
        {
            CoreAudioDevice ca = cac.DefaultPlaybackDevice;
            if (adjustAudio)
            {
                string appName = Assembly.GetEntryAssembly().GetName().Name;

                if (ca.State == AudioSwitcher.AudioApi.DeviceState.Active)
                {
                    IAudioSessionController controller = ca.SessionController;
                    if (controller == null)
                    {
                        cac = new CoreAudioController();
                        controller = cac.DefaultPlaybackDevice.SessionController;
                    }
                    IEnumerable<IAudioSession> sessions = controller.ActiveSessions();
                    foreach (IAudioSession session in sessions)
                    {
                        if (!session.DisplayName.Equals(appName))
                        {
                            if (volume.TryGetValue(session.Id, out double value))
                            {
                                session.Volume = value;
                            }

                        }
                    }
                }
            }
        }

        private void AddOrUpdateKV(string key, double value)
        {
            if (adjustAudio)
            {
                if (volume.ContainsKey(key))
                {
                    volume[key] = value;
                }
                else
                {
                    volume.Add(key, value);
                }
            }
        }

    }

}

