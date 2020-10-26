using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BDLauncherCSharp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    { 
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //GameEnvironment.CheckGameEnvi.CheckSaveDir()
            await Task.Run(GameEnvironment.CheckGameEnvi.CheckSaveDir);
        }
    }
}
