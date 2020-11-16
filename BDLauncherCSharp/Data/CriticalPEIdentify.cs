using System;
using System.IO;
using System.Security.Cryptography;

namespace BDLauncherCSharp.Data
{
    class CriticalPEIdentify
    {
        public static bool IsBDFilelist = false;
        public static bool FuncExistence = File.Exists(Path.Combine(OverAll.MainPath, OverAll.AresMainFunc));
        public static bool InjExistence = File.Exists(Path.Combine(OverAll.MainPath, OverAll.AresInjector));

        public static void SpawnerHash(string k)
        {
            var DllPath = new DirectoryInfo(k);
            var sha512 = new SHA512CryptoServiceProvider();
            foreach (var l in DllPath.GetFiles("*.dll"))
            {
                byte[] hashCodeRaw = sha512.ComputeHash(l.OpenRead());
                string hashCode = BitConverter.ToString(hashCodeRaw).Replace("-", string.Empty);
                if (hashCode == "EC1C3976697D3C7755259A31E33B8D1E072FE1DD07D4B24251458EDC858C410C4A43AC3AB9C75F295D19ADE94C278BCB1FB20FD309A09C051610F895806D6503")
                {
                    IsBDFilelist = true;
                    break;
                }
            }
        }

        public static bool IsThereAres = FuncExistence & InjExistence;
    }
}
