using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Linq;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

using WaterLibrary.MySQL;
using WaterLibrary.Tools;
using WaterLibrary.pilipala.Database;
using WaterLibrary.pilipala.Entity;
using WaterLibrary.pilipala.Entity.PostProperty;

namespace WaterLibrary.pilipala
{
    namespace Database
    {
        /// <summary>
        /// 啪啦数据库接口
        /// </summary>
        interface IPLDatabase
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
        public interface IIndexTable
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
        public interface IBackupTable
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
        public interface IUserTable
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
        public struct PLDatabase : IPLDatabase
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
    interface IPLComponent<T>
    {
        T Ready(ICORE CORE);
    }
    interface IPLComponentFactory
    {
        void Ready(ICORE CORE);
    }
    /// <summary>
    /// 噼里啪啦内核接口
    /// </summary>
    public interface ICORE
    {
        /// <summary>
        /// 内核视图访问器
        /// </summary>
        PLViews Views { get; }
        /// <summary>
        /// 内核表访问器
        /// </summary>
        PLTables Tables { get; }
        /// <summary>
        /// 内核MySql数据库控制器
        /// </summary>
        MySqlManager MySqlManager { get; }

        /// <summary>
        /// 开始配件连接事件
        /// </summary>
        public event LinkEventHandler LinkOn;

        /// <summary>
        /// 登录到内核的用户GUID
        /// </summary>
        string UserGUID { get; }

        /// <summary>
        /// 启动内核
        /// </summary>
        /// <returns></returns>
        Entity.User Run();
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
    /// 配件连接委托
    /// </summary>
    /// <param name="CORE">内核对象</param>
    public delegate void LinkEventHandler(CORE CORE);
    /// <summary>
    /// pilipala内核
    /// </summary>
    public class CORE : ICORE
    {
        /// <summary>
        /// 开始配件连接事件
        /// </summary>
        public event LinkEventHandler LinkOn;

        /// <summary>
        /// 核心表结构
        /// </summary>
        public PLTables Tables { get; private set; }
        /// <summary>
        /// 核心视图结构
        /// </summary>
        public PLViews Views { get; private set; }
        /// <summary>
        /// MySql控制器
        /// </summary>
        public MySqlManager MySqlManager { get; private set; }

        /// <summary>
        /// 登录内核的用户GUID
        /// </summary>
        public string UserGUID { get; }

        internal string UserName { get; }
        internal string UserPWD { get; }

        /// <summary>
        /// 初始化pilipala内核
        /// </summary>
        /// <param name="PLDatabase">pilipala数据库信息</param>
        /// <param name="UserName">用户名</param>
        /// <param name="UserPWD">用户密码</param>
        public CORE(PLDatabase PLDatabase, string UserName, string UserPWD)
        {
            MySqlManager = PLDatabase.MySqlManager;
            SetTables(PLDatabase.Tables.User, PLDatabase.Tables.Index, PLDatabase.Tables.Backup);
            SetViews(PLDatabase.Views.PosUnion, PLDatabase.Views.NegUnion);

            /* 用户信息录入 */
            this.UserName = UserName;
            this.UserPWD = UserPWD;
        }

        /// <summary>
        /// 内核启动
        /// </summary>
        /// <returns>返回用户数据</returns>
        public Entity.User Run()
        {
            MySqlManager.Open();
            string SQL = $"SELECT COUNT(*) FROM {Tables.User} WHERE Name = ?UserName AND PWD = ?UserPWD";

            if (MySqlManager.GetKey(SQL, new MySqlParameter[]
            {
                new("UserName", UserName),
                new("UserPWD", MathH.MD5(UserPWD))
            })
            .ToString() == "1")
            {
                /* 通知所有订阅到当前内核的所有配件内核已经准备完成，并分发内核到各配件 */
                LinkOn(this);
                /* 取得用户数据并返回 */
                return new Entity.User()
                {
                    GUID = MySqlManager.GetRow($"SELECT GUID FROM {Tables.User} WHERE Name = '{UserName}'")["GUID"].ToString(),
                    Tables = Tables,
                    MySqlManager = MySqlManager,
                };
            }
            else
            {
                MySqlManager.Close();
                throw (new Exception("非法的用户签名"));
            }
        }
        /// <summary>
        /// 关闭内核
        /// </summary>
        public void Shutdown()
        {
            MySqlManager.Close();
        }

        /// <summary>
        /// 设置内核所需要的表
        /// </summary>
        /// <param name="User">用户表</param>
        /// <param name="Index">索引表</param>
        /// <param name="Backup">备份表</param>
        /// <param name="Comment">备份表</param>
        public void SetTables(string User = "pl_user", string Index = "pl_index", string Backup = "pl_backup", string Comment = "comment_lake")
        {
            Tables = new PLTables(User, Index, Backup, Comment);

        }
        /// <summary>
        /// 设置内核所需要的视图
        /// </summary>
        /// <param name="PosUnion">积极联合视图</param>
        /// <param name="NegUnion">消极联合视图</param>
        public void SetViews(string PosUnion = "pos>union", string NegUnion = "neg>union")
        {
            Views = new PLViews(PosUnion, NegUnion);
        }
    }


