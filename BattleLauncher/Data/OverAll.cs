using System;
using System.IO;
using System.Security.Principal;

using BattleLauncher.Extensions;

namespace BattleLauncher.Data
{
    /// <summary>
    /// 全局类
    /// </summary>
    public static class OverAll
    {
        public static FileInfo Syringe { get; }
        public static FileInfo Ares_DLL { get; }
        public static FileInfo Lobos_DLL { get; }
        public static FileInfo CacoNyan_DLL { get; }
        public static FileInfo CnCNet5_DLL { get; }
        public static FileInfo SpawnIni { get; }

        public static DirectoryInfo ArchiveFolder { get; }
        public static DirectoryInfo MainFolder { get; }

        public static bool ExtExistence()
        {
            Ares_DLL.Refresh();
            Lobos_DLL.Refresh();
            Syringe.Refresh();
            return Ares_DLL.Exists & Lobos_DLL.Exists & Syringe.Exists;
        }

        public static bool SpawnerExists()
        {
            CnCNet5_DLL.Refresh();
            CacoNyan_DLL.Refresh();
#if RELEASE
            return CnCNet5_DLL.Exists & CacoNyan_DLL.SHA512Verify(Hash.CACONYAN);
#elif DEBUG
            return CnCNet5_DLL.Exists & CacoNyan_DLL.Exists;
#endif
        }

        static OverAll()
        {
            MainFolder = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            ArchiveFolder = MainFolder.CreateSubdirectory("Saved Games");

            SpawnIni = new FileInfo(Path.Combine(MainFolder.FullName, "spawn.ini"));

            Lobos_DLL = new FileInfo(Path.Combine(MainFolder.FullName, "Lobos.dll"));
            Ares_DLL = new FileInfo(Path.Combine(MainFolder.FullName, "Ares.dll"));
            Syringe = new FileInfo(Path.Combine(MainFolder.FullName, "Syringe.exe"));

            CnCNet5_DLL = new FileInfo(Path.Combine(MainFolder.FullName, "cncnet5.dll"));
            CacoNyan_DLL = new FileInfo(Path.Combine(MainFolder.FullName, "CacoNyan.dll"));
        }

        public static bool IsAdministrator()
        {
            using (var identity = WindowsIdentity.GetCurrent())
                return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
