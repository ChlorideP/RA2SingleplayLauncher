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
        public const string CNCD = "41A62990D4EE78D57C8AB192127A4442F0C196D4EF6E094ECC9A1184101D522A2B9AAA1E3264C940015167BCC35AEF68496E180A94622E8ED07A4A01E434D550";
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

            DDrawIO = new DDrawIO(new FileInfo(Path.Combine(WorkDir.FullName, "ddraw.ini")));
            ConfigureIO = new ConfigureIO(new FileInfo(Path.Combine(WorkDir.FullName, "ra2md.ini")));
        }

        public static bool IsAdministrator()
        {
            using (var identity = WindowsIdentity.GetCurrent())
                return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
