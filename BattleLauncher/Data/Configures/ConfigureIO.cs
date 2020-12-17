using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

using BattleLauncher.Data.Model;

using Shimakaze.Struct.Ini;
using Shimakaze.Struct.Ini.Utils;

namespace BattleLauncher.Data.Configures
{
    /// <summary>
    /// 负责游戏配置数据读写的类
    /// </summary>
    public class ConfigureIO
    {
        private readonly FileInfo file;
        public ConfigureIO(FileInfo file) => this.file = file;

        /// <summary>
        /// 从文件中加载所需配置到内存
        /// </summary>
        /// <exception cref="FileNotFoundException">当所需要的配置文件不存在时引发此异常</exception>
        /// <returns>不包含渲染器的游戏配置信息</returns>
        public async Task<GameConfigure> GetConfigure()
        {
            if (!file.Exists)
            {
                //default val
                return new GameConfigure
                {
                    ScreenWidth = (ushort)SystemParameters.PrimaryScreenWidth,
                    ScreenHeight = (ushort)SystemParameters.PrimaryScreenHeight,
                    IsWindowed = false,
                    NoBorder = false,
                    BackBuffer = false,
                    Difficult = Difficult.NORMAL
                };
            }
            else
            {
                var doc = await IniDocumentUtils.ParseAsync(file.Open(FileMode.Open, FileAccess.Read, FileShare.Read));

                return new GameConfigure
                {
                    ScreenWidth = (ushort)doc["Video", "ScreenWidth", SystemParameters.PrimaryScreenWidth],
                    ScreenHeight = (ushort)doc["Video", "ScreenHeight", SystemParameters.PrimaryScreenHeight],

                    IsWindowed = (bool)doc["Video", "Video.Windowed"],
                    NoBorder = (bool)doc["Video", "NoWindowFrame"],
                    BackBuffer = (bool)doc["Video", "VideoBackBuffer"],

                    Difficult = (Difficult)Enum.Parse(typeof(Difficult), doc["Options", "Difficulty", 0], true)
                };
            }
        }
        /// <summary>
        /// 将改动写入到文件
        /// </summary>
        public async Task SetConfigure(GameConfigure configure)
        {
            IIniDocument iniDocuments;
            using (var fs = file.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
                iniDocuments = await IniDocumentUtils.ParseAsync(fs);
            iniDocuments["Video", "ScreenWidth"] = configure.ScreenWidth;
            iniDocuments["Video", "ScreenHeight"] = configure.ScreenHeight;

            iniDocuments["Video", "Video.Windowed"] = configure.IsWindowed;
            iniDocuments["Video", "NoWindowFrame"] = configure.NoBorder;
            iniDocuments["Video", "VideoBackBuffer"] = configure.BackBuffer;

            iniDocuments["Options", "Difficulty"] = (byte)configure.Difficult;

            file.Delete();
            using (var fs = file.Open(FileMode.Create, FileAccess.Write, FileShare.Read))
                await iniDocuments.DeparseAsync(fs);
        }
    }
}
