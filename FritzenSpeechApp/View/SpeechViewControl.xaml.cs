using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace FritzenSpeechApp.View
{
    /// <summary>
    /// Interaction logic for SpeechViewControl.xaml
    /// </summary>
    public partial class SpeechViewControl : UserControl
    {
        public SpeechViewControl()
        {
            InitializeComponent();
        }

        private string text;

        public void UpdateSelection(int characterPosition, int length)
        {

            string before = text.Substring(0, characterPosition);
            string word = text.Substring(characterPosition, length);
            string after = text.Substring((characterPosition + length));
            TextSpeech.Inlines.Clear(); 
            TextSpeech.Inlines.Add(before);

            Run run = new Run(word)
            {
                FontWeight = FontWeights.Bold
            };
            TextSpeech.Inlines.Add(run);
            TextSpeech.Inlines.Add(after);

        }

        public void SetEditMode(bool value)
        {
            TextSpeechEdit.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            TextSpeech.Visibility = value ? Visibility.Hidden : Visibility.Visible;
        }

        public string GetText()
        {
            return TextSpeech.Text;
        }

        public void SetText(string text)
        {
            this.text = text;
            TextSpeech.Text = text;
            TextSpeechEdit.Text = text;
        }

        private void TextSpeechEdit_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetText(TextSpeechEdit.Text);
        }
    }
}
