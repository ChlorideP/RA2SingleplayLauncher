using System;
using System.IO;
using System.Security.Principal;

using BattleLauncher.Data.Configures;

namespace BattleLauncher.Data
{
    /// <summary>
    /// 全局类
    /// </summary>
    public static class OverAll
    {
        public const string CNCD = "4F6B722C4800815A6E5D3BADB625FD1A4EB4068BFD67C6761EF7435E5EBF7FE317A240503719ECBB3BA57B30F8F3F7BB4EC369C8C8BD2B53224E9A3084916E4B";
        public const string CNCNET5 = "77BAAAB986A05D9E81A662EC98384C3F57BB0765911E4DB0FC1689EA22B619D912BB10D36CF31F17B71C4A9F3B89A6D277F93F349AD93FAC8D41D13815F49E8A";

        public static FileInfo AresInjector { get; }
        public static FileInfo AresMainFunc { get; }

        public static DDrawIO DDrawIO { get; }
        public static ConfigureIO ConfigureIO { get; }

        public static DirectoryInfo SavedGameDirectory { get; }
        public static DirectoryInfo WorkDir { get; }

        public static FileInfo SpawnIni { get; }
        public static FileInfo CNCNET5DLL { get; }
        public static FileInfo DDRAWDLL { get; }

        public static bool AresExistence()
        {
            AresMainFunc.Refresh();
            AresInjector.Refresh();
            return AresMainFunc.Exists & AresInjector.Exists;
        }

        static OverAll()
        {
            WorkDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            SavedGameDirectory = WorkDir.CreateSubdirectory("SaveData");
            SpawnIni = new FileInfo(Path.Combine(WorkDir.FullName, "spawn.ini"));

            CNCNET5DLL = new FileInfo(Path.Combine(WorkDir.FullName, "cncnet5.dll"));
            DDRAWDLL = new FileInfo(Path.Combine(WorkDir.FullName, "ddraw.dll"));

            AresMainFunc = new FileInfo(Path.Combine(WorkDir.FullName, "Ares.DLL"));
            AresInjector = new FileInfo(Path.Combine(WorkDir.FullName, "Syringe.EXE"));

            DDrawIO = new DDrawIO(DDRAWDLL);
            ConfigureIO = new ConfigureIO(new FileInfo(Path.Combine(WorkDir.FullName, "ra2md.ini")));
        }

        public static bool IsAdministrator()
        {
            using (var identity = WindowsIdentity.GetCurrent())
                return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