    namespace Entity
    {
        /// <summary>
        /// 用户
        /// </summary>
        public class User
        {
            internal PLTables Tables { get; set; }
            internal MySqlManager MySqlManager { get; set; }

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
            /// 用户GUID
            /// </summary>
            public string GUID { get; internal set; }

            /// <summary>
            /// 用户名
            /// </summary>
            public string Name { get { return Getter("Name"); } set { Setter("Name", value); } }
            /// <summary>
            /// 密码(MD5值)
            /// </summary>
            public string PWD { get { return Getter("PWD"); } set { Setter("PWD", value); } }

            /// <summary>
            /// 自我介绍
            /// </summary>
            public string Bio { get { return Getter("Bio"); } set { Setter("Bio", value); } }
            /// <summary>
            /// 分组
            /// </summary>
            public string Group { get { return Getter("Group"); } set { Setter("Group", value); } }
            /// <summary>
            /// 邮箱
            /// </summary>
            public string Email { get { return Getter("Email"); } set { Setter("Email", value); } }
            /// <summary>
            /// 头像(链接)
            /// </summary>
            public string Avatar { get { return Getter("Avatar"); } set { Setter("Avatar", value); } }

            private string Getter(string Key)
            {
                return MySqlManager.GetRow($"SELECT {Key} FROM {Tables.User} WHERE GUID = '{GUID}'")[Key].ToString();
            }
            private bool Setter(string Key, string Value)
            {
                return MySqlManager.UpdateKey(new MySqlKey() { Table = Tables.User, Name = "GUID", Val = GUID }, Key, Value);
            }
        }
        /// <summary>
        /// 文章
        /// </summary>
        public class Post : IIndexTable, IBackupTable
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
            public string HtmlContent(int Length) => ConvertH.MarkdownToHtml(Content).Substring(0, Length);
            /// <summary>
            /// 获得纯文本格式的文章内容，过滤掉任何Markdown和Html标记
            /// </summary>
            /// <returns></returns>
            public string TextContent() => ConvertH.HtmlFilter(HtmlContent());
            /// <summary>
            /// 获得纯文本格式的文章内容，过滤掉任何Markdown和Html标记，并从首位置限定取用长度
            /// </summary>
            /// <param name="Length">取用长度</param>
            /// <returns></returns>
            public string TextContent(int Length) => ConvertH.HtmlFilter(HtmlContent()).Substring(0, Length);

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
            public List<string> LabelList() => ConvertH.StringToList(Label, '$');

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
                return JsonConvert
                    .SerializeObject(this, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
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
            public Post Last() => PostList.Last();
            /// <summary>
            /// 添加文章
            /// </summary>
            /// <param name="Post">文章对象</param>
            public void Add(Post Post) => PostList.Add(Post);
            /// <summary>
            /// 对数据集内的每一个对象应用Action
            /// </summary>
            /// <param name="action">Action委托</param>
            /// <returns>返回操作后的数据集</returns>
            public PostSet ForEach(Action<Post> action)
            {
                PostList.ForEach(action);
                return this;
            }


            /// <summary>
            /// 数据集内最近一月(30天内)的文章创建数
            /// </summary>
            /// <returns></returns>
            public int WithinMonthCreateCount() => CreateCounter(-30);
            /// <summary>
            /// 数据集内最近一周(7天内)的文章创建数
            /// </summary>
            /// <returns></returns>
            public int WithinWeekCreateCount() => CreateCounter(-7);
            /// <summary>
            /// 数据集内最近一天(24小时内)的文章创建数
            /// </summary>
            /// <returns></returns>
            public int WithinDayCreateCount() => CreateCounter(-1);
            private int CreateCounter(int Days) => (from el in PostList where el.CT > DateTime.Now.AddDays(Days) select el).Count();

            /// <summary>
            /// 数据集内最近一月(30天内)的文章修改数
            /// </summary>
            /// <returns></returns>
            public int WithinMonthUpdateCount() => UpdateCounter(-30);
            /// <summary>
            /// 数据集内最近一周(7天内)的文章修改数
            /// </summary>
            /// <returns></returns>
            public int WithinWeekUpdateCount() => UpdateCounter(-7);
            /// <summary>
            /// 数据集内最近一天(24小时内)的文章修改数
            /// </summary>
            /// <returns></returns>
            public int WithinDayUpdateCount() => UpdateCounter(-1);
            private int UpdateCounter(int Days) => (from el in PostList where el.CT > DateTime.Now.AddDays(Days) select el).Count();
        }

        namespace PostProperty
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
    namespace Components
    {
        /// <summary>
        /// 组件工厂
        /// </summary>
        public class ComponentFactory : IPLComponentFactory
        {
            private ICORE CORE;
            /// <summary>
            /// 准备完成
            /// </summary>
            /// <param name="CORE"></param>
            public void Ready(ICORE CORE) => this.CORE = CORE;


            /// <summary>
            /// 生成读组件
            /// </summary>
            /// <returns></returns>
            public Reader GenReader() => new Reader().Ready(CORE.Views, CORE.MySqlManager);
            /// <summary>
            /// 生成写组件
            /// </summary>
            /// <returns></returns>
            public Writer GenWriter() => new Writer().Ready(CORE.Tables, CORE.MySqlManager, CORE.UserGUID);
            /// <summary>
            /// 生成计数组件
            /// </summary>
            /// <returns></returns>
            public Counter GenCounter() => new Counter().Ready(CORE.Tables, CORE.MySqlManager);
        }

