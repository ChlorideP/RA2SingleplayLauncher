using System.IO;
using System.Windows;

using BDLauncherCSharp.Extensions;

using static BDLauncherCSharp.Data.OverAll;

namespace BDLauncherCSharp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            String.Resource.ResourceManager.I18NInitialize();// I18N 初始化
            base.OnStartup(e);
            //GameEnvironment.CheckGameEnvi.CheckSaveDir()
            if (!SavedGameDirectory.Exists) SavedGameDirectory.Create();
            if (File.Exists(SpawnIni)) File.Delete(SpawnIni);
        }
    }
}
