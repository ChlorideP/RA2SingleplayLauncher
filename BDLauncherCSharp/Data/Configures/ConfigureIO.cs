using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

using BDLauncherCSharp.Data.Model;

using Shimakaze.Struct.Ini;

namespace BDLauncherCSharp.Data.Configures
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
                var iniDocuments = await IniDocumentHelper.ParseAsync(file.Open(FileMode.Open, FileAccess.Read, FileShare.Read));

                return new GameConfigure
                {
                    ScreenWidth = (ushort)iniDocuments.TryGet("Video", "ScreenWidth", SystemParameters.PrimaryScreenWidth),
                    ScreenHeight = (ushort)iniDocuments.TryGet("Video", "ScreenHeight", SystemParameters.PrimaryScreenHeight),

                    IsWindowed = (bool)iniDocuments.TryGet("Video", "Video.Windowed"),
                    NoBorder = (bool)iniDocuments.TryGet("Video", "NoWindowFrame"),
                    BackBuffer = (bool)iniDocuments.TryGet("Video", "VideoBackBuffer"),

                    Difficult = (Difficult)Enum.Parse(typeof(Difficult), iniDocuments.TryGet("Options", "Difficulty", 0), true)
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
                iniDocuments = await IniDocumentHelper.ParseAsync(fs);
            iniDocuments.Put("Video", "ScreenWidth", configure.ScreenWidth);
            iniDocuments.Put("Video", "ScreenHeight", configure.ScreenHeight);

            iniDocuments.Put("Video", "Video.Windowed", configure.IsWindowed);
            iniDocuments.Put("Video", "NoWindowFrame", configure.NoBorder);
            iniDocuments.Put("Video", "VideoBackBuffer", configure.BackBuffer);

            iniDocuments.Put("Options", "Difficulty", (byte)configure.Difficult);

            file.Delete();
            using (var fs = file.Open(FileMode.Create, FileAccess.Write, FileShare.Read))
                await iniDocuments.DeparseAsync(fs);
        }
    }
}