        /// <summary>
        /// 啪啦数据读取器
        /// </summary>
        public class Reader : IPLComponent<Reader>
        {
            private PLViews Views { get; set; }
            private MySqlManager MySqlManager { get; set; }

            /// <summary>
            /// 准备读取器
            /// </summary>
            /// <param name="CORE"></param>
            public Reader Ready(ICORE CORE)
            {
                Views = CORE.Views;
                MySqlManager = CORE.MySqlManager;

                return this;
            }

            internal Reader Ready(PLViews Views, MySqlManager MySqlManager)
            {
                this.Views = Views;
                this.MySqlManager = MySqlManager;
                return this;
            }

            /// <summary>
            /// 获取指定文章数据
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public Post GetPost(int ID)
            {
                string SQL = $"SELECT * FROM `{Views.PosUnion}` WHERE ID={ID}";
                DataRow Row = MySqlManager.GetRow(SQL);

                return new Post
                {
                    ID = Convert.ToInt32(Row["ID"]),
                    GUID = Convert.ToString(Row["GUID"]),

                    CT = Convert.ToDateTime(Row["CT"]),
                    LCT = Convert.ToDateTime(Row["LCT"]),
                    Title = Convert.ToString(Row["Title"]),
                    Summary = Convert.ToString(Row["Summary"]),
                    Content = Convert.ToString(Row["Content"]),

                    Archiv = Convert.ToString(Row["Archiv"]),
                    Label = Convert.ToString(Row["Label"]),
                    Cover = Convert.ToString(Row["Cover"]),

                    Mode = Convert.ToString(Row["Mode"]),
                    Type = Convert.ToString(Row["Type"]),
                    User = Convert.ToString(Row["User"]),

                    UVCount = Convert.ToInt32(Row["UVCount"]),
                    StarCount = Convert.ToInt32(Row["StarCount"])
                };

            }
            /// <summary>
            /// 取得指定文章属性
            /// </summary>
            /// <typeparam name="T">目标属性类型</typeparam>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public object GetProperty<T>(int ID) where T : IProperty
            {
                string SQL = $"SELECT {typeof(T).Name} FROM `{Views.PosUnion}` WHERE ID = ?ID";

                return MySqlManager.GetKey(SQL, new MySqlParameter[]
                {
                    new("ID", ID)
                });
            }

            /// <summary>
            /// 获取文章数据
            /// </summary>
            /// <typeparam name="T">正则表达式匹配的属性类型</typeparam>
            /// <param name="REGEXP">正则表达式</param>
            /// <param name="IncludeNeg">是否包含消极文章(备份)</param>
            /// <returns></returns>
            public PostSet GetPost<T>(string REGEXP, bool IncludeNeg = false) where T : IProperty
            {
                string SQL;
                if (IncludeNeg == false)
                {
                    SQL = $"SELECT * FROM `{Views.PosUnion}` WHERE {typeof(T).Name} REGEXP ?REGEXP ORDER BY CT DESC";
                }
                else
                {
                    SQL = $"SELECT * FROM `{Views.NegUnion}` WHERE {typeof(T).Name} REGEXP ?REGEXP ORDER BY CT DESC";
                }

                PostSet PostSet = new PostSet();

                foreach (DataRow Row in MySqlManager.GetTable(SQL, new MySqlParameter[]
                {
                    new("REGEXP", REGEXP)
                }).Rows)
                {
                    PostSet.Add(new Post
                    {
                        ID = Convert.ToInt32(Row["ID"]),
                        GUID = Convert.ToString(Row["GUID"]),

                        CT = Convert.ToDateTime(Row["CT"]),
                        LCT = Convert.ToDateTime(Row["LCT"]),
                        Title = Convert.ToString(Row["Title"]),
                        Summary = Convert.ToString(Row["Summary"]),
                        Content = Convert.ToString(Row["Content"]),

                        Archiv = Convert.ToString(Row["Archiv"]),
                        Label = Convert.ToString(Row["Label"]),
                        Cover = Convert.ToString(Row["Cover"]),

                        Mode = Convert.ToString(Row["Mode"]),
                        Type = Convert.ToString(Row["Type"]),
                        User = Convert.ToString(Row["User"]),

                        UVCount = Convert.ToInt32(Row["UVCount"]),
                        StarCount = Convert.ToInt32(Row["StarCount"])
                    });
                }


                return PostSet;
            }
            /// <summary>
            /// 获取文章数据
            /// </summary>
            /// <typeparam name="T">正则表达式匹配的属性类型</typeparam>
            /// <param name="REGEXP">正则表达式</param>
            /// <param name="Properties">所需属性类型</param>
            /// <param name="IncludeNeg">是否包含消极文章(备份)</param>
            /// <returns></returns>
            public PostSet GetPost<T>(string REGEXP, System.Type[] Properties, bool IncludeNeg = false) where T : IProperty
            {
                /* 键名字符串格式化 */
                string KeysStr = ConvertH.ListToString(Properties, "Name", ',');
                string SQL;
                if (IncludeNeg == false)
                {
                    SQL = $"SELECT {KeysStr} FROM `{Views.PosUnion}` WHERE {typeof(T).Name} REGEXP ?REGEXP ORDER BY CT DESC";
                }
                else//显示消极
                {
                    SQL = $"SELECT {KeysStr} FROM `{Views.NegUnion}` WHERE {typeof(T).Name} REGEXP ?REGEXP ORDER BY CT DESC";
                }

                PostSet PostSet = new PostSet();

                foreach (DataRow Row in MySqlManager.GetTable(SQL, new MySqlParameter[]
                {
                    new("REGEXP", REGEXP)
                }).Rows)
                {
                    Post Post = new Post();

                    for (int i = 0; i < Properties.Length; i++)
                    {
                        Post[Properties[i].Name] = Row.ItemArray[i];
                    }

                    PostSet.Add(Post);
                }

                return PostSet;
            }

