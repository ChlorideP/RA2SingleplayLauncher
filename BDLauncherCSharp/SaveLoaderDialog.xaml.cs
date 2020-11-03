using BDLauncherCSharp.Controls;
using BDLauncherCSharp.Extensions;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using static BDLauncherCSharp.Data.OverAll;

namespace BDLauncherCSharp
{
    /// <summary>
    /// SaveLoaderDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SaveLoaderDialog : GDialog
    {

        public SaveLoaderDialog()
        {
            InitializeComponent();
            this.I18NInitialize();
            
            this.SaveList.ItemsSource = new ObservableCollection<Data.SavedGameInfo>(GetSavedGameInfoList(SavedGameDirectory));

        }

        private static IEnumerable<Data.SavedGameInfo> GetSavedGameInfoList(DirectoryInfo dir)
            => dir.GetFiles("*.sav").Select(SavedGameExtension.GetSavedGameInfo);
    }
}
