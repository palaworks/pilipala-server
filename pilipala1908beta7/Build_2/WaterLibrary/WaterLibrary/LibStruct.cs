using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibFrame;
using LibStruct.Interface;
using dataUnit;
using businessUnit.pilipala;

namespace LibStruct
{
    namespace Interface
    {
        /// <summary>
        /// 索引表数据接口
        /// </summary>
        public interface IPaTextIndex
        {

            /// <summary>
            /// 索引
            /// </summary>
            int text_id { get; set; }
            /// <summary>
            /// GUID标识
            /// </summary>
            string text_guid { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            DateTime time_set { get; set; }
            /// <summary>
            /// 展示模式
            /// </summary>
            string text_mode { get; set; }
            /// <summary>
            /// 文本类型
            /// </summary>
            string text_type { get; set; }
            /// <summary>
            /// 持有用户
            /// </summary>
            string text_user { get; set; }
            
        }
        /// <summary>
        /// 迭代表数据接口
        /// </summary>
        public interface IPaTextMain
        {

            /// <summary>
            /// GUID标识
            /// </summary>
            string text_guid { get; set; }
            /// <summary>
            /// 修改时间
            /// </summary>
            DateTime time_change { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            string text_title { get; set; }
            /// <summary>
            /// 摘要
            /// </summary>
            string text_summary { get; set; }
            /// <summary>
            /// 正文
            /// </summary>
            string text_content { get; set; }
            /// <summary>
            /// 文本归档
            /// </summary>
            string text_archiv { get; set; }
            /// <summary>
            /// 文本标签
            /// </summary>
            string text_tag { get; set; }

        }
        /// <summary>
        /// 参数表数据接口
        /// </summary>
        public interface IPaTextSub
        {

            /// <summary>
            /// 索引
            /// </summary>
            int text_id { get; set; }
            /// <summary>
            /// 浏览计数
            /// </summary>
            int count_pv { get; set; }
            /// <summary>
            /// 评论计数
            /// </summary>
            int count_comment { get; set; }
            /// <summary>
            /// 星星计数
            /// </summary>
            int count_star { get; set; }
            /// <summary>
            /// 封面链接
            /// </summary>
            string before_html { get; set; }

        }

        /// <summary>
        /// 用户数据接口
        /// </summary>
        public interface IPaUser
        {
            /// <summary>
            /// 权限ID
            /// </summary>
            int root_id { get; set; }
            /// <summary>
            /// 权限定义者
            /// </summary>
            string root_definer { get; set; }
            /// <summary>
            /// 站点开发者模式状态
            /// </summary>
            bool site_debug { get; set; }
            /// <summary>
            /// 站点可用（该值用于关闭站点）
            /// </summary>
            bool site_access { get; set; }
            /// <summary>
            /// 站点URL
            /// </summary>
            string site_url { get; set; }
            /// <summary>
            /// 站点标题
            /// </summary>
            string site_title { get; set; }
            /// <summary>
            /// 站点概要
            /// </summary>
            string site_summary { get; set; }
        }

        /// <summary>
        /// 文本基本数据接口
        /// </summary>
        public interface ITextBasic
        {
            /// <summary>
            /// 索引
            /// </summary>
            int text_id { get; set; }
            /// <summary>
            /// 文本
            /// </summary>
            string val { get; set; }
        }
    }

    /// <summary>
    /// Json本库信息
    /// </summary>
    public class JlinfObject : ILibFrameInfo
    {
        //主要信息
        /// <summary>
        /// 版本号
        /// </summary>
        public int projectVer
        {
            get; set;
        }
        /// <summary>
        /// 版本名字对象
        /// </summary>
        public string projectMoniker
        {
            get; set;
        }
        /// <summary>
        /// 版本类型
        /// </summary>
        public string editionType
        {
            get; set;
        }
        /// <summary>
        /// 步进
        /// </summary>
        public string stepping
        {
            get; set;
        }
        /// <summary>
        /// 类库的目标框架
        /// </summary>
        public string targetFramework
        {
            get; set;
        }
        /// <summary>
        /// 类库的目标框架名字对象
        /// </summary>
        public string targetFrameworkMoniker
        {
            get; set;
        }
        /// <summary>
        /// 针对最近一次发行版的全局兼容性
        /// </summary>
        public bool compat
        {
            get; set;
        }
        /// <summary>
        /// 适用平台
        /// </summary>
        public string platform
        {
            get; set;
        }

        //次要信息
        /// <summary>
        /// 架构名
        /// </summary>
        public string architecture
        {
            get; set;
        }
        /// <summary>
        /// 开发代号
        /// </summary>
        public string developmentCode
        {
            get; set;
        }
        /// <summary>
        /// 版本概要
        /// </summary>
        public string summary
        {
            get; set;
        }
        /// <summary>
        /// 是否为最新pub版本
        /// </summary>
        public bool isNewVer
        {
            get; set;
        }
        /// <summary>
        /// 最新pub版本下载URL
        /// </summary>
        public string newVerURL
        {
            get; set;
        }
    }

