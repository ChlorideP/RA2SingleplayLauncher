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
            MessageBox.MainWindow = this;
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
                    MessageBox.Show(ex.Message, "txtError".I18N());
                    break;
                case IOException ex:
                    e.Handled = true;
                    MessageBox.Show(ex.Message, "txtError".I18N());
                    break;
                default:
                    File.WriteAllText($"LauncherExcept.{DateTime.Now.ToString("o").Replace('/', '-').Replace(':', '-')}.log", e.Exception.ToString());
                    break;
            }
        }

        private async void Btn_UserInterface_Click(object sender, RoutedEventArgs e)
        {
            var ui = new UserInterface();
            switch (await ShowDialog(ui))
            {
                case GDialogResult.PrimaryButton:
                    switch (ui.DataContext)
                    {
                        case ViewModels.ConfigsViewModel cvm:
                            await ConfigureIO.SetConfigure(cvm.ToModel());
                            break;
                    }
                    break;
                case GDialogResult.CloseButton:
                    break;
            }
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
            //反而比直接new对象麻烦
            AresMainFunc.Refresh();
            AresInjector.Refresh();
            CNCNET5DLL.Refresh();

            //这才是流程
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
                App.Current.Shutdown();
            }
        }
        private Mutex _dialogMutex = new Mutex();
        public async Task<GDialogResult> ShowDialog(GDialog dialog)
        {
            GDialogResult result = GDialogResult.FaildOpen;
            if (_dialogMutex.WaitOne(500))
            {
                try
                {
                    var pause = new ManualResetEvent(false);
                    dialogMask.Child = dialog;
                    dialogMask.Visibility = Visibility.Visible;
                    dialog.Show(pause);
                    await Task.Run(pause.WaitOne);
                    dialogMask.Visibility = Visibility.Collapsed;
                    dialogMask.Child = null;
                    result = dialog.Result;
                }
                finally
                {
                    _dialogMutex.ReleaseMutex();
                }
            }
            return result;
        }
        public async Task ShowMessageDialog(GDialog dialog)
        {
            var pause = new ManualResetEvent(false);
            msgDialogMask.Child = dialog;
            msgDialogMask.Visibility = Visibility.Visible;
            dialog.Show(pause);
            await Task.Run(pause.WaitOne);
            msgDialogMask.Visibility = Visibility.Collapsed;
            msgDialogMask.Child = null;
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
