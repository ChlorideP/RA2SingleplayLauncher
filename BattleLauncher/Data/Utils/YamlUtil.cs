using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BattleLauncher.Data.Utils
{
    internal static class YamlUtil
    {
        // TODO: 这个可以改成弱引用
        private static IDeserializer _deserializer = null;
        private static IDeserializer deserializer
            => _deserializer ?? (_deserializer =
                new DeserializerBuilder()
                    .WithNamingConvention(LowerCaseNamingConvention.Instance)
                    .Build());

        private static ISerializer _serializer = null;
        private static ISerializer serializer
            => _serializer ?? (_serializer =
                new SerializerBuilder()
                    .WithNamingConvention(LowerCaseNamingConvention.Instance)
                    .Build());

        public static T FromYaml<T>(this string yml) => deserializer.Deserialize<T>(yml);
        public static string ToYaml(this object @this) => serializer.Serialize(@this);
    }
}