            /// <summary>
            /// 取得具有比目标文章的指定属性具有更大的值的文章ID
            /// </summary>
            /// <typeparam name="T">指定属性</typeparam>
            /// <param name="ID">目标文章的ID</param>
            /// <returns>不存在符合要求的ID时，返回-1</returns>
            public int Bigger<T>(int ID)
            {
                string SQL;

                if (typeof(T) == typeof(ID))/* 对查询ID有优化 */
                {
                    SQL = $"SELECT ID FROM `{Views.PosUnion}` WHERE ID=( SELECT min(ID) FROM `{Views.PosUnion}` WHERE ID > {ID})";
                }
                else
                {
                    SQL = string.Format
                    (
                    $"SELECT ID FROM `{0}` WHERE {1}=( SELECT min({1}) FROM `{0}` WHERE {1} > ( SELECT {1} FROM `{0}` WHERE ID = {2} ))"
                    , Views.PosUnion, typeof(T).Name, ID
                    );
                }
                object NextID = MySqlManager.GetKey(SQL);

                return NextID == null ? -1 : Convert.ToInt32(NextID);

            }
            /// <summary>
            /// 取得具有比目标文章的指定属性具有更大的值的文章ID
            /// </summary>
            /// <typeparam name="T">指定属性</typeparam>
            /// <param name="ID">目标文章的ID</param>
            /// <param name="REGEXP">正则表达式</param>
            /// <param name="Property">用于被正则表达式筛选的属性</param>
            /// <returns>不存在符合要求的ID时，返回-1</returns>
            public int Bigger<T>(int ID, string REGEXP, System.Type Property)
            {
                string SQL;
                if (typeof(T) == typeof(ID))
                {
                    SQL = string.Format
                    (
                    "SELECT ID FROM `{0}` WHERE {1}=( SELECT min({1}) FROM `{0}` WHERE ID > {2} AND {3} REGEXP ?REGEXP )"
                    , Views.PosUnion, typeof(T).Name, ID, Property.Name
                    );
                }
                else
                {
                    SQL = string.Format
                    (
                    "SELECT ID FROM `{0}` WHERE {1}=( SELECT min({1}) FROM `{0}` WHERE {1} > ( SELECT {1} FROM `{0}` WHERE ID = ?ID ) AND {2} REGEXP ?REGEXP )"
                    , Views.PosUnion, typeof(T).Name, Property.Name
                    );
                }

                object NextID = MySqlManager.GetKey(SQL, new MySqlParameter[]
                {
                    new("ID", ID),
                    new("REGEXP", REGEXP)
                });

                return NextID == null ? -1 : Convert.ToInt32(NextID);
            }
            /// <summary>
            /// 取得具有比目标文章的指定属性具有更小的值的文章ID
            /// </summary>
            /// <typeparam name="T">指定属性</typeparam>
            /// <param name="ID">目标文章的ID</param>
            /// <returns>不存在符合要求的ID时，返回-1</returns>
            public int Smaller<T>(int ID)
            {
                string SQL;

                if (typeof(T) == typeof(ID))/* 对查询ID有优化 */
                {
                    SQL = $"SELECT ID FROM `{Views.PosUnion}` WHERE ID=( SELECT max(ID) FROM `{Views.PosUnion}` WHERE ID < {ID})";
                }
                else
                {
                    SQL = string.Format
                    (
                    "SELECT ID FROM `{0}` WHERE {1}=( SELECT max({1}) FROM `{0}` WHERE {1} < ( SELECT {1} FROM `{0}` WHERE ID = {2} ))"
                    , Views.PosUnion, typeof(T).Name, ID
                    );
                }
                object PrevID = MySqlManager.GetKey(SQL);

                return PrevID == null ? -1 : Convert.ToInt32(PrevID);
            }
            /// <summary>
            /// 取得具有比目标文章的指定属性具有更小的值的文章ID
            /// </summary>
            /// <typeparam name="T">指定属性</typeparam>
            /// <param name="ID">目标文章的ID</param>
            /// <param name="REGEXP">正则表达式</param>
            /// <param name="Property">用于被正则表达式筛选的属性</param>
            /// <returns>不存在符合要求的ID时，返回-1</returns>
            public int Smaller<T>(int ID, string REGEXP, System.Type Property)
            {
                string SQL;
                if (typeof(T) == typeof(ID))
                {
                    SQL = string.Format
                    (
                    "SELECT ID FROM `{0}` WHERE {1}=( SELECT max({1}) FROM `{0}` WHERE ID < {2} AND {3} REGEXP ?REGEXP )"
                    , Views.PosUnion, typeof(T).Name, ID, Property.Name
                    );
                }
                else
                {
                    SQL = string.Format
                    (
                    "SELECT ID FROM `{0}` WHERE {1}=( SELECT max({1}) FROM `{0}` WHERE {1} < ( SELECT {1} FROM `{0}` WHERE ID = ?ID ) AND {2} REGEXP ?REGEXP )"
                    , Views.PosUnion, typeof(T).Name, Property.Name
                    );
                }

                object PrevID = MySqlManager.GetKey(SQL, new MySqlParameter[]
                {
                    new("ID", ID),
                    new("REGEXP", REGEXP)
                });

                return PrevID == null ? -1 : Convert.ToInt32(PrevID);
            }
        }
        /// <summary>
        /// 啪啦数据修改器
        /// </summary>
        public class Writer : IPLComponent<Writer>
        {
            private PLTables Tables { get; set; }
            private MySqlManager MySqlManager { get; set; }

