using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data.MySqlClient;

using WaterLibrary.com.MySQL;
using WaterLibrary.com.basic;
using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala;
using WaterLibrary.stru.pilipala.DB;
using WaterLibrary.stru.pilipala.PostKey;

using Type = WaterLibrary.stru.pilipala.PostKey.Type;
using System.Web.UI.HtmlControls;

namespace WaterLibrary.com.pilipala
{
    /// <summary>
    /// 啪啦系统
    /// </summary>
    public class PLSYS
    {
        /// <summary>
        /// 用户ID访问器
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PLTables SysTables { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public PLViews SysViews { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public MySqlManager MySqlManager { get; private set; }

        /// <summary>
        /// 初始化啪啦系统
        /// </summary>
        /// <parmm name="ID">用户ID</parmm>
        /// <parmm name="PLDB">啪啦数据库信息</parmm>
        public PLSYS(int ID, PLDB PLDB)
        {
            this.ID = ID;

            SysTables = PLDB.Tables;
            SysViews = PLDB.Views;
            MySqlManager = PLDB.MySqlManager;
        }

        /// <summary>
        /// 获得当前操作噼里啪啦系统的用户数据
        /// </summary>
        /// <returns>错误则返回Name为ERROR的User实例</returns>
        public stru.pilipala.User GetUser()
        {
            string SQL = string.Format("SELECT * FROM {0} WHERE ID = ?ID", SysTables.Index);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                DataRow result = MySqlManager.GetRow(MySqlCommand);/* 得到一行数据，使用GetRow方法 */

                return new stru.pilipala.User
                {
                    ID = Convert.ToInt32(result["ID"]),
                    GUID = Convert.ToString(result["GUID"]),
                    Name = Convert.ToString(result["Name"]),
                    Note = Convert.ToString(result["Note"])
                };
            }
        }

        /// <summary>
        /// 内置默认PL表
        /// </summary>
        private struct InnerDefTables
        {
            public static string User = "pl_user";
            public static string Index = "pl_index";
            public static string Primary = "pl_primary";
        }
        /// <summary>
        /// 内置默认PL视图
        /// </summary>
        private struct InnerDefViews
        {
            public static string Index = "view>index";
            public static string Primary = "view>primary";
            public static string Total = "view>total";
        }

        /// <summary>
        /// 默认PL表访问器
        /// </summary>
        public static PLTables DefTables
        {
            get { return new PLTables(InnerDefTables.User, InnerDefTables.Index, InnerDefTables.Primary); }
        }
        /// <summary>
        /// 默认PL视图访问器
        /// </summary>
        public static PLViews DefViews
        {
            get { return new PLViews(InnerDefViews.Index, InnerDefViews.Primary, InnerDefViews.Total); }
        }


        /// <summary>
        /// 以默认值定义表名
        /// </summary>
        public void DefaultSysTables()
        {
            PLTables T = new PLTables
            {
                User = InnerDefTables.User,
                Index = InnerDefTables.Index,
                Primary = InnerDefTables.Primary
            };

            SysTables = T;
        }
        /// <summary>
        /// 以默认值定义视图名
        /// </summary>
        public void DefaultSysViews()
        {
            PLViews V = new PLViews
            {
                Index = InnerDefViews.Index,
                Primary = InnerDefViews.Primary,
                Total = InnerDefViews.Total
            };

            SysViews = V;
        }

        /// <summary>
        /// 初始化数据库连接控制器
        /// </summary>
        /// <param name="MySqlConn">连接信息</param>
        public void DBCHINIT(MySqlConn MySqlConn)
        {
            MySqlManager.Start(MySqlConn);
        }
    }

    /// <summary>
    /// 啪啦数据读取器
    /// </summary>
    public class PLDR : IPLDataReader
    {
        /// <summary>
        /// 
        /// </summary>
        public PLTables Tables { get; }
        /// <summary>
        /// 
        /// </summary>
        public PLViews Views { get; }
        /// <summary>
        /// 
        /// </summary>
        public MySqlManager MySqlManager { get; set; }


        /// <summary>
        /// 初始化啪啦数据读取器
        /// </summary>
        /// <parmm name="PLDB">啪啦数据库信息</parmm>
        public PLDR(PLSYS PLSYS)
        {
            Tables = PLSYS.SysTables;
            Views = PLSYS.SysViews;
            MySqlManager = PLSYS.MySqlManager;
        }

        /* 注意！读取器读取到的数据不包含设置隐藏的文章 */

