using System;
using System.Collections.Generic;

using BattleLauncher.Data.Model;
using BattleLauncher.Data.Native;
using BattleLauncher.ViewModels;

namespace BattleLauncher.Extensions
{
    public static class ConfigsViewModelExtension
    {
        public static ConfigsViewModel GetViewModel(this GameConfig configure)
        {
            return new ConfigsViewModel
            {
                Difficult = (byte)configure.Difficult,
                UseBuffer = configure.BackBuffer,
                NoBorder = configure.Borderless,
                Windowed = configure.IsWindowMode,

                ScreenSize = string.Join("*", configure.ScreenWidth, configure.ScreenHeight),
            };
        }
        public static GameConfig ToModel(this ConfigsViewModel viewModel)
        {
            var result = new GameConfig
            {
                BackBuffer = viewModel.UseBuffer,
                Borderless = viewModel.NoBorder,
                IsFullScreen = !viewModel.Windowed,
                IsWindowMode = viewModel.Windowed,
                Difficult = (Difficult)Enum.Parse(typeof(Difficult), viewModel.Difficult.ToString())
            };

            var tmp = viewModel.ScreenSize.Split('*');
            result.ScreenWidth = ushort.Parse(tmp[0]);
            result.ScreenHeight = ushort.Parse(tmp[1]);
            return result;
        }
    }
}
