using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
            base.OnStartup(e);
            //GameEnvironment.CheckGameEnvi.CheckSaveDir()
            if  (!SavedGameDirectory.Exists) SavedGameDirectory.Create();
        }
    }
}
