using AudioSwitcher.AudioApi.CoreAudio;
using AudioSwitcher.AudioApi.Session;
using System.Collections.Generic;
using System.Reflection;

namespace FritzenSpeech
{
    public class VolumeMixer
    {

        private readonly CoreAudioDevice ca = null;
        private readonly Dictionary<string, double> volume = new Dictionary<string, double>();
        private bool adjustAudio = true;

        public void SetAdjustAudio(bool value)
        {
            adjustAudio = value;
        }

        public VolumeMixer()
        {
            ca = new CoreAudioController().DefaultPlaybackDevice;
        }

        public void AdjustVolume()
        {
            if (adjustAudio)
            {
                string appName = Assembly.GetEntryAssembly().GetName().Name;

                if (ca.State == AudioSwitcher.AudioApi.DeviceState.Active)
                {
                    IEnumerable<IAudioSession> sessions = ca.SessionController.ActiveSessions();
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
            if (adjustAudio)
            {
                string appName = Assembly.GetEntryAssembly().GetName().Name;

                if (ca.State == AudioSwitcher.AudioApi.DeviceState.Active)
                {
                    IEnumerable<IAudioSession> sessions = ca.SessionController.ActiveSessions();
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