        /// <summary>
        /// 获得文章数据列表(使用MatchList匹配)
        /// 注意！在单次查询量较低时使用此方法带来的性能收益可能不佳
        /// </summary>
        /// <param name="PropertyType">所需属性类型</param>
        /// <returns>返回根据所需类型填充的Post集合</returns>
        public virtual List<Post> GetList(params System.Type[] PropertyType)
        {
            /* 介于类型转到字符串而生成SQL的方式，此方法无需对SQL注入防护。 */
            string KeysStr = "";
            foreach (System.Type Type in PropertyType)
            {
                KeysStr += "," + Type.Name;
            }
            KeysStr.Substring(0, KeysStr.Length - 1);

            string SQL = string.Format("SELECT ID{0} FROM `{1}`", KeysStr, Views.Total);

            List<Post> PostList = new List<Post>();

            foreach (DataRow Row in MySqlManager.GetTable(SQL).Rows)
            {
                Post Post = new Post();

                /* ID列赋值 */
                Post["ID"] = Row.ItemArray[0];
                for (int i = 1; i < Row.ItemArray.Length; i++)
                {
                    /* ItemArray比PropertyType在首部多出一个ID列，使用i-1可正确匹配 */
                    Post[PropertyType[i - 1].Name] = Row.ItemArray[i];
                }

                PostList.Add(Post);
            }
            return PostList;
        }
        /// <summary>
        /// 获得文章数据列表(使用MatchList匹配)
        /// 注意！在单次查询量较低时使用此方法带来的性能收益可能不佳
        /// </summary>
        /// <typeparam name="T">属性匹配列表的属性类型</typeparam>
        /// <param name="MatchList">属性匹配列表</param>
        /// <param name="PropertyType">所需属性类型</param>
        /// <returns></returns>
        public virtual List<Post> GetList<T>(List<string> MatchList, params System.Type[] PropertyType) where T : IKey
        {
            /* 键名字符串格式化 */
            string KeysStr = "";
            foreach (System.Type Type in PropertyType)
            {
                KeysStr += "," + Type.Name;
            }
            /* 正则表达式格式化 */
            string REGEXP = ConvertH.ListToString(MatchList, '|');

            string SQL = string.Format
                (
                "SELECT ID{0} FROM `{1}` WHERE {2} REGEXP ?REGEXP"
                , KeysStr, Views.Total, typeof(T).Name
                );

            List<Post> PostList = new List<Post>();

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "REGEXP", Val = REGEXP }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                foreach (DataRow Row in MySqlManager.GetTable(MySqlCommand).Rows)
                {
                    Post Post = new Post();

                    /* ID列赋值 */
                    Post["ID"] = Row.ItemArray[0];
                    for (int i = 1; i < Row.ItemArray.Length; i++)
                    {
                        /* ItemArray比PropertyType在首部多出一个ID列，使用i-1可正确匹配 */
                        Post[PropertyType[i - 1].Name] = Row.ItemArray[i];
                    }

                    PostList.Add(Post);
                }
            }
            return PostList;
        }


        /// <summary>
        /// 获得全部文章ID列表
        /// </summary>
        /// <returns></returns>
        public virtual List<int> GetIDList()
        {
            List<int> IDList = new List<int>();

            string SQL = string.Format("SELECT ID FROM `{0}`", Views.Index);

            foreach (int ID in MySqlManager.GetColumn<int>(SQL))
            {
                IDList.Add(ID);
            }
            return IDList;
        }

