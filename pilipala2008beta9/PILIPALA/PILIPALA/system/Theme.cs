using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using PILIPALA.Models;

namespace PILIPALA.system.Theme
{
    public class ThemeHandler
    {
        private string Path { get; set; }
        /// <summary>
        /// 标准构造
        /// </summary>
        /// <param name="ThemeModel">主题模型，存有主题配置信息</param>
        public ThemeHandler(ThemeModel ThemeModel)
        {
            Path = ThemeModel.Path;
        }
        /// <summary>
        /// 配置信息
        /// </summary>
        public JObject Config
        {
            get
            {
                string ConfigString = System.IO.File.ReadAllText(Path);
                return JObject.Parse(ConfigString);
            }
        }
    }
}
