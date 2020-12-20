using System;
using System.Collections.Generic;

using BattleLauncher.Data.Model;
using BattleLauncher.Data.Native;
using BattleLauncher.Extensions;

namespace BattleLauncher.ViewModels
{
    public class ConfigsViewModel : ConfigsViewModelBase
    {
        public ConfigsViewModel(GameConfigure configure)
        {
            Difficult = (byte)configure.Difficult;
            UseBuffer = configure.BackBuffer;
            NoBorder = configure.NoBorder;
            Windowed = configure.IsWindowed;

            ScreenSize = string.Join("*", configure.ScreenWidth, configure.ScreenHeight);


            ScreeSize_Source = new HashSet<string>();
            for (var i = 0; User32.EnumDisplaySettings(null, i, out var vDevMode); i++)
                ScreeSize_Source.Add(string.Join("*", vDevMode.dmPelsWidth, vDevMode.dmPelsHeight));

            Renderers_Source = new[] {
                I18NExtension.I18N("cbRenderer.None"),
                I18NExtension.I18N("cbRenderer.CNCDDraw")
            };
        }
        public GameConfigure ToModel()
        {
            var result = new GameConfigure
            {
                BackBuffer = UseBuffer,
                NoBorder = NoBorder,
                IsWindowed = Windowed,
                Difficult = (Difficult)Enum.Parse(typeof(Difficult), Difficult.ToString())
            };

            var tmp = ScreenSize.Split('*');
            result.ScreenWidth = ushort.Parse(tmp[0]);
            result.ScreenHeight = ushort.Parse(tmp[1]);
            return result;
        }
    }
}
