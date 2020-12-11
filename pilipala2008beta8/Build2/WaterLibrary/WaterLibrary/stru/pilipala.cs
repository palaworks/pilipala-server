using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WaterLibrary.com.basic;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;
using WaterLibrary.stru.pilipala.DB;


namespace WaterLibrary.stru.pilipala
{
    namespace DB
    {
        /// <summary>
        /// 啪啦数据库接口
        /// </summary>
        interface IPLDataBase
        {
            /// <summary>
            /// 文本表
            /// </summary>
            PLTables Tables { get; }
            /// <summary>
            /// 文本视图
            /// </summary>
            PLViews Views { get; }
            /// <summary>
            /// 数据库管理器实例
            /// </summary>
            MySqlManager MySqlManager { get; }
        }

        /// <summary>
        /// 索引表数据接口
        /// </summary>
        public interface ITableIndex
        {

            /// <summary>
            /// 索引
            /// </summary>
            int ID { get; set; }
            /// <summary>
            /// GUID标识
            /// </summary>
            string GUID { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            DateTime CT { get; set; }
            /// <summary>
            /// 模式
            /// </summary>
            string Mode { get; set; }
            /// <summary>
            /// 类型
            /// </summary>
            string Type { get; set; }
            /// <summary>
            /// 隶属用户
            /// </summary>
            string User { get; set; }
            /// <summary>
            /// 浏览计数
            /// </summary>
            int UVCount { get; set; }
            /// <summary>
            /// 星星计数
            /// </summary>
            int StarCount { get; set; }

        }
        /// <summary>
        /// 备份表数据接口
        /// </summary>
        public interface ITableBackup
        {
            /// <summary>
            /// 索引
            /// </summary>
            int ID { get; set; }
            /// <summary>
            /// GUID标识
            /// </summary>
            string GUID { get; set; }
            /// <summary>
            /// 最后修改时间
            /// </summary>
            DateTime LCT { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            string Title { get; set; }
            /// <summary>
            /// 摘要
            /// </summary>
            string Summary { get; set; }
            /// <summary>
            /// 正文
            /// </summary>
            string Content { get; set; }
            /// <summary>
            /// 归档
            /// </summary>
            string Archiv { get; set; }
            /// <summary>
            /// 标签
            /// </summary>
            string Label { get; set; }
            /// <summary>
            /// 封面
            /// </summary>
            string Cover { get; set; }
        }
        /// <summary>
        /// 用户表数据接口
        /// </summary>
        public interface ITableUser
        {
            /// <summary>
            /// 用户ID
            /// </summary>
            int ID { get; set; }
            /// <summary>
            /// GUID标识
            /// </summary>
            string GUID { get; set; }
            /// <summary>
            /// 用户名
            /// </summary>
            string Name { get; set; }
            /// <summary>
            /// 签名
            /// </summary>
            string Note { get; set; }
        }

        /// <summary>
        /// 数据库表集合
        /// </summary>
        public struct PLTables
        {
            /// <summary>
            /// 初始化表名结构
            /// </summary>
            /// <parmm name="root"></parmm>
            /// <parmm name="IndexTable"></parmm>
            /// <parmm name="BackupTable"></parmm>
            /// <parmm name="text_sub"></parmm>
            public PLTables(string User, string Index, string Backup, string Comment) : this()
            {
                this.User = User;
                this.Index = Index;
                this.Backup = Backup;
                this.Comment = Comment;
            }

            /// <summary>
            /// 用户表
            /// </summary>
            public string User { get; set; }
            /// <summary>
            /// 索引表
            /// </summary>
            public string Index { get; set; }
            /// <summary>
            /// 主表
            /// </summary>
            public string Backup { get; set; }
            /// <summary>
            /// 评论表
            /// </summary>
            public string Comment { get; set; }
        }
        /// <summary>
        /// 数据库视图视图集合
        /// </summary>
        public struct PLViews
        {
            /// <summary>
            /// 初始化视图名结构
            /// </summary>
            /// <param name="PosUnion">积极联合视图</param>
            /// <param name="NegUnion">消极联合视图</param>
            public PLViews(string PosUnion, string NegUnion) : this()
            {
                this.PosUnion = PosUnion;
                this.NegUnion = NegUnion;
            }
            /// <summary>
            /// 积极联合视图（不包含备份）
            /// </summary>
            public string PosUnion { get; set; }
            /// <summary>
            /// 消极联合视图（包含备份）
            /// </summary>
            public string NegUnion { get; set; }
        }

