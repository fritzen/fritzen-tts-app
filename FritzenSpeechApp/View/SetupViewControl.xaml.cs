using FritzenSpeech;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace FritzenSpeechApp
{
    /// <summary>
    /// Interaction logic for SetupViewControl.xaml
    /// </summary>
    public partial class SetupViewControl : UserControl
    {
        private readonly SpeechApi api;

        public SetupViewControl(SpeechApi api)
        {
            InitializeComponent();

            this.api = api;


            foreach (VoiceRate voice in api.Voices)
            {
                lvVoices.Items.Add(voice);
            }
            lvVoices.SelectedIndex = 0;

            sliderVolume.Value = api.Volume;

            cbFocus.IsChecked = Properties.Settings.Default.IsFocus;
            cbStartupFocus.IsChecked = Properties.Settings.Default.IsStartupFocus;
            cboShortcut.SelectedIndex = Properties.Settings.Default.ShortcutIndex;

            api.SetHotkeys(Properties.Settings.Default.ShortcutIndex);



        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider s = (Slider)sender;
            api.AdjustVoiceRate(s.Uid, (int)s.Value);
        }

        private void btnChangeOrder_Click(object sender, RoutedEventArgs e)
        {
            object lvi = lvVoices.SelectedItem;
            lvVoices.Items.Remove(lvi);
            lvVoices.Items.Insert(0, lvi);
            lvVoices.SelectedIndex = 0;

            List<VoiceRate> vr = lvVoices.Items.Cast<VoiceRate>().ToList();
            api.SpeechVoicesReorder(vr);
        }

        private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider s = (Slider)sender;
            api.SpeakVolume((int)s.Value);
        }

        private void cbFocus_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.IsFocus = (bool)cbFocus.IsChecked;
            Properties.Settings.Default.Save();
        }

        private void cbStartupFocus_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.IsStartupFocus = (bool)cbStartupFocus.IsChecked;
            Properties.Settings.Default.Save();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
            {
                return;
            }
            Properties.Settings.Default.ShortcutIndex = cboShortcut.SelectedIndex;
            Properties.Settings.Default.Save();

            api.SetHotkeys(Properties.Settings.Default.ShortcutIndex);

        }


        private void CmdPaypal_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=DBYX8KUYC9LMG&currency_code=BRL&source=url");
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start("http://fritzen.io/speech");
        }
    }
}
