using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows;

namespace BDLauncherCSharp.Extensions
{
    /// <summary>
    /// 全球化扩展
    /// </summary>
    public static class I18NExtension
    {
        private static IEnumerable<string> names;
        private static ResourceManager resourceManager;

        /// <summary>
        /// 对应用进行初始化
        /// </summary>
        /// <param name="resourceMgr">资源管理</param>
        public static void I18NInitialize(this ResourceManager resourceMgr)
        {
            if (resourceMgr is null) throw new NullReferenceException($"this {nameof(ResourceManager)} is null");
            resourceManager = resourceMgr ?? String.Resource.ResourceManager;
            //resourceManager.GetString(string.Empty);
            names = resourceManager.GetResourceSet(new CultureInfo("en"), true, true)
                               ?.Cast<DictionaryEntry>()
                                .Select(item => item.Key as string);
        }

        /// <summary>
        /// 对控件进行初始化
        /// </summary>
        /// <param name="element">控件</param>
        public static void I18NInitialize(this DependencyObject element)
        {
            if (element is null) return;
            // 获取子元素
            foreach (var child in LogicalTreeHelper.GetChildren(element))
                (child as DependencyObject).I18NInitialize();

            if (element is UIElement target)
            {
                if (string.IsNullOrWhiteSpace(target.Uid)) return;

                foreach (var (name, property)
                    in names.Select(name => (name, prefx: $"{target.Uid}."))
                                          .Where(i => i.name?.StartsWith(i.prefx) ?? false)
                                          .Select(i => (i.name, i.name.Substring(i.prefx.Length))))
                {
                    var propertyInfo = element.GetType().GetProperty(property);

                    var i18nStr = I18N(name);

                    if (propertyInfo is null || string.IsNullOrWhiteSpace(i18nStr)) continue;


                    if (propertyInfo.PropertyType == typeof(string))
                        propertyInfo.SetValue(element, i18nStr);
                    else
                    {
                        var constructor = propertyInfo.PropertyType.GetConstructor(new[] { typeof(string) });
                        if (constructor is ConstructorInfo)
                            propertyInfo.SetValue(element, constructor.Invoke(new object[] { i18nStr }));
                        else
                        {
                            var method = propertyInfo.PropertyType.GetMethod("Parse",
                                BindingFlags.Static | BindingFlags.Public,
                                null, new[] { typeof(string) }, null);

                            if (method is MethodInfo)
                                propertyInfo.SetValue(element, method.Invoke(null, new object[] { i18nStr }));
                            else
                                propertyInfo.SetValue(element, i18nStr);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取对应的本地化资源
        /// </summary>
        /// <param name="key">资源键</param>
        /// <returns>本地化字符串</returns>
        public static string I18N(this string key) => resourceManager.GetString(key);
    }
}
