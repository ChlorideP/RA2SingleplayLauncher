using System.Windows;

namespace BattleLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await Data.OverAll.OverAll_Initialize().ConfigureAwait(false);
            Data.RenderersManager.Init();
        }
    }
}
