using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;//GDI+命名空间

namespace StdLib
{

    /// <summary>
    /// 连接数据库，structspace
    /// </summary>
    public struct connStr
    {
        #region 字段

        private string DataSource;
        private string Port;
        private string UserName;
        private string Password;

        #endregion

        #region 字段访问器

        /// <summary>
        /// 被连接的数据源
        /// </summary>
        public string dataSource
        {
            get { return DataSource; }
            set { DataSource = value; }
        }
        /// <summary>
        /// 被连接的端口
        /// </summary>
        public string port
        {
            get { return Port; }
            set { Port = value; }
        }
        /// <summary>
        /// 连接所使用的用户名
        /// </summary>
        public string userName
        {
            get { return UserName; }
            set { UserName = value; }
        }
        /// <summary>
        /// 连接所使用的密码
        /// </summary>
        public string password
        {
            get { return Password; }
            set { Password = value; }
        }

        #endregion
    }

    /// <summary>
    /// 操作xml文件，structspace
    /// </summary>
    public struct xmlStr
    {
        #region 字段

        private string Path;
        private string FileName;
        private string XmlName;
        private string RootName;
        private string NodeName;
        private string AttName;
        private string AttValue;
        private string InnerText;
        private string Type;

        #endregion

        #region 字段访问器

        /// <summary>
        /// 节点地址，如父节点、实节点、子节点的地址，用于XmlCreater类中除reStream、CreateXml方法外的所有方法
        /// </summary>
        public string path
        {
            get { return Path; }
            set { Path = value; }
        }
        /// <summary>
        /// 被创建的Xml文档的文件地址，用于XmlCreater类的CreateXml方法
        /// </summary>
        public string fileName
        {
            get { return FileName; }
            set { FileName = value; }
        }
        /// <summary>
        /// 被创建的Xml文档的文件名，用于XmlCreater类的CreateXml方法
        /// </summary>
        public string xmlName
        {
            get { return XmlName; }
            set { XmlName = value; }
        }
        /// <summary>
        /// 被创建的Xml文档的根元素名，用于XmlCreater类的CreateXml方法
        /// </summary>
        public string rootName
        {
            get { return RootName; }
            set { RootName = value; }
        }
        /// <summary>
        /// 节点名，可表示子节点、父节点、新建空\实节点名，用于XmlCreater类的AddRealNode、AddEmptyNode、RemoveNode方法
        /// </summary>
        public string nodeName
        {
            get { return NodeName; }
            set { NodeName = value; }
        }
        /// <summary>
        /// 节点的属性名，用于XmlCreater类的AddRealNode、ReadAtt方法
        /// </summary>
        public string attName
        {
            get { return AttName; }
            set { AttName = value; }
        }
        /// <summary>
        /// 节点的属性值，用于XmlCreater类的AddRealNode方法
        /// </summary>
        public string attValue
        {
            get { return AttValue; }
            set { AttValue = value; }
        }
        /// <summary>
        /// 节点的子文本，用于XmlCreater类的AddRealNode方法
        /// </summary>
        public string innerText
        {
            get { return InnerText; }
            set { InnerText = value; }
        }
        /// <summary>
        /// 读取类型，可选值有"_name"、"_value"，用于XmlCreater类的ReadNode方法
        /// </summary>
        public string type
        {
            get { return Type; }
            set { Type = value; }
        }

        #endregion

    }

    /// <summary>
    /// 查询位置，structspace
    /// </summary>
    public struct locateStr
    {
        #region 字段

        private string DataBaseName;
        private string TableName;
        private string WhereColumnName;
        private string TargetColumnName;

        #endregion

        #region 字段访问器

        /// <summary>
        /// 被操作表所在数据库名
        /// </summary>
        public string dataBaseName
        {
            get { return DataBaseName; }
            set { DataBaseName = value; }
        }
        /// <summary>
        /// 被操作表名
        /// </summary>
        public string tableName
        {
            get { return TableName; }
            set { TableName = value; }
        }
        /// <summary>
        /// 定位列的列名
        /// </summary>
        public string whereColumnName
        {
            get { return WhereColumnName; }
            set { WhereColumnName = value; }
        }
        /// <summary>
        /// 目标列的列名，目标列承担读取/更改操作
        /// </summary>
        public string targetColumnName
        {
            get { return TargetColumnName; }
            set { TargetColumnName = value; }
        }

