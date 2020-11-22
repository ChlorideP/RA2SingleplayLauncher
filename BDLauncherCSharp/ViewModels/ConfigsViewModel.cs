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
