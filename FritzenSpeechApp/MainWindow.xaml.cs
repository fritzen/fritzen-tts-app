using FritzenSpeech;
using FritzenSpeechApp.View;
using System.Globalization;
using System.Reflection;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows;

namespace FritzenSpeechApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SpeechApi api;
        private readonly SpeechViewControl speechViewControl;
        private readonly SetupViewControl setupViewControl;

        private bool shutdown = false;
        private const double GRID_WIDTH = 575;


        public MainWindow()
        {
            api = new SpeechApi();

            InitializeComponent();

            string currentCulture = Thread.CurrentThread.CurrentCulture.Name;

            if (currentCulture.Equals("pt-BR"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("pt-BR");
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
            }

            api.api.SpeakProgress += Api_SpeakProgress;
            api.api.SpeakStarted += Api_SpeakStarted;
            api.api.SpeakCompleted += Api_SpeakCompleted;


            api.EventFired += Api_EventFired;


            speechViewControl = new SpeechViewControl();
            setupViewControl = new SetupViewControl(api);

            ContentA.Content = speechViewControl;
            ContentB.Content = setupViewControl;

            SetupShow(Properties.Settings.Default.IsSetupVisible);

        }

        private void Api_EventFired(SpeechEventArgs e)
        {
            switch (e.Actions)
            {
                case Actions.RESTORE:
                    if (!IsVisible)
                    {
                        Show();
                    }

                    if (WindowState == WindowState.Minimized)
                    {
                        WindowState = WindowState.Normal;
                    }
                    Opacity = 100;
                    ShowInTaskbar = true;
                    Visibility = Visibility.Visible;
                    Activate();
                    Topmost = true;
                    Topmost = false;
                    Focus();
                    break;
                case Actions.ABOUT:
                    //MessageBox.Show("Sobre: speech@fritzen.io");
                    break;
                case Actions.EXIT:
                    shutdown = true;
                    Application.Current.Shutdown(0);
                    break;
                default:
                    break;
            }
        }

        private void SetFocus()
        {
            if (Properties.Settings.Default.IsFocus)
            {
                ShowInTaskbar = true;
                Show();
                Activate();
                Topmost = true;
                Topmost = false;
                Focus();
            }
        }

        private static string GetTextFieldValue(Prompt p)
        {
            object text = typeof(Prompt).GetField("_text", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(p);
            return (string)(text.GetType() == typeof(string) ? text : string.Empty);
        }
        private void Api_SpeakStarted(object sender, System.Speech.Synthesis.SpeakStartedEventArgs e)
        {
            string spokenWord = GetTextFieldValue(e.Prompt);
            speechViewControl.SetText(spokenWord);
            SetFocus();
            speechViewControl.SetEditMode(false);
        }

        private void Api_SpeakProgress(object sender, System.Speech.Synthesis.SpeakProgressEventArgs e)
        {

            speechViewControl.UpdateSelection(e.CharacterPosition, e.CharacterCount);
        }

        private void Api_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            BtnPlay.IsEnabled = true;
            BtnPause.IsEnabled = false;
            speechViewControl.SetEditMode(true);
        }

        private void BtnSetup_Click(object sender, RoutedEventArgs e)
        {

            SetupToggle();
        }

        private void SetupToggle()
        {
            SetupShow(!(bool)Properties.Settings.Default["IsSetupVisible"]);
        }

        private void SetupShow(bool visible)
        {
            //Salvar o tamanho atual
            Properties.Settings.Default["IsSetupVisible"] = visible;
            Properties.Settings.Default.Save();


            if (visible)
            {
                GridSetupColumn.Width = new GridLength(GRID_WIDTH);
                GridSetupColumn.MinWidth = GRID_WIDTH;
            }
            else
            {
                GridSetupColumn.Width = new GridLength(0);
                GridSetupColumn.MinWidth = 0;
            }

            BtnSetup.IsChecked = visible;
        }


        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            api.SpeakStartOrResume(speechViewControl.GetText());

            BtnPlay.IsEnabled = false;
            BtnPause.IsEnabled = true;
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            api.SpeakPause();
            BtnPlay.IsEnabled = true;
            BtnPause.IsEnabled = false;
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            speechViewControl.SetEditMode(true);
            api.SpeakStop();
            BtnPlay.IsEnabled = true;
            BtnPause.IsEnabled = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!shutdown)
            {
                e.Cancel = true;
                Hide();
                api.ShowBalloonTip();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.IsStartupFocus) //Iniciar minimizado
            {
                Hide();
                api.ShowBalloonTip();
            }
        }
    }
}