            private string UserGUID;

            /// <summary>
            /// 准备修改器
            /// </summary>
            /// <param name="CORE"></param>
            public Writer Ready(ICORE CORE)
            {
                Tables = CORE.Tables;
                MySqlManager = CORE.MySqlManager;
                UserGUID = CORE.UserGUID;

                return this;
            }

            internal Writer Ready(PLTables Tables, MySqlManager MySqlManager, string UserGUID)
            {
                this.Tables = Tables;
                this.MySqlManager = MySqlManager;
                this.UserGUID = UserGUID;

                return this;
            }

            /// <summary>
            /// 得到最大文章ID（私有）
            /// </summary>
            /// <returns></returns>
            internal int GetMaxID()
            {
                string SQL = $"SELECT MAX(ID) FROM {Tables.Index}";
                var value = MySqlManager.GetKey(SQL);
                /* 若取不到最大ID(没有任何文章时)，返回12000作为初始ID */
                return Convert.ToInt32(value == DBNull.Value ? 12000 : value);
            }
            /// <summary>
            /// 得到最小文章ID（私有）
            /// </summary>
            /// <returns>错误则返回-1</returns>
            internal int GetMinID()
            {
                string SQL = $"SELECT MIN(ID) FROM {Tables.Index}";
                var value = MySqlManager.GetKey(SQL);
                /* 若取不到最大ID(没有任何文章时)，返回12000作为初始ID */
                return Convert.ToInt32(value == DBNull.Value ? 12000 : value);
            }
            /// <summary>
            /// 获取指定文章的积极备份的GUID
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            internal string GetPositiveGUID(int ID)
            {
                return Convert.ToString(MySqlManager.GetKey($"SELECT GUID FROM {Tables.Index} WHERE ID={ID}"));
            }
            /// <summary>
            /// 获取指定文章的消极备份的GUID
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            internal string GetNegativeGUID(int ID)
            {
                return Convert.ToString(MySqlManager.GetKey(
                    string.Format
                    (
                    "SELECT {1}.GUID FROM {0} JOIN {1} ON {0}.ID={1}.ID AND {0}.GUID<>{1}.GUID WHERE {0}.ID={2}"
                    , Tables.Index, Tables.Backup, ID
                    )
                    ));
            }


            /*
            Reg       注册文章：新建一个拷贝，并将index指向该拷贝
            Dispose   注销文章：删除所有拷贝和index指向

            Update    更新拷贝：新建一个拷贝，并将index更改为指向该拷贝
            Delete    删除拷贝：删除指定拷贝，且该拷贝不能为当前index指向
            Apply     应用拷贝：将现有index指向删除（顶出），然后将index指向设置为指定文章拷贝
            Rollback  回滚拷贝：将现有index指向删除（顶出），然后将index指向设置到另一个最近更新的拷贝
            Release   释放拷贝：删除非当前index指向的所有拷贝
            */

