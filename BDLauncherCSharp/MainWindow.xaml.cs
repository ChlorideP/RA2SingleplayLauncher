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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Text.RegularExpressions;

namespace BDLauncherCSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Btn_UserInterface_Click(object sender, RoutedEventArgs e)
        {
            //new UserInterface().ShowDialog();
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
            //Check if Ares function exists
            var MainExePath = Environment.CurrentDirectory;
            // it seems has another way to optimize the "if"..
            // get on it tomorrow.
            if (!File.Exists(MainExePath + "\\gamemd.exe"))
            {
                MessageBox.Show("没主程序玩个锤子，爪巴", "「脑死」启动器");
            }
            else if (!File.Exists(MainExePath + "\\Ares.dll"))
            {
                MessageBox.Show("此任务有诸多内容需要Ares扩展平台支持。\n\n请检查您的游戏文件是否包含 Ares.dll。\n如否，建议重新下载安装。", "「脑死」启动器");
            }
            else if (!File.Exists(MainExePath + "\\Syringe.exe"))
            {
                MessageBox.Show("Ares扩展逻辑需要Syringe注入方可生效。\n\n请检查您的游戏文件是否包含 Syringe.exe。\n如否，建议重新下载安装。", "「脑死」启动器");
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
