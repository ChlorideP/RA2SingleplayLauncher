using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDLauncherCSharp.Data.Model
{
    public sealed class GameConfigure
    {
        public ushort ScreenWidth { get; set; }
        public ushort ScreenHeight { get; set; }

        public bool IsFullScreen { get; set; }
        public bool IsWindowed { get; set; }
        public bool NoBorder { get; set; }
        public bool BackBuffer { get; set; }

        public Difficult Difficult { get; set; }
    }
    public enum Renderers : byte
    {
        None,
        CNCDDRAW
    }
    public enum Difficult : byte
    {
        EASY, NORMAL, HARD
    }
}
