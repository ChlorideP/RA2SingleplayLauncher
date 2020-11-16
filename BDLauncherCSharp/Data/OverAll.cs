using System;
using System.IO;

namespace BDLauncherCSharp.Data
{
    /// <summary>
    /// 全局类
    /// </summary>
    public static class OverAll
    {
        public static string MainPath = Environment.CurrentDirectory;
        public static string SpawnIni = Path.Combine(MainPath, "spawn.ini");
        public static DirectoryInfo SavedGameDirectory { get; } = new DirectoryInfo(Path.Combine(MainPath, "SaveData"));
        public const string GameMD = "GAMEMD.EXE";
        public const string AresMainFunc = "Ares.DLL";
        public const string AresInjector = "Syringe.EXE";
    }
}
