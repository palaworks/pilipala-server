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
using WaterLibrary.Util;
using WaterLibrary.pilipala.Database;
using WaterLibrary.pilipala.Entity;
using WaterLibrary.pilipala.Entity.PostProp;
using Type = WaterLibrary.pilipala.Entity.PostProp.Type;


namespace WaterLibrary.pilipala
{
    namespace Database
    {
        /// <summary>
        /// 数据库表集合
        /// 用户表
        /// 索引表
        /// 主表
        /// 归档表
        /// 评论表
        /// </summary>
        public record PLTables(string User, string Index, string Stack, string Archive, string Comment);
        /// <summary>
        /// 数据库视图视图集合
        /// 显性联合视图（不包含备份）
        /// 隐性联合视图（包含备份）
        /// </summary>
        public record PLViews(string PosUnion, string NegUnion);

        /// <summary>
        /// 啪啦数据库操作盒
        /// </summary>
        public struct PLDatabase
        {
            /// <summary>
            /// 数据库名
            /// </summary>
            public string Database { get; set; }
            /// <summary>
            /// 数据表
            /// </summary>
            public PLTables Tables { get; set; }
            /// <summary>
            /// 数据视图
            /// </summary>
            public (PLViews CleanViews, PLViews DirtyViews) ViewsSet { get; set; }
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

    }
    interface IPLComponentFactory
    {
        void Ready(ICORE CORE, Components.User User);
    }
    /// <summary>
    /// 噼里啪啦内核接口
    /// </summary>
    public interface ICORE
    {
        /// <summary>
        /// 内核表访问器
        /// </summary>
        PLTables Tables { get; }
        /// <summary>
        /// 内核视图访问器
        /// </summary>
        (PLViews CleanViews, PLViews DirtyViews) ViewsSet { get; }
        /// <summary>
        /// 内核MySql数据库控制器
        /// </summary>
        MySqlManager MySqlManager { get; }

        /// <summary>
        /// 开始配件连接事件
        /// </summary>
        public event CoreReadyEventHandler CoreReady;

        /// <summary>
        /// 登录到内核的用户UUID
        /// </summary>
        string UserAccount { get; }

        /// <summary>
        /// 以其他用户身份启动内核
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="UserPWD">用户密码</param>
        /// <returns></returns>
        public Components.User Run(string UserName, string UserPWD);
        /// <summary>
        /// 以初始化用户身份启动内核
        /// </summary>
        /// <returns></returns>
        public void Run();
    }



    /// <summary>
    /// 内核准备完成委托
    /// </summary>
    /// <param name="CORE">内核对象</param>
    /// <param name="User">用户对象</param>
    public delegate void CoreReadyEventHandler(ICORE CORE, Components.User User);
    /// <summary>
    /// pilipala内核
    /// </summary>
    public class CORE : ICORE
    {
        /// <summary>
        /// 内核准备完成事件
        /// </summary>
        public event CoreReadyEventHandler CoreReady;

        /// <summary>
        /// 核心表结构
        /// </summary>
        public PLTables Tables { get; private set; }
        /// <summary>
        /// 核心视图结构
        /// </summary>
        public (PLViews CleanViews, PLViews DirtyViews) ViewsSet { get; private set; }
        /// <summary>
        /// MySql控制器
        /// </summary>
        public MySqlManager MySqlManager { get; init; }

        /// <summary>
        /// 登录内核的用户UUID
        /// </summary>
        public string UserAccount { get; private set; }

        /// <summary>
        /// 初始化pilipala内核
        /// </summary>
        /// <param name="PLDatabase">pilipala数据库信息</param>
        public CORE(PLDatabase PLDatabase)
        {
            MySqlManager = PLDatabase.MySqlManager;
            Tables = PLDatabase.Tables;
            ViewsSet = PLDatabase.ViewsSet;
        }

        /// <summary>
        /// 以有效用户身份启动内核
        /// </summary>
        /// <param name="UserAccount">用户账号</param>
        /// <param name="UserPWD">用户密码</param>
        /// <returns></returns>
        public Components.User Run(string UserAccount, string UserPWD)
        {
            string SQL = $"SELECT COUNT(*) FROM {Tables.User} WHERE Account = ?UserAccount AND PWD = ?UserPWD";

            if (MySqlManager.GetKey(
                SQL,
                new("UserAccount", UserAccount), new("UserPWD", MathH.MD5(UserPWD))
                )
            .ToString() == "1")
            {
                Components.User User = new Components.User(Tables, MySqlManager, UserAccount);

                /* 触发内核准备完成事件，并分发数据到工厂 */
                CoreReady(this, User);
                /* 验证成功，赋值内核UserAccount */
                this.UserAccount = UserAccount;
                /* 返回用户对象 */
                return User;
            }
            else
            {
                throw new Exception("非法的用户签名");
            }
        }
        /// <summary>
        /// 以访客身份启动内核
        /// </summary>
        /// <returns></returns>
        public void Run()
        {
            CoreReady(this, null);/* 分配一个空用户给工厂 */
        }
    }

    namespace Entity
    {
        /// <summary>
        /// 文章
        /// </summary>
        public class Post
        {
            /// <summary>
            /// 索引器
            /// </summary>
            /// <param name="Key">索引名</param>
            /// <returns></returns>
            public object this[string Key]
            {
                /* 通过反射获取属性 */
                get => GetType().GetProperty(Key).GetValue(this);
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
                PostID = -1;
                UUID = "";

                Title = "";
                Summary = "";
                Content = "";
                Cover = "";

                ArchiveID = -1;
                Label = "";

                Mode = "";
                Type = "";
                User = "";

                CT = new();
                LCT = new();

                UVCount = -1;
                StarCount = -1;

