using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using BattleLauncher.Data.Model;
using BattleLauncher.Exceptions;

using static BattleLauncher.Data.OverAll;

namespace BattleLauncher.Extensions
{
    /// <summary>
    /// 启动游戏工具类 
    /// </summary>
    public static class GameExecuteExtensions
    {
        public static void RunGame(this GameExecuteOptions options)
        {
            if (!SpawnerExists())
                throw new SpawnerInvalidException();

            if (!ExtExistence())
                throw new AresNotFoundException();

            var list = new List<string>
            {
                "\"GAMEMD.EXE\"",
                "-cd",
                "-hidewarning",
                "-name \"Braindead RL v1.14\""
            };

            var proc = new Process();
            proc.StartInfo.FileName = Syringe.FullName;
            if (options.RunAs)
                proc.StartInfo.Verb = "runas";
            if (options.LogMode)
                list.Add("-log");
            list.AddRange(options.Others);
            proc.StartInfo.Arguments = string.Join(" ", list);

            proc.Start();
        }
    }
}