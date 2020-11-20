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

namespace WaterLibrary.com.pilipala
{
    public class CORE
    {
        /// <summary>
        /// MySql控制器
        /// </summary>
        public MySqlManager MySqlManager { get; private set; }

        public PLTables Tables { get; private set; }
        public PLViews Views { get; private set; }

        public CORE(PLDB PLDB)
        {
            MySqlManager = PLDB.MySqlManager;
            SetTables(PLDB.Tables.User, PLDB.Tables.Index, PLDB.Tables.Backup);
            SetViews(PLDB.Views.Index, PLDB.Views.Backup, PLDB.Views.Union);
        }

        public void Run()
        {
            MySqlManager.Open();
        }

        public void Shutdown()
        {
            MySqlManager.Close();
        }

        public void SetTables(string User = "pl_user", string Index = "pl_index", string Backup = "pl_backup")
        {
            Tables = new PLTables(User, Index, Backup);

        }
        public void SetViews(string Index = "view>index", string Backup = "view>backup", string Union = "view>union")
        {
            Views = new PLViews(Index, Backup, Union);
        }

    }

    /// <summary>
    /// 啪啦数据读取器
    /// </summary>
    public class PLDR : IPLDataReader
    {
        /// <summary>
        /// 表集
        /// </summary>
        public PLTables Tables { get; private set; }
        /// <summary>
        /// 视图集
        /// </summary>
        public PLViews Views { get; private set; }
        /// <summary>
        /// MySql数据库管理器
        /// </summary>
        public MySqlManager MySqlManager { get; private set; }

        public PLDR(CORE CORE)
        {
            Tables = CORE.Tables;
            Views = CORE.Views;
            MySqlManager = CORE.MySqlManager;
        }


