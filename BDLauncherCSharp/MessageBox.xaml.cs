using System;
using System.Collections.Generic;
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
    /// MessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBox
    {
        /// <summary>
        /// 消息框承载
        /// </summary>
        public static MainWindow MainWindow;
        public MessageBox()
        {
            InitializeComponent();
        }
        private MessageBox(string msg) : this()
        {
            _message.Text = msg;
        }

        public static Task Show(string message,string title = null)
        {
             return MainWindow.ShowMessageDialog(new MessageBox(message) { Title = title });
        }
    }
}
