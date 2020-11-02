using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BDLauncherCSharp.Controls;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Security.Cryptography;

namespace BDLauncherCSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static string MainPath = Environment.CurrentDirectory;
        public static string BDVol = MainPath.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(':'));
        public const string GameMD = "GAMEMD.EXE";
        public const string AresMainFunc = "Ares.DLL";
        public const string AresInjector = "Syringe.EXE";
        public static bool IsBDFilelist;

        public MainWindow()
        {
            InitializeComponent();
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
            // preset a default val for the bool below
            IsBDFilelist = false;

            // hash calculate
            var k = new DirectoryInfo(MainPath);
            var sha512 = new SHA512CryptoServiceProvider();
            foreach (var l in k.GetFiles("*.dll"))
            {
                byte[] hashCodeRaw = sha512.ComputeHash(l.OpenRead());
                string hashCode = BitConverter.ToString(hashCodeRaw).Replace("-", string.Empty);
                if (hashCode == "EC1C3976697D3C7755259A31E33B8D1E072FE1DD07D4B24251458EDC858C410C4A43AC3AB9C75F295D19ADE94C278BCB1FB20FD309A09C051610F895806D6503")
                {
                    IsBDFilelist = true;
                    break;
                }
            }

            // Combine path
            var p = Path.Combine(MainPath, GameMD);
            var q = Path.Combine(MainPath, AresMainFunc);
            var r = Path.Combine(MainPath, AresInjector);

            // Check critical pe files.            
            if (!File.Exists(p)) MessageBox.Show("没主程序玩个锤子，爪巴", "「脑死」启动器"); 
            else if (!File.Exists(q))
                MessageBox.Show("此任务需要Ares扩展平台支持。\n\n请检查您的游戏文件是否包含 Ares.dll。\n如否，建议重新下载安装。", "「脑死」启动器");
            else if (!File.Exists(r))
                MessageBox.Show("Ares需要Syringe注入方可生效。\n\n请检查您的游戏文件是否包含 Syringe.exe。\n如否，建议重新下载安装。", "「脑死」启动器");
            else if (!IsBDFilelist)
                MessageBox.Show("无法加载「脑死」文件列表！", "「脑死」启动器");
            else
            {
                var option = new Data.GameExecuteOptions
                {
                    LogMode = Debug_Check.IsChecked ?? false,
                    RunAs = Admin_Check.IsChecked ?? false,
                    Others = TB_Command.Text.Split(' ')
                };
                Extensions.GameExecute.RunGame(option);
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
