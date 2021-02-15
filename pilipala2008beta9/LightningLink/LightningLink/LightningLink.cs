using System;
using System.Collections.Generic;
using System.Data;

using WaterLibrary.MySQL;


namespace WaterLibrary.pilipala.Component
{
    public class LightningLink
    {
        private string LinkTable { get; init; }
        private MySqlManager MySqlManager { get; init; }
        private Dictionary<string, string> LinkCache { get; set; }
        private void RefreshCache()
        {
            LinkCache.Clear();
            //重建缓存
            foreach (DataRow Row in MySqlManager.GetTable($"SELECT * FROM {LinkTable}").Rows)
            {
                string LinkName = "<{" + Row["LinkName"].ToString() + "}>";
                LinkCache.Add(LinkName, Row["LinkText"].ToString());
            }
        }

        /// <summary>
        /// 私有构造
        /// </summary>
        private LightningLink() { }
        /// <summary>
        /// 工厂构造
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="MySqlManager"></param>
        internal LightningLink(string LinkTable, MySqlManager MySqlManager)
        {
            this.LinkTable = LinkTable;
            this.MySqlManager = MySqlManager;

            RefreshCache();
        }

        /// <summary>
        /// 新建链接
        /// </summary>
        /// <param name="LinkName">标记，即<{mark}>的mark文本</param>
        /// <param name="Text">文本，即被标记替换的内容</param>
        public void NewLink(string LinkName, string LinkText)
        {
            LinkName = "<{" + LinkName + "}>";
            MySqlManager.ExecuteInsert(LinkTable, new("LinkName", LinkName), new("LinkText", LinkText));
            LinkCache.Add(LinkName, LinkText);
        }
        /// <summary>
        /// 删除链接
        /// </summary>
        /// <param name="LinkName">标记</param>
        public void DelLink(string LinkName)
        {
            LinkName = "<{" + LinkName + "}>";
            MySqlManager.ExecuteDelete(LinkTable, ("LinkName", LinkName));
            LinkCache.Remove(LinkName);
        }

        /// <summary>
        /// 应用链接
        /// </summary>
        /// <param name="Text">含有链接的文本</param>
        /// <returns></returns>
        public string ApplyLink(string Text)
        {
            foreach (var el in LinkCache)
            {
                Text = Text.Replace(el.Key, el.Value);
            }
            return Text;
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
        public static LightningLink GenLightningLink(this ComponentFactory src) => new(CORE.TableCache["plugin__lightning_link"], CORE.MySqlManager);
    }
}
