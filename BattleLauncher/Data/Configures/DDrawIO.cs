using System.IO;
using System.Threading.Tasks;

using BattleLauncher.Data.Model;

using Shimakaze.Struct.Ini;

namespace BattleLauncher.Data.Configures
{
    public class DDrawIO
    {
        private readonly FileInfo file;
        public DDrawIO(FileInfo file) => this.file = file;

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
                    IsFullScreen = (bool)iniDocuments.TryGet("ddraw", "fullscreen"),
                    IsWindowed = (bool)iniDocuments.TryGet("ddraw", "windowed"),
                    NoBorder = (bool)iniDocuments.TryGet("ddraw", "border"),
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
            iniDocuments.Put("ddraw", "fullscreen", !configure.IsWindowed);
            iniDocuments.Put("ddraw", "windowed", configure.IsWindowed);
            iniDocuments.Put("ddraw", "border", configure.NoBorder);

            file.Delete();//so why need to generate a new one(
            using (var fs = file.Open(FileMode.Create, FileAccess.Write, FileShare.Read))
                await iniDocuments.DeparseAsync(fs);
        }
    }
}