            /// <summary>
            /// 注册文章
            /// </summary>
            /// <param name="Post">文章数据（其中的ID、GUID、CT、LCT、User由系统生成）</param>
            /// <returns>返回受影响的行数</returns>
            public bool Reg(Post Post)
            {
                DateTime t = DateTime.Now;

                string SQL = $"INSERT INTO {Tables.Index}" +
                            " ( ID, GUID, CT, Mode, Type, User, UVCount, StarCount) VALUES" +
                            " (?ID,?GUID,?CT,?Mode,?Type,?User,?UVCount,?StarCount);" +
                            $"INSERT INTO {Tables.Backup}" +
                            " ( ID, GUID, LCT, Title, Summary, Content, Archiv, Label, Cover) VALUES" +
                            " (?ID,?GUID,?LCT,?Title,?Summary,?Content,?Archiv,?Label,?Cover);";


                MySqlParameter[] parameters =
                {
                    new("ID", GetMaxID() + 1 ),
                    new("GUID", MathH.GenerateGUID("N") ),

                    new("CT", t),
                    new("LCT", t),

                    new("User", UserGUID),/* 使用登录内核的用户GUID */

                    /* 可传参数 */
                    new("Mode", Post.Mode),
                    new("Type", Post.Type),

                    new("UVCount", Post.UVCount),
                    new("StarCount", Post.StarCount),

                    new("Title", Post.Title),
                    new("Summary", Post.Summary ),
                    new("Content", Post.Content ),

                    new("Archiv", Post.Archiv ),
                    new("Label", Post.Label ),
                    new("Cover", Post.Cover )
                };

                MySqlCommand MySqlCommand = new MySqlCommand(SQL, MySqlManager.Connection);
                MySqlCommand.Parameters.AddRange(parameters);

                /* 开始事务 */
                MySqlCommand.Transaction = MySqlManager.Connection.BeginTransaction();

                if (MySqlCommand.ExecuteNonQuery() == 2)
                {
                    /* 指向表和拷贝表分别添加1行数据 */
                    MySqlCommand.Transaction.Commit();
                    return true;
                }
                else
                {
                    MySqlCommand.Transaction.Rollback();
                    return false;
                }
            }
            /// <summary>
            /// 注销文章
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public bool Dispose(int ID)
            {
                /* int参数无法用于参数化攻击 */
                MySqlCommand MySqlCommand = new MySqlCommand
                {
                    CommandText =
                    $"DELETE FROM {Tables.Index} WHERE ID={ID};DELETE FROM {Tables.Backup} WHERE ID={ID};",

                    Connection = MySqlManager.Connection,

                    /* 开始事务 */
                    Transaction = MySqlManager.Connection.BeginTransaction()
                };

                if (MySqlCommand.ExecuteNonQuery() >= 2)
                {
                    /* 指向表只删除1行数据，拷贝表至少删除1行数据 */
                    MySqlCommand.Transaction.Commit();
                    return true;
                }
                else
                {
                    MySqlCommand.Transaction.Rollback();
                    return false;
                }
            }
            /// <summary>
            /// 更新文章
            /// </summary>
            /// <param name="Post">文章数据</param>
            /// <returns></returns>
            public bool Update(Post Post)
            {
                string SQL =
                    $"UPDATE {Tables.Index} SET GUID=?GUID, Mode=?Mode, Type=?Type, User=?User, UVCount=?UVCount, StarCount=?StarCount WHERE ID=?ID;" +
                    $"INSERT INTO {Tables.Backup}" +
                    " ( ID, GUID, LCT, Title, Summary, Content, Archiv, Label, Cover) VALUES" +
                    " (?ID,?GUID,?LCT,?Title,?Summary,?Content,?Archiv,?Label,?Cover);";

                MySqlParameter[] parameters =
                {
                    new("GUID", MathH.GenerateGUID("N") ),
                    new("LCT", DateTime.Now ),

                    new("User", UserGUID),/* 使用登录内核的用户GUID */

                    /* 可传参数 */
                    new("ID", Post.ID),

                    new("Mode", Post.Mode),
                    new("Type", Post.Type),

                    new("UVCount", Post.UVCount),
                    new("StarCount", Post.StarCount),

                    new("Title", Post.Title),
                    new("Summary", Post.Summary),
                    new("Content", Post.Content),

                    new("Archiv", Post.Archiv),
                    new("Label", Post.Label),
                    new("Cover", Post.Cover)
                };

                MySqlCommand MySqlCommand = new MySqlCommand(SQL, MySqlManager.Connection);
                MySqlCommand.Parameters.AddRange(parameters);

                /* 开始事务 */
                MySqlCommand.Transaction = MySqlManager.Connection.BeginTransaction();

                if (MySqlCommand.ExecuteNonQuery() == 2)
                {
                    /* 指向表修改1行数据，拷贝表添加1行数据 */
                    MySqlCommand.Transaction.Commit();
                    return true;
                }
                else
                {
                    MySqlCommand.Transaction.Rollback();
                    return false;
                    /* 由于GUID更新，影响行始终为2，若出现其他情况则一定为错误 */
                }
            }

