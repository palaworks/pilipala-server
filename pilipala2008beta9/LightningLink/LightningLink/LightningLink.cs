using System;
using System.IO;
using System.Collections.Generic;
using System.Data;

using Newtonsoft.Json.Linq;
using WaterLibrary.MySQL;


namespace WaterLibrary.pilipala.Component
{
    public class LightningLink
    {
        private readonly Dictionary<string, string> cache = new();
        private readonly string path = "";
        /// <summary>
        /// 工厂构造
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="MySqlManager"></param>
        internal LightningLink()
        {

        }

        /// <summary>
        /// 新建链接
        /// </summary>
        /// <param name="LinkName">标记，即<{mark}>的mark文本</param>
        /// <param name="Text">文本，即被标记替换的内容</param>
        public void NewLink(string LinkName, string LinkText)
        {
            ReadJson(path, out JObject jObject);
            jObject[LinkName] = LinkText;
            WriteJson(path, jObject);

            cache[LinkName] = LinkText;
        }
        /// <summary>
        /// 删除链接
        /// </summary>
        /// <param name="LinkName">标记</param>
        public void DelLink(string LinkName)
        {
            ReadJson(path, out JObject jObject);
            jObject.Remove(LinkName);
            WriteJson(path, jObject);

            cache.Remove(LinkName);
        }

        /// <summary>
        /// 应用链接
        /// </summary>
        /// <param name="Text">含有链接的文本</param>
        /// <returns></returns>
        public string ApplyLink(string Text)
        {
            foreach (var el in cache)
            {
                Text = Text.Replace("<{" + el.Key + "}>", el.Value);
            }
            return Text;
        }

        /// <summary>
        /// 读JSON
        /// </summary>
        /// <param name="path">绝对位置</param>
        /// <param name="jObject"></param>
        private static void ReadJson(string path, out JObject jObject)
        {
            string jsonString = File.ReadAllText(path, System.Text.Encoding.UTF8);
            jObject = JObject.Parse(jsonString);
        }
        /// <summary>
        /// 写JSON
        /// </summary>
        /// <param name="path">绝对位置</param>
        /// <param name="jObject"></param>
        private static void WriteJson(string path, JObject jObject)
        {
            string jsonString = Convert.ToString(jObject);
            File.WriteAllText(path, jsonString, System.Text.Encoding.UTF8);
        }
    }

    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        /// 生成评论湖组件
        /// </summary>
        /// <returns></returns>
        public static LightningLink GenLightningLink(this ComponentFactory src) => new();
    }
}
