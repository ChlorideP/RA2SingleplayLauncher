using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static BattleLauncher.Data.OverAll;

namespace BattleLauncher.Data
{
    public static class DDRAWUtils
    {
        public static void CleanAll()
        {
            var aqrit_cfg = new FileInfo(Path.Combine(MainFolder.FullName, "aqrit.cfg"));
            if (aqrit_cfg.Exists) aqrit_cfg.Delete();
            foreach (var item in MainFolder.GetFiles("*wine*.dll"))
                item.Delete();
            foreach (var item in MainFolder.GetFiles("ddraw.*"))
                item.Delete();
            foreach (var item in MainFolder.GetFiles("dxwnd.*"))
                item.Delete();
        }

        public static void Apply(DirectoryInfo ddrawFolder)
        {
            foreach (var file in ddrawFolder.GetFiles())
                file.CopyTo(Path.Combine(MainFolder.FullName, file.Name), true);
        }
    }
}
