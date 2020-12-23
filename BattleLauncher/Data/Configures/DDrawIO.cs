using System.IO;
using System.Threading.Tasks;

using BattleLauncher.Data.Model;

using Shimakaze.Struct.Ini;
using Shimakaze.Struct.Ini.Utils;

namespace BattleLauncher.Data.Configures
{
    public class DDrawIO
    {
        private readonly FileInfo file;
        public DDrawIO(FileInfo file) => this.file = file;

        /// <summary>
        /// 从文件中加载所需配置到内存
        /// </summary>
        public async Task<RendererConfigure> GetConfigure()
        {
            if (!file.Exists)
            {
                return new RendererConfigure
                {
                    IsFullScreen = true,
                    IsWindowed = false,
                    NoBorder = false
                };
            }
            else
            {
                var doc = await IniDocumentUtils.ParseAsync(file.Open(FileMode.Open, FileAccess.Read, FileShare.Read));
                return new RendererConfigure
                {
                    IsFullScreen = (bool)doc["ddraw", "fullscreen"],
                    IsWindowed = (bool)doc["ddraw", "windowed"],
                    NoBorder = !(bool)doc["ddraw", "border"],
                };
            }
        }
        /// <summary>
        /// 将改动写入到文件
        /// </summary>
        public async Task SetConfigure(RendererConfigure configure)
        {
            IIniDocument doc;
            using (var fs = file.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
                doc = await IniDocumentUtils.ParseAsync(fs);
            doc["ddraw", "fullscreen"] = configure.IsFullScreen;
            doc["ddraw", "windowed"] = configure.IsWindowed;
            doc["ddraw", "border"] = !configure.NoBorder;

            file.Delete();//so why need to generate a new one(
            using (var fs = file.Open(FileMode.Create, FileAccess.Write, FileShare.Read))
                await doc.DeparseAsync(fs);
        }
    }
}
