using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Shimakaze.Struct.Ini;

namespace BDLauncherCSharp.ViewModels
{
    public class ConfigsViewModel : INotifyPropertyChanged
    {
        public ConfigsViewModel(string path)
        {
            this.filePath = path;
            _ = LoadConfigs();
        }
        private IniDocument _ini;
        private string filePath;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        // 私有 通常只有创建对象的时候才会读取文件
        // TODO：未完成
        private async Task LoadConfigs()
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _ini = await IniDocument.ParseAsync(fs);
                var value = _ini["Section"]["Key"];
            }

        }
        /// <summary>
        /// 保存方法
        /// </summary>
        public async Task SaveConfigures()
        {
            /// 8.0写法：
            /// using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)
            /// using var sw = new StreamWriter(fs)
            /// 然后也不需要搞啥花括号（第二句using底下嵌套的都可以直接搬到外面）

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var sw = new StreamWriter(fs))
            {
                foreach (var section in _ini.Sections)
                {
                    await sw.WriteLineAsync($"[{section.Name}]");
                    foreach (var line in section.Content)
                    {
                        await sw.WriteLineAsync($"{line.Key}={line.Value}");
                    }
                    await sw.WriteLineAsync();
                }
                await sw.FlushAsync();
                await fs.FlushAsync();
            }
        }
    }
}