        /// <summary>
        /// 取得文章ID列表（步进式）
        /// </summary>
        /// <parmm name="Start">步进起始行（包含该行）</parmm>
        /// <parmm name="Length">加载行数</parmm>
        /// <returns></returns>
        public virtual List<int> GetIDList(int Start, int Length)
        {
            List<int> IDList = new List<int>();

            /* 此处以时间降序排列查询 */
            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` ORDER BY CT DESC LIMIT ?Start , ?Length"
                , Views.Index
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "Start", Val = Start },
                    new MySqlParm() { Name = "Length", Val = Length }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                foreach (int ID in MySqlManager.GetColumn<int>(MySqlCommand))
                {
                    IDList.Add(ID);
                }
            }
            return IDList;
        }
        /// <summary>
        /// 取得指定类型的文章ID列表（步进式）
        /// </summary>
        /// <parmm name="Start">步进起始行（包含该行）</parmm>
        /// <parmm name="Length">加载行数</parmm>
        /// <parmm name="Type">自定义文章类型</parmm>
        /// <returns></returns>
        public virtual List<int> GetIDList(int Start, int Length, string Type)
        {

            List<int> IDList = new List<int>();

            /* 此处以时间降序排列查询 */
            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE Type = ?Type ORDER BY CT DESC LIMIT ?Start , ?Length"
                , Views.Index
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "Type", Val = Type },
                    new MySqlParm() { Name = "Start", Val = Start },
                    new MySqlParm() { Name = "Length", Val = Length }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                foreach (int ID in MySqlManager.GetColumn<int>(MySqlCommand))
                {
                    IDList.Add(ID);
                }
            }
            return IDList;
        }

        /// <summary>
        /// 取得全部文章ID列表
        /// </summary>
        /// <param name="OrderProperty">排序属性</param>
        /// <param name="OrderType">asc(升序)或desc(降序)，不区分大小写</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(string OrderProperty, string OrderType)
        {
            List<int> IDList = new List<int>();

            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` ORDER BY ?OrderKey ?OrderType"
                , Views.Index
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "OrderKey", Val = OrderProperty },
                    new MySqlParm() { Name = "OrderType", Val = OrderType }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                foreach (int ID in MySqlManager.GetColumn<int>(MySqlCommand))
                {
                    IDList.Add(ID);
                }
            }
            return IDList;
        }
        /// <summary>
        /// 取得指定长度的文章ID列表
        /// </summary>
        /// <param name="OrderProperty">排序属性</param>
        /// <param name="OrderType">排序类型，可选值：asc(升序)或desc(降序)，不区分大小写</param>
        /// <param name="Length">截止长度</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(string OrderProperty, string OrderType, int Length)
        {
            List<int> IDList = new List<int>();

            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` ORDER BY ?OrderKey ?OrderType LIMIT 0,?Length"
                , Views.Index
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "OrderKey", Val = OrderProperty },
                    new MySqlParm() { Name = "OrderType", Val = OrderType },
                    new MySqlParm() { Name = "Length", Val = Length }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                foreach (int ID in MySqlManager.GetColumn<int>(MySqlCommand))
                {
                    IDList.Add(ID);
                }
            }
            return IDList;
        }

        /// <summary>
        /// 匹配得到指定类型的文章ID列表
        /// </summary>
        /// <typeparam name="T">属性匹配列表的属性类型</typeparam>
        /// <param name="MatchList">属性匹配列表</param>
        /// <returns></returns>
        public virtual List<int> GetIDList<T>(List<string> MatchList) where T : IKey
        {
            List<int> IDList = new List<int>();

            /* 正则表达式格式化 */
            string REGEXP = ConvertH.ListToString(MatchList, '|');

            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE {1} REGEXP ?REGEXP"
                , Views.Total, typeof(T).Name
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "REGEXP", Val = REGEXP }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                foreach (int ID in MySqlManager.GetColumn<int>(MySqlCommand))
                {
                    IDList.Add(ID);
                }
            }
            return IDList;
        }
        /// <summary>
        /// 匹配得到指定类型的文章ID列表（重排序）
        /// </summary>
        /// <typeparam name="T">属性匹配列表的属性类型</typeparam>
        /// <param name="MatchList">属性匹配列表</param>
        /// <param name="OrderKey">排序类型，可选值：asc(升序)或desc(降序)，不区分大小写</param>
        /// <param name="OrderType">排序类型</param>
        /// <returns></returns>
        public virtual List<int> GetIDList<T>(List<string> MatchList, string OrderKey, string OrderType) where T : IKey
        {
            List<int> IDList = new List<int>();

            /* 正则表达式格式化 */
            string REGEXP = ConvertH.ListToString(MatchList, '|');

            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE {1} REGEXP ?REGEXP ORDER BY ?OrderKey ?OrderType"
                , Views.Total, typeof(T).Name
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "REGEXP", Val = REGEXP },

                    new MySqlParm() { Name = "OrderKey", Val = OrderKey },
                    new MySqlParm() { Name = "OrderType", Val = OrderType }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                foreach (int ID in MySqlManager.GetColumn<int>(MySqlCommand))
                {
                    IDList.Add(ID);
                }
            }
            return IDList;
        }
        /// <summary>
        /// 获得指定类型的文章ID列表（重排序，限制列表的最大长度）
        /// </summary>
        /// <typeparam name="T">属性匹配列表的属性类型</typeparam>
        /// <param name="MatchList">属性匹配列表</param>
        /// <param name="OrderProperty">排序属性</param>
        /// <param name="OrderType">排序类型，可选值：asc(升序)或desc(降序)，不区分大小写</param>
        /// <param name="Length">最大取用长度</param>
        /// <returns></returns>
        public virtual List<int> GetIDList<T>(List<string> MatchList, string OrderProperty, string OrderType, int Length) where T : IKey
        {
            List<int> IDList = new List<int>();

            /* 正则表达式格式化 */
            string REGEXP = ConvertH.ListToString(MatchList, '|');

            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE {1} REGEXP ?REGEXP ORDER BY ?Key ?OrderType LIMIT 0,?Length"
                , Views.Total, typeof(T).Name
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "REGEXP", Val = REGEXP },

                    new MySqlParm() { Name = "OrderKey", Val = OrderProperty },
                    new MySqlParm() { Name = "OrderType", Val = OrderType },

                    new MySqlParm() { Name = "Length", Val = Length }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                foreach (int ID in MySqlManager.GetColumn<int>(MySqlCommand))
                {
                    IDList.Add(ID);
                }
            }
            return IDList;
        }

        /// <summary>
        /// 通用文章匹配器
        /// </summary>
        /// <typeparmm name="T">被匹配的属性类型</typeparmm>
        /// <parmm name="REGEXP">正则表达式</parmm>
        /// <returns></returns>
        public virtual List<int> MatchID<T>(string REGEXP) where T : IKey
        {
            List<int> IDList = new List<int>();

            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE {1} REGEXP ?REGEXP"
                , Views.Total, typeof(T).Name
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "REGEXP", Val = REGEXP }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                foreach (int ID in MySqlManager.GetColumn<int>(MySqlCommand))
                {
                    IDList.Add(ID);
                }
            }
            return IDList;
        }
        /// <summary>
        /// 通用文章匹配器
        /// </summary>
        /// <typeparmm name="T">被匹配的属性类型</typeparmm>
        /// <parmm name="REGEXP">正则表达式</parmm>
        /// <returns></returns>
        public virtual List<Post> MatchPost<T>(string REGEXP) where T : IKey
        {
            List<Post> PostList = new List<Post>();

            string SQL = string.Format
                (
                "SELECT * FROM `{0}` WHERE {1} REGEXP ?REGEXP"
                , Views.Total, typeof(T).Name
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "REGEXP", Val = REGEXP }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                foreach (DataRow Row in MySqlManager.GetTable(MySqlCommand).Rows)
                {
                    PostList.Add(new Post
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
            }
            return PostList;
        }


        /// <summary>
        /// 通过ID获取Index表中的目标文章数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public virtual Post GetIndex(int ID)
        {
            string SQL = string.Format("SELECT * FROM `{0}` WHERE ID = ?ID", Views.Index);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                DataRow result = MySqlManager.GetRow(MySqlCommand);

                return new Post
                {
                    ID = Convert.ToInt32(result["ID"]),
                    GUID = Convert.ToString(result["GUID"]),
                    CT = Convert.ToDateTime(result["CT"]),
                    Mode = Convert.ToString(result["Mode"]),
                    Type = Convert.ToString(result["Type"]),
                    User = Convert.ToString(result["User"]),
                    UVCount = Convert.ToInt32(result["UVCount"]),
                    StarCount = Convert.ToInt32(result["StarCount"])
                };
            }
        }
        /// <summary>
        /// 通过ID获取Primary表中的目标文章数据
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public virtual Post GetPrimary(int ID)
        {
            string SQL = string.Format("SELECT * FROM `{0}` WHERE ID = ?ID", Views.Primary);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                DataRow result = MySqlManager.GetRow(MySqlCommand);

                return new Post
                {
                    ID = Convert.ToInt32(result["ID"]),
                    GUID = Convert.ToString(result["GUID"]),
                    LCT = Convert.ToDateTime(result["LCT"]),
                    Title = Convert.ToString(result["Title"]),
                    Summary = Convert.ToString(result["Summary"]),
                    Content = Convert.ToString(result["Content"]),
                    Archiv = Convert.ToString(result["Archiv"]),
                    Label = Convert.ToString(result["Label"]),
                    Cover = Convert.ToString(result["Cover"])
                };
            }
        }
        /// <summary>
        /// 通过ID获取Total视图中的目标文章数据(Index Join Primary)
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public virtual Post GetTotal(int ID)
        {
            string SQL = string.Format("SELECT * FROM `{0}` WHERE ID = ?ID", Views.Total);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                DataRow result = MySqlManager.GetRow(MySqlCommand);

                return new Post
                {
                    ID = Convert.ToInt32(result["ID"]),
                    GUID = Convert.ToString(result["GUID"]),

                    CT = Convert.ToDateTime(result["CT"]),
                    LCT = Convert.ToDateTime(result["LCT"]),
                    Title = Convert.ToString(result["Title"]),
                    Summary = Convert.ToString(result["Summary"]),
                    Content = Convert.ToString(result["Content"]),

                    Archiv = Convert.ToString(result["Archiv"]),
                    Label = Convert.ToString(result["Label"]),
                    Cover = Convert.ToString(result["Cover"]),

                    Mode = Convert.ToString(result["Mode"]),
                    Type = Convert.ToString(result["Type"]),
                    User = Convert.ToString(result["User"]),

                    UVCount = Convert.ToInt32(result["UVCount"]),
                    StarCount = Convert.ToInt32(result["StarCount"])
                };
            }
        }
        /// <summary>
        /// 获取Index表中目标文章的指定属性
        /// </summary>
        /// <typeparam name="T">目标属性类型</typeparam>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public virtual object GetIndex<T>(int ID) where T : IKey
        {
            string SQL = string.Format
                (
                "SELECT {0} FROM `{1}` WHERE ID = ?ID"
                , typeof(T).Name, Views.Index
                );

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlManager.GetRow(MySqlCommand)[0];
            }
        }
        /// <summary>
        /// 获取Primary表中目标文章的指定属性
        /// </summary>
        /// <typeparam name="T">目标属性类型</typeparam>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public virtual object GetPrimary<T>(int ID) where T : IKey
        {
            string SQL = string.Format
                (
                "SELECT {0} FROM `{1}` WHERE ID = ?ID"
                , typeof(T).Name, Views.Primary
                );

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlManager.GetRow(MySqlCommand)[0];
            }
        }

        /// <summary>
        /// 以字符串形式取得文章属性
        /// </summary>
        /// <typeparam name="T">目标属性类型</typeparam>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public virtual string Get<T>(int ID) where T : IKey
        {
            string SQL = string.Format
                (
                "SELECT {0} FROM `{1}` WHERE ID = ?ID"
                , typeof(T).Name, Views.Total
                );

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlManager.GetKey(MySqlCommand).ToString();
            }
        }
        /// <summary>
        /// 以字符串形式取得文章属性（限制最大取用长度）
        /// </summary>
        /// <typeparam name="T">目标属性类型</typeparam>
        /// <param name="ID">目标文章ID</param>
        /// <param name="Length">最大取用长度</param>
        /// <returns></returns>
        public virtual string Get<T>(int ID, int Length) where T : IKey
        {
            string SQL = string.Format
                (
                "SELECT SUBSTRING(( SELECT {0} FROM `{1}` WHERE ID = ?ID ) FROM 1 FOR ?Length)"
                , typeof(T).Name, Views.Total
                );

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "ID", Val = ID },
                    new MySqlParm() { Name = "Length", Val = Length }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlManager.GetKey(MySqlCommand).ToString();
            }
        }

        /// <summary>
        /// 取得目标文章ID 的下一个文章ID（按ID升序查找）
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns>不存在符合要求的ID时，返回-1</returns>
        public virtual int NextID(int ID)
        {
            /* 取得比当前 ID 大的一行，实现对下一条数据的抓取 */
            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE ID=( SELECT min(ID) FROM `{0}` WHERE ID > ?ID );"
                , Views.Index
                );

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                object NextID = MySqlManager.GetKey(MySqlCommand);

                return NextID == null ? -1 : Convert.ToInt32(NextID);
            }
        }
        /// <summary>
        /// 取得目标文章ID 的下一个文章ID(按指定属性在Index表升序查找)
        /// </summary>
        /// <typeparam name="T">目标属性类型</typeparam>
        /// <param name="ID">目标文章ID</param>
        /// <returns>不存在符合要求的ID时，返回-1</returns>
        public virtual int NextID<T>(int ID)
        {
            /* 取得比当前 ID 大的一行，实现对下一条数据的抓取 */
            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE " +
                "{1} =( SELECT min({1}) FROM `{0}` WHERE " +
                "{1} >( SELECT {1} FROM `{0}` WHERE ID = ?ID ))"
                , Views.Total, typeof(T).Name
                );

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                object NextID = MySqlManager.GetKey(MySqlCommand);

                return NextID == null ? -1 : Convert.ToInt32(NextID);
            }
        }
        /// <summary>
        /// 取得目标文章ID 的下一个文章ID（在正则表达式匹配到的归档范围内，按ID升序查找）
        /// </summary>
        /// <param name="REGEXP">正则表达式</param>
        /// <param name="ID">目标文章ID</param>
        /// <returns>不存在符合要求的ID时，返回-1</returns>
        public virtual int NextID(string REGEXP, int ID)
        {
            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE ID=( SELECT min(ID) FROM `{0}` WHERE ID > ?ID AND Archiv REGEXP ?REGEXP )"
                , Views.Primary
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "ID", Val = ID },
                    new MySqlParm() { Name = "REGEXP", Val = REGEXP }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                object NextID = MySqlManager.GetKey(MySqlCommand);

                return NextID == null ? -1 : Convert.ToInt32(NextID);
            }
        }

        /// <summary>
        /// 取得指定目标文章ID 的上一个文章ID（按ID升序查找）
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns>不存在符合要求的ID时，返回-1</returns>
        public virtual int PrevID(int ID)
        {
            /* 取得比当前 ID 小的一行，实现对上一条数据的抓取 */
            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE " +
                "ID =( SELECT max(ID) FROM `{0}` WHERE ID < ?ID )"
                , Views.Index
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                object PrevID = MySqlManager.GetKey(MySqlCommand);

                return PrevID == null ? -1 : Convert.ToInt32(PrevID);
            }
        }
        /// <summary>
        /// 取得指定目标文章ID 的上一个文章ID(按指定属性在Index表升序查找)
        /// </summary>
        /// <typeparam name="T">目标属性类型</typeparam>
        /// <param name="ID">目标文章ID</param>
        /// <returns>不存在符合要求的ID时，返回-1</returns>
        public virtual int PrevID<T>(int ID)
        {
            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE " +
                "{1} =( SELECT max({1}) FROM `{0}` WHERE " +
                "{1} <( SELECT {1} FROM `{0}` WHERE ID = ?ID ))"
                , Views.Total, typeof(T).Name
                );

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                object PrevID = MySqlManager.GetKey(MySqlCommand);

                return PrevID == null ? -1 : Convert.ToInt32(PrevID);
            }
        }
        /// <summary>
        /// 取得指定目标文章ID 的上一个文章ID（在正则表达式匹配到的归档范围内，按ID升序查找）
        /// </summary>
        /// <param name="REGEXP">正则表达式</param>
        /// <param name="ID">目标文章ID</param>
        /// <returns>不存在符合要求的ID时，返回-1</returns>
        public virtual int PrevID(string REGEXP, int ID)
        {
            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE ID=(SELECT max(ID) FROM `{0}` WHERE ID < ?ID AND Archiv REGEXP ?REGEXP)"
                , Views.Primary
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "ID", Val = ID },
                    new MySqlParm() { Name = "REGEXP", Val = REGEXP }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                object PrevID = MySqlManager.GetKey(MySqlCommand);

                return PrevID == null ? -1 : Convert.ToInt32(PrevID);
            }
        }
    }
    /// <summary>
    /// 啪啦数据修改器
    /// </summary>
    public class PLDU : IPLDataUpdater
    {
        /// <summary>
        /// 
        /// </summary>
        public PLTables Tables { get; }
        /// <summary>
        /// 
        /// </summary>
        public PLViews Views { get; }
        /// <summary>
        /// 
        /// </summary>
        public MySqlManager MySqlManager { get; set; }

        /// <summary>
        /// 得到最大文章ID（私有）
        /// </summary>
        /// <returns>错误则返回-1</returns>
        private int GetMaxID()
        {
            string SQL = string.Format("SELECT max(ID) FROM `{0}`", Tables.Index);
            var value = MySqlManager.GetKey(SQL);
            /* 若取不到最大ID(没有任何文章时)，返回12000作为初始ID */
            return Convert.ToInt32(value.GetType() == typeof(DBNull) ? 12000 : value);
        }

        /// <summary>
        /// 初始化啪啦数据修改器
        /// </summary>
        /// <parmm name="PLDB">啪啦数据库信息</parmm>
        public PLDU(PLSYS PLSYS)
        {
            Tables = PLSYS.SysTables;
            Views = PLSYS.SysViews;
            MySqlManager = PLSYS.MySqlManager;
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

            string SQL = string.Format
                (
                "INSERT INTO {0}" +
                " ( ID, GUID, CT, Mode, Type, User, UVCount, StarCount) VALUES" +
                " (?ID,?GUID,?CT,?Mode,?Type,?User,?UVCount,?StarCount);" +
                "INSERT INTO {1}" +
                " ( ID, GUID, LCT, Title, Summary, Content, Archiv, Label, Cover) VALUES" +
                " (?ID,?GUID,?LCT,?Title,?Summary,?Content,?Archiv,?Label,?Cover);"
                , Tables.Index, Tables.Primary
                );


            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "ID", Val = GetMaxID() + 1 },
                new MySqlParm() { Name = "GUID", Val = MathH.GenerateGUID("D") },

                new MySqlParm() { Name = "CT", Val = t },
                new MySqlParm() { Name = "LCT", Val = t },

                /* 可传参数 */
                new MySqlParm() { Name = "Mode", Val = Post.Mode },
                new MySqlParm() { Name = "Type", Val = Post.Type },
                new MySqlParm() { Name = "User", Val = Post.User },

                new MySqlParm() { Name = "UVCount", Val = Post.UVCount },
                new MySqlParm() { Name = "StarCount", Val = Post.StarCount },

                new MySqlParm() { Name = "Title", Val = Post.Title },
                new MySqlParm() { Name = "Summary", Val = Post.Summary },
                new MySqlParm() { Name = "Content", Val = Post.Content },

                new MySqlParm() { Name = "Archiv", Val = Post.Archiv },
                new MySqlParm() { Name = "Label", Val = Post.Label },
                new MySqlParm() { Name = "Cover", Val = Post.Cover }
            };

            MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList);
            MySqlCommand.Connection = MySqlManager.Connection;

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
                /* 操作行数异常，回滚事务 */
                MySqlCommand.Transaction.Rollback();
                throw new Exception("操作行数异常，事务已回滚");
            }
        }
        /// <summary>
        /// 注销文章
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public bool Dispose(int ID)
        {
            string SQL = string.Format
                (
                "DELETE FROM {0} WHERE ID=?ID;" +
                "DELETE FROM {1} WHERE ID=?ID;"
                , Tables.Index, Tables.Primary
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "ID", Val = ID }
            };

            MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList);
            MySqlCommand.Connection = MySqlManager.Connection;

            /* 开始事务 */
            MySqlCommand.Transaction = MySqlManager.Connection.BeginTransaction();

            if (MySqlManager.QueryOnly(ref MySqlCommand) >= 2)
            {
                /* 指向表只删除1行数据，拷贝表至少删除1行数据 */
                MySqlCommand.Transaction.Commit();
                return true;
            }
            else
            {
                /* 操作行数异常，回滚事务 */
                MySqlCommand.Transaction.Rollback();
                throw new Exception("操作行数异常，事务已回滚");
            }
        }
        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="Post">文章数据</param>
        /// <returns></returns>
        public bool Update(Post Post)
        {
            string SQL = string.Format
                (
                "UPDATE {0} SET GUID=?GUID, Mode=?Mode, Type=?Type, UVCount=?UVCount, StarCount=?StarCount WHERE ID=?ID;" +
                "INSERT INTO {1}" +
                " ( ID, GUID, LCT, Title, Summary, Content, Archiv, Label, Cover) VALUES" +
                " (?ID,?GUID,?LCT,?Title,?Summary,?Content,?Archiv,?Label,?Cover);"
                , Tables.Index, Tables.Primary
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "ID", Val = Post.ID },
                new MySqlParm() { Name = "GUID", Val = MathH.GenerateGUID("D") },

                new MySqlParm() { Name = "LCT", Val = DateTime.Now },

                /* 可传参数 */
                new MySqlParm() { Name = "Mode", Val = Post.Mode },
                new MySqlParm() { Name = "Type", Val = Post.Type },

                new MySqlParm() { Name = "UVCount", Val = Post.UVCount },
                new MySqlParm() { Name = "StarCount", Val = Post.StarCount },

                new MySqlParm() { Name = "Title", Val = Post.Title },
                new MySqlParm() { Name = "Summary", Val = Post.Summary },
                new MySqlParm() { Name = "Content", Val = Post.Content },

                new MySqlParm() { Name = "Archiv", Val = Post.Archiv },
                new MySqlParm() { Name = "Label", Val = Post.Label },
                new MySqlParm() { Name = "Cover", Val = Post.Cover }
            };

            MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList);
            MySqlCommand.Connection = MySqlManager.Connection;

            /* 开始事务 */
            MySqlCommand.Transaction = MySqlManager.Connection.BeginTransaction();

            if (MySqlManager.QueryOnly(ref MySqlCommand) == 2)
            {
                /* 指向表修改1行数据，拷贝表添加1行数据 */
                MySqlCommand.Transaction.Commit();
                return true;
            }
            else
            {
                /* 操作行数异常，回滚事务 */
                MySqlCommand.Transaction.Rollback();
                throw new Exception("操作行数异常，事务已回滚");
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
                "DELETE {1} FROM {0},{1} WHERE ({0}.GUID <> ?GUID) AND ({1}.GUID = ?GUID) AND ({0}.ID = {1}.ID)"
                , Tables.Index, Tables.Primary
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "GUID", Val = GUID }
            };

            MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList);
            MySqlCommand.Connection = MySqlManager.Connection;

            /* 开始事务 */
            MySqlCommand.Transaction = MySqlManager.Connection.BeginTransaction();

            if (MySqlManager.QueryOnly(ref MySqlCommand) == 1)
            {
                /* 拷贝表删除一行数据 */
                MySqlCommand.Transaction.Commit();
                return true;
            }
            else
            {
                /* 操作行数异常，回滚事务 */
                MySqlCommand.Transaction.Rollback();
                throw new Exception("操作行数异常，事务已回滚");
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
            object ID = MySqlManager.GetKey
                (
                string.Format("SELECT ID FROM {0} WHERE GUID = '{1}'", Tables.Primary, GUID)
                );

            string SQL = string.Format
                (
                "DELETE FROM {1} WHERE GUID = (SELECT GUID FROM {0} WHERE ID = ?ID);" +
                "UPDATE {0} SET GUID = ?GUID WHERE ID = ?ID;"
                , Tables.Index, Tables.Primary
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "ID", Val = ID },
                new MySqlParm() { Name = "GUID", Val = GUID }
            };

            MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList);
            MySqlCommand.Connection = MySqlManager.Connection;

            /* 开始事务 */
            MySqlCommand.Transaction = MySqlManager.Connection.BeginTransaction();

            if (MySqlManager.QueryOnly(ref MySqlCommand) == 2)
            {
                /* 指向表修改1行数据，拷贝表删除一行数据 */
                MySqlCommand.Transaction.Commit();
                return true;
            }
            else
            {
                /* 操作行数异常，回滚事务 */
                MySqlCommand.Transaction.Rollback();
                throw new Exception("操作行数异常，事务已回滚");
            }
        }
        /// <summary>
        /// 回滚拷贝
        /// </summary>
        /// <param name="ID">目标文章的ID</param>
        /// <returns></returns>
        public bool Rollback(int ID)
        {
            string SQL = string.Format
                (
                "DELETE {1} FROM {0},{1} WHERE ({0}.GUID = {1}.GUID) AND ({0}.ID = ?ID);" +
                "UPDATE {0} SET GUID = (SELECT GUID FROM {1} WHERE ID=?ID ORDER BY LCT DESC LIMIT 0,1) WHERE ID=?ID;"
                , Tables.Index, Tables.Primary
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "ID", Val = ID }
            };

            MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList);
            MySqlCommand.Connection = MySqlManager.Connection;

            /* 开始事务 */
            MySqlCommand.Transaction = MySqlManager.Connection.BeginTransaction();

            if (MySqlManager.QueryOnly(ref MySqlCommand) == 2)
            {
                /* 指向表修改1行数据，拷贝表删除1行数据 */
                MySqlCommand.Transaction.Commit();
                return true;
            }
            else
            {
                /* 操作行数异常，回滚事务 */
                MySqlCommand.Transaction.Rollback();
                throw new Exception("操作行数异常，事务已回滚");
            }
        }
        /// <summary>
        /// 释放拷贝
        /// </summary>
        /// <param name="ID">目标文章的ID</param>
        /// <returns></returns>
        public bool Release(int ID)
        {
            string SQL = string.Format
                (
                "DELETE {1} FROM {0},{1} WHERE ({0}.ID = {1}.ID) AND ({0}.GUID <> {1}.GUID) AND ({0}.ID = ?ID)"
                , Tables.Index, Tables.Primary
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "?ID", Val = ID }
            };

            MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList);
            MySqlCommand.Connection = MySqlManager.Connection;

            /* 开始事务 */
            MySqlCommand.Transaction = MySqlManager.Connection.BeginTransaction();

            if (MySqlManager.QueryOnly(ref MySqlCommand) >= 0)
            {
                /* 删除拷贝表的所有冗余，不存在冗余时影响行数为0 */
                MySqlCommand.Transaction.Commit();
                return true;
            }
            else
            {
                /* 操作行数异常，回滚事务 */
                MySqlCommand.Transaction.Rollback();
                throw new Exception("操作行数异常，事务已回滚");
            }
        }

        /// <summary>
        /// 将目标文章指向的模式设为：展示
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public bool ShowMode(int ID)
        {
            //初始化键定位
            MySqlKey MySqlKey = new MySqlKey
            {
                Table = Tables.Index,
                Name = "ID",
                Val = ID.ToString()
            };
            return MySqlManager.UpdateKey(MySqlKey, "Mode", "show");
        }
        /// <summary>
        /// 将目标文章指向的模式设为：隐藏
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public bool CloseMode(int ID)
        {
            //初始化键定位
            MySqlKey MySqlKey = new MySqlKey
            {
                Table = Tables.Index,
                Name = "ID",
                Val = ID.ToString()
            };
            return MySqlManager.UpdateKey(MySqlKey, "Mode", "closed");
        }
        /// <summary>
        /// 将目标文章指向的模式设为：计划中
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public bool ScheduleMode(int ID)
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
        public bool ArchivMode(int ID)
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
        public bool UpdateIndex<T>(int ID, object Value) where T : IKey
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
        /// <param name="GUID">目标拷贝GUID</param>
        /// <param name="Value">新属性值</param>
        /// <returns></returns>
        public bool UpdatePrimary<T>(int GUID, object Value) where T : IKey
        {
            //初始化键定位
            MySqlKey MySqlKey = new MySqlKey
            {
                Table = Tables.Primary,
                Name = "GUID",
                Val = GUID.ToString()
            };
            return MySqlManager.UpdateKey(MySqlKey, typeof(T).Name, Convert.ToString(Value));
        }
        //ID更改方式需要编写

        /// <summary>
        /// 检测ID、GUID是否匹配，之后合并Post数据表
        /// </summary>
        /// <parmm name="Index">索引表Post实例</parmm>
        /// <parmm name="Primary">主表Post实例</parmm>
        /// <returns></returns>
        public static Post Join(Post Index, Post Primary)
        {
            if (Index.ID == Primary.ID && Index.GUID == Primary.GUID)
            {
                return new Post
                {
                    ID = Index.ID,
                    GUID = Index.GUID,

                    Mode = Index.Mode,
                    Type = Index.Type,

                    Title = Primary.Title,
                    Summary = Primary.Summary,
                    Content = Primary.Content,

                    User = Index.User,
                    Archiv = Primary.Archiv,
                    Label = Primary.Label,

                    CT = Index.CT,
                    LCT = Primary.LCT,

                    UVCount = Index.UVCount,
                    StarCount = Index.StarCount,

                    Cover = Primary.Cover
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
        /// <parmm name="Primary">主表Post实例</parmm>
        /// <returns>始终返回以Index的ID、GUID为最终合并结果的Post实例</returns>
        public static Post ForcedJoin(Post Index, Post Primary)
        {
            return new Post
            {
                ID = Index.ID,
                GUID = Index.GUID,

                Mode = Index.Mode,
                Type = Index.Type,

                Title = Primary.Title,
                Summary = Primary.Summary,
                Content = Primary.Content,

                User = Index.User,
                Archiv = Primary.Archiv,
                Label = Primary.Label,

                CT = Index.CT,
                LCT = Primary.LCT,

                UVCount = Index.UVCount,
                StarCount = Index.StarCount,

                Cover = Primary.Cover
            };
        }
    }
}