                PropertyContainer = new();
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
            public int PostID { get; set; }
            /// <summary>
            /// 全局标识
            /// </summary>
            public string UUID { get; set; }

            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 概要
            /// </summary>
            public string Summary { get; set; }
            /// <summary>
            /// 尝试概要
            /// </summary>
            /// <param name="todo">概要为空时的操作</param>
            /// <returns></returns>
            public string TrySummary(Func<string> todo)
            {
                return Summary switch
                {
                    "" => todo(),
                    _ => Summary
                };
            }
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
            /// 归档ID
            /// </summary>
            public int ArchiveID { get; set; }

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
        /// <summary>
        /// 文章属性枚举
        /// </summary>
        public enum PostPropEnum
        {
            /// <summary>
            /// 文章索引
            /// </summary>
            PostID,
            /// <summary>
            /// 文章全局标识
            /// </summary>
            UUID,
            /// <summary>
            /// 文章标题
            /// </summary>
            Title,
            /// <summary>
            /// 文章摘要
            /// </summary>
            Summary,
            /// <summary>
            /// 文章内容
            /// </summary>
            Content,
            /// <summary>
            /// 封面
            /// </summary>
            Cover,
            /// <summary>
            /// 归档
            /// </summary>
            ArchiveID,
            /// <summary>
            /// 标签
            /// </summary>
            Label,
            /// <summary>
            /// 模式
            /// </summary>
            Mode,
            /// <summary>
            /// 类型
            /// </summary>
            Type,
            /// <summary>
            /// 作者
            /// </summary>
            User,
            /// <summary>
            /// 创建时间
            /// </summary>
            CT,
            /// <summary>
            /// 最后修改时间
            /// </summary>
            LCT,
            /// <summary>
            /// 浏览计数
            /// </summary>
            UVCount,
            /// <summary>
            /// 星星计数
            /// </summary>
            StarCount
        }
        //思路：利用枚举和反射取得枚举名的方式，代替部分PostProp的使用（方法参数，而不是泛型）
        namespace PostProp
        {
            /// <summary>
            /// 文章属性接口
            /// </summary>
            public interface IPostProp
            {

            }

            /// <summary>
            /// 文章索引
            /// </summary>
            public struct PostID : IPostProp
            {

            }
            /// <summary>
            /// 文章全局标识
            /// </summary>
            public struct UUID : IPostProp
            {

            }

            /// <summary>
            /// 标题
            /// </summary>
            public struct Title : IPostProp
            {

            }
            /// <summary>
            /// 摘要
            /// </summary>
            public struct Summary : IPostProp
            {

            }
            /// <summary>
            /// 内容
            /// </summary>
            public struct Content : IPostProp
            {

            }
            /// <summary>
            /// 封面
            /// </summary>
            public struct Cover : IPostProp
            {

            }

            /// <summary>
            /// 归档索引
            /// </summary>
            public struct ArchiveID : IPostProp
            {

            }
            /// <summary>
            /// 标签
            /// </summary>
            public struct Label : IPostProp
            {

            }

            /// <summary>
            /// 模式
            /// </summary>
            public struct Mode : IPostProp
            {
                /// <summary>
                /// 状态枚举
                /// </summary>
                public enum States
                {
                    /// <summary>
                    /// 未设置
                    /// </summary>
                    /// <remarks>默认的文章模式，不带有任何模式特性</remarks>
                    Unset,
                    /// <summary>
                    /// 隐藏
                    /// </summary>
                    /// <remarks>文章被隐藏，此状态下的文章不会被展示</remarks>
                    Hidden,
                    /// <summary>
                    /// 计划
                    /// </summary>
                    /// <remarks>表示文章处于计划状态</remarks>
                    Scheduled,
                    /// <summary>
                    /// 归档
                    /// </summary>
                    /// <remarks>表示文章处于归档状态</remarks>
                    Archived
                }
            }

            /// <summary>
            /// 类型
            /// </summary>
            public struct Type : IPostProp
            {
                /// <summary>
                /// 状态枚举
                /// </summary>
                public enum States
                {
                    /// <summary>
                    /// 未设置
                    /// </summary>
                    /// <remarks>默认的文章类型，不带有任何类型特性</remarks>
                    Unset,
                    /// <summary>
                    /// 便签
                    /// </summary>
                    /// <remarks>表示文章以便签形式展示</remarks>
                    Note,
                }
            }
            /// <summary>
            /// 作者
            /// </summary>
            public struct User : IPostProp
            {

            }

            /// <summary>
            /// 创建时间
            /// </summary>
            public struct CT : IPostProp
            {

            }
            /// <summary>
            /// 最后修改时间
            /// </summary>
            public struct LCT : IPostProp
            {

            }

            /// <summary>
            /// 浏览计数
            /// </summary>
            public struct UVCount : IPostProp
            {

            }
            /// <summary>
            /// 星星计数
            /// </summary>
            public struct StarCount : IPostProp
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
            private User User;
            /// <summary>
            /// 准备完成
            /// </summary>
            /// <param name="CORE">内核对象</param>
            /// <param name="User">用户对象</param>
            public void Ready(ICORE CORE, User User)
            {
                (this.CORE, this.User) = (CORE, User);
            }


