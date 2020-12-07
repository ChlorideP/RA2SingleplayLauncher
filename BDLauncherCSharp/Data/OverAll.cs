using System;
using System.IO;

namespace BDLauncherCSharp.Data
{
    /// <summary>
    /// 全局类
    /// </summary>
    public static class OverAll
    {
        public static string MainPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string SpawnIni = Path.Combine(MainPath, "spawn.ini");
        public static DirectoryInfo SavedGameDirectory { get; } = new DirectoryInfo(Path.Combine(MainPath, "SaveData"));
        public const string GameMD = "GAMEMD.EXE";
        public const string AresMainFunc = "Ares.DLL";
        public const string AresInjector = "Syringe.EXE";
        public const string CNCD = "4F6B722C4800815A6E5D3BADB625FD1A4EB4068BFD67C6761EF7435E5EBF7FE317A240503719ECBB3BA57B30F8F3F7BB4EC369C8C8BD2B53224E9A3084916E4B";
        public const string CNCNET5 = "EC1C3976697D3C7755259A31E33B8D1E072FE1DD07D4B24251458EDC858C410C4A43AC3AB9C75F295D19ADE94C278BCB1FB20FD309A09C051610F895806D6503";
    }
}
