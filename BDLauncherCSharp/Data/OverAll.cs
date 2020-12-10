using System;
using System.IO;
using System.Security.Cryptography;

using BDLauncherCSharp.Data.Configures;
using BDLauncherCSharp.Extensions;

namespace BDLauncherCSharp.Data
{
    /// <summary>
    /// 全局类
    /// </summary>
    public static class OverAll
    {
        public const string CNCD = "4F6B722C4800815A6E5D3BADB625FD1A4EB4068BFD67C6761EF7435E5EBF7FE317A240503719ECBB3BA57B30F8F3F7BB4EC369C8C8BD2B53224E9A3084916E4B";
        public const string CNCNET5 = "EC1C3976697D3C7755259A31E33B8D1E072FE1DD07D4B24251458EDC858C410C4A43AC3AB9C75F295D19ADE94C278BCB1FB20FD309A09C051610F895806D6503";
        public static FileInfo AresInjector { get; }

        public static FileInfo AresMainFunc { get; }

        public static ConfigureIO ConfigureIO { get; }

        public static FileInfo GameMD { get; }

        public static DirectoryInfo SavedGameDirectory { get; }
        public static DirectoryInfo WorkDir { get; }

        public static FileInfo SpawnIni { get; }
        public static FileInfo CNCNET5DLL { get; }
        public static FileInfo DDRAWDLL { get; }
        public static bool AresExistence => OverAll.AresMainFunc.Exists & OverAll.AresInjector.Exists;


        public static bool IsCNCDDraw => SHA512Verify(DDRAWDLL, CNCD);
        public static string CurRenderer => IsCNCDDraw ? I18NExtension.I18N("cbRenderer.CNCDDraw") : I18NExtension.I18N("cbRenderer.None");

        static OverAll()
        {
            WorkDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            SavedGameDirectory = WorkDir.CreateSubdirectory("SaveData");
            SpawnIni = new FileInfo(Path.Combine(WorkDir.FullName, "spawn.ini"));

            CNCNET5DLL = new FileInfo(Path.Combine(WorkDir.FullName, "cncnet5.dll"));
            DDRAWDLL = new FileInfo(Path.Combine(WorkDir.FullName, "ddraw.dll"));

            GameMD = new FileInfo(Path.Combine(WorkDir.FullName, "GAMEMD.EXE"));
            AresMainFunc = new FileInfo(Path.Combine(WorkDir.FullName, "Ares.DLL"));
            AresInjector = new FileInfo(Path.Combine(WorkDir.FullName, "Syringe.EXE"));


            ConfigureIO = new ConfigureIO(new FileInfo(Path.Combine(WorkDir.FullName, "ra2md.ini")));
        }


        public static bool SHA512Verify(FileInfo file, string CorCode)
        {
            if (file.Exists)
            {
                using (var sha512 = SHA512.Create())
                using (FileStream fs = file.OpenRead())
                {
                    var buffer = sha512.ComputeHash(fs);
                    return BitConverter.ToString(buffer).Replace("-", string.Empty) == CorCode;
                }
            }
            else return false;
        }
    }
}
