using System;

using WaterLibrary.MySQL;


namespace WaterLibrary.pilipala.Component
{
    public class LightningLink
    {
        private string Table { get; init; }
        private MySqlManager MySqlManager { get; init; }

        /// <summary>
        /// 私有构造
        /// </summary>
        private LightningLink() { }
        /// <summary>
        /// 工厂构造
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="MySqlManager"></param>
        internal LightningLink(string Table, MySqlManager MySqlManager)
        {
            this.Table = Table;
            this.MySqlManager = MySqlManager;
        }

        /// <summary>
        /// 新建链接
        /// </summary>
        /// <param name="Identifier">标记，即<{mark}>的mark文本</param>
        /// <param name="Text">文本，即被标记替换的内容</param>
        public void NewLink(string Identifier, string Text)
        {

        }
        /// <summary>
        /// 删除链接
        /// </summary>
        /// <param name="Identifier">标记</param>
        public void DelLink(string Identifier)
        {

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
