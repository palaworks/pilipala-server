using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Data;

using WaterLibrary.com.MySQL;
using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala;
using WaterLibrary.stru.pilipala.core;

using System.Text.RegularExpressions;

using Type = WaterLibrary.stru.pilipala.Type;
using Label = WaterLibrary.stru.pilipala.Label;

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
        public string DataBase { get; private set; }
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
        public MySqlConnH MySqlConnH { get; private set; }


        /// <summary>
        /// 初始化啪啦系统（重载一::不定义系统和数据库的关系）
        /// </summary>
        /// <parmm name="ID">用户ID</parmm>
        public PLSYS(int ID)
        {
            this.ID = ID;
        }
        /// <summary>
        /// 初始化啪啦系统（重载二::定义系统和数据库的关系）
        /// </summary>
        /// <parmm name="ID">用户ID</parmm>
        /// <parmm name="PLDB">啪啦数据库信息</parmm>
        public PLSYS(int ID, PLDB PLDB)
        {
            this.ID = ID;

            DataBase = PLDB.DataBase;
            SysTables = PLDB.Tables;
            SysViews = PLDB.Views;
            MySqlConnH = PLDB.MySqlConnH;
        }

        /// <summary>
        /// 获得当前操作噼里啪啦系统的用户数据
        /// </summary>
        /// <returns>错误则返回Name为ERROR的User实例</returns>
        public User GetUser()
        {
            string SQL = string.Format("CALL {0}.`get>user`( ?ID )", DataBase);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {

                    new MySqlParm() { Name = "?ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                DataRow result = MySqlConnH.GetRow(MySqlCommand);/* 得到一行数据，使用GetRow方法 */

                return new User
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
            public static string UserTable = "pl_user";
            public static string IndexTable = "pl_index";
            public static string PrimaryTable = "pl_primary";
        }
        /// <summary>
        /// 内置默认PL视图
        /// </summary>
        private struct InnerDefViews
        {
            public static string IndexView = "view>index";
            public static string PrimaryView = "view>primary";
        }

        /// <summary>
        /// 默认PL表访问器
        /// </summary>
        public static PLTables DefTables
        {
            get { return new PLTables(InnerDefTables.UserTable, InnerDefTables.IndexTable, InnerDefTables.PrimaryTable); }
        }
        /// <summary>
        /// 默认PL视图访问器
        /// </summary>
        public static PLViews DefViews
        {
            get { return new PLViews(InnerDefViews.IndexView, InnerDefViews.PrimaryView); }
        }


        /// <summary>
        /// 以默认值定义表名
        /// </summary>
        public void DefaultSysTables()
        {
            PLTables T = new PLTables
            {
                UserTable = InnerDefTables.UserTable,
                IndexTable = InnerDefTables.IndexTable,
                PrimaryTable = InnerDefTables.PrimaryTable
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
                IndexView = InnerDefViews.IndexView,
                PrimaryView = InnerDefViews.PrimaryView
            };

            SysViews = V;
        }

        /// <summary>
        /// 初始化数据库连接
        /// </summary>
        /// <param name="MySqlConn">连接信息</param>
        public void DBCINIT(MySqlConn MySqlConn)
        {
            MySqlConnH.Start(MySqlConn);
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
        public string DataBase { get; }
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
        public MySqlConnH MySqlConnH { get; set; }

        /// <summary>
        /// 初始化啪啦数据读取器
        /// </summary>
        /// <parmm name="PLDB">啪啦数据库信息</parmm>
        public PLDR(PLSYS PLSYS)
        {
            DataBase = PLSYS.DataBase;
            Tables = PLSYS.SysTables;
            Views = PLSYS.SysViews;
            MySqlConnH = PLSYS.MySqlConnH;
        }


        /// <summary>
        /// 获得全部文章ID列表
        /// </summary>
        /// <returns></returns>
        public virtual List<int> GetIDList()
        {
            List<int> IDList = new List<int>();

            string SQL = string.Format("SELECT ID FROM {0}.`{1}`", DataBase, Views.IndexView);

            foreach (object ID in MySqlConnH.GetColumn(SQL))
            {
                IDList.Add(Convert.ToInt32(ID));
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
            string SQL = string.Format("SELECT ID FROM {0}.`{1}` ORDER BY Time DESC LIMIT ?Start , ?Length", DataBase, Views.IndexView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?Start", Val = Start },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
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
            string SQL = string.Format("SELECT ID FROM {0}.`{1}` WHERE Type = ?Type ORDER BY Time DESC LIMIT ?Start , ?Length", DataBase, Views.IndexView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?Type", Val = Type },
                    new MySqlParm() { Name = "?Start", Val = Start },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
                }
            }
            return IDList;
        }

        /// <summary>
        /// 取得全部文章ID列表
        /// </summary>
        /// <param name="Key">用于排序的键名</param>
        /// <param name="OrderType">asc(升序)或desc(降序)，不区分大小写</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(string Key, string OrderType)
        {
            List<int> IDList = new List<int>();

            string SQL = string.Format("SELECT ID FROM {0}.`{1}` ORDER BY ?Key ?OrderType", DataBase, Views.IndexView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
                }
            }
            return IDList;
        }
        /// <summary>
        /// 取得指定长度的文章ID列表
        /// </summary>
        /// <param name="Key">用于排序的键名</param>
        /// <param name="OrderType">asc(升序)或desc(降序)，不区分大小写</param>
        /// <param name="Length">截止长度</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(string Key, string OrderType, int Length)
        {
            List<int> IDList = new List<int>();

            string SQL = string.Format("SELECT ID FROM {0}.`{1}` ORDER BY ?Key ?OrderType LIMIT 0,?Length", DataBase, Views.IndexView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
                }
            }
            return IDList;
        }

        #region  GetIDList的剩余重载（按照文章类型、归档、标签查询）

        /// <summary>
        /// 获得指定类型的文章ID列表
        /// </summary>
        /// <param name="Type">类型集合</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Type> Type)
        {
            List<int> IDList = new List<int>();

            string str = "";
            foreach (Type a in Type)
            {
                str += a.Val + "|";
            }
            str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

            string SQL = string.Format("SELECT ID FROM {0}.`{1}` WHERE Type REGEXP ?str", DataBase, Views.IndexView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?str", Val = str }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
                }
            }
            return IDList;
        }
        /// <summary>
        /// 获得指定归档的文章ID列表
        /// </summary>
        /// <param name="Archiv">归档集合</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Archiv> Archiv)
        {
            List<int> IDList = new List<int>();

            string str = "";
            foreach (Archiv a in Archiv)
            {
                str += a.Val + "|";
            }
            str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

            string SQL = string.Format("SELECT ID FROM {0}.`{1}` WHERE Archiv REGEXP ?str", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?str", Val = str }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
                }
            }
            return IDList;
        }
        /// <summary>
        /// 获得指定标签的文章ID列表
        /// </summary>
        /// <param name="Label">标签集合</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Label> Label)
        {
            List<int> IDList = new List<int>();

            string str = "";
            foreach (Label a in Label)
            {
                str += a.Val + "|";
            }
            str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

            string SQL = string.Format("SELECT ID FROM {0}.`{1}` WHERE Label REGEXP ?str", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?str", Val = str }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
                }
            }
            return IDList;
        }

        /// <summary>
        /// 获得指定类型的文章ID列表(排序指定)
        /// </summary>
        /// <param name="Type">类型集合</param>
        /// <param name="Key">被排序键</param>
        /// <param name="OrderType">排序类型</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Type> Type, string Key, string OrderType)
        {
            List<int> IDList = new List<int>();

            string str = "";
            foreach (Type a in Type)
            {
                str += a.Val + "|";
            }
            str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

            string SQL = string.Format("SELECT ID FROM {0}.`{1}` WHERE Type REGEXP ?str ORDER BY ?Key ?OrderType", DataBase, Views.IndexView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?str", Val = str },
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
                }
            }
            return IDList;
        }
        /// <summary>
        /// 获得指定归档的文章ID列表(排序指定)
        /// </summary>
        /// <param name="Archiv">归档集合</param>
        /// <param name="Key">被排序键</param>
        /// <param name="OrderType">排序类型</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Archiv> Archiv, string Key, string OrderType)
        {
            List<int> IDList = new List<int>();

            string str = "";
            foreach (Archiv a in Archiv)
            {
                str += a.Val + "|";
            }
            str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

            string SQL = string.Format("SELECT ID FROM {0}.`{1}` WHERE Archiv REGEXP ?str ORDER BY ?Key ?OrderType", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?str", Val = str },
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
                }
            }
            return IDList;
        }
        /// <summary>
        /// 获得指定标签的文章ID列表(排序指定)
        /// </summary>
        /// <param name="Label">标签集合</param>
        /// <param name="Key">被排序键</param>
        /// <param name="OrderType">排序类型</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Label> Label, string Key, string OrderType)
        {
            List<int> IDList = new List<int>();

            string str = "";
            foreach (Label a in Label)
            {
                str += a.Val + "|";
            }
            str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

            string SQL = string.Format("SELECT ID FROM {0},`{1}` WHERE Label REGEXP ?str ORDER BY ?Key ?OrderType", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?str", Val = str },
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
                }
            }
            return IDList;
        }

        /// <summary>
        /// 获得指定类型的文章ID列表(排序指定,长度限制)
        /// </summary>
        /// <param name="Type">类型集合</param>
        /// <param name="Key">被排序键</param>
        /// <param name="OrderType">排序类型</param>
        /// <param name="Length">取用长度</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Type> Type, string Key, string OrderType, int Length)
        {
            List<int> IDList = new List<int>();

            string str = "";
            foreach (Type a in Type)
            {
                str += a.Val + "|";
            }
            str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

            string SQL = string.Format("SELECT ID FROM {0}.`{1}` WHERE Type REGEXP ?str ORDER BY ?Key ?OrderType LIMIT 0,?Length", DataBase, Views.IndexView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?str", Val = str },
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
                }
            }
            return IDList;
        }
        /// <summary>
        /// 获得指定归档的文章ID列表(排序指定,长度限制)
        /// </summary>
        /// <param name="Archiv">归档集合</param>
        /// <param name="Key">被排序键</param>
        /// <param name="OrderType">排序类型</param>
        /// <param name="Length">取用长度</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Archiv> Archiv, string Key, string OrderType, int Length)
        {
            List<int> IDList = new List<int>();

            string str = "";
            foreach (Archiv a in Archiv)
            {
                str += a.Val + "|";
            }
            str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

            string SQL = string.Format("SELECT ID FROM {0}.`{1}` WHERE Archiv REGEXP ?str ORDER BY ?Key ?OrderType LIMIT 0,?Length", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?str", Val = str },
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
                }
            }
            return IDList;
        }
        /// <summary>
        /// 获得指定标签的文章ID列表(排序指定,长度限制)
        /// </summary>
        /// <param name="Label">标签集合</param>
        /// <param name="Key">被排序键</param>
        /// <param name="OrderType">排序类型</param>
        /// <param name="Length">取用长度</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Label> Label, string Key, string OrderType, int Length)
        {
            List<int> IDList = new List<int>();

            string str = "";
            foreach (Label a in Label)
            {
                str += a.Val + "|";
            }
            str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

            string SQL = string.Format("SELECT ID FROM {0}.`{1}` WHERE Label REGEXP ?str ORDER BY ?Key ?OrderType LIMIT 0,?Length", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?str", Val = str },
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
                }
            }
            return IDList;
        }

        #endregion


        /// <summary>
        /// 通用文章匹配器
        /// </summary>
        /// <typeparmm name="T">继承自IKey的类型</typeparmm>
        /// <parmm name="OBJ">继承自IKey的对象实例</parmm>
        /// <returns></returns>
        public virtual List<int> MatchPost<T>(T OBJ) where T : IKey
        {
            List<int> IDList = new List<int>();

            string SQL = string.Format("SELECT ID FROM {0}.`{1}` WHERE ?Key REGEXP ?Val", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?Key", Val = OBJ.GetType().ToString() },
                    new MySqlParm() { Name = "?Val", Val = OBJ.Val.ToString() }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
                }
            }
            return IDList;
        }

        /// <summary>
        /// 文章标题(Title)匹配器
        /// </summary>
        /// <param name="Val">匹配文章</param>
        /// <returns>返回符合匹配文章的ID集合</returns>
        public virtual List<int> MatchTitle(string Val)
        {
            List<int> IDList = new List<int>();

            string SQL = string.Format("CALL {0}.`match<title`( ?Val )", DataBase);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?Val", Val = Val }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
                }
            }
            return IDList;
        }
        /// <summary>
        /// 文章概要(Summary)匹配器
        /// </summary>
        /// <param name="Val">匹配文章</param>
        /// <returns>返回符合匹配文章的ID集合</returns>
        public virtual List<int> MatchSummary(string Val)
        {
            List<int> IDList = new List<int>();

            string SQL = string.Format("CALL {0}.`match<summary`( ?Val )", DataBase);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?Val", Val = Val }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
                }
            }
            return IDList;
        }
        /// <summary>
        /// 文章内容(Content)匹配器
        /// </summary>
        /// <param name="Val">匹配文章</param>
        /// <returns>返回符合匹配文章的ID集合</returns>
        public virtual List<int> MatchContent(string Val)
        {
            List<int> IDList = new List<int>();

            string SQL = string.Format("CALL {0}.`match<content`( ?Val )", DataBase);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?Val", Val = Val }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                foreach (object ID in MySqlConnH.GetColumn(MySqlCommand))
                {
                    IDList.Add(Convert.ToInt32(ID));
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
            string SQL = string.Format("CALL {0}.`get>index`( ?ID )", DataBase);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                DataRow result = MySqlConnH.GetRow(MySqlCommand);

                return new Post
                {
                    ID = Convert.ToInt32(result["ID"]),
                    GUID = Convert.ToString(result["GUID"]),
                    CT = Convert.ToDateTime(result["Time"]),
                    Mode = Convert.ToString(result["Mode"]),
                    Type = Convert.ToString(result["Type"]),
                    User = Convert.ToString(result["User"]),
                    UVCount = Convert.ToInt32(result["UVCount"]),
                    StarCount = Convert.ToInt32(result["StarCount"])
                };
            }
        }
        /// <summary>
        /// 取得Index表的一个键值
        /// </summary>
        /// <param name="ID">文章序列号</param>
        /// <param name="Key">键名</param>
        /// <returns></returns>
        public virtual string GetIndex(int ID, string Key)
        {
            string SQL = string.Format("SELECT ?Key FROM {0}.`{1}` WHERE ID = ?ID", DataBase, Views.IndexView);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlConnH.GetRow(MySqlCommand)[0].ToString();
            }
        }

        /// <summary>
        /// 通过ID获取Primary表中的目标文章数据
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public virtual Post GetPrimary(int ID)
        {
            string SQL = string.Format("CALL {0}.`get>primary`( ?ID )", DataBase);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                DataRow result = MySqlConnH.GetRow(MySqlCommand);

                return new Post
                {
                    ID = Convert.ToInt32(result["ID"]),
                    GUID = Convert.ToString(result["GUID"]),
                    LCT = Convert.ToDateTime(result["Time"]),
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
        /// 取得Primary表的一个键值（object）
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <param name="Key">键名</param>
        /// <returns></returns>
        public virtual object GetPrimary(int ID, string Key)
        {
            string SQL = string.Format("SELECT ?Key FROM {0}.`{1}` WHERE ID = ?ID", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlConnH.GetRow(MySqlCommand)[0];
            }
        }
        /// <summary>
        /// 取得文章标题（Title）
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public virtual string GetTitle(int ID)
        {
            string SQL = string.Format("SELECT Title FROM {0}.`{1}` WHERE ID = ?ID", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlConnH.GetRow(MySqlCommand)[0].ToString();
            }
        }
        /// <summary>
        /// 取得文章概要(Summary)
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public virtual string GetSummary(int ID)
        {
            string SQL = string.Format("SELECT Summary FROM {0}.`{1}` WHERE ID = ?ID", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlConnH.GetRow(MySqlCommand)[0].ToString();
            }
        }
        /// <summary>
        /// 取得文章正文(Content)
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public virtual string GetContent(int ID)
        {
            string SQL = string.Format("SELECT Content FROM {0}.`{1}` WHERE ID = ?ID", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlConnH.GetRow(MySqlCommand)[0].ToString();
            }
        }

        /// <summary>
        /// 取得一定长度的文章标题(Title)
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <param name="Length">取得的文章内容长度</param>
        /// <returns></returns>
        public virtual string GetTitle(int ID, int Length)
        {
            string SQL = string.Format("SELECT SUBSTRING(( SELECT Title FROM {0}.`{1}` WHERE ID = ?ID ) FROM 1 FOR ?Length)", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlConnH.GetRow(MySqlCommand)[0].ToString();
            }
        }
        /// <summary>
        /// 按长度取得文章概要（Summary）
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <param name="Length">取得的文章内容长度</param>
        /// <returns></returns>
        public virtual string GetSummary(int ID, int Length)
        {
            string SQL = string.Format("SELECT SUBSTRING(( SELECT Summary FROM {0}.`{1}` WHERE ID = ?ID ) FROM 1 FOR ?Length)", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlConnH.GetRow(MySqlCommand)[0].ToString();
            }
        }
        /// <summary>
        /// 按长度取得文章正文(Content)
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <param name="Length">取得的文章内容长度</param>
        /// <returns></returns>
        public virtual string GetContent(int ID, int Length)
        {
            string SQL = string.Format("SELECT SUBSTRING(( SELECT Content FROM {0}.`{1}` WHERE ID = ?ID ) FROM 1 FOR ?Length)", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                return MySqlConnH.GetRow(MySqlCommand)[0].ToString();
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
            string SQL = string.Format("SELECT ID FROM {0}.`{1}` " +
                "WHERE ID =( SELECT min(ID) FROM {0}.`{1}` WHERE ID > ?ID );", DataBase, Views.IndexView);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                DataRow DataRow = MySqlConnH.GetRow(MySqlCommand);
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
        /// <param name="REGEXP">正则表达式</param>
        /// <param name="ID">目标文章ID</param>
        /// <returns>不存在符合要求的ID时，返回-1</returns>
        public virtual int NextID(string REGEXP, int ID)
        {
            string SQL = string.Format("SELECT ID FROM {0}.`{1}` " +
                "WHERE ID =( SELECT min(ID) FROM {0}.`{1}` WHERE ID > ?ID AND Archiv REGEXP ?REGEXP);", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?REGEXP", Val = REGEXP }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                DataRow DataRow = MySqlConnH.GetRow(MySqlCommand);
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
        /// 取得目标文章ID 的下一个文章ID(按指定键在Index表升序查找)
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <param name="Key">指定键</param>
        /// <returns>不存在符合要求的ID时，返回-1</returns>
        public virtual int NextID(int ID, string Key)
        {
            /* 取得比当前 ID 大的一行，实现对下一条数据的抓取 */
            string SQL = string.Format("SELECT ID FROM {0}.`{1}` WHERE " +
                         "?Key =( SELECT min(?Key) FROM {0}.`{1}` WHERE " +
                         "?Key >( SELECT ?Key FROM {0}.`{1}` WHERE ID = ?ID ))", DataBase, Views.IndexView);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?Key", Val = Key }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                DataRow DataRow = MySqlConnH.GetRow(MySqlCommand);
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
            string SQL = string.Format("SELECT ID FROM {0}.`{1}` WHERE " +
                         "ID =( SELECT max(ID) FROM {0}.`{1}` WHERE ID < ?ID );", DataBase, Views.IndexView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                DataRow DataRow = MySqlConnH.GetRow(MySqlCommand);
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
        /// <param name="REGEXP">正则表达式</param>
        /// <param name="ID">目标文章ID</param>
        /// <returns>不存在符合要求的ID时，返回-1</returns>
        public virtual int PrevID(string REGEXP, int ID)
        {
            string SQL = string.Format("SELECT ID FROM {0}.`{1}` WHERE " +
                         "ID =( SELECT max(ID) FROM {0}.`{1}` WHERE ID < ?ID AND Archiv REGEXP ?REGEXP);", DataBase, Views.PrimaryView);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?REGEXP", Val = REGEXP }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                DataRow DataRow = MySqlConnH.GetRow(MySqlCommand);
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
        /// 取得指定目标文章ID 的上一个文章ID(按指定键在Index表升序查找)
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <param name="Key">指定键</param>
        /// <returns>不存在符合要求的ID时，返回-1</returns>
        public virtual int PrevID(int ID, string Key)
        {
            string SQL = string.Format("SELECT ID FROM {0}.`{1}` WHERE " +
                         "?Key =( SELECT max(?Key) FROM {0}.`{1}` WHERE " +
                         "?Key <( SELECT ?Key FROM {0}.`{1}` WHERE ID = ?ID ))", DataBase, Views.IndexView);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?Key", Val = Key }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                DataRow DataRow = MySqlConnH.GetRow(MySqlCommand);
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
        public string DataBase { get; }
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
        public MySqlConnH MySqlConnH { get; set; }

        /// <summary>
        /// 得到最大文章ID（私有）
        /// </summary>
        /// <returns>错误则返回-1</returns>
        private int GetMaxID()
        {
            try
            {
                string SQL = string.Format("SELECT max(ID) FROM {0}.`{1}`", DataBase, Tables.IndexTable);

                return Convert.ToInt32(MySqlConnH.GetRow(SQL)[0]);
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
            DataBase = PLSYS.DataBase;
            Tables = PLSYS.SysTables;
            Views = PLSYS.SysViews;
            MySqlConnH = PLSYS.MySqlConnH;
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
                "INSERT INTO {0}.`{1}`" +
                " (`ID`, `GUID`, `Time`, `Mode`, `Type`, `User`, `UVCount`, `StarCount`) VALUES" +
                " ('?ID','?GUID', '?CT','?Mode','?Type','?User','?UVCount','?StarCount');"
                +
                "INSERT INTO {0}.`{2}`" +
                " (`ID`, `GUID`, `Time`, `Title`, `Summary`, `Content`, `Archiv`, `Label`, `Cover`) VALUES" +
                " ('?ID','?GUID','?LCT','?Title','?Summary','?Content','?Archiv','?Label','?Cover');"

                , DataBase, Tables.IndexTable, Tables.PrimaryTable);


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
            if (MySqlConnH.ParmQueryCMD(SQL, ParmList).ExecuteNonQuery() == 2)
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

            string SQL = string.Format("UPDATE {0}.`{1}` SET" +
                " GUID='?GUID', Mode='?Mode', Type='?Type', UVCount='?UVCount', StarCount='?StarCount' WHERE ID = ?ID;"
                +
                "INSERT INTO {0}.`{2}`" +
                " (`ID`, `GUID`, `Time`, `Title`, `Summary`, `Content`, `Archiv`, `Label`, `Cover`) VALUES" +
                " ('?ID','?GUID','?LCT','?Title','?Summary','?Content','?Archiv','?Label','?Cover');"

                , DataBase, Tables.IndexTable, Tables.PrimaryTable);

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
            if (MySqlConnH.ParmQueryCMD(SQL, ParmList).ExecuteNonQuery() == 2)
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
            string SQL = string.Format("DELETE FROM {0}.`{1}` WHERE ID = ?ID; DELETE FROM {0}.`{2}` WHERE ID = ?ID;", DataBase, Tables.IndexTable, Tables.PrimaryTable);

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "?ID", Val = ID }
            };

            if (MySqlConnH.ParmQueryCMD(SQL, ParmList).ExecuteNonQuery() >= 2)
                return true;
            else
                throw new Exception("多行操作异常");
        }
        /// <summary>
        /// 删除副本（仅单个副本）
        /// </summary>
        /// <param name="GUID">目标文章的GUID</param>
        /// <returns></returns>
        public bool DeletePost(int GUID)
        {
            string SQL = string.Format("DELETE FROM {0}.`{1}` WHERE GUID = ?GUID;", DataBase, Tables.PrimaryTable);

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "?GUID", Val = GUID }
            };

            if (MySqlConnH.ParmQueryCMD(SQL, ParmList).ExecuteNonQuery() >= 1)
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
                DataBase = DataBase,
                Table = Tables.IndexTable,
                Name = "ID",
                Val = ID.ToString()
            };
            return MySqlConnH.UpdateKey(MySqlKey, "Mode", "show");
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
                DataBase = DataBase,
                Table = Tables.IndexTable,
                Name = "ID",
                Val = ID.ToString()
            };
            return MySqlConnH.UpdateKey(MySqlKey, "Mode", "closed");
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
                DataBase = DataBase,
                Table = Tables.IndexTable,
                Name = "ID",
                Val = ID.ToString()
            };
            return MySqlConnH.UpdateKey(MySqlKey, "Mode", "scheduled");
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
                DataBase = DataBase,
                Table = Tables.IndexTable,
                Name = "ID",
                Val = ID.ToString()
            };
            return MySqlConnH.UpdateKey(MySqlKey, "Mode", "archived");
        }


        /// <summary>
        /// 通用文章键更改器
        /// </summary>
        /// <typeparam name="T">继承自IKey的类型</typeparam>
        /// <param name="OBJ">继承自IKey的对象实例</param>
        /// <returns></returns>
        public bool UpdateKey<T>(T OBJ) where T : IKey
        {
            //初始化键定位
            MySqlKey MySqlKey = new MySqlKey
            {
                DataBase = DataBase,
                Table = Tables.PrimaryTable,
                Name = "ID",
                Val = OBJ.ID.ToString()
            };
            return MySqlConnH.UpdateKey(MySqlKey, OBJ.GetType().Name, OBJ.Val.ToString());
        }
        /// <summary>
        /// 更改文章标题(Title)
        /// </summary>
        /// <param name="ID">文章序列号</param>
        /// <param name="Val">新标题</param>
        /// <returns></returns>
        public bool UpdateTitle(int ID, string Val)
        {
            //初始化键定位
            MySqlKey MySqlKey = new MySqlKey
            {
                DataBase = DataBase,
                Table = Tables.PrimaryTable,
                Name = "ID",
                Val = ID.ToString()
            };
            return MySqlConnH.UpdateKey(MySqlKey, "Title", Val);
        }
        /// <summary>
        /// 更改文章概要(Summary)
        /// </summary>
        /// <param name="ID">文章序列号</param>
        /// <param name="Val">新概要</param>
        /// <returns></returns>
        public bool UpdateSummary(int ID, string Val)
        {
            MySqlKey MySqlKey = new MySqlKey
            {
                DataBase = DataBase,
                Table = Tables.PrimaryTable,
                Name = "ID",
                Val = ID.ToString()
            };
            return MySqlConnH.UpdateKey(MySqlKey, "Summary", Val);
        }
        /// <summary>
        /// 更改文章内容(Content)
        /// </summary>
        /// <param name="ID">文章序列号</param>
        /// <param name="Val">新内容</param>
        /// <returns></returns>
        public bool UpdateContent(int ID, string Val)
        {
            MySqlKey MySqlKey = new MySqlKey
            {
                DataBase = DataBase,
                Table = Tables.PrimaryTable,
                Name = "ID",
                Val = ID.ToString()
            };
            return MySqlConnH.UpdateKey(MySqlKey, "Content", Val);
        }

        /// <summary>
        /// 设置浏览计数(UVCount)
        /// </summary>
        /// <parmm name="ID">文章ID</parmm>
        /// <parmm name="Value">值</parmm>
        /// <returns></returns>
        public bool UpdateUVCount(int ID, int Val)
        {
            //初始化键定位
            MySqlKey MySqlKey = new MySqlKey
            {
                DataBase = DataBase,
                Table = Tables.IndexTable,
                Name = "ID",
                Val = ID.ToString()
            };
            return MySqlConnH.UpdateKey(MySqlKey, "UVCount", Val.ToString());
        }
        /// <summary>
        /// 设置星星计数(StarCount)
        /// </summary>
        /// <parmm name="ID">文章ID</parmm>
        /// <parmm name="Value">值</parmm>
        /// <returns></returns>
        public bool UpdateStarCount(int ID, int Val)
        {
            MySqlKey MySqlKey = new MySqlKey
            {
                DataBase = DataBase,
                Table = Tables.IndexTable,
                Name = "ID",
                Val = ID.ToString()
            };
            return MySqlConnH.UpdateKey(MySqlKey, "StarCount", Val.ToString());
        }


        /// <summary>
        /// 检测ID、GUID是否匹配，之后合并Post数据表
        /// </summary>
        /// <parmm name="IndexTable">索引表Post实例</parmm>
        /// <parmm name="PrimaryTable">主表Post实例</parmm>
        /// <returns></returns>
        public static Post Join(Post IndexTable, Post PrimaryTable)
        {
            if (IndexTable.ID == PrimaryTable.ID && IndexTable.GUID == PrimaryTable.GUID)
            {
                return new Post
                {
                    ID = IndexTable.ID,
                    GUID = IndexTable.GUID,

                    Mode = IndexTable.Mode,
                    Type = IndexTable.Type,

                    Title = PrimaryTable.Title,
                    Summary = PrimaryTable.Summary,
                    Content = PrimaryTable.Content,

                    User = IndexTable.User,
                    Archiv = PrimaryTable.Archiv,
                    Label = PrimaryTable.Label,

                    CT = IndexTable.CT,
                    LCT = PrimaryTable.LCT,

                    UVCount = IndexTable.UVCount,
                    StarCount = IndexTable.StarCount,

                    Cover = PrimaryTable.Cover
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
        /// <parmm name="IndexTable">索引表Post实例</parmm>
        /// <parmm name="PrimaryTable">主表Post实例</parmm>
        /// <returns>始终返回以IndexTable的ID、GUID为最终合并结果的Post实例</returns>
        public static Post ForcedJoin(Post IndexTable, Post PrimaryTable)
        {
            return new Post
            {
                ID = IndexTable.ID,
                GUID = IndexTable.GUID,

                Mode = IndexTable.Mode,
                Type = IndexTable.Type,

                Title = PrimaryTable.Title,
                Summary = PrimaryTable.Summary,
                Content = PrimaryTable.Content,

                User = IndexTable.User,
                Archiv = PrimaryTable.Archiv,
                Label = PrimaryTable.Label,

                CT = IndexTable.CT,
                LCT = PrimaryTable.LCT,

                UVCount = IndexTable.UVCount,
                StarCount = IndexTable.StarCount,

                Cover = PrimaryTable.Cover
            };
        }
    }
}