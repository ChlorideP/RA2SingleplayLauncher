using System.Linq;

using BDLauncherCSharp.Controls;
using BDLauncherCSharp.Data;
using BDLauncherCSharp.Extensions;

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
            SaveList.ItemsSource = OverAll.SavedGameDirectory.GetFiles("*.sav").Select(SavedGameHelper.GetSavedGameInfo);
        }
    }
}