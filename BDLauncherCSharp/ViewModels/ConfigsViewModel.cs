using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

using BDLauncherCSharp.Data;
using BDLauncherCSharp.Data.Model;
using BDLauncherCSharp.Data.Native;
using BDLauncherCSharp.Extensions;

namespace BDLauncherCSharp.ViewModels
{
    public class ConfigsViewModel : INotifyPropertyChanged
    {
        private string _renderer;
        private byte difficult;
        private bool noBorder;
        private string screenSize;
        private bool windowed;
        public bool useBuffer;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
                    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public byte Difficult
        {
            get => difficult; set
            {
                // 数据验证
                if (value > 2) return;
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

        public string[] Renderers_Source { get; }

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

        public HashSet<string> ScreeSize_Source { get; }

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
                IsFullScreen = !Windowed,
                Difficult = (Difficult)Enum.Parse(typeof(Difficult), Difficult.ToString())
            };

            var tmp = ScreenSize.Split('*');
            result.ScreenWidth = ushort.Parse(tmp[0]);
            result.ScreenHeight = ushort.Parse(tmp[1]);
            return result;
        }
    }
}
