using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using BattleLauncher.Data.Model;
using BattleLauncher.Extensions;

using static BattleLauncher.Data.OverAll;

namespace BattleLauncher.ViewModels
{
    public class ConfigsViewModel : INotifyPropertyChanged
    {
        protected RendererOptions _renderer;
        protected byte difficult;
        protected bool noBorder;
        protected string screenSize;
        protected bool useBuffer;
        protected bool windowed;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public byte Difficult
        {
            get => difficult; set
            {
                // 数据验证
                if (value > 2)
                    return;
                difficult = value;
                OnPropertyChanged();
            }
        }

        public bool NoBorder
        {
            get => noBorder; set
            {
                noBorder = value;
                OnPropertyChanged();
            }
        }

        public RendererOptions Renderer
        {
            get => _renderer; set
            {
                _renderer = value;
                OnPropertyChanged();
            }
        }

        public string ScreenSize
        {
            get => screenSize;
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;
                if (!value.Contains("*"))
                    return;

                screenSize = value;
                OnPropertyChanged();
            }
        }

        public bool UseBuffer
        {
            get => useBuffer; set
            {
                useBuffer = value;
                OnPropertyChanged();
            }
        }

        public bool Windowed
        {
            get => windowed; set
            {
                if (!(windowed = value))
                    NoBorder = false;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