            /// <summary>
            /// 生成权限管理组件
            /// </summary>
            /// <returns></returns>
            public Authentication GenAuthentication() => new(CORE.Tables, CORE.MySqlManager, User);
            /// <summary>
            /// 生成读组件
            /// </summary>
            /// <param name="ReadMode">读取模式枚举</param>
            /// <param name="WithRawMode">以原始数据读取模式初始化(读取到的数据包含隐性文章)</param>
            /// <returns></returns>
            public Reader GenReader(Reader.ReadMode ReadMode, bool WithRawMode = false)
            {
                return ReadMode switch
                {
                    Reader.ReadMode.CleanRead => WithRawMode switch
                    {
                        false => new(CORE.ViewsSet.CleanViews.PosUnion, CORE.MySqlManager),
                        true => new(CORE.ViewsSet.CleanViews.NegUnion, CORE.MySqlManager),
                    },
                    Reader.ReadMode.DirtyRead => WithRawMode switch
                    {
                        false => new(CORE.ViewsSet.DirtyViews.PosUnion, CORE.MySqlManager),
                        true => new(CORE.ViewsSet.DirtyViews.NegUnion, CORE.MySqlManager),
                    },
                    _ => throw new NotImplementedException(),
                };
            }
            /// <summary>
            /// 生成写组件
            /// </summary>
            /// <returns></returns>
            public Writer GenWriter() => new(CORE.Tables.Index, CORE.Tables.Stack, CORE.MySqlManager);
            /// <summary>
            /// 生成计数组件
            /// </summary>
            /// <returns></returns>
            public Counter GenCounter() => new(CORE.Tables.Index, CORE.Tables.Stack, CORE.MySqlManager);
            /// <summary>
            /// 生成评论湖组件
            /// </summary>
            /// <returns></returns>
            public CommentLake GenCommentLake() => new(CORE.Tables.Index, CORE.Tables.Comment, CORE.MySqlManager);
        }

        /// <summary>
        /// 权限管理组件
        /// </summary>
        public class Authentication : IPLComponent<Authentication>
        {
            private PLTables Tables { get; init; }
            private MySqlManager MySqlManager { get; init; }

            private readonly User User;

            /// <summary>
            /// 默认构造
            /// </summary>
            private Authentication() { }
            /// <summary>
            /// 工厂构造
            /// </summary>
            /// <param name="Tables">数据库表</param>
            /// <param name="MySqlManager">数据库管理器</param>
            /// <param name="User">用户对象</param>
            internal Authentication(PLTables Tables, MySqlManager MySqlManager, User User)
            {
                this.Tables = Tables;
                this.MySqlManager = MySqlManager;
                this.User = User;
            }

            /// <summary>
            /// 权限验证
            /// </summary>
            /// <param name="Token">Token</param>
            /// <param name="todo">行为委托</param>
            /// <returns></returns>
            public T Auth<T>(string Token, Func<T> todo)
            {
                return (DateTime.Now - Convert.ToDateTime(MathH.RSADecrypt(GetPrivateKey(), Token))).TotalSeconds switch
                {
                    < 10 => todo(),
                    _ => default
                };
            }

