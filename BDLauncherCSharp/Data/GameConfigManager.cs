using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shimakaze.Struct.Ini;

namespace BDLauncherCSharp.Data
{
    public class GameConfigManager
    {
        public static IIniDocument Configs { get; private set; } = null;
        public static FileInfo File { get; set; } = new FileInfo(Path.Combine(OverAll.MainPath, "ra2md.ini"));
        public static async Task<IIniDocument> LoadGameConfig() => Configs = File.Exists ? await IniDocumentHelper.ParseAsync(File.OpenRead()) : IniDocumentHelper.Create();

        public static async Task<IIniDocument> InitGameConfig() => Configs ?? await LoadGameConfig();
    }
}
