using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using static BDLauncherCSharp.Data.OverAll;

namespace BDLauncherCSharp.Extensions
{
    /// <summary>
    /// 启动游戏工具类 
    /// </summary>
    public static class GameExecute
    {
        public static void RunGame(this Data.GameExecuteOptions options)
        {
            var list = new List<string>();
            list.Add($"\"{GameMD}\"");
            list.Add("-SPAWN");

            var proc = new Process();
            proc.StartInfo.FileName = Path.Combine(MainPath, AresInjector);
            if (options.RunAs)
                proc.StartInfo.Verb = "runas";
            if (options.LogMode)
                list.Add("-LOG");
            list.AddRange(options.Others);
            proc.StartInfo.Arguments = string.Join(" ", list);

            proc.Start();
        }
    }
}