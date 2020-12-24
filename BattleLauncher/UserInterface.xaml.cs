using System;
using System.Collections.Generic;
using System.IO;
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
        private static ViewModels.RendererViewModel[] _renderers = null;
        private static HashSet<string> _resolutions = null;
        private static ViewModels.RendererViewModel[] GetRenderers()
        {
            _renderers = new[]
            {
                new ViewModels.RendererViewModel{
                    Name="NONE",
                    FriendlyName=I18NExtension.I18N("cbRenderer.None"),
                },
                new ViewModels.RendererViewModel{
                    Name="CNCDDRAW",
                    FriendlyName= I18NExtension.I18N("cbRenderer.CNCDDraw") ,
                    Directory = new DirectoryInfo(Path.Combine(MainFolder.FullName,"Assets","Renderers","CNCDDRAW"))
                }
            };
            return _renderers;
        }

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
            var varify = Task.Run(() => new FileInfo(Path.Combine(MainFolder.FullName, "ddraw.dll")).SHA512Verify(Data.Hash.CNCD));
            var model = GameConfigExtensions.ReadConfig();

            var resolutions = _resolutions ?? GetResolutions();
            var renderers = _renderers ?? GetRenderers();
            cbSize.ItemsSource = resolutions;
            cbRenderer.ItemsSource = renderers;

            if (await varify)
                model = GameConfigExtensions.ReadCNCDDRAWConfig(await model);

            var vm = ConfigsViewModelExtension.GetViewModel(await model);

            vm.Renderer = await varify ? renderers[1] : renderers[0];

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
