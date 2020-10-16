using BDLauncherCSharp.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