        #endregion
    }

    /// <summary>
    /// Json本库信息，structspace
    /// </summary>
    public class JlinfObject : FrameHandler.IStdLibFrame
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
    /// 参数单元（用于参数化查询添加参数），structspace
    /// </summary>
    public struct para
    {

        private string _paraName;
        /// <summary>
        /// 参数名
        /// </summary>
        public string paraName
        {
            get { return _paraName; }
            set { _paraName = value; }
        }


        private object _paraValue;
        /// <summary>
        /// 参数值
        /// </summary>
        public object paraValue
        {
            get { return _paraValue; }
            set { _paraValue = value; }
        }

    }

    /// <summary>
    /// pala文章接口
    /// </summary>
    public interface IpaPost
    {
        /// <summary>
        /// 文章序列号
        /// </summary>
        int post_id
        {
            get; set;
        }
        /// <summary>
        /// 文章标题
        /// </summary>
        string post_title
        {
            get; set;
        }
        /// <summary>
        /// 文章概要
        /// </summary>
        string post_summary
        {
            get; set;
        }
        /// <summary>
        /// 文章内容
        /// </summary>
        string post_content
        {
            get; set;
        }
        /// <summary>
        /// 文章归档
        /// </summary>
        string post_archive
        {
            get; set;
        }
        /// <summary>
        /// 文章展示状态
        /// </summary>
        bool post_isShow
        {
            get; set;
        }
        /// <summary>
        /// 文章类型
        /// </summary>
        string post_type
        {
            get; set;
        }
        /// <summary>
        /// 文章作者序列号
        /// </summary>
        string post_author
        {
            get; set;
        }
        /// <summary>
        /// 文章只读状态
        /// </summary>
        bool post_isReadOnly
        {
            get; set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        DateTime date_created
        {
            get; set;
        }
        /// <summary>
        /// 更改日期
        /// </summary>
        DateTime date_changed
        {
            get; set;
        }

        /// <summary>
        /// 文章列表封面图片地址
        /// </summary>
        string path_cover
        {
            get; set;
        }

    }
    /// <summary>
    /// pala页面接口
    /// </summary>
    public interface IpaPage
    {
        /// <summary>
        /// 文章序列号
        /// </summary>
        int page_id
        {
            get; set;
        }
        /// <summary>
        /// 文章标题
        /// </summary>
        string page_title
        {
            get; set;
        }
        /// <summary>
        /// 文章概要
        /// </summary>
        string page_summary
        {
            get; set;
        }
        /// <summary>
        /// 文章内容
        /// </summary>
        string page_content
        {
            get; set;
        }
        /// <summary>
        /// 文章归档
        /// </summary>
        string page_archive
        {
            get; set;
        }
        /// <summary>
        /// 文章展示状态
        /// </summary>
        bool page_isShow
        {
            get; set;
        }
        /// <summary>
        /// 文章类型
        /// </summary>
        string page_type
        {
            get; set;
        }
        /// <summary>
        /// 文章作者序列号
        /// </summary>
        string page_author
        {
            get; set;
        }
        /// <summary>
        /// 文章只读状态
        /// </summary>
        bool page_isReadOnly
        {
            get; set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        DateTime date_created
        {
            get; set;
        }
        /// <summary>
        /// 更改日期
        /// </summary>
        DateTime date_changed
        {
            get; set;
        }

        /// <summary>
        /// 文章列表封面图片地址
        /// </summary>
        string path_cover
        {
            get; set;
        }

    }

    /// <summary>
    /// 文章基本数据，structspace
    /// </summary>
    public struct paPost : IpaPost
    {
        #region 字段
        private int _post_id;
        private string _post_title;
        private string _post_summary;
        private string _post_content;
        private string _post_archive;
        private bool _post_isShow;
        private string _post_type;
        private string _post_author;
        private bool _post_isReadOnly;

