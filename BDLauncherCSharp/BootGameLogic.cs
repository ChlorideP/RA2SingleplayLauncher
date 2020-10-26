using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDLauncherCSharp.BootGameLogic
{
    public class GameExecuteOptions
    {
        public bool LogMode;
        public bool RunAs;
        public string[] Others;
    }

    public class GameExecute
    {
        public static void RunGame(GameExecuteOptions options)
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