        /// <summary>
        /// 啪啦数据库信息
        /// </summary>
        public struct PLDB : IPLDataBase
        {
            /// <summary>
            /// 数据表
            /// </summary>
            public PLTables Tables { get; set; }
            /// <summary>
            /// 数据视图
            /// </summary>
            public PLViews Views { get; set; }
            /// <summary>
            /// 数据库管理器实例
            /// </summary>
            public MySqlManager MySqlManager { get; set; }
        }
    }

    /// <summary>
    /// 噼里啪啦配件接口
    /// </summary>
    interface IPLComponent
    {
        void Ready(CORE CORE);
    }
    /// <summary>
    /// 噼里啪啦内核接口
    /// </summary>
    public interface ICORE
    {
        /// <summary>
        /// 开始配件连接事件
        /// </summary>
        public event LinkEventHandler LinkOn;

        /// <summary>
        /// 启动内核
        /// </summary>
        /// <returns></returns>
        User Run();
        /// <summary>
        /// 关闭内核
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 设置内核所需要的表
        /// </summary>
        /// <param name="User">用户表</param>
        /// <param name="Index">索引表</param>
        /// <param name="Backup">备份表</param>
        /// <param name="Comment">备份表</param>
        void SetTables(string User = "pl_user", string Index = "pl_index", string Backup = "pl_backup", string Comment = "comment_lake");
        /// <summary>
        /// 设置内核所需要的视图
        /// </summary>
        /// <param name="PosUnion">积极联合视图</param>
        /// <param name="NegUnion">消极联合视图</param>
        void SetViews(string PosUnion = "pos>union", string NegUnion = "neg>union");
    }

    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="Key">索引名</param>
        /// <returns></returns>
        public object this[string Key]
        {
            get
            {
                /* 通过反射获取属性 */
                return GetType().GetProperty(Key).GetValue(this);
            }
            set
            {
                /* 通过反射设置属性 */
                System.Type ThisType = GetType();
                System.Type KeyType = ThisType.GetProperty(Key).GetValue(this).GetType();
                ThisType.GetProperty(Key).SetValue(this, Convert.ChangeType(value, KeyType));
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密码(MD5值)
        /// </summary>
        public string PWD { get; set; }

        /// <summary>
        /// 用户GUID
        /// </summary>
        public string GUID { get; set; }
        /// <summary>
        /// 自我介绍
        /// </summary>
        public string Bio { get; set; }
        /// <summary>
        /// 分组
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 头像(链接)
        /// </summary>
        public string Avatar { get; set; }
    }

