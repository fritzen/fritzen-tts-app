using System;

namespace FritzenSpeech
{

    public enum Actions
    {
        ABOUT,
        HOTKEY_FIRED,
        RESTORE,
        EXIT
    };

    public class SpeechEventArgs : EventArgs
    {
        public Actions Actions { get; set; }
        public string Param { get; set; }

        public SpeechEventArgs(Actions Actions, string Param)
        {
            this.Actions = Actions;
            this.Param = Param;
        }

        public SpeechEventArgs(Actions Actions)
        {
            this.Actions = Actions;
        }
    }
}
