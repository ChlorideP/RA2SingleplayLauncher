using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static BDLauncherCSharp.MainWindow;

namespace BDLauncherCSharp.Data
{
    /// <summary>
    /// 全局类
    /// </summary>
    public static class OverAll
    {
        /// <summary>
        /// 游戏存档目录 
        /// </summary>
        public static DirectoryInfo SavedGameDirectory { get; } = new DirectoryInfo(Path.Combine(MainPath, "SaveData"));

    }
}
