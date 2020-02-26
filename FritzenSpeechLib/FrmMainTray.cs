using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace FritzenSpeech
{
    public partial class FrmMainTray : Form
    {

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string sClassName, string sAppName);

        public Hotkey hotkey;

        private readonly ResourceManager res_man;
        private readonly CultureInfo cul;

        public delegate void FritzenTrayEventHandler(SpeechEventArgs e);
        public event FritzenTrayEventHandler SpeechActionFired;

        protected virtual void OnChanged(SpeechEventArgs e)
        {
            SpeechActionFired?.Invoke(e);
        }

        public FrmMainTray()
        {

            InitializeComponent();

            res_man = new ResourceManager("FritzenSpeech.Lang.Res", Assembly.GetExecutingAssembly());


            string currentCulture = Thread.CurrentThread.CurrentCulture.Name;


            if (currentCulture.Equals("pt-BR"))
            {
                cul = CultureInfo.CreateSpecificCulture("pt-BR");
            }
            else if (currentCulture.Equals("en"))
            {
                cul = CultureInfo.CreateSpecificCulture("en");
            }

            ctxAbout.Text = res_man.GetString("cmdAbout", cul);
            ctxExit.Text = res_man.GetString("cmdExit", cul);
            ctxRestore.Text = res_man.GetString("cmdRestore", cul);

            trayIcon.ContextMenuStrip = ctxStatus;
            trayIcon.Icon = Icon;
            trayIcon.Text = "Fritzen Speech";
            trayIcon.BalloonTipText = "Fritzen Speech (TTS)";
            trayIcon.BalloonTipTitle = "....";
            trayIcon.Visible = true;
            trayIcon.BalloonTipClicked += TrayIcon_BalloonTipClicked;

        }

        private void TrayIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            OnChanged(new SpeechEventArgs(Actions.RESTORE));
        }

        public void ShowBalloonTip()
        {
            string tipTray = res_man.GetString("tipTray", cul);

            if (hotkey.Modifier == Hotkey.FsModifiers.None)
            {
                trayIcon.BalloonTipTitle = string.Format(tipTray, "F1");
            }
            else if (hotkey.Modifier == Hotkey.FsModifiers.Control)
            {
                trayIcon.BalloonTipTitle = string.Format(tipTray, "CTRL + F1");
            }
            trayIcon.ShowBalloonTip(1500);
        }

        protected override void WndProc(ref Message keyPressed)
        {
            if (keyPressed.Msg == 0x0312) // WM_HOTKEY-F1
            {
                OnChanged(new SpeechEventArgs(Actions.HOTKEY_FIRED));
                base.WndProc(ref keyPressed);
            }

            base.WndProc(ref keyPressed);
        }

        private void ctxExit_Click(object sender, EventArgs e)
        {
            OnChanged(new SpeechEventArgs(Actions.EXIT));
        }

        private void ctxRestore_Click(object sender, EventArgs e)
        {
            OnChanged(new SpeechEventArgs(Actions.RESTORE));
        }

        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            OnChanged(new SpeechEventArgs(Actions.RESTORE));
        }

        protected override void SetVisibleCore(bool value)
        {
            bool allowshowdisplay = true;
            base.SetVisibleCore(allowshowdisplay ? value : allowshowdisplay);
        }

        private void FrmMainTray_Load(object sender, EventArgs e)
        {
            IntPtr thisWindow = FindWindow(null, Text);
            hotkey = new Hotkey(thisWindow);
        }

        public void RegisterHotkey(int index)
        {
            hotkey.UnRegisterHotKeys();
            Keys k = Keys.F1;
            hotkey.RegisterHotKeys(index, k);
        }
        public void UnRegisterHotkey()
        {
            hotkey.UnRegisterHotKeys();
        }
        private void FrmMainTray_FormClosed(object sender, FormClosedEventArgs e)
        {
            UnRegisterHotkey();
        }

        private void ctxAbout_Click(object sender, EventArgs e)
        {
            OnChanged(new SpeechEventArgs(Actions.ABOUT));
        }

        private void ctxPTBR_Click(object sender, EventArgs e)
        {

        }
        private void ctxEN_Click(object sender, EventArgs e)
        {

        }
    }
}
