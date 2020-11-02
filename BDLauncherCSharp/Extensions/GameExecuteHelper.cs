using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BDLauncherCSharp.Controls;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;

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
            list.Add($"\"{MainWindow.GameMD}\"");
            list.Add("-SPAWN");

            var proc = new Process();
            proc.StartInfo.FileName = Path.Combine(MainWindow.MainPath, MainWindow.AresInjector);
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