    /// <summary>
    /// XML描述结构
    /// </summary>
    public struct XmlSign
    {
        /// <summary>
        /// 节点地址，如父节点、实节点、子节点的地址，用于XmlCreater类中除reStream、CreateXml方法外的所有方法
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 被创建的Xml文档的文件地址，用于XmlCreater类的CreateXml方法
        /// </summary>
        public string fileName { get; set; }
        /// <summary>
        /// 被创建的Xml文档的文件名，用于XmlCreater类的CreateXml方法
        /// </summary>
        public string xmlName { get; set; }
        /// <summary>
        /// 被创建的Xml文档的根元素名，用于XmlCreater类的CreateXml方法
        /// </summary>
        public string rootName { get; set; }
        /// <summary>
        /// 节点名，可表示子节点、父节点、新建空\实节点名，用于XmlCreater类的AddRealNode、AddEmptyNode、RemoveNode方法
        /// </summary>
        public string nodeName { get; set; }
        /// <summary>
        /// 节点的属性名，用于XmlCreater类的AddRealNode、ReadAtt方法
        /// </summary>
        public string attName { get; set; }
        /// <summary>
        /// 节点的属性值，用于XmlCreater类的AddRealNode方法
        /// </summary>
        public string attValue { get; set; }
        /// <summary>
        /// 节点的子文本，用于XmlCreater类的AddRealNode方法
        /// </summary>
        public string val { get; set; }
        /// <summary>
        /// 读取类型，可选值有"_name"、"_value"，用于XmlCreater类的ReadNode方法
        /// </summary>
        public string type { get; set; }
    }

    namespace MySql
    {
        /// <summary>
        /// 键（用于键匹配查询）
        /// </summary>
        public struct mysqlKey
        {
            /// <summary>
            /// 数据库名
            /// </summary>
            public string dataBase { get; set; }
            /// <summary>
            /// 表名
            /// </summary>
            public string table { get; set; }

            /// <summary>
            /// 主键名
            /// </summary>
            public string primaryKeyName { get; set; }
            /// <summary>
            /// 主键值
            /// </summary>
            public string primaryKeyValue { get; set; }
        }
        /// <summary>
        /// 连接信息
        /// </summary>
        public struct mysqlConn
        {
            /// <summary>
            /// 数据源
            /// </summary>
            public string dataSource { get; set; }
            /// <summary>
            /// 端口
            /// </summary>
            public string port { get; set; }
            /// <summary>
            /// 用户名
            /// </summary>
            public string user { get; set; }
            /// <summary>
            /// 用户名对应的密码
            /// </summary>
            public string password { get; set; }
        }
        /// <summary>
        /// 参数（用于参数化查询添加参数）
        /// </summary>
        public struct mysqlParm
        {
            /// <summary>
            /// 参数名
            /// </summary>
            public string parmName { get; set; }
            /// <summary>
            /// 参数值
            /// </summary>
            public object parmValue { get; set; }
        }
    }

    namespace pilipala
    {
        /// <summary>
        /// 啪啦文本数据
        /// </summary>
        public struct PaText : IPaTextIndex, IPaTextMain, IPaTextSub
        {
            /*文章索引*/
            /// <summary>
            /// 
            /// </summary>
            public int text_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string text_guid { get; set; }

            /*文章模式*/
            /// <summary>
            /// 
            /// </summary>
            public string text_mode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string text_type { get; set; }

            /*文章体*/
            /// <summary>
            /// 
            /// </summary>
            public string text_title { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string text_summary { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string text_content { get; set; }

            /*分类数据*/
            /// <summary>
            /// 
            /// </summary>
            public string text_user { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string text_archiv { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string text_tag { get; set; }

            /*时间数据*/
            /// <summary>
            /// 
            /// </summary>
            public DateTime time_set { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public DateTime time_change { get; set; }

            /*计数数据*/
            /// <summary>
            /// 
            /// </summary>
            public int count_pv { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int count_comment { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int count_star { get; set; }

            /*其他*/
            /// <summary>
            /// 
            /// </summary>
            public string before_html { get; set; }
        }
        /// <summary>
        /// 用户数据
        /// </summary>
        public struct PaUser : IPaUser
        {
            /// <summary>
            /// 
            /// </summary>
            public int root_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string root_definer { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public bool site_debug { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public bool site_access { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string site_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string site_title { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string site_summary { get; set; }
        }
        /// <summary>
        /// 啪啦数据库信息
        /// </summary>
        public struct PaDB : IPaRoot
        {
            /// <summary>
            /// Pala数据表所在数据库
            /// </summary>
            public string dataBase { get; set; }
            /// <summary>
            /// 文本表
            /// </summary>
            public PaRoot.PalaSysTables Tables { get; set; }
            /// <summary>
            /// 文本视图
            /// </summary>
            public PaRoot.PalaSysViews Views { get; set; }
            /// <summary>
            /// 数据库管理器实例
            /// </summary>
            public MySqlConnH MySqlConnH { get; set; }
        }

        /// <summary>
        /// 文章标题
        /// </summary>
        public struct TextTitle : ITextBasic
        {
            /// <summary>
            /// 索引
            /// </summary>
            public int text_id { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string val { get; set; }
        }
        /// <summary>
        /// 文章摘要
        /// </summary>
        public struct TextSummary : ITextBasic
        {
            /// <summary>
            /// 索引
            /// </summary>
            public int text_id { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string val { get; set; }
        }
        /// <summary>
        /// 文章内容
        /// </summary>
        public struct TextContent : ITextBasic
        {
            /// <summary>
            /// 索引
            /// </summary>
            public int text_id { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string val { get; set; }
        }

        /// <summary>
        /// 文章类型
        /// </summary>
        public struct TextType : ITextBasic
        {
            /// <summary>
            /// 索引
            /// </summary>
            public int text_id { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string val { get; set; }
        }
        /// <summary>
        /// 文章归档
        /// </summary>
        public struct TextArchiv : ITextBasic
        {
            /// <summary>
            /// 索引
            /// </summary>
            public int text_id { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string val { get; set; }
        }
        /// <summary>
        /// 文章标签
        /// </summary>
        public struct TextTag : ITextBasic
        {
            /// <summary>
            /// 索引
            /// </summary>
            public int text_id { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string val { get; set; }
        }
    }
}