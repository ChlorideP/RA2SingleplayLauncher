using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BDLauncherCSharp.Data.Model;

using Shimakaze.Struct.Ini;

namespace BDLauncherCSharp.Data.Configures
{
    public class DDrawIO
    {
        private readonly FileInfo file;
        public DDrawIO(FileInfo file)
        {
            this.file = file;
        }

        /// <summary>
        /// 从文件中加载所需配置到内存
        /// </summary>
        public async Task<GameConfigure> GetConfigure()
        {
            if (!file.Exists)
            {
                return new GameConfigure
                {
                    IsFullScreen = true,
                    IsWindowed = false,
                    NoBorder = false
                };
            }
            else
            {
                var iniDocuments = await IniDocumentHelper.ParseAsync(file.Open(FileMode.Open, FileAccess.Read, FileShare.Read));
                return new GameConfigure
                {
                    IsFullScreen = iniDocuments["ddraw"]["fullscreen"]?.Value is IniValue fullscreen && (bool)fullscreen,
                    IsWindowed = iniDocuments["ddraw"]["windowed"]?.Value is IniValue windowed && (bool)windowed,
                    NoBorder = iniDocuments["ddraw"]["border"]?.Value is IniValue noborder && (bool)noborder
                };
            }
        }
        /// <summary>
        /// 将改动写入到文件
        /// </summary>
        public async Task SetConfigure(GameConfigure configure)
        {
            using (var fs = file.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                var iniDocuments = await IniDocumentHelper.ParseAsync(fs);

                iniDocuments["ddraw"]["fullscreen"].Value = !configure.IsWindowed;
                iniDocuments["ddraw"]["windowed"].Value = configure.IsWindowed;
                iniDocuments["ddraw"]["border"].Value = configure.NoBorder;

                fs.Seek(0, SeekOrigin.Begin);
                await iniDocuments.DeparseAsync(fs);
            }
        }
    }
}
