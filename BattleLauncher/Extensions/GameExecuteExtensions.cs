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
            var CNCNET5_DLL = new FileInfo(Path.Combine(MainFolder.FullName, "cncnet5.dll"));
#if RELEASE
            if (!CNCNET5_DLL.SHA512Verify(Data.Hash.CNCNET5))
#elif DEBUG
            if (!CNCNET5_DLL.Exists)
#endif
                throw new SpawnerInvalidException();

            if (!AresExistence())
                throw new AresNotFoundException();

            var list = new List<string>
            {
                "\"GAMEMD.EXE\"",
                "-SPAWN",
                "-CD"
            };

            var proc = new Process();
            proc.StartInfo.FileName = Syringe.FullName;
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