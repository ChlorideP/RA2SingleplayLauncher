using System.Windows;

using static BattleLauncher.Data.OverAll;

namespace BattleLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //String.Resource.ResourceManager.I18NInitialize();// I18N 初始化
            base.OnStartup(e);
            //GameEnvironment.CheckGameEnvi.CheckSaveDir()
            if (!ArchiveFolder.Exists) ArchiveFolder.Create();
            if (SpawnIni.Exists) SpawnIni.Delete();
        }
    }
}
