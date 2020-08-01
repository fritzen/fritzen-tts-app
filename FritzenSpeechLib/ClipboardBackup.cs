using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Windows.Forms;

namespace FritzenSpeech
{
    public class ClipboardItem
    {
        public string Format { get; set; }
        public Type ObjectType { get; set; }
        public object Object { get; set; }
        public string ObjectString { get; set; }
        public StringCollection ObjectStringCollection { get; set; }

        public Stream ObjectStream { get; set; }

        public Image ObjectImage { get; set; }

    }
    public class ClipboardBackup
    {
        private readonly List<ClipboardItem> contents = new List<ClipboardItem>();

        public ClipboardBackup()
        {
            UIPermission clipBoard = new UIPermission(PermissionState.None)
            {
                Clipboard = UIPermissionClipboard.AllClipboard
            };
        }

        public void Backup()
        {
            contents.Clear();
            IDataObject data = Clipboard.GetDataObject();
            try
            {
                List<string> formats = data.GetFormats(false).ToList();

                foreach(string f in formats)
                {
                    /*
                  * TODO: BUG
                  * Managed Debugging Assistant 'FatalExecutionEngineError' : 'O tempo de execução encontrou um erro fatal. O endereço do erro é 0xb5252112, no thread 0x22dc. O código do erro é 0xc0000005. Esse erro pode ser um bug no CLR ou nas partes não segura ou não verificável do código do usuário. Origens comuns desse bug incluem erros de marshaling de usuário para COM-interop ou PInvoke, o que pode danificar a pilha.'
                  *
                  * System.ExecutionEngineException: 'Exception_WasThrown'
                  */
                  if (f.StartsWith("EnhancedMetafile"))
                    {
                        continue;
                    }
                    object clip = data.GetData(f, false);
                    if (clip != null)
                    {
                        Console.WriteLine(f);
                        contents.Add(new ClipboardItem()
                        {
                            Format = f,
                            ObjectType = clip.GetType(),
                            Object = clip,
                            ObjectString = Clipboard.GetText(),
                            ObjectImage = Clipboard.GetImage(),
                            ObjectStringCollection = Clipboard.GetFileDropList(),
                            ObjectStream = Clipboard.GetAudioStream()
                        });
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Restore()
        {
            try
            {
                Clipboard.Clear();
                Util.Delay(1);
                Clipboard.Clear();
                Util.Delay(1);

                DataObject data = new DataObject();
                contents.ForEach(item =>
                {
                    data.SetData(item.Format, true, item.Object);
                    Console.WriteLine(item.ObjectType);
                });
                Util.Delay(1);
                Clipboard.SetDataObject(data, false, 5, 250);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

}
