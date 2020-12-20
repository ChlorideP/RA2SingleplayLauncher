using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using BattleLauncher.Extensions;

using static BattleLauncher.Data.OverAll;

namespace BattleLauncher.ViewModels
{
    public abstract class ConfigsViewModelBase : INotifyPropertyChanged
    {
        protected string _renderer;
        protected byte difficult;
        protected bool noBorder;
        protected string screenSize;
        protected bool useBuffer;
        protected bool windowed;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public virtual byte Difficult
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

        public virtual bool NoBorder
        {
            get => noBorder; set
            {
                noBorder = value;
                OnPropertyChanged();
            }
        }

        public virtual string Renderer
        {
            get => _renderer ?? (DDRAWDLL.SHA512Verify(CNCD) ? "cbRenderer.CNCDDraw".I18N() : "cbRenderer.None".I18N()); set
            {
                _renderer = value;
                OnPropertyChanged();
            }
        }

        public virtual string[] Renderers_Source { get; protected set; }

        public virtual string ScreenSize
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

        public virtual HashSet<string> ScreeSize_Source { get; protected set; }

        public virtual bool UseBuffer
        {
            get => useBuffer; set
            {
                useBuffer = value;
                OnPropertyChanged();
            }
        }

        public virtual bool Windowed
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
