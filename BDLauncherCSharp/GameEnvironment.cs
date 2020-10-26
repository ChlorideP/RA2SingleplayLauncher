using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static BDLauncherCSharp.MainWindow;

namespace BDLauncherCSharp.GameEnvironment
{
    public static class CheckGameEnvi
    {
        
        public static string SaveData = Path.Combine(MainPath, "SaveData");
        public static bool SaveDataDirExistence = Directory.Exists(SaveData);

        public static void CheckSaveDir()
        {
            if (!SaveDataDirExistence)
            {
                Directory.CreateDirectory(SaveData);
            }
        }
    }
}
