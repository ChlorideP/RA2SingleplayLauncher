using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BattleLauncher.ViewModels
{
    public class ConfigsViewModel : INotifyPropertyChanged
    {
        protected RendererViewModel _renderer;
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

        public RendererViewModel Renderer
        {
            get => _renderer; set
            {
                _renderer = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<RendererViewModel> Renderers { get; }

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

        public ConfigsViewModel()
        {
            Renderers = new ObservableCollection<RendererViewModel>(Data.RenderersManager.Renderers.Keys.Select(RendererViewModel.CreateById));

            Renderer = Renderers.Where(renderer => Data.RenderersManager.Current.Id.Equals(renderer.Id)).First();
        }
    }
}
