using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using BDLauncherCSharp.Data;

namespace BDLauncherCSharp.ViewModels
{
    public abstract class ConfigsViewModelBase : INotifyPropertyChanged
    {
        private string _renderer;

        private byte difficult;

        private bool noBorder;

        private string screenSize;

        private bool windowed;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
                    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public bool useBuffer;

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

        public string Renderer
        {
            get => _renderer ?? OverAll.CurRenderer; set
            {
                _renderer = value;
                OnPropertyChanged();
            }
        }

        public string[] Renderers_Source { get; protected set; }

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

        public HashSet<string> ScreeSize_Source { get; protected set; }

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
