using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace FritzenSpeech
{
    public class Util
    {
        public static void Delay(int Time_delay)
        {
            int i = 0;
            System.Timers.Timer _delayTimer = new System.Timers.Timer
            {
                Interval = Time_delay,
                AutoReset = false //so that it only calls the method once
            };
            _delayTimer.Elapsed += (s, args) => i = 1;
            _delayTimer.Start();
            while (i == 0)
            {
                Application.DoEvents();
            };
        }

        public static string GetMd5Hash(string input)
        {

            MD5 md5Hash = MD5.Create();

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }


        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        public static void PressKey(Keys key, bool up)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;
            if (up)
            {
                keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            }
            else
            {
                keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            }
        }
    }
}