        private DateTime _date_created;
        private DateTime _date_changed;

        private string _path_cover;
        #endregion

        #region 字段访问器

        /// <summary>
        /// 文章序列号
        /// </summary>
        public int post_id
        {
            get { return _post_id; }
            set { _post_id = value; }
        }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string post_title
        {
            get { return _post_title; }
            set { _post_title = value; }
        }
        /// <summary>
        /// 文章概要
        /// </summary>
        public string post_summary
        {
            get { return _post_summary; }
            set { _post_summary = value; }
        }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string post_content
        {
            get { return _post_content; }
            set { _post_content = value; }
        }
        /// <summary>
        /// 文章归档
        /// </summary>
        public string post_archive
        {
            get { return _post_archive; }
            set { _post_archive = value; }
        }
        /// <summary>
        /// 文章展示状态
        /// </summary>
        public bool post_isShow
        {
            get { return _post_isShow; }
            set { _post_isShow = value; }
        }
        /// <summary>
        /// 文章类型
        /// </summary>
        public string post_type
        {
            get { return _post_type; }
            set { _post_type = value; }
        }
        /// <summary>
        /// 文章作者序列号
        /// </summary>
        public string post_author
        {
            get { return _post_author; }
            set { _post_author = value; }
        }
        /// <summary>
        /// 文章只读状态
        /// </summary>
        public bool post_isReadOnly
        {
            get { return _post_isReadOnly; }
            set { _post_isReadOnly = value; }
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime date_created
        {
            get { return _date_created; }
            set { _date_created = value; }
        }
        /// <summary>
        /// 更改日期
        /// </summary>
        public DateTime date_changed
        {
            get { return _date_changed; }
            set { _date_changed = value; }
        }

        /// <summary>
        /// 文章列表封面图片地址
        /// </summary>
        public string path_cover
        {
            get { return _path_cover; }
            set { _path_cover = value; }

        }

        #endregion
    }
    /// <summary>
    /// 文章拓展数据
    /// </summary>
    public struct paPage : IpaPage
    {
        #region 字段
        private int _page_id;
        private string _page_title;
        private string _page_summary;
        private string _page_content;
        private string _page_archive;
        private bool _page_isShow;
        private string _page_type;
        private string _page_author;
        private bool _page_isReadOnly;

        private DateTime _date_created;
        private DateTime _date_changed;

        private string _path_cover;
        #endregion

        #region 字段访问器

        /// <summary>
        /// 文章序列号
        /// </summary>
        public int page_id
        {
            get { return _page_id; }
            set { _page_id = value; }
        }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string page_title
        {
            get { return _page_title; }
            set { _page_title = value; }
        }
        /// <summary>
        /// 文章概要
        /// </summary>
        public string page_summary
        {
            get { return _page_summary; }
            set { _page_summary = value; }
        }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string page_content
        {
            get { return _page_content; }
            set { _page_content = value; }
        }
        /// <summary>
        /// 文章归档
        /// </summary>
        public string page_archive
        {
            get { return _page_archive; }
            set { _page_archive = value; }
        }
        /// <summary>
        /// 文章展示状态
        /// </summary>
        public bool page_isShow
        {
            get { return _page_isShow; }
            set { _page_isShow = value; }
        }
        /// <summary>
        /// 文章类型
        /// </summary>
        public string page_type
        {
            get { return _page_type; }
            set { _page_type = value; }
        }
        /// <summary>
        /// 文章作者
        /// </summary>
        public string page_author
        {
            get { return _page_author; }
            set { _page_author = value; }
        }
        /// <summary>
        /// 文章只读状态
        /// </summary>
        public bool page_isReadOnly
        {
            get { return _page_isReadOnly; }
            set { _page_isReadOnly = value; }
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime date_created
        {
            get { return _date_created; }
            set { _date_created = value; }
        }
        /// <summary>
        /// 更改日期
        /// </summary>
        public DateTime date_changed
        {
            get { return _date_changed; }
            set { _date_changed = value; }
        }

        /// <summary>
        /// 文章列表封面图片地址
        /// </summary>
        public string path_cover
        {
            get { return _path_cover; }
            set { _path_cover = value; }

        }

        #endregion
    }