            /// <summary>
            /// 取得私钥
            /// </summary>
            /// <returns></returns>
            public string GetPrivateKey()
            {
                return MySqlManager.GetKey($"SELECT PrivateKey FROM {Tables.User} WHERE Account = '{User.Account}'").ToString();
            }
            /// <summary>
            /// 设置私钥
            /// </summary>
            /// <returns></returns>
            public bool SetPrivateKey(string PrivateKey)
            {
                return MySqlManager.UpdateKey
                    ((Tables.User, "Account", User.Account), "PrivateKey", PrivateKey);
            }
            /// <summary>
            /// 取得最后Token获取时间
            /// </summary>
            /// <returns></returns>
            public DateTime GetTokenTime()
            {
                return Convert.ToDateTime(MySqlManager.GetKey($"SELECT TokenTime FROM {Tables.User} WHERE Account = '{User.Account}'"));
            }
            /// <summary>
            /// 设置最后Token获取时间为当前时间
            /// </summary>
            /// <returns></returns>
            public bool UpdateTokenTime()
            {
                return MySqlManager.UpdateKey
                    ((Tables.User, "Account", User.Account), "TokenTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }
        /// <summary>
        /// 用户组件
        /// </summary>
        public class User : IPLComponent<User>
        {
            internal PLTables Tables { get; init; }
            internal MySqlManager MySqlManager { get; init; }

            /// <summary>
            /// 默认构造
            /// </summary>
            private User() { }
            /// <summary>
            /// 内核构造
            /// </summary>
            /// <param name="Tables">数据库表</param>
            /// <param name="MySqlManager">数据库管理器</param>
            /// <param name="UserAccount">用户账号</param>
            internal User(PLTables Tables, MySqlManager MySqlManager, string UserAccount)
            {
                this.Tables = Tables;
                this.MySqlManager = MySqlManager;
                Account = UserAccount;
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
            /// 用户账号
            /// </summary>
            public string Account { get; internal set; }

            /// <summary>
            /// 检查密码
            /// </summary>
            /// <param name="PWD">等待检查是否正确的密码</param>
            /// <returns></returns>
            public bool CheckPWD(string PWD)
            {
                return MathH.MD5(PWD) == Get("PWD");
            }
            /// <summary>
            /// 设置密码
            /// </summary>
            /// <param name="NewPWD">新的密码</param>
            /// <returns></returns>
            public bool SetPWD(string NewPWD)
            {
                return Set("PWD", NewPWD);
            }

            /// <summary>
            /// 用户名
            /// </summary>
            public string Name { get => Get("Name"); set => Set("Name", value); }
            /// <summary>
            /// 自我介绍
            /// </summary>
            public string Bio { get => Get("Bio"); set => Set("Bio", value); }
            /// <summary>
            /// 组别
            /// </summary>
            public string GroupType { get => Get("GroupType"); set => Set("GroupType", value); }
            /// <summary>
            /// 邮箱
            /// </summary>
            public string Email { get => Get("Email"); set => Set("Email", value); }
            /// <summary>
            /// 头像(链接)
            /// </summary>
            public string Avatar { get => Get("Avatar"); set => Set("Avatar", value); }

            private string Get(string Key)
            {
                return MySqlManager.GetRow($"SELECT {Key} FROM {Tables.User} WHERE Account = '{Account}'")[Key].ToString();
            }
            private bool Set(string Key, string Value)
            {
                return MySqlManager.UpdateKey((Tables.User, "Account", Account), Key, Value);
            }
        }
        /// <summary>
        /// 数据读取组件
        /// </summary>
        public class Reader : IPLComponent<Reader>
        {
            /// <summary>
            /// 读取模式枚举
            /// </summary>
            public enum ReadMode
            {
                /// <summary>
                /// 净读，表示不读取隐藏文章。适用于面向访问者的渲染
                /// </summary>
                CleanRead = 0,
                /// <summary>
                /// 净读，表示读取隐藏文章。适用于面向管理员的渲染
                /// </summary>
                DirtyRead = 1
            }

            private string UnionView { get; init; }

            private MySqlManager MySqlManager { get; init; }

            /// <summary>
            /// 默认构造
            /// </summary>
            private Reader() { }
            /// <summary>
            /// 工厂构造
            /// </summary>
            /// <param name="UnionView">联合视图</param>
            /// <param name="MySqlManager">数据库管理器</param>
            /// <returns></returns>
            internal Reader(string UnionView, MySqlManager MySqlManager)
            {
                this.UnionView = UnionView;
                this.MySqlManager = MySqlManager;
            }

            /// <summary>
            /// 获取指定文章数据
            /// </summary>
            /// <param name="PostID">目标文章PostID</param>
            /// <returns></returns>
            public Post GetPost(int PostID)
            {
                string SQL = $"SELECT * FROM `{UnionView}` WHERE PostID={PostID}";
                DataRow Row = MySqlManager.GetRow(SQL);

                return new Post
                {
                    PostID = Convert.ToInt32(Row["PostID"]),
                    UUID = Convert.ToString(Row["UUID"]),

                    CT = Convert.ToDateTime(Row["CT"]),
                    LCT = Convert.ToDateTime(Row["LCT"]),
                    Title = Convert.ToString(Row["Title"]),
                    Summary = Convert.ToString(Row["Summary"]),
                    Content = Convert.ToString(Row["Content"]),

                    ArchiveID = Convert.ToInt32(Row["ArchiveID"]),
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
            /// <typeparam name="Prop">目标属性类型</typeparam>
            /// <param name="PostID">目标文章PostID</param>
            /// <returns></returns>
            public object GetPostProp<Prop>(int PostID) where Prop : IPostProp
            {
                string SQL = $"SELECT {typeof(Prop).Name} FROM `{UnionView}` WHERE PostID = ?PostID";

                return MySqlManager.GetKey(SQL, new MySqlParameter[]
                {
                    new("PostID", PostID)
                });
            }

            /// <summary>
            /// 获取文章数据
            /// </summary>
            /// <typeparam name="T">正则表达式匹配的属性类型</typeparam>
            /// <param name="REGEXP">正则表达式</param>
            /// <returns></returns>
            public PostSet GetPost<T>(string REGEXP) where T : IPostProp
            {
                string SQL = $"SELECT * FROM `{UnionView}` WHERE {typeof(T).Name} REGEXP ?REGEXP ORDER BY CT DESC";

                PostSet PostSet = new PostSet();

                foreach (DataRow Row in MySqlManager.GetTable(SQL, new MySqlParameter[]
                {
                    new("REGEXP", REGEXP)
                }).Rows)
                {
                    PostSet.Add(new Post
                    {
                        PostID = Convert.ToInt32(Row["PostID"]),
                        UUID = Convert.ToString(Row["UUID"]),

                        CT = Convert.ToDateTime(Row["CT"]),
                        LCT = Convert.ToDateTime(Row["LCT"]),
                        Title = Convert.ToString(Row["Title"]),
                        Summary = Convert.ToString(Row["Summary"]),
                        Content = Convert.ToString(Row["Content"]),

                        ArchiveID = Convert.ToInt32(Row["ArchiveID"]),
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
            /// <param name="PostProps">所需属性类型</param>
            /// <returns></returns>
            public PostSet GetPost<T>(string REGEXP, params PostPropEnum[] PostProps) where T : IPostProp
            {
                /* 键名字符串格式化 */
                string KeysStr = ConvertH.ListToString(PostProps, ',');
                string SQL = $"SELECT {KeysStr} FROM `{UnionView}` WHERE {typeof(T).Name} REGEXP ?REGEXP ORDER BY CT DESC";

                PostSet PostSet = new PostSet();

                foreach (DataRow Row in MySqlManager.GetTable(SQL, new MySqlParameter[]
                {
                    new("REGEXP", REGEXP)
                }).Rows)
                {
                    Post Post = new Post();

                    for (int i = 0; i < PostProps.Length; i++)
                    {
                        Post[PostProps[i].ToString()] = Row.ItemArray[i];
                    }

                    PostSet.Add(Post);
                }

                return PostSet;
            }

            /// <summary>
            /// 取得具有比目标文章的指定属性具有更大的值的文章PostID
            /// </summary>
            /// <typeparam name="Prop">指定属性</typeparam>
            /// <param name="PostID">目标文章的PostID</param>
            /// <returns>不存在符合要求的PostID时，返回-1</returns>
            public int Bigger<Prop>(int PostID)
            {
                string SQL = (typeof(Prop) == typeof(PostID)) switch /* 对查询PostID有优化 */
                {
                    true => $"SELECT PostID FROM `{UnionView}` WHERE PostID=( SELECT min(PostID) FROM `{UnionView}` WHERE PostID > {PostID})",
                    false => string.Format
                    (
                    $"SELECT PostID FROM `{0}` WHERE {1}=( SELECT min({1}) FROM `{0}` WHERE {1} > ( SELECT {1} FROM `{0}` WHERE PostID = {2} ))"
                    , UnionView, typeof(Prop).Name, PostID
                    )
                };

                object NextPostID = MySqlManager.GetKey(SQL);

                return NextPostID == null ? -1 : Convert.ToInt32(NextPostID);

            }
            /// <summary>
            /// 取得具有比目标文章的指定属性具有更大的值的文章PostID
            /// </summary>
            /// <typeparam name="Prop">指定属性</typeparam>
            /// <param name="PostID">目标文章的PostID</param>
            /// <param name="REGEXP">正则表达式</param>
            /// <param name="PostProp">用于被正则表达式筛选的属性</param>
            /// <returns>不存在符合要求的PostID时，返回-1</returns>
            public int Bigger<Prop>(int PostID, string REGEXP, PostPropEnum PostProp)
            {
                string SQL = (typeof(Prop) == typeof(PostID)) switch
                {
                    true => string.Format
                    (
                    "SELECT PostID FROM `{0}` WHERE {1}=( SELECT min({1}) FROM `{0}` WHERE PostID > {2} AND {3} REGEXP ?REGEXP )"
                    , UnionView, typeof(Prop).Name, PostID, PostProp
                    ),
                    false => string.Format
                    (
                    "SELECT PostID FROM `{0}` WHERE {1}=( SELECT min({1}) FROM `{0}` WHERE {1} > ( SELECT {1} FROM `{0}` WHERE PostID = ?PostID ) AND {2} REGEXP ?REGEXP )"
                    , UnionView, typeof(Prop).Name, PostProp
                    )
                };

                object NextPostID = MySqlManager.GetKey(SQL, new MySqlParameter[]
                {
                    new("PostID", PostID),
                    new("REGEXP", REGEXP)
                });

                return NextPostID == null ? -1 : Convert.ToInt32(NextPostID);
            }
            /// <summary>
            /// 取得具有比目标文章的指定属性具有更小的值的文章PostID
            /// </summary>
            /// <typeparam name="Prop">指定属性</typeparam>
            /// <param name="PostID">目标文章的PostID</param>
            /// <returns>不存在符合要求的PostID时，返回-1</returns>
            public int Smaller<Prop>(int PostID)
            {
                string SQL = (typeof(Prop) == typeof(PostID)) switch /* 对查询PostID有优化 */
                {
                    true => $"SELECT PostID FROM `{UnionView}` WHERE PostID=( SELECT max(PostID) FROM `{UnionView}` WHERE PostID < {PostID})",
                    false => string.Format
                    (
                    "SELECT PostID FROM `{0}` WHERE {1}=( SELECT max({1}) FROM `{0}` WHERE {1} < ( SELECT {1} FROM `{0}` WHERE PostID = {2} ))"
                    , UnionView, typeof(Prop).Name, PostID
                    )
                };

                object PrevPostID = MySqlManager.GetKey(SQL);

                return PrevPostID == null ? -1 : Convert.ToInt32(PrevPostID);
            }
            /// <summary>
            /// 取得具有比目标文章的指定属性具有更小的值的文章PostID
            /// </summary>
            /// <typeparam name="Prop">指定属性</typeparam>
            /// <param name="PostID">目标文章的PostID</param>
            /// <param name="REGEXP">正则表达式</param>
            /// <param name="PostProp">用于被正则表达式筛选的属性</param>
            /// <returns>不存在符合要求的PostID时，返回-1</returns>
            public int Smaller<Prop>(int PostID, string REGEXP, PostPropEnum PostProp)
            {
                string SQL = (typeof(Prop) == typeof(PostID)) switch
                {
                    true => string.Format
                    (
                    "SELECT PostID FROM `{0}` WHERE {1}=( SELECT max({1}) FROM `{0}` WHERE PostID < {2} AND {3} REGEXP ?REGEXP )"
                    , UnionView, typeof(Prop).Name, PostID, PostProp
                    ),
                    false => string.Format
                    (
                    "SELECT PostID FROM `{0}` WHERE {1}=( SELECT max({1}) FROM `{0}` WHERE {1} < ( SELECT {1} FROM `{0}` WHERE PostID = ?PostID ) AND {2} REGEXP ?REGEXP )"
                    , UnionView, typeof(Prop).Name, PostProp
                    )
                };

                object PrevPostID = MySqlManager.GetKey(SQL, new MySqlParameter[]
                {
                    new("PostID", PostID),
                    new("REGEXP", REGEXP)
                });

                return PrevPostID == null ? -1 : Convert.ToInt32(PrevPostID);
            }
        }
        /// <summary>
        /// 数据修改组件
        /// </summary>
        public class Writer : IPLComponent<Writer>
        {
            private string IndexTable { get; init; }
            private string StackTable { get; init; }
            private MySqlManager MySqlManager { get; init; }

            /// <summary>
            /// 默认构造
            /// </summary>
            private Writer() { }
            /// <summary>
            /// 工厂构造
            /// </summary>
            /// <param name="IndexTable">索引表</param>
            /// <param name="StackTable">主表</param>
            /// <param name="MySqlManager">数据库管理器</param>
            /// <returns></returns>
            internal Writer(string IndexTable, string StackTable, MySqlManager MySqlManager)
            {
                this.IndexTable = IndexTable;
                this.StackTable = StackTable;
                this.MySqlManager = MySqlManager;
            }

            /// <summary>
            /// 得到最大文章PostID（私有）
            /// </summary>
            /// <returns></returns>
            internal int GetMaxPostID()
            {
                string SQL = $"SELECT MAX(PostID) FROM {IndexTable}";
                var value = MySqlManager.GetKey(SQL);
                /* 若取不到最大PostID(没有任何文章时)，返回12000作为初始PostID */
                return Convert.ToInt32(value == DBNull.Value ? 12000 : value);
            }
            /// <summary>
            /// 得到最小文章PostID（私有）
            /// </summary>
            /// <returns>错误则返回-1</returns>
            internal int GetMinPostID()
            {
                string SQL = $"SELECT MIN(PostID) FROM {IndexTable}";
                var value = MySqlManager.GetKey(SQL);
                /* 若取不到最大PostID(没有任何文章时)，返回12000作为初始PostID */
                return Convert.ToInt32(value == DBNull.Value ? 12000 : value);
            }
            /// <summary>
            /// 获取指定文章的积极备份的UUID
            /// </summary>
            /// <param name="PostID">目标文章PostID</param>
            /// <returns></returns>
            internal string GetPositiveUUID(int PostID)
            {
                return Convert.ToString(MySqlManager.GetKey($"SELECT UUID FROM {IndexTable} WHERE PostID={PostID}"));
            }
            /// <summary>
            /// 获取指定文章的消极备份的UUID
            /// </summary>
            /// <param name="PostID">目标文章PostID</param>
            /// <returns></returns>
            internal string GetNegativeUUID(int PostID)
            {
                return Convert.ToString(MySqlManager.GetKey(
                    string.Format
                    (
                    "SELECT {1}.UUID FROM {0} JOIN {1} ON {0}.PostID={1}.PostID AND {0}.UUID<>{1}.UUID WHERE {0}.PostID={2}"
                    , IndexTable, StackTable, PostID
                    )
                    ));
            }

            /// <summary>
            /// 注册文章
            /// </summary>
            /// <remarks>
            /// 新建一个拷贝，并将index指向该拷贝
            /// </remarks>
            /// <param name="Post">文章数据（其中的PostID、UUID、CT、LCT、User由系统生成）</param>
            /// <returns>返回受影响的行数</returns>
            public bool Reg(Post Post)
            {
                return MySqlManager.DoInConnection(conn =>
                {
                    DateTime t = DateTime.Now;

                    string SQL = $"INSERT INTO {IndexTable}" +
                                " ( PostID, UUID, CT, Mode, Type, User, UVCount, StarCount) VALUES" +
                                " (?PostID,?UUID,?CT,?Mode,?Type,?User,?UVCount,?StarCount);" +
                                $"INSERT INTO {StackTable}" +
                                " ( PostID, UUID, LCT, Title, Summary, Content, ArchiveID, Label, Cover) VALUES" +
                                " (?PostID,?UUID,?LCT,?Title,?Summary,?Content,?ArchiveID,?Label,?Cover);";


                    MySqlParameter[] parameters =
                    {
                    new("PostID", GetMaxPostID() + 1 ),
                    new("UUID", MathH.GenerateUUID("N") ),

                    new("CT", t),
                    new("LCT", t),

                    new("User", Post.User),/* 指定用户账号 */

                    /* 可传参数 */
                    new("Mode", Post.Mode),
                    new("Type", Post.Type),

                    new("UVCount", Post.UVCount),
                    new("StarCount", Post.StarCount),

                    new("Title", Post.Title),
                    new("Summary", Post.Summary ),
                    new("Content", Post.Content ),

                    new("ArchiveID", Post.ArchiveID ),
                    new("Label", Post.Label ),
                    new("Cover", Post.Cover )
                };

                    using MySqlCommand MySqlCommand = new MySqlCommand(SQL, conn);
                    MySqlCommand.Parameters.AddRange(parameters);

                    /* 开始事务 */
                    MySqlCommand.Transaction = conn.BeginTransaction();

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
                });
            }
            /// <summary>
            /// 注销文章
            /// </summary>
            /// <remarks>
            /// 删除所有拷贝和index指向
            /// </remarks>
            /// <param name="PostID">目标文章PostID</param>
            /// <returns></returns>
            public bool Dispose(int PostID)
            {
                return MySqlManager.DoInConnection(conn =>
                {
                    /* int参数无法用于参数化攻击 */
                    using MySqlCommand MySqlCommand = new MySqlCommand
                    {
                        CommandText =
                    $"DELETE FROM {IndexTable} WHERE PostID={PostID};DELETE FROM {StackTable} WHERE PostID={PostID};",

                        Connection = conn,

                        /* 开始事务 */
                        Transaction = conn.BeginTransaction()
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
                });
            }
            /// <summary>
            /// 更新文章
            /// </summary>
            /// <remarks>
            /// 新建一个拷贝，并将index更改为指向该拷贝
            /// </remarks>
            /// <param name="Post">文章数据</param>
            /// <returns></returns>
            public bool Update(Post Post)
            {
                return MySqlManager.DoInConnection(conn =>
                {
                    string SQL =
                    $"UPDATE {IndexTable} SET UUID=?UUID, Mode=?Mode, Type=?Type, User=?User, UVCount=?UVCount, StarCount=?StarCount WHERE PostID=?PostID;" +
                    $"INSERT INTO {StackTable}" +
                    " ( PostID, UUID, LCT, Title, Summary, Content, ArchiveID, Label, Cover) VALUES" +
                    " (?PostID,?UUID,?LCT,?Title,?Summary,?Content,?ArchiveID,?Label,?Cover);";

                    MySqlParameter[] parameters =
                    {
                    new("UUID", MathH.GenerateUUID("N") ),
                    new("LCT", DateTime.Now ),

                    new("User", Post.User),/* 指定用户账号 */

                    /* 可传参数 */
                    new("PostID", Post.PostID),

                    new("Mode", Post.Mode),
                    new("Type", Post.Type),

                    new("UVCount", Post.UVCount),
                    new("StarCount", Post.StarCount),

                    new("Title", Post.Title),
                    new("Summary", Post.Summary),
                    new("Content", Post.Content),

                    new("ArchiveID", Post.ArchiveID),
                    new("Label", Post.Label),
                    new("Cover", Post.Cover)
                };

                    using MySqlCommand MySqlCommand = new MySqlCommand(SQL, conn);
                    MySqlCommand.Parameters.AddRange(parameters);

                    /* 开始事务 */
                    MySqlCommand.Transaction = conn.BeginTransaction();

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
                        /* 由于UUID更新，影响行始终为2，若出现其他情况则一定为错误 */
                    }
                });
            }

            /// <summary>
            /// 删除拷贝
            /// </summary>
            /// <remarks>
            /// 删除指定拷贝，且该拷贝不能为当前index指向
            /// </remarks>
            /// <param name="UUID">目标文章的UUID</param>
            /// <returns></returns>
            public bool Delete(string UUID)
            {
                return MySqlManager.DoInConnection(conn =>
                {
                    string SQL = string.Format
                    (
                    "DELETE {1} FROM {0} INNER JOIN {1} ON {0}.PostID={1}.PostID AND {0}.UUID<>{1}.UUID AND {1}.UUID = ?UUID"
                    , IndexTable, StackTable
                    );

                    MySqlParameter[] parameters =
                    {
                    new("UUID", UUID )
                };

                    using MySqlCommand MySqlCommand = new MySqlCommand(SQL, conn);
                    MySqlCommand.Parameters.AddRange(parameters);

                    /* 开始事务 */
                    MySqlCommand.Transaction = conn.BeginTransaction();

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
                });
            }
            /// <summary>
            /// 应用拷贝
            /// </summary>
            /// <remarks>
            /// 将现有index指向删除（顶出），然后将index指向设置为指定文章拷贝
            /// </remarks>
            /// <param name="UUID">目标拷贝的UUID</param>
            /// <returns></returns>
            public bool Apply(string UUID)
            {
                return MySqlManager.DoInConnection(conn =>
                {
                    /* 此处，即使SQL注入造成了PostID错误，由于第二步参数化查询的作用，UUID也会造成错误无法成功攻击 */
                    object PostID = MySqlManager.GetKey($"SELECT PostID FROM {StackTable} WHERE UUID = '{UUID}'");

                    string SQL =
                        $"DELETE FROM {StackTable} WHERE UUID = (SELECT UUID FROM {IndexTable} WHERE PostID = ?PostID);" +
                        $"UPDATE {StackTable} SET UUID = ?UUID WHERE PostID = ?PostID;";

                    MySqlParameter[] parameters =
                    {
                    new("PostID", PostID),
                    new("UUID", UUID)
                };

                    using MySqlCommand MySqlCommand = new MySqlCommand(SQL, conn);
                    MySqlCommand.Parameters.AddRange(parameters);

                    /* 开始事务 */
                    MySqlCommand.Transaction = conn.BeginTransaction();

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
                });
            }
            /// <summary>
            /// 回滚拷贝
            /// </summary>
            /// <remarks>
            /// 将现有index指向删除（顶出），然后将index指向设置到另一个最近更新的拷贝
            /// </remarks>
            /// <param name="PostID">目标文章的PostID</param>
            /// <returns></returns>
            public bool Rollback(int PostID)
            {
                return MySqlManager.DoInConnection(conn =>
                {
                    using MySqlCommand MySqlCommand = new MySqlCommand
                    {
                        CommandText = string.Format
                     (
                     "DELETE {1} FROM {0} INNER JOIN {1} ON {0}.UUID={1}.UUID AND {0}.PostID={2};" +
                     "UPDATE {0} SET UUID = (SELECT UUID FROM {1} WHERE PostID={2} ORDER BY LCT DESC LIMIT 0,1) WHERE PostID={2};"
                     , IndexTable, StackTable, PostID
                     ),

                        Connection = conn,

                        /* 开始事务 */
                        Transaction = conn.BeginTransaction()
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
                });
            }
            /// <summary>
            /// 释放拷贝
            /// </summary>
            /// <remarks>删除非当前index指向的所有拷贝
            /// </remarks>
            /// <param name="PostID">目标文章的PostID</param>
            /// <returns></returns>
            public bool Release(int PostID)
            {
                return MySqlManager.DoInConnection(conn =>
                {
                    using MySqlCommand MySqlCommand = new MySqlCommand
                    {
                        CommandText = string.Format
                     (
                     "DELETE {1} FROM {0} INNER JOIN {1} ON {0}.PostID={1}.PostID AND {0}.UUID<>{1}.UUID AND {0}.PostID={2}"
                     , IndexTable, StackTable, PostID
                     ),

                        Connection = conn,

                        /* 开始事务 */
                        Transaction = conn.BeginTransaction()
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
                });
            }

            /// <summary>
            /// 设置文章类型
            /// </summary>
            /// <param name="PostID">文章索引</param>
            /// <param name="TypeState">目标类型</param>
            /// <returns></returns>
            public bool UpdateType(int PostID, Type.States TypeState)
            {
                bool fun(string value) => MySqlManager.UpdateKey((IndexTable, "PostID", PostID), "Type", value);
                return TypeState switch
                {
                    Type.States.Unset => fun(""),
                    Type.States.Note => fun("note"),
                    _ => throw new NotImplementedException("模式匹配失败")
                };
            }
            /// <summary>
            /// 设置文章模式
            /// </summary>
            /// <param name="PostID">文章索引</param>
            /// <param name="ModeState">目标模式</param>
            /// <returns></returns>
            public bool UpdateMode(int PostID, Mode.States ModeState)
            {
                bool fun(string value) => MySqlManager.UpdateKey((IndexTable, "PostID", PostID), "Mode", value);

                return ModeState switch
                {
                    Mode.States.Unset => fun(""),
                    Mode.States.Hidden => fun("hidden"),
                    Mode.States.Scheduled => fun("scheduled"),
                    Mode.States.Archived => fun("archived"),
                    _ => throw new NotImplementedException("模式匹配失败")
                };
            }

            /// <summary>
            /// 通用文章指向更新器
            /// </summary>
            /// <typeparam name="T">目标属性类型</typeparam>
            /// <param name="PostID">目标文章PostID</param>
            /// <param name="Value">新属性值</param>
            /// <returns></returns>
            public bool UpdateIndexTable<T>(int PostID, object Value) where T : IPostProp
            {
                //初始化键定位
                var MySqlKey = (IndexTable, "PostID", PostID);
                return MySqlManager.UpdateKey(MySqlKey, typeof(T).Name, Value);
            }
            /// <summary>
            /// 通用文章拷贝更新器
            /// </summary>
            /// <typeparam name="T">目标属性类型</typeparam>
            /// <param name="PostID">目标文章PostID</param>
            /// <param name="Value">新属性值</param>
            /// <returns></returns>
            public bool UpdateStackTable<T>(int PostID, object Value) where T : IPostProp
            {
                //初始化键定位
                var MySqlKey = (StackTable, "UUID", GetPositiveUUID(PostID));
                return MySqlManager.UpdateKey(MySqlKey, typeof(T).Name, Value);
            }

            /// <summary>
            /// 检测PostID、UUID是否匹配，之后合并Post数据表
            /// </summary>
            /// <parmm name="Index">索引表Post实例</parmm>
            /// <parmm name="Stack">主表Post实例</parmm>
            /// <returns></returns>
            public static Post Join(Post Index, Post Stack)
            {
                if (Index.PostID == Stack.PostID && Index.UUID == Stack.UUID)
                {
                    return new Post
                    {
                        PostID = Index.PostID,
                        UUID = Index.UUID,

                        Mode = Index.Mode,
                        Type = Index.Type,

                        Title = Stack.Title,
                        Summary = Stack.Summary,
                        Content = Stack.Content,

                        User = Index.User,
                        ArchiveID = Stack.ArchiveID,
                        Label = Stack.Label,

                        CT = Index.CT,
                        LCT = Stack.LCT,

                        UVCount = Index.UVCount,
                        StarCount = Index.StarCount,

                        Cover = Stack.Cover
                    };
                }
                else
                {
                    throw new Exception("UUID不匹配，该联合存在安全隐患");
                }
            }
            /// <summary>
            /// 强制合并Post数据表（风险性重载，不考虑PostID、UUID是否匹配，调用不当易引发逻辑故障）
            /// </summary>
            /// <parmm name="Index">索引表Post实例</parmm>
            /// <parmm name="Stack">主表Post实例</parmm>
            /// <returns>始终返回以Index的PostID、UUID为最终合并结果的Post实例</returns>
            public static Post ForcedJoin(Post Index, Post Stack)
            {
                return new Post
                {
                    PostID = Index.PostID,
                    UUID = Index.UUID,

                    Mode = Index.Mode,
                    Type = Index.Type,

                    Title = Stack.Title,
                    Summary = Stack.Summary,
                    Content = Stack.Content,

                    User = Index.User,
                    ArchiveID = Stack.ArchiveID,
                    Label = Stack.Label,

                    CT = Index.CT,
                    LCT = Stack.LCT,

                    UVCount = Index.UVCount,
                    StarCount = Index.StarCount,

                    Cover = Stack.Cover
                };
            }
        }
        /// <summary>
        /// 计数管理组件
        /// </summary>
        public class Counter : IPLComponent<Counter>
        {
            private string IndexTable { get; init; }
            private string StackTable { get; init; }
            private MySqlManager MySqlManager { get; init; }

            /// <summary>
            /// 默认构造
            /// </summary>
            private Counter() { }
            /// <summary>
            /// 工厂构造
            /// </summary>
            /// <param name="IndexTable">索引表</param>
            /// <param name="StackTable">主表</param>
            /// <param name="MySqlManager">数据库管理器</param>
            /// <returns></returns>
            internal Counter(string IndexTable, string StackTable, MySqlManager MySqlManager)
            {
                this.IndexTable = IndexTable;
                this.StackTable = StackTable;
                this.MySqlManager = MySqlManager;
            }

            /// <summary>
            /// 文章计数
            /// </summary>
            public int TotalPostCount
            {
                get => GetPostCountByMode("^");
            }
            /// <summary>
            /// 拷贝计数
            /// </summary>
            public int StackCount
            {
                get => GetStackCount();
            }

            /// <summary>
            /// 隐藏文章计数
            /// </summary>
            public int HiddenCount
            {
                get => GetPostCountByMode("^hidden$");
            }
            /// <summary>
            /// 展示中文章计数
            /// </summary>
            public int OnDisplayCount
            {
                get => GetPostCountByMode("^$");
            }
            /// <summary>
            /// 归档中文章计数
            /// </summary>
            public int ArchivedCount
            {
                get => GetPostCountByMode("^archived$");
            }
            /// <summary>
            /// 计划中文章计数
            /// </summary>
            public int ScheduledCount
            {
                get => GetPostCountByMode("^scheduled$");
            }

            private int GetPostCountByMode(string REGEXP)
            {
                object Count = MySqlManager.GetKey($"SELECT Count(*) FROM {IndexTable} WHERE Mode REGEXP '{REGEXP}';");
                return Count == DBNull.Value ? 0 : Convert.ToInt32(Count);
            }
            private int GetStackCount()
            {
                object Count = MySqlManager.GetKey(string.Format("SELECT COUNT(*) FROM {0},{1} WHERE {0}.PostID={1}.PostID AND {0}.UUID<>{1}.UUID;", IndexTable, StackTable));
                return Count == DBNull.Value ? 0 : Convert.ToInt32(Count);
            }
        }
        /// <summary>
        /// 归档管理组件
        /// </summary>
        public class Archive
        {
            private string ArchiveTable { get; set; }
            private MySqlManager MySqlManager { get; init; }

            /// <summary>
            /// 默认构造
            /// </summary>
            private Archive() { }
            /// <summary>
            /// 使用归档名构造
            /// </summary>
            /// <param name="ArchiveTable">归档表</param>
            /// <param name="MySqlManager">数据库管理器</param>
            /// <returns></returns>
            internal Archive(string ArchiveTable, MySqlManager MySqlManager)
            {
                this.ArchiveTable = ArchiveTable;
                this.MySqlManager = MySqlManager;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="ReaderTodo"></param>
            /// <returns></returns>
            public T Select<T>(Func<T> ReaderTodo) where T : IEnumerable
            {
                return ReaderTodo();
            }
        }
        /// <summary>
        /// 插件管理组件
        /// </summary>
        public class Plugin
        {
            //private PLTables Tables { get; init; }
            //private MySqlManager MySqlManager { get; init; }

            //private List<string> PluginPool;
            /* 此组件是为未来而保留的 */
        }
    }
}
