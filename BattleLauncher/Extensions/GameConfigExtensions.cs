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
        private static readonly FileInfo file = new FileInfo(Path.Combine(OverAll.MainFolder.FullName, "ra2md.ini"));

        public static async Task<GameConfig> ReadConfig(this GameConfig configure, string renderer_id)
            => RenderersManager.Renderers.TryGetValue(renderer_id, out var renderer) ? await configure.ReadConfig(renderer).ConfigureAwait(false) : configure;

        public static async Task<GameConfig> ReadConfig(this GameConfig configure, Renderer renderer)
        {
            if (!renderer.UseConfig ||
                !File.Exists(Path.Combine(OverAll.MainFolder.FullName, renderer.Config)))
                return configure;

            var fs = File.OpenRead(Path.Combine(OverAll.MainFolder.FullName, renderer.Config));
            var doc = await IniDocumentUtils.ParseAsync(fs);
            fs.Dispose();
            configure.IsFullScreen = (bool)doc[renderer.ConfigSection, renderer.ConfigKeyMap["full-screen"].Key];
            configure.IsWindowMode = (bool)doc[renderer.ConfigSection, renderer.ConfigKeyMap["window-mode"].Key];
            configure.Borderless = (bool)doc[renderer.ConfigSection, renderer.ConfigKeyMap["borderless"].Key];

            if (renderer.ConfigKeyMap["full-screen"].Reverse)
                configure.IsFullScreen = !configure.IsFullScreen;
            if (renderer.ConfigKeyMap["window-mode"].Reverse)
                configure.IsWindowMode = !configure.IsWindowMode;
            if (renderer.ConfigKeyMap["borderless"].Reverse)
                configure.Borderless = !configure.Borderless;

            return configure;
        }

        public static async Task WriteConfig(this GameConfig configure, string renderer_id)
        {
            if (RenderersManager.Renderers.TryGetValue(renderer_id, out var renderer))
                await configure.WriteConfig(renderer).ConfigureAwait(false);
            else
                await configure.WriteConfig();
        }

        public static async Task WriteConfig(this GameConfig configure, Renderer renderer)
        {
            await configure.WriteConfig();
            if (!renderer.UseConfig)
                return;

            var fs = File.OpenRead(Path.Combine(OverAll.MainFolder.FullName, renderer.Config));
            var doc = await IniDocumentUtils.ParseAsync(fs);
            fs.Dispose();
            doc[renderer.ConfigSection, renderer.ConfigKeyMap["full-screen"].Key]
                = renderer.ConfigKeyMap["full-screen"].Reverse
                ? !configure.IsFullScreen : configure.IsFullScreen;

            doc[renderer.ConfigSection, renderer.ConfigKeyMap["window-mode"].Key]
                = renderer.ConfigKeyMap["window-mode"].Reverse
                ? !configure.Borderless : configure.IsWindowMode;

            doc[renderer.ConfigSection, renderer.ConfigKeyMap["borderless"].Key]
                = renderer.ConfigKeyMap["borderless"].Reverse
                ? !configure.Borderless : configure.Borderless;

            fs = File.Open(Path.Combine(OverAll.MainFolder.FullName, renderer.Config), FileMode.Create);
            await doc.DeparseAsync(fs);
            fs.Dispose();
        }

        /// <summary>
        /// 从文件中加载所需配置到内存
        /// </summary>
        /// <exception cref="FileNotFoundException">当所需要的配置文件不存在时引发此异常</exception>
        /// <returns>不包含渲染器的游戏配置信息</returns>
        public static async Task<GameConfig> ReadConfig()
        {
            if (!file.Exists)
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
            else
            {
                var doc = await IniDocumentUtils.ParseAsync(file.OpenRead());

                return new GameConfig
                {
                    ScreenWidth = (ushort)doc["Video", "ScreenWidth", SystemParameters.PrimaryScreenWidth],
                    ScreenHeight = (ushort)doc["Video", "ScreenHeight", SystemParameters.PrimaryScreenHeight],

                    IsWindowMode = (bool)doc["Video", "Video.Windowed"],
                    Borderless = (bool)doc["Video", "NoWindowFrame"],
                    BackBuffer = (bool)doc["Video", "VideoBackBuffer"],

                    Difficult = (Difficult)Enum.Parse(typeof(Difficult), doc["Options", "Difficulty", 0], true)
                };
            }
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
