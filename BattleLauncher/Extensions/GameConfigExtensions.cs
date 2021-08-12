using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

using BattleLauncher.Data;
using BattleLauncher.Data.Model;

using Shimakaze.Struct.Ini;
using Shimakaze.Struct.Ini.Utils;

namespace BattleLauncher.Extensions
{
    /// <summary>
    /// 负责游戏配置数据读写的类
    /// </summary>
    public static class GameConfigExtensions
    {
        private static readonly FileInfo ddraw = new FileInfo(Path.Combine(OverAll.MainFolder.FullName, "ddraw.ini"));
        private static readonly FileInfo dxwnd = new FileInfo(Path.Combine(OverAll.MainFolder.FullName, "dxwnd.ini"));
        private static readonly FileInfo file = new FileInfo(Path.Combine(OverAll.MainFolder.FullName, "ra2md.ini"));

        public static async Task<GameConfig> ReadCNCDDRAWConfig(this GameConfig configure)
        {
            ddraw.Refresh();
            if (ddraw.Exists)
            {
                var stream = ddraw.OpenRead();
                var doc = await IniDocumentUtils.ParseAsync(stream);
                configure.IsFullScreen = (bool)doc["ddraw", "fullscreen"];
                configure.IsWindowMode = (bool)doc["ddraw", "windowed"];
                configure.Borderless = !(bool)doc["ddraw", "border"];
                stream.Dispose();
            }
            else
            {
                configure.IsFullScreen = true;
                configure.IsWindowMode = false;
                configure.Borderless = false;
            }

            return configure;
        }

        public static async Task<GameConfig> ReadDxWndConfig(this GameConfig configure)
        {
            dxwnd.Refresh();
            if (dxwnd.Exists)
            {
                var stream = dxwnd.OpenRead();
                var doc = await IniDocumentUtils.ParseAsync(stream);
                configure.IsFullScreen = !(bool)doc["DxWnd", "RunInWindow"];
                configure.IsWindowMode = (bool)doc["DxWnd", "RunInWindow"];
                configure.Borderless = (bool)doc["DxWnd", "NoWindowFrame"];
                stream.Dispose();
            }
            else
            {
                configure.IsFullScreen = true;
                configure.IsWindowMode = false;
                configure.Borderless = false;
            }

            return configure;
        }

        /// <summary>
        /// 从文件中加载所需配置到内存
        /// </summary>
        /// <exception cref="FileNotFoundException">当所需要的配置文件不存在时引发此异常</exception>
        /// <returns>不包含渲染器的游戏配置信息</returns>
        public static async Task<GameConfig> ReadConfig()
        {
            file.Refresh();
            if (file.Exists)
            {
                var stream = file.OpenRead();
                var doc = await IniDocumentUtils.ParseAsync(stream);

                var ret = new GameConfig
                {
                    ScreenWidth = (ushort)doc["Video", "ScreenWidth", SystemParameters.PrimaryScreenWidth],
                    ScreenHeight = (ushort)doc["Video", "ScreenHeight", SystemParameters.PrimaryScreenHeight],

                    IsWindowMode = (bool)doc["Video", "Video.Windowed"],
                    Borderless = (bool)doc["Video", "NoWindowFrame"],
                    BackBuffer = (bool)doc["Video", "VideoBackBuffer"],

                    Difficult = (Difficult)Enum.Parse(typeof(Difficult), doc["Options", "Difficulty", 0], true)
                };

                stream.Dispose();
                return ret;
            }
            else
            {
                //default val
                return new GameConfig
                {
                    ScreenWidth = (ushort)SystemParameters.PrimaryScreenWidth,
                    ScreenHeight = (ushort)SystemParameters.PrimaryScreenHeight,
                    IsWindowMode = false,
                    Borderless = false,
                    BackBuffer = false,
                    Difficult = Difficult.NORMAL
                };
            }
        }
        public static async Task WriteCNCDDRAWConfig(this GameConfig configure)
        {
            IIniDocument doc;
            using (var fs = ddraw.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
                doc = await IniDocumentUtils.ParseAsync(fs);
            doc["ddraw", "fullscreen"] = configure.IsFullScreen;
            doc["ddraw", "windowed"] = configure.IsWindowMode;
            doc["ddraw", "border"] = !configure.Borderless;

            using (var fs = ddraw.Open(FileMode.Create, FileAccess.Write, FileShare.Read))
                await doc.DeparseAsync(fs);
        }

        public static async Task WriteDxWndConfig(this GameConfig configure)
        {
            IIniDocument doc;
            using (var fs = dxwnd.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
                doc = await IniDocumentUtils.ParseAsync(fs);
            doc["DxWnd", "RunInWindow"] = configure.IsWindowMode;
            doc["DxWnd", "NoWindowFrame"] = configure.Borderless;

            using (var fs = dxwnd.Open(FileMode.Create, FileAccess.Write, FileShare.Read))
                await doc.DeparseAsync(fs);
        }

        /// <summary>
        /// 将改动写入到文件
        /// </summary>
        public static async Task WriteConfig(this GameConfig configure)
        {
            IIniDocument iniDocuments;
            using (var fs = file.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
                iniDocuments = await IniDocumentUtils.ParseAsync(fs);
            iniDocuments["Video", "ScreenWidth"] = configure.ScreenWidth;
            iniDocuments["Video", "ScreenHeight"] = configure.ScreenHeight;

            iniDocuments["Video", "Video.Windowed"] = configure.IsWindowMode;
            iniDocuments["Video", "NoWindowFrame"] = configure.Borderless;
            iniDocuments["Video", "VideoBackBuffer"] = configure.BackBuffer;

            iniDocuments["Options", "Difficulty"] = (byte)configure.Difficult;

            using (var fs = file.Open(FileMode.Create, FileAccess.Write, FileShare.Read))
                await iniDocuments.DeparseAsync(fs);
        }
    }
}
