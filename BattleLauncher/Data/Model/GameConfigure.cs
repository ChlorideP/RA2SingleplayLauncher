namespace BattleLauncher.Data.Model
{
    public class GameConfigure
    {
        public ushort ScreenWidth { get; set; }
        public ushort ScreenHeight { get; set; }

        public bool IsFull { get; set; }
        public bool IsWindowMode { get; set; }
        public bool Borderless { get; set; }
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
