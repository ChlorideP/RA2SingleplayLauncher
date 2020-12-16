using System.Threading.Tasks;

namespace BattleLauncher
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
        public MessageBox() => InitializeComponent();
        private MessageBox(string msg) : this() => _message.Text = msg;

        public static Task Show(string message, string title = null) => MainWindow.ShowMessageDialog(new MessageBox(message) { Title = title });
    }
}
