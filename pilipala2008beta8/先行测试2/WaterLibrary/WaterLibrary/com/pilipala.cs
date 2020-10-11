﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Data;

using WaterLibrary.com.MySQL;
using WaterLibrary.stru;
using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala;
using WaterLibrary.stru.pilipala.DB;
using WaterLibrary.stru.pilipala.PostKey;

using Label = WaterLibrary.stru.pilipala.PostKey.Label;
using Type = WaterLibrary.stru.pilipala.PostKey.Type;

using System.Text.RegularExpressions;

using System.Dynamic;
using WaterLibrary.com.basic;
using System.Web.UI.WebControls;

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
            string SQL = "CALL `get>user`( ?ID )";

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {

                    new MySqlParm() { Name = "?ID", Val = ID }
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

            string SQL = string.Format("SELECT ID{0} FROM `view>total`", KeysStr);

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
            string RegExp = ConvertH.ListToString(MatchList, '|');

            string SQL = string.Format("SELECT ID{0} FROM `view>total` WHERE {1} REGEXP ?RegExp", KeysStr, typeof(T).Name);

            List<Post> PostList = new List<Post>();

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL,
                new MySqlParm() { Name = "?RegExp", Val = RegExp }
                ))
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
            string SQL = string.Format("SELECT ID FROM `{0}` ORDER BY CT DESC LIMIT ?Start , ?Length", Views.Index);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?Start", Val = Start },
                    new MySqlParm() { Name = "?Length", Val = Length }
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
            string SQL = string.Format("SELECT ID FROM `{0}` WHERE Type = ?Type ORDER BY CT DESC LIMIT ?Start , ?Length", Views.Index);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?Type", Val = Type },
                    new MySqlParm() { Name = "?Start", Val = Start },
                    new MySqlParm() { Name = "?Length", Val = Length }
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

            string SQL = string.Format("SELECT ID FROM `{0}` ORDER BY ?OrderKey ?OrderType", Views.Index);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?OrderKey", Val = OrderProperty },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType }
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

            string SQL = string.Format("SELECT ID FROM `{0}` ORDER BY ?OrderKey ?OrderType LIMIT 0,?Length", Views.Index);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?OrderKey", Val = OrderProperty },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType },
                    new MySqlParm() { Name = "?Length", Val = Length }
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
            string str = ConvertH.ListToString(MatchList, '|');

            string SQL = string.Format("SELECT ID FROM `{0}` WHERE ?Key REGEXP ?str", Views.Index);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?Key", Val = typeof(T).Name },
                    new MySqlParm() { Name = "?str", Val = str }
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
            string str = ConvertH.ListToString(MatchList, '|');

            string SQL = string.Format("SELECT ID FROM `{0}` WHERE ?Key REGEXP ?str ORDER BY ?OrderKey ?OrderType", Views.Index);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?Key", Val = typeof(T).Name },
                    new MySqlParm() { Name = "?str", Val = str },

                    new MySqlParm() { Name = "?OrderKey", Val = OrderKey },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType }
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
            string str = ConvertH.ListToString(MatchList, '|');

            string SQL = string.Format("SELECT ID FROM `{0}` WHERE ?Key REGEXP ?str ORDER BY ?Key ?OrderType LIMIT 0,?Length", Views.Index);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?Key", Val = typeof(T).Name },
                    new MySqlParm() { Name = "?str", Val = str },

                    new MySqlParm() { Name = "?OrderKey", Val = OrderProperty },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType },

                    new MySqlParm() { Name = "?Length", Val = Length }
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
        /// <parmm name="Value">被匹配的属性值</parmm>
        /// <returns></returns>
        public virtual List<int> MatchPost<T>(string Value) where T : IKey
        {
            List<int> IDList = new List<int>();

            string SQL = string.Format("SELECT ID FROM `{0}` WHERE ?Key REGEXP ?Val", Views.Total);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?Key", Val = typeof(T).Name },
                    new MySqlParm() { Name = "?Val", Val = Value }
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
        /// 通过ID获取Index表中的目标文章数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public virtual Post GetIndex(int ID)
        {
            string SQL = "CALL `get>index`( ?ID )";

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?ID", Val = ID }
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
        /// 获取Index表中目标文章的指定属性
        /// </summary>
        /// <typeparam name="T">目标属性类型</typeparam>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public virtual object GetIndex<T>(int ID) where T : IKey
        {
            string SQL = string.Format("SELECT ?Key FROM `{0}` WHERE ID = ?ID", Views.Index);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?Key", Val = typeof(T).Name },
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlManager.GetRow(MySqlCommand)[0];
            }
        }

        /// <summary>
        /// 通过ID获取Primary表中的目标文章数据
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public virtual Post GetPrimary(int ID)
        {
            string SQL = "CALL `get>primary`( ?ID )";

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID }
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
        /// 获取Primary表中目标文章的指定属性
        /// </summary>
        /// <typeparam name="T">目标属性类型</typeparam>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public virtual object GetPrimary<T>(int ID) where T : IKey
        {
            string SQL = string.Format("SELECT ?Key FROM `{0}` WHERE ID = ?ID", Views.Primary);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?Key", Val = typeof(T).Name },
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlManager.GetRow(MySqlCommand)[0];
            }
        }

        /// <summary>
        /// 取得文章文本型属性（从Primary视图）
        /// </summary>
        /// <typeparam name="T">目标属性类型</typeparam>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public virtual string Get<T>(int ID) where T : IKey
        {
            string SQL = string.Format("SELECT {0} FROM `{1}` WHERE ID = ?ID", typeof(T).Name, Views.Total);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlManager.GetKey<string>(MySqlCommand);
            }
        }
        /// <summary>
        /// 取得文章文本型属性（从Primary视图，限制最大取用长度）
        /// </summary>
        /// <typeparam name="T">目标属性类型</typeparam>
        /// <param name="ID">目标文章ID</param>
        /// <param name="Length">最大取用长度</param>
        /// <returns></returns>
        public virtual string Get<T>(int ID, int Length) where T : IKey
        {
            string SQL = string.Format("SELECT SUBSTRING(( SELECT {0} FROM `{1}` WHERE ID = ?ID ) FROM 1 FOR ?Length)", typeof(T).Name, Views.Total);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlManager.GetKey<string>(MySqlCommand);
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
            string SQL = string.Format("SELECT ID FROM `{0}` WHERE ID =( SELECT min(ID) FROM `{0}` WHERE ID > ?ID );", Views.Index);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                DataRow DataRow = MySqlManager.GetRow(MySqlCommand);
                if (DataRow == null)
                {
                    return -1;
                }
                else
                {
                    return Convert.ToInt32(DataRow[0]);
                }
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
            string SQL = string.Format("SELECT ID FROM `{0}` WHERE " +
                         "?Key =( SELECT min(?Key) FROM `{0}` WHERE " +
                         "?Key >( SELECT ?Key FROM `{0}` WHERE ID = ?ID ))", Views.Index);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?Key", Val = typeof(T).Name }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                DataRow DataRow = MySqlManager.GetRow(MySqlCommand);
                if (DataRow == null)
                {
                    return -1;
                }
                else
                {
                    return Convert.ToInt32(DataRow[0]);
                }
            }
        }
        /// <summary>
        /// 取得目标文章ID 的下一个文章ID（在正则表达式匹配到的归档范围内，按ID升序查找）
        /// </summary>
        /// <param name="str">正则表达式</param>
        /// <param name="ID">目标文章ID</param>
        /// <returns>不存在符合要求的ID时，返回-1</returns>
        public virtual int NextID(string str, int ID)
        {
            string SQL = string.Format("SELECT ID FROM `{0}` WHERE ID =( SELECT min(ID) FROM `{0}` WHERE ID > ?ID AND Archiv REGEXP ?str);", Views.Primary);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?str", Val = str }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                DataRow DataRow = MySqlManager.GetRow(MySqlCommand);
                if (DataRow == null)
                {
                    return -1;
                }
                else
                {
                    return Convert.ToInt32(DataRow[0]);
                }
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
            string SQL = string.Format("SELECT ID FROM `{0}` WHERE " +
                         "ID =( SELECT max(ID) FROM `{0}` WHERE ID < ?ID );", Views.Index);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                DataRow DataRow = MySqlManager.GetRow(MySqlCommand);
                if (DataRow == null)
                {
                    return -1;
                }
                else
                {
                    return Convert.ToInt32(DataRow[0]);
                }
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
            string SQL = string.Format("SELECT ID FROM `{0}` WHERE " +
                         "?Key =( SELECT max(?Key) FROM `{0}` WHERE " +
                         "?Key <( SELECT ?Key FROM `{0}` WHERE ID = ?ID ))", Views.Index);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?Key", Val = typeof(T).Name }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                DataRow DataRow = MySqlManager.GetRow(MySqlCommand);
                if (DataRow == null)
                {
                    return -1;
                }
                else
                {
                    return Convert.ToInt32(DataRow[0]);
                }
            }
        }
        /// <summary>
        /// 取得指定目标文章ID 的上一个文章ID（在正则表达式匹配到的归档范围内，按ID升序查找）
        /// </summary>
        /// <param name="str">正则表达式</param>
        /// <param name="ID">目标文章ID</param>
        /// <returns>不存在符合要求的ID时，返回-1</returns>
        public virtual int PrevID(string str, int ID)
        {
            string SQL = string.Format("SELECT ID FROM `{0}` WHERE " +
                         "ID =( SELECT max(ID) FROM `{0}` WHERE ID < ?ID AND Archiv REGEXP ?str);", Views.Primary);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?str", Val = str }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                DataRow DataRow = MySqlManager.GetRow(MySqlCommand);
                if (DataRow == null)
                {
                    return -1;
                }
                else
                {
                    return Convert.ToInt32(DataRow[0]);
                }
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
            try
            {
                string SQL = string.Format("SELECT max(ID) FROM `{0}`", Tables.Index);

                return Convert.ToInt32(MySqlManager.GetRow(SQL)[0]);
            }
            catch
            {
                return -1;
            }
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


        /// <summary>
        /// 注册文章
        /// </summary>
        /// <param name="Post">文章数据（ID、GUID、CT、LCT、User由系统生成）</param>
        /// <returns>返回受影响的行数</returns>
        public bool Reg(Post Post)
        {
            DateTime t = DateTime.Now;

            string SQL = string.Format(
                "INSERT INTO `{0}`" +
                " (`ID`, `GUID`, `CT`, `Mode`, `Type`, `User`, `UVCount`, `StarCount`) VALUES" +
                " ('?ID','?GUID', '?CT','?Mode','?Type','?User','?UVCount','?StarCount');"
                +
                "INSERT INTO `{1}`" +
                " (`ID`, `GUID`, `LCT`, `Title`, `Summary`, `Content`, `Archiv`, `Label`, `Cover`) VALUES" +
                " ('?ID','?GUID','?LCT','?Title','?Summary','?Content','?Archiv','?Label','?Cover');"

                , Tables.Index, Tables.Primary);


            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "?ID", Val = GetMaxID() + 1 },
                new MySqlParm() { Name = "?GUID", Val = MathH.GenerateGUID("D") },

                new MySqlParm() { Name = "?CT", Val = t },
                new MySqlParm() { Name = "?LCT", Val = t },

                new MySqlParm() { Name = "?Mode", Val = Post.Mode },
                new MySqlParm() { Name = "?Type", Val = Post.Type },
                new MySqlParm() { Name = "?User", Val = Post.User },

                new MySqlParm() { Name = "?UVCount", Val = Post.UVCount },
                new MySqlParm() { Name = "?StarCount", Val = Post.StarCount },

                new MySqlParm() { Name = "?Title", Val = Post.Title },
                new MySqlParm() { Name = "?Summary", Val = Post.Summary },
                new MySqlParm() { Name = "?Content", Val = Post.Content },

                new MySqlParm() { Name = "?Archiv", Val = Post.Archiv },
                new MySqlParm() { Name = "?Label", Val = Post.Label },
                new MySqlParm() { Name = "?Cover", Val = Post.Cover }
            };
            if (MySqlManager.ParmQueryCMD(SQL, ParmList).ExecuteNonQuery() == 2)
                return true;
            else
                throw new Exception("多行操作异常");
        }
        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="Post">文章数据</param>
        /// <returns></returns>
        public bool UpdatePost(Post Post)
        {
            DateTime t = DateTime.Now;

            string SQL = string.Format(
                "UPDATE `{0}` SET" +
                " GUID='?GUID', Mode='?Mode', Type='?Type', UVCount='?UVCount', StarCount='?StarCount' WHERE ID = ?ID;"
                +
                "INSERT INTO `{1}`" +
                " (`ID`, `GUID`, `LCT`, `Title`, `Summary`, `Content`, `Archiv`, `Label`, `Cover`) VALUES" +
                " ('?ID','?GUID','?LCT','?Title','?Summary','?Content','?Archiv','?Label','?Cover');"

                , Tables.Index, Tables.Primary);

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "?ID", Val = GetMaxID() + 1 },
                new MySqlParm() { Name = "?GUID", Val = MathH.GenerateGUID("D") },

                new MySqlParm() { Name = "?LCT", Val = t },

                new MySqlParm() { Name = "?Mode", Val = Post.Mode },
                new MySqlParm() { Name = "?Type", Val = Post.Type },

                new MySqlParm() { Name = "?UVCount", Val = Post.UVCount },
                new MySqlParm() { Name = "?StarCount", Val = Post.StarCount },

                new MySqlParm() { Name = "?Title", Val = Post.Title },
                new MySqlParm() { Name = "?Summary", Val = Post.Summary },
                new MySqlParm() { Name = "?Content", Val = Post.Content },

                new MySqlParm() { Name = "?Archiv", Val = Post.Archiv },
                new MySqlParm() { Name = "?Label", Val = Post.Label },
                new MySqlParm() { Name = "?Cover", Val = Post.Cover }
            };
            if (MySqlManager.ParmQueryCMD(SQL, ParmList).ExecuteNonQuery() == 2)
                return true;
            else
                throw new Exception("多行操作异常");
        }

        /* 风险性功能，需要周全的开发逻辑以防止生产用数据库的意外灾难 */
        /// <summary>
        /// 注销文章（删除所有副本）
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public bool DisposeReg(int ID)
        {
            string SQL = string.Format("DELETE FROM `{0}` WHERE ID = ?ID; DELETE FROM `{1}` WHERE ID = ?ID;", Tables.Index, Tables.Primary);

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "?ID", Val = ID }
            };

            if (MySqlManager.ParmQueryCMD(SQL, ParmList).ExecuteNonQuery() >= 2)
                return true;
            else
                throw new Exception("多行操作异常");
        }
        /// <summary>
        /// 删除副本（仅单个副本）
        /// </summary>
        /// <param name="GUID">目标文章的GUID</param>
        /// <returns></returns>
        public bool DeletePost(string GUID)
        {
            string SQL = string.Format("DELETE FROM `{0}` WHERE GUID = ?GUID;", Tables.Primary);

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "?GUID", Val = GUID }
            };

            if (MySqlManager.ParmQueryCMD(SQL, ParmList).ExecuteNonQuery() >= 1)
                return true;
            else
                throw new Exception("多行操作异常");
        }

        /// <summary>
        /// 将目标文章状态标记为展示
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public bool ShowReg(int ID)
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
        /// 将目标文章状态标记为隐藏
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public bool CloseReg(int ID)
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
        /// 将目标文章状态标记为计划中
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public bool ScheduleReg(int ID)
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
        /// 将目标文章状态标记为已归档
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public bool ArchivReg(int ID)
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
        /// 通用文章属性更新器
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
        /// 通用文章属性更新器
        /// </summary>
        /// <typeparam name="T">目标属性类型</typeparam>
        /// <param name="ID">目标文章ID</param>
        /// <param name="Value">新属性值</param>
        /// <returns></returns>
        public bool UpdatePrimary<T>(int ID, object Value) where T : IKey
        {
            //初始化键定位
            MySqlKey MySqlKey = new MySqlKey
            {
                Table = Tables.Primary,
                Name = "ID",
                Val = ID.ToString()
            };
            return MySqlManager.UpdateKey(MySqlKey, typeof(T).Name, Convert.ToString(Value));
        }

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