using System;
using System.Diagnostics;
using System.IO;
//using System.Windows.Shapes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using BDLauncherCSharp.Controls;
using BDLauncherCSharp.Data;
using BDLauncherCSharp.Data.Model;
using BDLauncherCSharp.Extensions;

using static BDLauncherCSharp.Data.OverAll;

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
            this.I18NInitialize();// I18N 初始化
            App.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Admin_Check.IsEnabled = !(bool)(Admin_Check.IsChecked = IsAdministrator());
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            switch (e.Exception)
            {
                case DirectoryNotFoundException ex:
                    e.Handled = true;
                    MessageBox.Show(ex.Message);
                    break;
                default:
                    File.WriteAllText($"LauncherExcept.{DateTime.Now.ToString("o").Replace('/', '-').Replace(':', '-')}.log", e.Exception.ToString());
                    break;
            }
        }

        private async void Btn_UserInterface_Click(object sender, RoutedEventArgs e)
        {
            var UI = new UserInterface();
            await ShowDialog(UI);
        }

        private async void Btn_ArchiveLoader_Click(object sender, RoutedEventArgs e)
        {
            var SL = new SaveLoaderDialog();
            var IsAttached = await ShowDialog(SL);
            if (IsAttached == 0) Btn_GameStart_Click(null, null);
        }

        private void Btn_CommandClear_Click(object sender, RoutedEventArgs e)
        {
            TB_Command.Text = string.Empty;
        }

        private void Btn_GameStart_Click(object sender, RoutedEventArgs e)
        {
            if (!SHA512Verify(CNCNET5DLL, CNCNET5)) MessageBox.Show(I18NExtension.I18N("msgSpawnerInvalidError"), I18NExtension.I18N("msgCaptain"));
            else if (!AresExistence) MessageBox.Show(I18NExtension.I18N("msgAresNotFoundError"), I18NExtension.I18N("msgCaptain"));
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

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
