using System;
using System.IO;
//using System.Windows.Shapes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using BDLauncherCSharp.Controls;
using BDLauncherCSharp.Data;
using BDLauncherCSharp.Extensions;

namespace BDLauncherCSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static string BDVol = OverAll.MainPath.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(':'));

        public MainWindow()
        {
            InitializeComponent();
            this.I18NInitialize();// I18N 初始化
            App.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Admin_Check.IsChecked = BDVol == "C";
            Admin_Check.IsEnabled = BDVol != "C";
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            File.WriteAllText($"LauncherExcept.{DateTime.Now.ToString("o").Replace('/', '-').Replace(':', '-')}.log", e.Exception.ToString());
        }

        private async void Btn_UserInterface_Click(object sender, RoutedEventArgs e)
        {
            var UI = new UserInterface();
            await ShowDialog(UI);
        }

        private async void Btn_ArchiveLoader_Click(object sender, RoutedEventArgs e)
        {
            var SL = new SaveLoaderDialog();
            await ShowDialog(SL);
        }

        private void Btn_CommandClear_Click(object sender, RoutedEventArgs e)
        {
            TB_Command.Text = string.Empty;
        }

        private void Btn_GameStart_Click(object sender, RoutedEventArgs e)
        {
            CriticalPEIdentify.SpawnerHash(OverAll.MainPath);
            if (!CriticalPEIdentify.IsBDFilelist) MessageBox.Show("无法加载「脑死」文件列表！", "「脑死」启动器");
            else if (!CriticalPEIdentify.IsThereAres)
                MessageBox.Show("此任务需要 Ares 扩展平台支持。\n\n请检查您的游戏文件是否含 Ares.dll 和 Syringe.exe。\n如找不到，建议重新下载安装。", "「脑死」启动器");
            else
            {
                var option = new GameExecuteOptions
                {
                    LogMode = Debug_Check.IsChecked ?? false,
                    RunAs = Admin_Check.IsChecked ?? false,
                    Others = TB_Command.Text.Split(' ')
                };
                GameExecute.RunGame(option);
                Environment.Exit(0);
            }
        }

        public async Task<GDialogResult> ShowDialog(GDialog dialog)
        {
            var pause = new ManualResetEvent(false);
            dialogMask.Child = dialog;
            dialogMask.Visibility = Visibility.Visible;
            dialog.Show(pause);
            await Task.Run(pause.WaitOne);
            dialogMask.Visibility = Visibility.Collapsed;
            dialogMask.Child = null;
            return dialog.Result;
        }
    }
}
