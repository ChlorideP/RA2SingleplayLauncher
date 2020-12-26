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
        private static Data.Model.RendererOptions[] _renderers = null;
        private static HashSet<string> _resolutions = null;
        private static Data.Model.RendererOptions[] GetRenderers()
        {
            _renderers = new[]
            {
                new Data.Model.RendererOptions{
                    Name="NONE",
                    FriendlyName=I18NExtension.I18N("cbRenderer.None"),
                },
                new Data.Model.RendererOptions{
                    Name="CNCDDRAW",
                    FriendlyName= I18NExtension.I18N("cbRenderer.CNCDDraw") ,
                    Directory = new DirectoryInfo(Path.Combine(MainFolder.FullName,"Resources","Renderers", "cnc-ddraw"))
                },
                new Data.Model.RendererOptions{
                    Name="DDWRAPPER",
                    FriendlyName="DDWrapper",
                    Directory = new DirectoryInfo(Path.Combine(MainFolder.FullName,"Resources","Renderers", "DDWrapper"))
                },
                new Data.Model.RendererOptions{
                    Name="DXWND",
                    FriendlyName="DxWnd",
                    Directory = new DirectoryInfo(Path.Combine(MainFolder.FullName,"Resources","Renderers", "DxWnd"))
                },
                new Data.Model.RendererOptions{
                    Name="TSDDRAW",
                    FriendlyName="TS-DDraw",
                    Directory = new DirectoryInfo(Path.Combine(MainFolder.FullName,"Resources","Renderers", "ts-ddraw"))
                },
                new Data.Model.RendererOptions{
                    Name="IEDDRAW",
                    FriendlyName="IE-DDraw",
                    Directory = new DirectoryInfo(Path.Combine(MainFolder.FullName,"Resources","Renderers", "ie-ddraw"))
                },
                new Data.Model.RendererOptions{
                    Name="COMPAT",
                    FriendlyName="DDrawCompat",
                    Directory = new DirectoryInfo(Path.Combine(MainFolder.FullName,"Resources","Renderers", "compat"))
                },
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
            var hashget = Task.Run(() => new FileInfo(Path.Combine(MainFolder.FullName, "ddraw.dll")).GetSHA512());
            var model = GameConfigExtensions.ReadConfig();

            var resolutions = _resolutions ?? GetResolutions();
            var renderers = _renderers ?? GetRenderers();
            cbSize.ItemsSource = resolutions;
            cbRenderer.ItemsSource = renderers;

            if (await hashget == Data.Hash.R_CNC)
                model = GameConfigExtensions.ReadCNCDDRAWConfig(await model);
            if (await hashget == Data.Hash.R_DXWND)
                model = GameConfigExtensions.ReadDxWndConfig(await model);

            var vm = ConfigsViewModelExtension.GetViewModel(await model);

            var result = await hashget;

            switch (result)
            {
                case Data.Hash.R_CNC:
                    vm.Renderer = renderers[1];
                    break;
                case Data.Hash.R_Wrapper:
                    vm.Renderer = renderers[2];
                    break;
                case Data.Hash.R_DXWND:
                    vm.Renderer = renderers[3];
                    break;
                case Data.Hash.R_TS:
                    vm.Renderer = renderers[4];
                    break;
                case Data.Hash.R_IE:
                    vm.Renderer = renderers[5];
                    break;
                case Data.Hash.R_Compat:
                    vm.Renderer = renderers[6];
                    break;
                default:
                    vm.Renderer = renderers[0];
                    break;
            }

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
