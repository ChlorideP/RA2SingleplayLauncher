using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using BattleLauncher.Controls;
using BattleLauncher.Data.Model;
using BattleLauncher.Exceptions;
using BattleLauncher.Extensions;

using static BattleLauncher.Data.OverAll;

namespace BattleLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Mutex _dialogMutex = new Mutex();

        private async void ApplySetting(object sender, ExecutedRoutedEventArgs e)
        {
            switch (e.Parameter)
            {
                case ViewModels.RendererViewModel rvm:
                    var (rc,gc) = rvm.ToModel();
                    await DDrawIO.SetConfigure(rc);
                    gc.IsWindowMode = false;
                    gc.Borderless = false;
                    await ConfigureIO.SetConfigure(gc);
                    break;
                case ViewModels.ConfigsViewModel cvm:
                    await ConfigureIO.SetConfigure(cvm.ToModel());
                    break;
                default:
                    break;
            }
            Commands.DialogRoutedCommands.CloseCommand.Execute(null, e.Source as IInputElement);
        }

        private void CanClearCommandLine(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = !string.IsNullOrEmpty((e.Source as TextBox)?.Text);

        private void ClearCommandLine(object sender, ExecutedRoutedEventArgs e) => (e.Source as TextBox).Text = string.Empty;

        private void CloseDialog(object sender, ExecutedRoutedEventArgs e) => (e.Source as GDialog).Hide();

        private async void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            switch (e.Exception)
            {
                case SpawnerInvalidException _:
                    await MessageBox.Show("txtSpawnerInvalidError".I18N(), "txtCaptain".I18N());
                    break;
                case AresNotFoundException _:
                    await MessageBox.Show("txtAresNotFoundError".I18N(), "txtCaptain".I18N());
                    break;
                case NoSaveLoadedException _:
                    await MessageBox.Show("txtNoSaveLoadedError".I18N(), "txtCaptain".I18N());
                    break;
                case DirectoryNotFoundException ex:
                    await MessageBox.Show(ex.Message, "txtError".I18N());
                    break;
                case IOException ex:
                    await MessageBox.Show(ex.Message, "txtError".I18N());
                    break;
                default:
                    e.Handled = false;
                    await MessageBox.Show(e.Exception.Message, "txtFatal".I18N());
                    File.WriteAllText($"LauncherExcept.{DateTime.Now.ToString("o").Replace('/', '-').Replace(':', '-')}.log", e.Exception.ToString());
                    break;
            }
        }

        private void Exit(object sender, ExecutedRoutedEventArgs e) => Close();

        private async void LoadGame(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(e.Parameter is ViewModels.SavedGameViewModel vm))
                throw new NoSaveLoadedException();
            await vm.WriteSpawnAsync();
            Commands.MainWindowRoutedCommands.RunGameCommand.Execute(null, this);
        }

        private async void OpenArchiveLoader(object sender, ExecutedRoutedEventArgs e) => await ShowDialog(new SaveLoaderDialog());

        private async void OpenGameSettings(object sender, ExecutedRoutedEventArgs e) => await ShowDialog(new UserInterface());

        private void RunGame(object sender, ExecutedRoutedEventArgs e)
        {
#if RELEASE
            if (!CNCNET5DLL.SHA512Verify(CNCNET5))
#endif
#if DEBUG
            if (!CNCNET5DLL.Exists)
#endif
                throw new SpawnerInvalidException();

            if (!AresExistence())
                throw new AresNotFoundException();

            new GameExecuteOptions
            {
                LogMode = cbDebug_Check.IsChecked ?? false,
                RunAs = cbAdmin_Check.IsChecked ?? false,
                Others = tbCommand.Text.Split(' ')
            }.RunGame();

            Close();
        }

        public MainWindow()
        {
            MessageBox.MainWindow = this;
            InitializeComponent();
            this.I18NInitialize();// I18N 初始化

            cbAdmin_Check.IsEnabled = !(bool)(cbAdmin_Check.IsChecked = IsAdministrator());

            App.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }
        public async Task<GDialogResult> ShowDialog(GDialog dialog)
        {
            var result = GDialogResult.FaildOpen;
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
    }
}
