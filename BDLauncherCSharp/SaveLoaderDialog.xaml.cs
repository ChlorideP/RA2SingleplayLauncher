using BDLauncherCSharp.Controls;
using BDLauncherCSharp.GameEnvironment;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BDLauncherCSharp
{
    /// <summary>
    /// SaveLoaderDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SaveLoaderDialog : GDialog
    {
        public SaveLoaderDialog()
        {
            InitializeComponent();
        }

        //get Save time
        public static List<string> GetSaveGameTime()
        {
            List<string> time = new List<string>();
            var a = new DirectoryInfo(CheckGameEnvi.SaveData);
            foreach (var b in a.GetFiles("*.sav"))
            {
                DateTime c = b.LastWriteTime;
                time.Add(c.ToString());
            }
            return time;
        }

        //ASYNC Method to get Save name
        public static async Task<string> GetSavedGameNameAsync(Stream stream)
        {
            var buffer = new byte[0x40];
            stream.Seek(0x09C0, SeekOrigin.Begin);
            await stream.ReadAsync(buffer, 0, 0x40);
            var sb = new StringBuilder(40);
            using (var ms = new MemoryStream(buffer))
            using (var sr = new StreamReader(ms, Encoding.Unicode))
                while (sr.Peek() > 0) sb.Append((char)sr.Read());
            return sb.ToString();
        }
    }
}
