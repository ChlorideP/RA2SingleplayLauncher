using System;
using System.Windows.Markup;

namespace BDLauncherCSharp.Extensions
{
    /// <summary>
    /// 本地化资源标记扩展
    /// </summary>
    public class L10NExtension : MarkupExtension
    {
        /// <summary>
        /// 本地化资源键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 空构造方法
        /// </summary>
        public L10NExtension() { }

        /// <summary>
        /// 获取本地化资源
        /// </summary>
        /// <param name="key">资源键</param>
        public L10NExtension(string key) : this() => Key = key;

        public override object ProvideValue(IServiceProvider serviceProvider) => Key.I18N();
    }
}
