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
        public static void Clear()
        {
            foreach (var item in MainFolder.GetFiles("ddraw.*"))
                item.Delete();
        }
        public static void Apply(DirectoryInfo ddrawFolder)
        {
            foreach (var file in ddrawFolder.GetFiles())
                //可 扩 展 序 列
                file.CopyTo(Path.Combine(MainFolder.FullName, file.Name), true);
        }
    }
}
