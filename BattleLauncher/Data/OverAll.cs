using System;
using System.IO;
using System.Security.Principal;

namespace BattleLauncher.Data
{
    /// <summary>
    /// 全局类
    /// </summary>
    public static class OverAll
    {
        public static FileInfo Syringe { get; }
        public static FileInfo Ares_DLL { get; }
        public static FileInfo SpawnIni { get; }
        public static FileInfo CNCNET5_DLL { get; }

        public static DirectoryInfo ArchiveFolder { get; }
        public static DirectoryInfo MainFolder { get; }


        public static bool AresExistence()
        {
            Ares_DLL.Refresh();
            Syringe.Refresh();
            return Ares_DLL.Exists & Syringe.Exists;
        }

        static OverAll()
        {
            MainFolder = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            ArchiveFolder =
#if DEBUG
                MainFolder.CreateSubdirectory("Saved Games");
#elif RELEASE
                MainFolder.CreateSubdirectory("SaveData");
#endif

            SpawnIni = new FileInfo(Path.Combine(MainFolder.FullName, "spawn.ini"));


            Ares_DLL = new FileInfo(Path.Combine(MainFolder.FullName, "Ares.DLL"));
            Syringe = new FileInfo(Path.Combine(MainFolder.FullName, "Syringe.EXE"));
        }

        public static bool IsAdministrator()
        {
            using (var identity = WindowsIdentity.GetCurrent())
                return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