    /// <summary>
    /// 文章拓展数据，structspace
    /// </summary>
    public struct pluPaPost
    {
        #region 字段
        private int _post_id;

        private int _count_read;
        private int _count_review;
        private int _count_like;

        private string _tagA;
        private string _tagB;
        private string _tagC;

        private string _color_strip;
        #endregion

        #region 字段访问器

        /// <summary>
        /// 文章序列号
        /// </summary>
        public int post_id
        {
            get { return _post_id; }
            set { _post_id = value; }
        }

        /// <summary>
        /// 阅读计数
        /// </summary>
        public int count_read
        {
            get { return _count_read; }
            set { _count_read = value; }
        }
        /// <summary>
        /// 评论计数
        /// </summary>
        public int count_review
        {
            get { return _count_review; }
            set { _count_review = value; }
        }
        /// <summary>
        /// 点赞计数
        /// </summary>
        public int count_like
        {
            get { return _count_like; }
            set { _count_like = value; }
        }

        /// <summary>
        /// 特征A
        /// </summary>
        public string tagA
        {
            get { return _tagA; }
            set { _tagA = value; }
        }
        /// <summary>
        /// 特征B
        /// </summary>
        public string tagB
        {
            get { return _tagB; }
            set { _tagB = value; }
        }
        /// <summary>
        /// 特征C
        /// </summary>
        public string tagC
        {
            get { return _tagC; }
            set { _tagC = value; }
        }

        /// <summary>
        /// 文章列表条带颜色
        /// </summary>
        public string color_strip
        {
            get { return _color_strip; }
            set { _color_strip = value; }

        }

        #endregion
    }
    /// <summary>
    /// 页面拓展数据
    /// </summary>
    public struct pluPaPage
    {
        #region 字段
        private int _post_id;

        private int _count_read;
        private int _count_review;
        private int _count_like;

        private string _tagA;
        private string _tagB;
        private string _tagC;

        private string _color_strip;
        #endregion

        #region 字段访问器

        /// <summary>
        /// 文章序列号
        /// </summary>
        public int post_id
        {
            get { return _post_id; }
            set { _post_id = value; }
        }

        /// <summary>
        /// 阅读计数
        /// </summary>
        public int count_read
        {
            get { return _count_read; }
            set { _count_read = value; }
        }
        /// <summary>
        /// 评论计数
        /// </summary>
        public int count_review
        {
            get { return _count_review; }
            set { _count_review = value; }
        }
        /// <summary>
        /// 点赞计数
        /// </summary>
        public int count_like
        {
            get { return _count_like; }
            set { _count_like = value; }
        }

        /// <summary>
        /// 特征A
        /// </summary>
        public string tagA
        {
            get { return _tagA; }
            set { _tagA = value; }
        }
        /// <summary>
        /// 特征B
        /// </summary>
        public string tagB
        {
            get { return _tagB; }
            set { _tagB = value; }
        }
        /// <summary>
        /// 特征C
        /// </summary>
        public string tagC
        {
            get { return _tagC; }
            set { _tagC = value; }
        }

        /// <summary>
        /// 文章列表条带颜色
        /// </summary>
        public string color_strip
        {
            get { return _color_strip; }
            set { _color_strip = value; }

        }

        #endregion
    }

    /// <summary>
    /// 用户数据
    /// </summary>
    public struct paUser
    {
        #region 字段
        private int _user_id;
        private string _user_level;
        private string _user_name;
        private string _user_word;
        #endregion

        #region 字段访问器

        /// <summary>
        /// 用户id
        /// </summary>
        public int user_id
        {
            get { return _user_id; }
            set { _user_id = value; }
        }
        /// <summary>
        /// 用户权限级
        /// </summary>
        public string user_level
        {
            get { return _user_level; }
            set { _user_level = value; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string user_name
        {
            get { return _user_name; }
            set { _user_name = value; }
        }
        /// <summary>
        /// 用户签名
        /// </summary>
        public string user_word
        {
            get { return _user_word; }
            set { _user_word = value; }
        }

        #endregion
    }

}