    namespace Post
    {
        /// <summary>
        /// 文章
        /// </summary>
        public class Post : ITableIndex, ITableBackup
        {
            /// <summary>
            /// 索引器
            /// </summary>
            /// <param name="Key">索引名</param>
            /// <returns></returns>
            public object this[string Key]
            {
                get
                {
                    /* 通过反射获取属性 */
                    return GetType().GetProperty(Key).GetValue(this);
                }
                set
                {
                    /* 通过反射设置属性 */
                    System.Type ThisType = GetType();
                    System.Type KeyType = ThisType.GetProperty(Key).GetValue(this).GetType();
                    ThisType.GetProperty(Key).SetValue(this, Convert.ChangeType(value, KeyType));
                }
            }
            /// <summary>
            /// 将当前对象序列化到JSON
            /// </summary>
            /// <returns></returns>
            public string ToJSON()
            {
                return JsonConvert.SerializeObject
                    (this, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            /// <summary>
            /// 初始化
            /// </summary>
            public Post()
            {
                /* -1表示未被赋值，同时也于数据库的非负冲突 */
                ID = -1;
                GUID = "";

                Title = "";
                Summary = "";
                Content = "";
                Cover = "";

                Archiv = "";
                Label = "";

                Mode = "";
                Type = "";
                User = "";

                CT = new DateTime();
                LCT = new DateTime();

                UVCount = -1;
                StarCount = -1;

                PropertyContainer = new Hashtable();
            }

            /// <summary>
            /// 计算由标题、概要、内容签名的MD5
            /// </summary>
            /// <returns></returns>
            public string MD5()
            {
                return MathH.MD5(Title + Summary + Content);
            }
            /// <summary>
            /// 计算由标题、概要、内容签名的MD5，并从首位限定取用长度
            /// </summary>
            /// <param name="Length">取用长度</param>
            /// <returns></returns>
            public string MD5(int Length)
            {
                return MD5().Substring(0, Length);
            }

            /// <summary>
            /// 索引
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 全局标识
            /// </summary>
            public string GUID { get; set; }

            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 概要
            /// </summary>
            public string Summary { get; set; }
            /// <summary>
            /// 内容
            /// </summary>
            public string Content { get; set; }
            /// <summary>
            /// 获得Html格式的文章内容，所有Markdown标记均会被转换为等效的Html标记
            /// </summary>
            /// <returns></returns>
            public string HtmlContent()
            {
                return ConvertH.MarkdownToHtml(Content);
            }
            /// <summary>
            /// 获得Html格式的文章内容，所有Markdown标记均会被转换为等效的Html标记，并从首位置限定取用长度
            /// </summary>
            /// <param name="Length">取用长度</param>
            /// <returns></returns>
            public string HtmlContent(int Length)
            {
                return ConvertH.MarkdownToHtml(Content).Substring(0, Length);
            }
            /// <summary>
            /// 获得纯文本格式的文章内容，过滤掉任何Markdown和Html标记
            /// </summary>
            /// <returns></returns>
            public string TextContent()
            {
                return ConvertH.HtmlFilter(HtmlContent());
            }
            /// <summary>
            /// 获得纯文本格式的文章内容，过滤掉任何Markdown和Html标记，并从首位置限定取用长度
            /// </summary>
            /// <param name="Length">取用长度</param>
            /// <returns></returns>
            public string TextContent(int Length)
            {
                return ConvertH.HtmlFilter(HtmlContent()).Substring(0, Length);
            }
            /// <summary>
            /// 封面
            /// </summary>
            public string Cover { get; set; }

            /// <summary>
            /// 归档
            /// </summary>
            public string Archiv { get; set; }
            /// <summary>
            /// 标签
            /// </summary>
            public string Label { get; set; }
            /// <summary>
            /// 获得标签集合
            /// </summary>
            /// <returns></returns>
            public List<string> LabelList() { return ConvertH.StringToList(Label, '$'); }

            /// <summary>
            /// 文章模式
            /// </summary>
            public string Mode { get; set; }
            /// <summary>
            /// 文章类型
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// 归属用户
            /// </summary>
            public string User { get; set; }

            /// <summary>
            /// 创建时间
            /// </summary>
            public DateTime CT { get; set; }
            /// <summary>
            /// 最后修改时间
            /// </summary>
            public DateTime LCT { get; set; }

            /// <summary>
            /// 访问计数
            /// </summary>
            public int UVCount { get; set; }
            /// <summary>
            /// 星星计数
            /// </summary>
            public int StarCount { get; set; }

            /// <summary>
            /// 属性容器
            /// </summary>
            public Hashtable PropertyContainer { get; set; }
        }
        /// <summary>
        /// 文章数据集
        /// </summary>
        public class PostSet : IEnumerable
        {
            IEnumerator IEnumerable.GetEnumerator()
            {
                return PostList.GetEnumerator();
            }
            private readonly List<Post> PostList = new List<Post>();
            /// <summary>
            /// 将当前对象序列化到JSON
            /// </summary>
            /// <returns></returns>
            public string ToJSON()
            {
                return JsonConvert.SerializeObject
                    (this, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            /// <summary>
            /// 当前数据集的文章对象数
            /// </summary>
            public int Count
            {
                get { return PostList.Count; }
            }
            /// <summary>
            /// 取得数据集中的最后一个评论对象
            /// </summary>
            /// <returns></returns>
            public Post Last()
            {
                return PostList.Last();
            }

            /// <summary>
            /// 添加文章
            /// </summary>
            /// <param name="Post">文章对象</param>
            public void Add(Post Post)
            {
                PostList.Add(Post);
            }

            /// <summary>
            /// 数据集内最近一月(30天内)的文章创建数
            /// </summary>
            /// <returns></returns>
            public int WithinMonthCreateCount()
            {
                int Count = 0;
                foreach (Post el in PostList)
                {
                    if (el.CT > DateTime.Now.AddDays(-30))
                    {
                        Count++;
                    }
                }
                return Count;
            }
            /// <summary>
            /// 数据集内最近一周(7天内)的文章创建数
            /// </summary>
            /// <returns></returns>
            public int WithinWeekCreateCount()
            {
                int Count = 0;
                foreach (Post el in PostList)
                {
                    if (el.CT > DateTime.Now.AddDays(-7))
                    {
                        Count++;
                    }
                }
                return Count;
            }
            /// <summary>
            /// 数据集内最近一天(24小时内)的文章创建数
            /// </summary>
            /// <returns></returns>
            public int WithinDayCreateCount()
            {
                int Count = 0;
                foreach (Post el in PostList)
                {
                    if (el.CT > DateTime.Now.AddDays(-1))
                    {
                        Count++;
                    }
                }
                return Count;
            }

            /// <summary>
            /// 数据集内最近一月(30天内)的文章修改数
            /// </summary>
            /// <returns></returns>
            public int WithinMonthUpdateCount()
            {
                int Count = 0;
                foreach (Post el in PostList)
                {
                    if (el.LCT > DateTime.Now.AddDays(-30))
                    {
                        Count++;
                    }
                }
                return Count;
            }
            /// <summary>
            /// 数据集内最近一周(7天内)的文章修改数
            /// </summary>
            /// <returns></returns>
            public int WithinWeekUpdateCount()
            {
                int Count = 0;
                foreach (Post el in PostList)
                {
                    if (el.LCT > DateTime.Now.AddDays(-7))
                    {
                        Count++;
                    }
                }
                return Count;
            }
            /// <summary>
            /// 数据集内最近一天(24小时内)的文章修改数
            /// </summary>
            /// <returns></returns>
            public int WithinDayUpdateCount()
            {
                int Count = 0;
                foreach (Post el in PostList)
                {
                    if (el.LCT > DateTime.Now.AddDays(-1))
                    {
                        Count++;
                    }
                }
                return Count;
            }
        }

        namespace Property
        {
            /// <summary>
            /// 文章属性接口
            /// </summary>
            public interface IProperty
            {

            }

            /// <summary>
            /// 文章索引
            /// </summary>
            public struct ID : IProperty
            {

            }
            /// <summary>
            /// 文章全局标识
            /// </summary>
            public struct GUID : IProperty
            {

            }

            /// <summary>
            /// 文章标题
            /// </summary>
            public struct Title : IProperty
            {

            }
            /// <summary>
            /// 文章摘要
            /// </summary>
            public struct Summary : IProperty
            {

            }
            /// <summary>
            /// 文章内容
            /// </summary>
            public struct Content : IProperty
            {

            }
            /// <summary>
            /// 文章标题
            /// </summary>
            public struct Cover : IProperty
            {

            }

            /// <summary>
            /// 文章归档
            /// </summary>
            public struct Archiv : IProperty
            {

            }
            /// <summary>
            /// 文章标签
            /// </summary>
            public struct Label : IProperty
            {

            }

            /// <summary>
            /// 文章模式
            /// </summary>
            public struct Mode : IProperty
            {

            }
            /// <summary>
            /// 文章类型
            /// </summary>
            public struct Type : IProperty
            {

            }
            /// <summary>
            /// 归属用户
            /// </summary>
            public struct User : IProperty
            {

            }

            /// <summary>
            /// 创建时间
            /// </summary>
            public struct CT : IProperty
            {

            }
            /// <summary>
            /// 最后修改时间
            /// </summary>
            public struct LCT : IProperty
            {

            }

            /// <summary>
            /// 浏览计数
            /// </summary>
            public struct UVCount : IProperty
            {

            }
            /// <summary>
            /// 星星计数
            /// </summary>
            public struct StarCount : IProperty
            {

            }
        }
    }
}