            /// <summary>
            /// 删除拷贝
            /// </summary>
            /// <param name="GUID">目标文章的GUID</param>
            /// <returns></returns>
            public bool Delete(string GUID)
            {
                string SQL = string.Format
                    (
                    "DELETE {1} FROM {0} INNER JOIN {1} ON {0}.ID={1}.ID AND {0}.GUID<>{1}.GUID AND {1}.GUID = ?GUID"
                    , Tables.Index, Tables.Backup
                    );

                MySqlParameter[] parameters =
                {
                    new("GUID", GUID )
                };

                MySqlCommand MySqlCommand = new MySqlCommand(SQL, MySqlManager.Connection);
                MySqlCommand.Parameters.AddRange(parameters);

                /* 开始事务 */
                MySqlCommand.Transaction = MySqlManager.Connection.BeginTransaction();

                if (MySqlCommand.ExecuteNonQuery() == 1)
                {
                    /* 拷贝表删除一行数据 */
                    MySqlCommand.Transaction.Commit();
                    return true;
                }
                else
                {
                    MySqlCommand.Transaction.Rollback();
                    return false;
                }
            }
            /// <summary>
            /// 应用拷贝
            /// </summary>
            /// <param name="GUID">目标拷贝的GUID</param>
            /// <returns></returns>
            public bool Apply(string GUID)
            {
                /* 此处，即使SQL注入造成了ID错误，由于第二步参数化查询的作用，GUID也会造成错误无法成功攻击 */
                object ID = MySqlManager.GetKey($"SELECT ID FROM {Tables.Backup} WHERE GUID = '{GUID}'");

                string SQL =
                    $"DELETE FROM {Tables.Backup} WHERE GUID = (SELECT GUID FROM {Tables.Index} WHERE ID = ?ID);" +
                    $"UPDATE {Tables.Index} SET GUID = ?GUID WHERE ID = ?ID;";

                MySqlParameter[] parameters =
                {
                    new("ID", ID),
                    new("GUID", GUID)
                };

                MySqlCommand MySqlCommand = new MySqlCommand(SQL, MySqlManager.Connection);
                MySqlCommand.Parameters.AddRange(parameters);

                /* 开始事务 */
                MySqlCommand.Transaction = MySqlManager.Connection.BeginTransaction();

                if (MySqlCommand.ExecuteNonQuery() == 2)
                {
                    /* 指向表修改1行数据，拷贝表删除一行数据 */
                    MySqlCommand.Transaction.Commit();
                    return true;
                }
                else
                {
                    MySqlCommand.Transaction.Rollback();
                    return false;
                }
            }
            /// <summary>
            /// 回滚拷贝
            /// </summary>
            /// <param name="ID">目标文章的ID</param>
            /// <returns></returns>
            public bool Rollback(int ID)
            {
                MySqlCommand MySqlCommand = new MySqlCommand
                {
                    CommandText = string.Format
                    (
                    "DELETE {1} FROM {0} INNER JOIN {1} ON {0}.GUID={1}.GUID AND {0}.ID={2};" +
                    "UPDATE {0} SET GUID = (SELECT GUID FROM {1} WHERE ID={2} ORDER BY LCT DESC LIMIT 0,1) WHERE ID={2};"
                    , Tables.Index, Tables.Backup, ID
                    ),

                    Connection = MySqlManager.Connection,

                    /* 开始事务 */
                    Transaction = MySqlManager.Connection.BeginTransaction()
                };

                if (MySqlCommand.ExecuteNonQuery() == 2)
                {
                    /* 指向表修改1行数据，拷贝表删除1行数据 */
                    MySqlCommand.Transaction.Commit();
                    return true;
                }
                else
                {
                    MySqlCommand.Transaction.Rollback();
                    return false;
                }
            }
            /// <summary>
            /// 释放拷贝
            /// </summary>
            /// <param name="ID">目标文章的ID</param>
            /// <returns></returns>
            public bool Release(int ID)
            {
                MySqlCommand MySqlCommand = new MySqlCommand
                {
                    CommandText = string.Format
                   (
                   "DELETE {1} FROM {0} INNER JOIN {1} ON {0}.ID={1}.ID AND {0}.GUID<>{1}.GUID AND {0}.ID={2}"
                   , Tables.Index, Tables.Backup, ID
                   ),

                    Connection = MySqlManager.Connection,

                    /* 开始事务 */
                    Transaction = MySqlManager.Connection.BeginTransaction()
                };

                if (MySqlCommand.ExecuteNonQuery() >= 0)
                {
                    /* 删除拷贝表的所有冗余，不存在冗余时影响行数为0 */
                    MySqlCommand.Transaction.Commit();
                    return true;
                }
                else
                {
                    MySqlCommand.Transaction.Rollback();
                    return false;
                }
            }

            /// <summary>
            /// 将目标文章指向的类型设为：未设置
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public bool UnsetType(int ID)
            {

                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Index,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlManager.UpdateKey(MySqlKey, "Type", "");
            }
            /// <summary>
            /// 将目标文章指向的类型设为：便签
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public bool NoteType(int ID)
            {

                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Index,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlManager.UpdateKey(MySqlKey, "Type", "note");
            }

            /// <summary>
            /// 将目标文章指向的模式设为：未设置
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public bool UnsetMode(int ID)
            {
                //初始化键定位
                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Index,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlManager.UpdateKey(MySqlKey, "Mode", "");
            }
            /// <summary>
            /// 将目标文章指向的模式设为：隐藏
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public bool HiddenMode(int ID)
            {
                //初始化键定位
                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Index,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlManager.UpdateKey(MySqlKey, "Mode", "hidden");
            }
            /// <summary>
            /// 将目标文章指向的模式设为：计划中
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public bool ScheduledMode(int ID)
            {
                //初始化键定位
                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Index,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlManager.UpdateKey(MySqlKey, "Mode", "scheduled");
            }
            /// <summary>
            /// 将目标文章指向的模式设为：已归档
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public bool ArchivedMode(int ID)
            {
                //初始化键定位
                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Index,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlManager.UpdateKey(MySqlKey, "Mode", "archived");
            }