        /// <summary>
        /// 获得文章数据列表(使用MatchList匹配)
        /// 注意！在数据需求量较大或较小时，此方法性能损耗较大
        /// </summary>
        /// <typeparam name="T">属性匹配列表的属性类型</typeparam>
        /// <param name="MatchList">属性匹配列表</param>
        /// <param name="PropertyType">所需属性类型</param>
        /// <returns></returns>
        public List<Post> GetList<T>(List<string> MatchList, params System.Type[] PropertyType) where T : IPostKey
        {
            /* 键名字符串格式化 */
            string KeysStr = ConvertH.ListToString(PropertyType, "Name", ',');
            /* 正则表达式格式化 */
            string REGEXP = ConvertH.ListToString(MatchList, '|');

            string SQL = string.Format
                (
                "SELECT ID,{0} FROM `{1}` WHERE {2} REGEXP ?REGEXP"
                , KeysStr, Views.Union, typeof(T).Name
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
        /// 获得文章数据列表
        /// 注意！在数据需求量较大或较小时，此方法性能损耗较大
        /// </summary>
        /// <param name="PropertyType">所需属性类型</param>
        /// <returns>返回根据所需类型填充的Post集合</returns>
        public List<Post> GetList(params System.Type[] PropertyType)
        {
            /* 介于类型转到字符串而生成SQL的方式，此方法无需对SQL注入防护。 */
            string KeysStr = ConvertH.ListToString(PropertyType, "Name", ',');

            string SQL = string.Format("SELECT ID,{0} FROM `{1}`", KeysStr, Views.Union);

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
        /// 取得全部文章的全部数据列表
        /// 注意！在数据需求量较小时，此方法性能损耗较大
        /// </summary>
        /// <returns></returns>
        public List<Post> GetList()
        {
            string SQL = string.Format("SELECT * FROM `{0}`", Views.Union);

            List<Post> List = new List<Post>();

            foreach (DataRow Row in MySqlManager.GetTable(SQL).Rows)
            {
                List.Add(new Post
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

            return List;
        }

        /// <summary>
        /// 获得全部文章ID列表
        /// </summary>
        /// <returns></returns>
        public List<int> GetIDList()
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
        public List<int> GetIDList(int Start, int Length)
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
        public List<int> GetIDList(int Start, int Length, string Type)
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
        /// <param name="OrderType">ASC(升序)或DESC(降序)，不区分大小写</param>
        /// <returns></returns>
        public List<int> GetIDList(string OrderProperty, string OrderType)
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
        /// <param name="OrderType">排序类型，可选值：ASC(升序)或DESC(降序)，不区分大小写</param>
        /// <param name="Length">截止长度</param>
        /// <returns></returns>
        public List<int> GetIDList(string OrderProperty, string OrderType, int Length)
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
        public List<int> GetIDList<T>(List<string> MatchList) where T : IPostKey
        {
            List<int> IDList = new List<int>();

            /* 正则表达式格式化 */
            string REGEXP = ConvertH.ListToString(MatchList, '|');

            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE {1} REGEXP ?REGEXP"
                , Views.Union, typeof(T).Name
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
        /// <param name="OrderKey">排序类型，选值：ASC(升序)或DESC(降序)，不区分大小写</param>
        /// <param name="OrderType">排序类型</param>
        /// <returns></returns>
        public List<int> GetIDList<T>(List<string> MatchList, string OrderKey, string OrderType) where T : IPostKey
        {
            List<int> IDList = new List<int>();

            /* 正则表达式格式化 */
            string REGEXP = ConvertH.ListToString(MatchList, '|');

            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE {1} REGEXP ?REGEXP ORDER BY ?OrderKey ?OrderType"
                , Views.Union, typeof(T).Name
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
        /// <param name="OrderType">排序类型，可选值：ASC(升序)或DESC(降序)，不区分大小写</param>
        /// <param name="Length">最大取用长度</param>
        /// <returns></returns>
        public List<int> GetIDList<T>(List<string> MatchList, string OrderProperty, string OrderType, int Length) where T : IPostKey
        {
            List<int> IDList = new List<int>();

            /* 正则表达式格式化 */
            string REGEXP = ConvertH.ListToString(MatchList, '|');

            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE {1} REGEXP ?REGEXP ORDER BY ?Key ?OrderType LIMIT 0,?Length"
                , Views.Union, typeof(T).Name
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
        public List<int> MatchID<T>(string REGEXP) where T : IPostKey
        {
            List<int> IDList = new List<int>();

            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE {1} REGEXP ?REGEXP"
                , Views.Union, typeof(T).Name
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
        public List<Post> MatchPost<T>(string REGEXP) where T : IPostKey
        {
            List<Post> PostList = new List<Post>();

            string SQL = string.Format
                (
                "SELECT * FROM `{0}` WHERE {1} REGEXP ?REGEXP"
                , Views.Union, typeof(T).Name
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
        public Post GetIndex(int ID)
        {
            string SQL = string.Format("SELECT * FROM `{0}` WHERE ID = ?ID", Views.Index);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                DataRow Row = MySqlManager.GetRow(MySqlCommand);

                return new Post
                {
                    ID = Convert.ToInt32(Row["ID"]),
                    GUID = Convert.ToString(Row["GUID"]),
                    CT = Convert.ToDateTime(Row["CT"]),
                    Mode = Convert.ToString(Row["Mode"]),
                    Type = Convert.ToString(Row["Type"]),
                    User = Convert.ToString(Row["User"]),
                    UVCount = Convert.ToInt32(Row["UVCount"]),
                    StarCount = Convert.ToInt32(Row["StarCount"])
                };
            }
        }
        /// <summary>
        /// 通过ID获取Backup表中的目标文章数据
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public Post GetBackup(int ID)
        {
            string SQL = string.Format("SELECT * FROM `{0}` WHERE ID = ?ID", Views.Backup);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                DataRow Row = MySqlManager.GetRow(MySqlCommand);

                return new Post
                {
                    ID = Convert.ToInt32(Row["ID"]),
                    GUID = Convert.ToString(Row["GUID"]),
                    LCT = Convert.ToDateTime(Row["LCT"]),
                    Title = Convert.ToString(Row["Title"]),
                    Summary = Convert.ToString(Row["Summary"]),
                    Content = Convert.ToString(Row["Content"]),
                    Archiv = Convert.ToString(Row["Archiv"]),
                    Label = Convert.ToString(Row["Label"]),
                    Cover = Convert.ToString(Row["Cover"])
                };
            }
        }
        /// <summary>
        /// 通过ID获取Union视图中的目标文章数据(Index Join Backup)
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public Post GetUnion(int ID)
        {
            string SQL = string.Format("SELECT * FROM `{0}` WHERE ID = ?ID", Views.Union);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                DataRow Row = MySqlManager.GetRow(MySqlCommand);

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
        }
        /// <summary>
        /// 获取Index表中目标文章的指定属性
        /// </summary>
        /// <typeparam name="T">目标属性类型</typeparam>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public object GetIndex<T>(int ID) where T : IPostKey
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
        /// 获取Backup表中目标文章的指定属性
        /// </summary>
        /// <typeparam name="T">目标属性类型</typeparam>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public object GetBackup<T>(int ID) where T : IPostKey
        {
            string SQL = string.Format
                (
                "SELECT {0} FROM `{1}` WHERE ID = ?ID"
                , typeof(T).Name, Views.Backup
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
        public string Get<T>(int ID) where T : IPostKey
        {
            string SQL = string.Format
                (
                "SELECT {0} FROM `{1}` WHERE ID = ?ID"
                , typeof(T).Name, Views.Union
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
        public string Get<T>(int ID, int Length) where T : IPostKey
        {
            string SQL = string.Format
                (
                "SELECT SUBSTRING(( SELECT {0} FROM `{1}` WHERE ID = ?ID ) FROM 1 FOR ?Length)"
                , typeof(T).Name, Views.Union
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
        public int NextID(int ID)
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
        public int NextID<T>(int ID)
        {
            /* 取得比当前 ID 大的一行，实现对下一条数据的抓取 */
            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE " +
                "{1} =( SELECT min({1}) FROM `{0}` WHERE " +
                "{1} >( SELECT {1} FROM `{0}` WHERE ID = ?ID ))"
                , Views.Union, typeof(T).Name
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
        public int NextID(string REGEXP, int ID)
        {
            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE ID=( SELECT min(ID) FROM `{0}` WHERE ID > ?ID AND Archiv REGEXP ?REGEXP )"
                , Views.Backup
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
        public int PrevID(int ID)
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
        public int PrevID<T>(int ID)
        {
            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE " +
                "{1} =( SELECT max({1}) FROM `{0}` WHERE " +
                "{1} <( SELECT {1} FROM `{0}` WHERE ID = ?ID ))"
                , Views.Union, typeof(T).Name
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
        public int PrevID(string REGEXP, int ID)
        {
            string SQL = string.Format
                (
                "SELECT ID FROM `{0}` WHERE ID=(SELECT max(ID) FROM `{0}` WHERE ID < ?ID AND Archiv REGEXP ?REGEXP)"
                , Views.Backup
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

        public List<Post> GetBackupList(int ID)
        {
            string SQL = string.Format
                (
                "SELECT {0}.ID,{1}.GUID,CT,LCT,Title,Summary,Content,Archiv,Label,Cover,Mode,Type,User,UVCount,StarCount" +
                " FROM {0} JOIN {1} ON {0}.ID={1}.ID WHERE {0}.ID=?ID"
                , Tables.Index, Tables.Backup
                );

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                List<Post> List = new List<Post>();
                foreach (DataRow Row in MySqlManager.GetTable(MySqlCommand).Rows)
                {
                    List.Add(new Post
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

                return List;
            }
        }
    }
    /// <summary>
    /// 啪啦数据修改器
    /// </summary>
    public class PLDU : IPLDataUpdater
    {
        /// <summary>
        /// 表集
        /// </summary>
        public PLTables Tables { get; private set; }
        /// <summary>
        /// 视图集
        /// </summary>
        public PLViews Views { get; private set; }
        /// <summary>
        /// MySql数据库管理器
        /// </summary>
        public MySqlManager MySqlManager { get; private set; }

        /// <summary>
        /// 得到最大文章ID（私有）
        /// </summary>
        /// <returns></returns>
        internal int GetMaxID()
        {
            string SQL = string.Format("SELECT MAX(ID) FROM {0}", Tables.Index);
            var value = MySqlManager.GetKey(SQL);
            /* 若取不到最大ID(没有任何文章时)，返回12000作为初始ID */
            return Convert.ToInt32(value.GetType() == typeof(DBNull) ? 12000 : value);
        }
        /// <summary>
        /// 得到最小文章ID（私有）
        /// </summary>
        /// <returns>错误则返回-1</returns>
        internal int GetMinID()
        {
            string SQL = string.Format("SELECT MIN(ID) FROM {0}", Tables.Index);
            var value = MySqlManager.GetKey(SQL);
            /* 若取不到最大ID(没有任何文章时)，返回12000作为初始ID */
            return Convert.ToInt32(value.GetType() == typeof(DBNull) ? 12000 : value);
        }
        /// <summary>
        /// 获取指定文章的活跃拷贝的GUID
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        internal string GetActiveGUID(int ID)
        {
            return Convert.ToString(MySqlManager.GetKey(string.Format("SELECT GUID FROM {0} WHERE ID={1}", Tables.Index, ID)));
        }

        public PLDU(CORE CORE)
        {
            Tables = CORE.Tables;
            Views = CORE.Views;
            MySqlManager = CORE.MySqlManager;
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
                , Tables.Index, Tables.Backup
                );


            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "ID", Val = GetMaxID() + 1 },
                new MySqlParm() { Name = "GUID", Val = MathH.GenerateGUID("N") },

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
                CommandText = string.Format
                (
                "DELETE FROM {0} WHERE ID={2};" +
                "DELETE FROM {1} WHERE ID={2};"
                , Tables.Index, Tables.Backup, ID
                ),

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
            string SQL = string.Format
                (
                "UPDATE {0} SET GUID=?GUID, Mode=?Mode, Type=?Type, UVCount=?UVCount, StarCount=?StarCount WHERE ID=?ID;" +
                "INSERT INTO {1}" +
                " ( ID, GUID, LCT, Title, Summary, Content, Archiv, Label, Cover) VALUES" +
                " (?ID,?GUID,?LCT,?Title,?Summary,?Content,?Archiv,?Label,?Cover);"
                , Tables.Index, Tables.Backup
                );

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "GUID", Val = MathH.GenerateGUID("N") },
                new MySqlParm() { Name = "LCT", Val = DateTime.Now },

                /* 可传参数 */
                new MySqlParm() { Name = "ID", Val = Post.ID },

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

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "GUID", Val = GUID }
            };

            MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList);
            MySqlCommand.Connection = MySqlManager.Connection;

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
            object ID = MySqlManager.GetKey
                (
                string.Format("SELECT ID FROM {0} WHERE GUID = '{1}'", Tables.Backup, GUID)
                );

            string SQL = string.Format
                (
                "DELETE FROM {1} WHERE GUID = (SELECT GUID FROM {0} WHERE ID = ?ID);" +
                "UPDATE {0} SET GUID = ?GUID WHERE ID = ?ID;"
                , Tables.Index, Tables.Backup
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
        public bool UpdateIndex<T>(int ID, object Value) where T : IPostKey
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
        public bool UpdateBackup<T>(int ID, object Value) where T : IPostKey
        {
            //初始化键定位
            MySqlKey MySqlKey = new MySqlKey
            {
                Table = Tables.Backup,
                Name = "GUID",
                Val = GetActiveGUID(ID)
            };
            return MySqlManager.UpdateKey(MySqlKey, typeof(T).Name, Convert.ToString(Value));
        }
        //ID更改方式需要编写

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
    public class PLDC : IPLDataCounter
    {
        /// <summary>
        /// 表集
        /// </summary>
        public PLTables Tables { get; private set; }
        /// <summary>
        /// 视图集
        /// </summary>
        public PLViews Views { get; private set; }
        /// <summary>
        /// MySql数据库管理器
        /// </summary>
        public MySqlManager MySqlManager { get; private set; }

        public PLDC(CORE CORE)
        {
            Tables = CORE.Tables;
            Views = CORE.Views;
            MySqlManager = CORE.MySqlManager;
        }


        /// <summary>
        /// 文章计数
        /// </summary>
        public int PostCount
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
            get { return GetPostCountByMode("x"); }
        }
        /// <summary>
        /// 展示中文章计数
        /// </summary>
        public int OnDisplayCount
        {
            get { return GetPostCountByMode("o"); }
        }
        /// <summary>
        /// 归档中文章计数
        /// </summary>
        public int ArchivedCount
        {
            get { return GetPostCountByMode("archived"); }
        }
        /// <summary>
        /// 计划中文章计数
        /// </summary>
        public int ScheduledCount
        {
            get { return GetPostCountByMode("sche"); }
        }

        private int GetPostCountByMode(string REGEXP)
        {
            string SQL = string.Format("SELECT Count(*) FROM {0} WHERE Mode REGEXP '{1}';", Tables.Index, REGEXP);

            return Convert.ToInt32(MySqlManager.GetKey(SQL));
        }
        private int GetBackupCount()
        {
            string SQL = string.Format("SELECT COUNT(*) FROM {0},{1} WHERE {0}.ID={1}.ID AND {0}.GUID<>{1}.GUID;", Tables.Index, Tables.Backup);

            return Convert.ToInt32(MySqlManager.GetKey(SQL));
        }
    }
}
