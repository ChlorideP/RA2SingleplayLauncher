using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YamlDotNet.Serialization;

namespace BattleLauncher.Data.Model
{
    public class Renderer
    {
        public string Id { get; set; }

        public string Main { get; set; }

        public string Dir { get; set; }

        public string Config { get; set; }

        public List<string> Files { get; set; }

        [YamlMember(Alias = "use-config", ApplyNamingConventions = false)]
        public bool UseConfig { get; set; }


        [YamlMember(Alias = "config-section", ApplyNamingConventions = false)]
        public string ConfigSection { get; set; }

        [YamlMember(Alias = "config-key-maps", ApplyNamingConventions = false)]
        public Dictionary<string, ConfigKeyMap> ConfigKeyMap { get; set; }
    }

    public class ConfigKeyMap
    {
        public string Key { get; set; }
        public bool Reverse { get; set; }
    }
}