            /// <summary>
            /// 通用文章指向更新器
            /// </summary>
            /// <typeparam name="T">目标属性类型</typeparam>
            /// <param name="ID">目标文章ID</param>
            /// <param name="Value">新属性值</param>
            /// <returns></returns>
            public bool UpdateIndex<T>(int ID, object Value) where T : IProperty
            {
                //初始化键定位
                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Index,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlManager.UpdateKey(MySqlKey, typeof(T).Name, Convert.ToString(Value));
            }
            /// <summary>
            /// 通用文章拷贝更新器
            /// </summary>
            /// <typeparam name="T">目标属性类型</typeparam>
            /// <param name="ID">目标拷贝GUID</param>
            /// <param name="Value">新属性值</param>
            /// <returns></returns>
            public bool UpdateBackup<T>(int ID, object Value) where T : IProperty
            {
                //初始化键定位
                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Backup,
                    Name = "GUID",
                    Val = GetPositiveGUID(ID)
                };
                return MySqlManager.UpdateKey(MySqlKey, typeof(T).Name, Convert.ToString(Value));
            }

            /// <summary>
            /// 检测ID、GUID是否匹配，之后合并Post数据表
            /// </summary>
            /// <parmm name="Index">索引表Post实例</parmm>
            /// <parmm name="Backup">主表Post实例</parmm>
            /// <returns></returns>
            public static Post Join(Post Index, Post Backup)
            {
                if (Index.ID == Backup.ID && Index.GUID == Backup.GUID)
                {
                    return new Post
                    {
                        ID = Index.ID,
                        GUID = Index.GUID,

                        Mode = Index.Mode,
                        Type = Index.Type,

                        Title = Backup.Title,
                        Summary = Backup.Summary,
                        Content = Backup.Content,

                        User = Index.User,
                        Archiv = Backup.Archiv,
                        Label = Backup.Label,

                        CT = Index.CT,
                        LCT = Backup.LCT,

                        UVCount = Index.UVCount,
                        StarCount = Index.StarCount,

                        Cover = Backup.Cover
                    };
                }
                else
                {
                    throw new Exception("ERROR");
                }
            }
            /// <summary>
            /// 强制合并Post数据表（风险性重载，不考虑ID、GUID是否匹配，调用不当易引发逻辑故障）
            /// </summary>
            /// <parmm name="Index">索引表Post实例</parmm>
            /// <parmm name="Backup">主表Post实例</parmm>
            /// <returns>始终返回以Index的ID、GUID为最终合并结果的Post实例</returns>
            public static Post ForcedJoin(Post Index, Post Backup)
            {
                return new Post
                {
                    ID = Index.ID,
                    GUID = Index.GUID,

                    Mode = Index.Mode,
                    Type = Index.Type,

                    Title = Backup.Title,
                    Summary = Backup.Summary,
                    Content = Backup.Content,

                    User = Index.User,
                    Archiv = Backup.Archiv,
                    Label = Backup.Label,

                    CT = Index.CT,
                    LCT = Backup.LCT,

                    UVCount = Index.UVCount,
                    StarCount = Index.StarCount,

                    Cover = Backup.Cover
                };
            }
        }
        /// <summary>
        /// 啪啦计数管理器
        /// </summary>
        public class Counter : IPLComponent<Counter>
        {
            private PLTables Tables { get; set; }
            private MySqlManager MySqlManager { get; set; }

            /// <summary>
            /// 准备计数器
            /// </summary>
            /// <param name="CORE"></param>
            public Counter Ready(ICORE CORE)
            {
                Tables = CORE.Tables;
                MySqlManager = CORE.MySqlManager;
                return this;
            }

            internal Counter Ready(PLTables Tables, MySqlManager MySqlManager)
            {
                this.Tables = Tables;
                this.MySqlManager = MySqlManager;
                return this;
            }

            /// <summary>
            /// 文章计数
            /// </summary>
            public int TotalPostCount
            {
                get { return GetPostCountByMode("^"); }
            }
            /// <summary>
            /// 拷贝计数
            /// </summary>
            public int BackupCount
            {
                get { return GetBackupCount(); }
            }

            /// <summary>
            /// 隐藏文章计数
            /// </summary>
            public int HiddenCount
            {
                get { return GetPostCountByMode("^hidden$"); }
            }
            /// <summary>
            /// 展示中文章计数
            /// </summary>
            public int OnDisplayCount
            {
                get { return GetPostCountByMode("^$"); }
            }
            /// <summary>
            /// 归档中文章计数
            /// </summary>
            public int ArchivedCount
            {
                get { return GetPostCountByMode("^archived$"); }
            }
            /// <summary>
            /// 计划中文章计数
            /// </summary>
            public int ScheduledCount
            {
                get { return GetPostCountByMode("^scheduled$"); }
            }

            private int GetPostCountByMode(string REGEXP)
            {
                object Count = MySqlManager.GetKey($"SELECT Count(*) FROM {Tables.Index} WHERE Mode REGEXP '{REGEXP}';");
                return Count == DBNull.Value ? 0 : Convert.ToInt32(Count);
            }
            private int GetBackupCount()
            {
                object Count = MySqlManager.GetKey(string.Format("SELECT COUNT(*) FROM {0},{1} WHERE {0}.ID={1}.ID AND {0}.GUID<>{1}.GUID;", Tables.Index, Tables.Backup));
                return Count == DBNull.Value ? 0 : Convert.ToInt32(Count);
            }
        }
    }
}
