using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace FritzenSpeechApp
{
    public partial class App : Application
    {
        private static readonly Mutex mutex = new Mutex(true, "{1F524RA6-A541-49BB-8DC9-FDDE4CA88975}");
        private static MainWindow mainWindow = null;

        private App()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
        }

        [STAThread]
        private static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                App app = new App();
                mainWindow = new MainWindow();
                app.Run(mainWindow);
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("Only one instance at a time. Click OK to quit.", "Fritzen Speech" , MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
            }

        }
    }

}
