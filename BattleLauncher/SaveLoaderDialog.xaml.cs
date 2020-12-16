using System.Linq;

using BattleLauncher.Controls;
using BattleLauncher.Data;
using BattleLauncher.Extensions;

namespace BattleLauncher
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