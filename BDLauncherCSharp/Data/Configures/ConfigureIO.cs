using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        public ConfigureIO(FileInfo file)
        {
            this.file = file;
        }

        /// <summary>
        /// 从文件中加载所需配置到内存
        /// </summary>
        /// <exception cref="FileNotFoundException">当所需要的配置文件不存在时引发此异常</exception>
        /// <returns>不包含渲染器的游戏配置信息</returns>
        public async Task<GameConfigure> GetConfigure()
        {
            if (!file.Exists)
                throw new FileNotFoundException($"Cannot found file\"{file.FullName}\".");


            var iniDocuments = await IniDocumentHelper.ParseAsync(file.Open(FileMode.Open, FileAccess.Read, FileShare.Read));

            return new GameConfigure
            {
                ScreenWidth = iniDocuments["Video"]["ScreenWidth"]?.Value is IniValue width ? (ushort)width : (ushort)SystemParameters.PrimaryScreenWidth,// 这不是有获取分辨率的属性嘛
                ScreenHeight = iniDocuments["Video"]["ScreenHeight"]?.Value is IniValue height ? (ushort)height : (ushort)SystemParameters.PrimaryScreenHeight,

                IsWindowed = iniDocuments["Video"]["Video.Windowed"]?.Value is IniValue windowed && (bool)windowed,
                NoBorder = iniDocuments["Video"]["NoWindowFrame"]?.Value is IniValue noborder && (bool)noborder,
                BackBuffer = iniDocuments["Video"]["VideoBackBuffer"]?.Value is IniValue buffer && (bool)buffer,

                Difficult = iniDocuments["Options"]["Difficulty"]?.Value is IniValue difficult ? (Difficult)Enum.Parse(typeof(Difficult), difficult, true) : Difficult.EASY
            };
        }
        /// <summary>
        /// 将改动写入到文件
        /// </summary>
        public async Task SetConfigure(GameConfigure configure)
        {
            using (var fs = file.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                var iniDocuments = await IniDocumentHelper.ParseAsync(fs);
                iniDocuments["Video"]["ScreenWidth"].Value = configure.ScreenWidth;
                iniDocuments["Video"]["ScreenHeight"].Value = configure.ScreenHeight;

                iniDocuments["Video"]["Video.Windowed"].Value = configure.IsWindowed;
                iniDocuments["Video"]["NoWindowFrame"].Value = configure.NoBorder;
                iniDocuments["Video"]["VideoBackBuffer"].Value = configure.BackBuffer;

                iniDocuments["Options"]["Difficulty"].Value = (byte)configure.Difficult;

                fs.Seek(0, SeekOrigin.Begin);
                await iniDocuments.DeparseAsync(fs);
            }
        }

    }
}
