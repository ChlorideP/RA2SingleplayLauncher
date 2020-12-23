using System;
using System.Collections.Generic;

using BattleLauncher.Data.Model;
using BattleLauncher.Data.Native;
using BattleLauncher.Extensions;

namespace BattleLauncher.ViewModels
{
    public class RendererViewModel : ConfigsViewModel
    {
        public RendererViewModel(GameConfigure configure, RendererConfigure config) : base(configure)
        {
            NoBorder = config.NoBorder;
            Windowed = config.IsWindowed;
        }
        public new (RendererConfigure, GameConfigure) ToModel()
        {
            var r = new RendererConfigure
            {
                NoBorder = NoBorder,
                IsFullScreen = !Windowed,
                IsWindowed = Windowed
            };
            var g = base.ToModel();
            return (r,g);
        }
    }
}
