using System;
using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;

using Shimakaze.Struct.Ini;
using Shimakaze.Struct.Ini.Utils;

namespace BattleLauncher.Data
{
    /// <summary>
    /// 全局类
    /// </summary>
    public static class OverAll
    {
        private const string CONFIGS_FOLDER_PATH = @"Resources\Configs";
        private const string GLOBAL_CONFIGS_NAME = "Config.conf";

        public static FileInfo Syringe { get; } = new FileInfo(Path.Combine(MainFolder.FullName, "Syringe.exe"));
        public static FileInfo Ares_DLL { get; } = new FileInfo(Path.Combine(MainFolder.FullName, "Ares.dll"));
        public static FileInfo Lobos_DLL { get; } = new FileInfo(Path.Combine(MainFolder.FullName, "Phobos.dll"));
        public static FileInfo SpawnIni { get; } = new FileInfo(Path.Combine(MainFolder.FullName, "spawn.ini"));
        public static FileInfo CNCNET5_DLL { get; }

        public static DirectoryInfo ArchiveFolder { get; } = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SaveData"));
        public static DirectoryInfo MainFolder { get; } = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

        public static IIniDocument GlobalConfig { get; private set; }

        public static bool ExtExistence()
        {
            Ares_DLL.Refresh();
            Lobos_DLL.Refresh();
            Syringe.Refresh();
            return Ares_DLL.Exists & Lobos_DLL.Exists & Syringe.Exists;
        }

        public static async Task OverAll_Initialize()
        {
            if (!ArchiveFolder.Exists)
                ArchiveFolder.Create();

            if (SpawnIni.Exists)
                SpawnIni.Delete();

            if (!Directory.Exists(CONFIGS_FOLDER_PATH))
                Directory.CreateDirectory(CONFIGS_FOLDER_PATH);

            GlobalConfig = await IniDocumentUtils.ParseAsync(
                File.Open(
                    Path.Combine(CONFIGS_FOLDER_PATH, GLOBAL_CONFIGS_NAME),
                    FileMode.OpenOrCreate))
                .ConfigureAwait(false);
        }

        public static async Task SaveGlobalConfig()
        {
            await GlobalConfig.DeparseAsync(
                File.Open(
                    Path.Combine(CONFIGS_FOLDER_PATH, GLOBAL_CONFIGS_NAME),
                    FileMode.Create))
                .ConfigureAwait(false);
        }

        public static bool IsAdministrator()
        {
            using (var identity = WindowsIdentity.GetCurrent())
                return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
