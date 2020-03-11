using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace ObjectCompareUtil
{
    /// <summary>
    /// 对比工具
    /// </summary>
    public static class CompareUtil
    {
        /// <summary>
        /// 该类型是否可直接进行值的比较
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsCanCompare(Type type)
        {
            if (type.IsValueType)
            {
                return true;
            }
            else
            {
                //String是特殊的引用类型，它可以直接进行值的比较
                if (type.FullName == typeof(string).FullName)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 获取新旧对象差异字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldObj">旧对象</param>
        /// <param name="newObj">新对象</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDiffs<T>(T oldObj, T newObj)
        {
            var diffs = new Dictionary<string, string>();
            try
            {
                var type = typeof(T);
                if (oldObj == null)
                {
                    oldObj = (T)Activator.CreateInstance(type);
                }
                if (newObj == null)
                {
                    newObj = (T)Activator.CreateInstance(type);
                }
                var properties = type.GetProperties();
                if (properties == null || !properties.Any())
                {
                    return diffs;
                }
                foreach (var property in properties)
                {
                    var attributes = property.GetCustomAttributesData();
                    if (attributes == null || !attributes.Any())
                    {
                        continue;
                    }
                    var findCompareDiffAttribute = attributes.Where(_ => _.AttributeType == typeof(CompareDiffAttribute))?.FirstOrDefault();
                    if (findCompareDiffAttribute != null)
                    {
                        var nameArgument = findCompareDiffAttribute.NamedArguments?.Where(_ => _.MemberName == "Name")?.FirstOrDefault();
                        if (nameArgument != null)
                        {
                            var oldValue = property.GetValue(oldObj);
                            var newValue = property.GetValue(newObj);
                            string key = nameArgument.Value.TypedValue.ToString().Trim('"');
                            string value = string.Empty;
                            if (IsCanCompare(property.PropertyType))
                            {
                                if (!Equals(oldValue, newValue))
                                {
                                    value = $"由【{oldValue}】更新为【{newValue}】";
                                    diffs.Add(key, value);
                                }
                            }
                            else
                            {
                                var oldValueJson = JsonConvert.SerializeObject(oldValue);
                                var newValueJson = JsonConvert.SerializeObject(newValue);
                                if (!string.Equals(oldValueJson, newValueJson))
                                {
                                    value = $"由【{oldValueJson}】更新为【{newValueJson}】";
                                    diffs.Add(key, value);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return diffs;
        }

        /// <summary>
        /// 获取新旧对象差异摘要文本
        /// </summary>
        /// <typeparam name="T">对象类型（可省略）</typeparam>
        /// <param name="oldObj">原对象</param>
        /// <param name="newObj">新对象</param>
        /// <returns></returns>
        public static string GetDiffSummary<T>(T oldObj, T newObj)
        {
            var diffSummary = new StringBuilder();
            var diffs = GetDiffs(oldObj, newObj);
            foreach (var item in diffs)
            {
                diffSummary.Append($"{item.Key}{item.Value};\n");
            }
            return diffSummary.ToString();
        }

        /// <summary>
        /// 获取新旧对象差异摘要文本
        /// </summary>
        /// <typeparam name="T">Json对象类型（不可省略）</typeparam>
        /// <param name="oldJson">原Json</param>
        /// <param name="oldJson">新Json</param>
        /// <returns></returns>
        public static string GetDiffSummary<T>(string oldJson, string newJson)
        {
            var oldObj = JsonConvert.DeserializeObject<T>(oldJson);
            var newObj = JsonConvert.DeserializeObject<T>(newJson);
            return GetDiffSummary<T>(oldObj, newObj);
        }
    }

    /// <summary>
    /// 对比差异特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CompareDiffAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
