using System;

using WaterLibrary.MySQL;


namespace WaterLibrary.pilipala.Component
{
    public class LightningLink
    {
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

        }
    }

    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// 生成评论湖组件
        /// </summary>
        /// <returns></returns>
        public static LightningLink GenLightningLink(this ComponentFactory src) => new(src.PluginTables["plugin__lightning_link"], src.MySqlManager);
    }
}
