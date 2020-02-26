using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FritzenSpeech
{
    public class Hotkey
    {

        private FsModifiers modifier = FsModifiers.None;

        public enum FsModifiers
        {
            None = 0x0000,
            Alt = 0x0001,
            Control = 0x0002,
            Shift = 0x0004, // Changes!
            Window = 0x0008
        }

        private readonly IntPtr _hWnd;

        public FsModifiers Modifier => modifier;

        public Hotkey(IntPtr hWnd)
        {
            _hWnd = hWnd;
        }

        public void RegisterHotKeys(int index, Keys key)
        {
            modifier = FsModifiers.None;
            switch (index)
            {
                case 1:
                    modifier = FsModifiers.Control; break; //OK
                case 2:
                    modifier = FsModifiers.Alt; break; //NOT WORKING
                case 3:
                    modifier = FsModifiers.Shift; break; //NOT WORKING
                default:
                    break;
            }

            RegisterHotKey(_hWnd, 1, (uint)modifier, (uint)key);
        }

        public void UnRegisterHotKeys()
        {
            UnregisterHotKey(_hWnd, 1);
        }

        #region WindowsAPI
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        #endregion
    }
}
