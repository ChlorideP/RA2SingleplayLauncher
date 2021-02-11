using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using BattleLauncher.Controls;
using BattleLauncher.Extensions;

using static BattleLauncher.Data.OverAll;

namespace BattleLauncher
{
    /// <summary>
    /// UserInterface.xaml 的交互逻辑
    /// </summary>
    public partial class UserInterface : GDialog
    {
        private static HashSet<string> _resolutions = null;

        private static HashSet<string> GetResolutions()
        {
            _resolutions = new HashSet<string>();
            for (var i = 0; Data.Native.User32.EnumDisplaySettings(null, i, out var vDevMode); i++)
                _resolutions.Add(string.Join("*", vDevMode.dmPelsWidth, vDevMode.dmPelsHeight));
            return _resolutions;
        }
        private void Difficulty_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            => (sender as Slider).Value = Math.Ceiling((sender as Slider).Value);

        private async Task InitView()
        {
            //Data.RenderersManager.Current.Config
            var dataTask = Task.Run(async () => await (await GameConfigExtensions.ReadConfig().ConfigureAwait(false)).ReadConfig(Data.RenderersManager.Current.Id).ConfigureAwait(false));

            var resolutions = _resolutions ?? GetResolutions();
            cbSize.ItemsSource = resolutions;

            var vm = (await dataTask).GetViewModel();
            DataContext = vm;

        }

        public UserInterface()
        {
            InitializeComponent();
            this.I18NInitialize();
            _ = InitView();
        }
    }
}
