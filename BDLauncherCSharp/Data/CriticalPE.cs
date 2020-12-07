using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

using BDLauncherCSharp.Extensions;

namespace BDLauncherCSharp.Data
{
    class CriticalPE
    {
        public static bool Func => File.Exists(Path.Combine(OverAll.MainPath, OverAll.AresMainFunc));
        public static bool Injector => File.Exists(Path.Combine(OverAll.MainPath, OverAll.AresInjector));
        public static bool AresExistence => Func & Injector;

        public static bool IsBDSpawner => FileHashIdentify.SHA512Verify("cncnet5.dll", OverAll.CNCNET5);
    }
}
