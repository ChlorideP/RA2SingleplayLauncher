using System.Collections.Generic;
using System.Diagnostics;

using BDLauncherCSharp.Data.Model;

using static BDLauncherCSharp.Data.OverAll;

namespace BDLauncherCSharp.Extensions
{
    /// <summary>
    /// 启动游戏工具类 
    /// </summary>
    public static class GameExecuteHelper
    {
        public static void RunGame(this GameExecuteOptions options)
        {
            var list = new List<string>
            {
                $"\"{GameMD}\"",
                "-SPAWN"
            };

            var proc = new Process();
            proc.StartInfo.FileName = AresInjector.FullName;
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