using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using BDLauncherCSharp.Controls;
using BDLauncherCSharp.Data;
using BDLauncherCSharp.Extensions;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace BDLauncherCSharp
{
    /// <summary>
    /// UserInterface.xaml 的交互逻辑
    /// </summary>
    public partial class UserInterface : GDialog
    {
        public UserInterface()
        {
            InitializeComponent();
            this.I18NInitialize();
            _ = InitView();
        }

        private async Task InitView()
        {
            var task = GameConfigManager.InitGameConfig();
            var vm = new ViewModels.ConfigsViewModel();
            await task;
            DataContext = vm;
            cbRenderer.SelectedIndex = 0;
        }

        /// <Summary>
        /// 数据验证逻辑
        /// </Summary>
        private void ScreenSize_CheckValidVal(object sender, RoutedEventArgs e)
        {/*
            if (string.IsNullOrEmpty(cbSize.Text))
                return;

            if (!Regex.IsMatch(cbSize.Text, @"\d+\*\d+"))
            {
                MessageBox.Show(I18NExtension.I18N("msgSizeNotValidError"), I18NExtension.I18N("msgCaptain"));
                cbSize.Text = string.Empty;
                return;
            }

            string[] size = cbSize.Text.Split('*');
            if (int.Parse(size[0]) > 2560 || int.Parse(size[1]) > 1440) //2k only
            {
                MessageBox.Show(I18NExtension.I18N("msgSizeTooLargeError"), I18NExtension.I18N("msgCaptain"));
                cbSize.Text = string.Empty;
                return;
            }*/
        }

        private void Difficulty_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (sender as Slider).Value = Math.Ceiling((sender as Slider).Value);
        }

        private void cbRenderer_Apply(object sender, RoutedEventArgs e)
        {
            var rmCD = new DirectoryInfo(OverAll.MainPath).GetFiles("ddraw.*");
            var rmRD = new DirectoryInfo(Path.Combine(OverAll.MainPath, "Renderer")).GetFiles("cnc-ddraw.*");
            if ((string)cbRenderer.SelectedItem == I18NExtension.I18N("cbRenderer.None") && UserCustoms.IsCNCDDraw)
            {
                foreach (FileInfo file in rmCD) file.Delete();
                return;
            }
            if ((string)cbRenderer.SelectedItem == I18NExtension.I18N("cbRenderer.CNCDDraw") && UserCustoms.IsCNCDDraw) return;
            if ((string)cbRenderer.SelectedItem == I18NExtension.I18N("cbRenderer.None") && !UserCustoms.IsCNCDDraw) return;
            if ((string)cbRenderer.SelectedItem == I18NExtension.I18N("cbRenderer.CNCDDraw") && !UserCustoms.IsCNCDDraw)
            {
                foreach (FileInfo source in rmRD) 
                {
                    var filetype = Path.GetExtension(source.FullName);
                    source.CopyTo(Path.Combine(OverAll.MainPath, "ddraw" + filetype), true);
                }
                return;
            }
        }
    }
}
