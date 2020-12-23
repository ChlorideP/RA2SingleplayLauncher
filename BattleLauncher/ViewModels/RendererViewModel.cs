using System;
using System.Collections.Generic;

using BattleLauncher.Data.Model;
using BattleLauncher.Data.Native;
using BattleLauncher.Extensions;

namespace BattleLauncher.ViewModels
{
    public class RendererViewModel : ConfigsViewModel
    {
        private new bool noBorder;
        private new bool windowed;

        public new bool NoBorder
        {
            get => noBorder; set
            {
                noBorder = value;
                OnPropertyChanged();
            }
        }

        public new bool Windowed
        {
            get => windowed; set
            {
                if (!(windowed = value))
                    NoBorder = false;
                OnPropertyChanged();
            }
        }

        public RendererViewModel(GameConfigure configure, RendererConfigure config) : base(configure)
        {
            base.NoBorder = false;
            base.Windowed = false;
            NoBorder = config.NoBorder;
            Windowed = config.IsWindowed;
        }
        public new RendererConfigure ToModel() => new RendererConfigure
        {
            NoBorder = NoBorder,
            IsFullScreen = !Windowed,
            IsWindowed = Windowed
        };
    }
}
