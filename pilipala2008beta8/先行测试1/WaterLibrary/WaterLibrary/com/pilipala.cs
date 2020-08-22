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
using WaterLibrary.stru.pilipala.Core;

using System.Text.RegularExpressions;

using Type = WaterLibrary.stru.pilipala.Type;
using Label = WaterLibrary.stru.pilipala.Label;

using System.Dynamic;
using WaterLibrary.com.basic;
using System.Web.UI.WebControls;

namespace WaterLibrary.com.pilipala
{


    /// <summary>
    /// 啪啦系统类
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
            try
            {
                string SQL = "CALL ?DataBase.`get>user`( ?ID )";

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        return new User
                        {
                            ID = Convert.ToInt32(Row["ID"]),
                            GUID = Convert.ToString(Row["GUID"]),
                            Name = Convert.ToString(Row["Name"]),
                            Note = Convert.ToString(Row["Note"])
                        };
                    }
                }
                return new User { Name = "ERROR" };
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
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
        /// 初始化TextListH
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
        /// 获得全部文本ID列表
        /// </summary>
        /// <returns></returns>
        public virtual List<int> GetIDList()
        {
            try
            {
                List<int> IDList = new List<int>();

                string SQL = "SELECT ID FROM ?DataBase.`?IndexView`";

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?IndexView", Val =Views.IndexView }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));/* 每次循环完成，将获取到的文本数据添加至文本列表 */
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }



        /// <summary>
        /// 取得文本ID列表（步进式）
        /// </summary>
        /// <parmm name="Start">步进起始行（包含该行）</parmm>
        /// <parmm name="Length">加载行数</parmm>
        /// <returns></returns>
        public virtual List<int> GetIDList(int Start, int Length)
        {
            try
            {
                List<int> IDList = new List<int>();

                /* 此处以时间降序排列查询 */
                string SQL = "SELECT ID FROM ?DataBase.`?IndexView` ORDER BY Time DESC LIMIT ?Start , ?Length";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?IndexView", Val = Views.IndexView },

                    new MySqlParm() { Name = "?Start", Val = Start },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));/* 每次循环完成，将获取到的文本数据添加至文本列表 */
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 取得指定类型的文本ID列表（步进式）
        /// </summary>
        /// <parmm name="Start">步进起始行（包含该行）</parmm>
        /// <parmm name="Length">加载行数</parmm>
        /// <parmm name="Type">自定义文本类型</parmm>
        /// <returns></returns>
        public virtual List<int> GetIDList(int Start, int Length, string Type)
        {
            try
            {
                List<int> IDList = new List<int>();

                /* 此处以时间降序排列查询 */
                string SQL = "SELECT ID FROM ?DataBase.`?IndexView` WHERE Type = ?Type ORDER BY Time DESC LIMIT ?Start , ?Length";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?IndexView", Val = Views.IndexView },

                    new MySqlParm() { Name = "?Type", Val = Type },
                    new MySqlParm() { Name = "?Start", Val = Start },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }

        /// <summary>
        /// 取得全部文章ID列表
        /// </summary>
        /// <param name="Key">用于排序的键名</param>
        /// <param name="OrderType">asc(升序)或desc(降序)，不区分大小写</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(string Key, string OrderType)
        {
            try
            {
                List<int> IDList = new List<int>();

                string SQL = "SELECT ID FROM ?DataBase.`?IndexView` ORDER BY ?Key ?OrderType";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?IndexView", Val = Views.IndexView },

                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
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
            try
            {
                List<int> IDList = new List<int>();

                string SQL = "SELECT ID FROM ?DataBase.`?IndexView` ORDER BY ?Key ?OrderType LIMIT 0,?Length";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?IndexView", Val = Views.IndexView },

                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }


        #region  GetIDList的剩余重载（按照文章类型、归档、标签查询）

        /// <summary>
        /// 获得指定类型的文本ID列表
        /// </summary>
        /// <param name="Type">类型集合</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Type> Type)
        {
            try
            {
                List<int> IDList = new List<int>();

                string str = "";
                foreach (Type a in Type)
                {
                    str += a.Val + "|";
                }
                str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                string SQL = "SELECT ID FROM ?DataBase.`?IndexView` WHERE Type REGEXP ?str";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?IndexView", Val = Views.IndexView },

                    new MySqlParm() { Name = "?str", Val = str }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 获得指定归档的文本ID列表
        /// </summary>
        /// <param name="Archiv">归档集合</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Archiv> Archiv)
        {
            try
            {
                List<int> IDList = new List<int>();

                string str = "";
                foreach (Archiv a in Archiv)
                {
                    str += a.Val + "|";
                }
                str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                string SQL = "SELECT ID FROM ?DataBase.`?PrimaryView` WHERE Archiv REGEXP ?str";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase},
                    new MySqlParm() { Name = "?PrimaryView", Val = Views.PrimaryView },

                    new MySqlParm() { Name = "?str", Val = str }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 获得指定标签的文本ID列表
        /// </summary>
        /// <param name="Label">标签集合</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Label> Label)
        {
            try
            {
                List<int> IDList = new List<int>();

                string str = "";
                foreach (Label a in Label)
                {
                    str += a.Val + "|";
                }
                str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                string SQL = "SELECT ID FROM ?DataBase.`?PrimaryView` WHERE Label REGEXP ?str";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?PrimaryView", Val = Views.PrimaryView },

                    new MySqlParm() { Name = "?str", Val = str }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }

        /// <summary>
        /// 获得指定类型的文本ID列表(排序指定)
        /// </summary>
        /// <param name="Type">类型集合</param>
        /// <param name="Key">被排序键</param>
        /// <param name="OrderType">排序类型</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Type> Type, string Key, string OrderType)
        {
            try
            {
                List<int> IDList = new List<int>();

                string str = "";
                foreach (Type a in Type)
                {
                    str += a.Val + "|";
                }
                str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                string SQL = "SELECT ID FROM ?DataBase.`?IndexView` WHERE Type REGEXP ?str ORDER BY ?Key ?OrderType";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?IndexView", Val = Views.IndexView },

                    new MySqlParm() { Name = "?str", Val = str },
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 获得指定归档的文本ID列表(排序指定)
        /// </summary>
        /// <param name="Archiv">归档集合</param>
        /// <param name="Key">被排序键</param>
        /// <param name="OrderType">排序类型</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Archiv> Archiv, string Key, string OrderType)
        {
            try
            {
                List<int> IDList = new List<int>();

                string str = "";
                foreach (Archiv a in Archiv)
                {
                    str += a.Val + "|";
                }
                str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                string SQL = "SELECT ID FROM ?DataBase.`?PrimaryView` WHERE Archiv REGEXP ?str ORDER BY ?Key ?OrderType";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?PrimaryView", Val = Views.PrimaryView },

                    new MySqlParm() { Name = "?str", Val = str },
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 获得指定标签的文本ID列表(排序指定)
        /// </summary>
        /// <param name="Label">标签集合</param>
        /// <param name="Key">被排序键</param>
        /// <param name="OrderType">排序类型</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Label> Label, string Key, string OrderType)
        {
            try
            {
                List<int> IDList = new List<int>();

                string str = "";
                foreach (Label a in Label)
                {
                    str += a.Val + "|";
                }
                str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                string SQL = "SELECT ID FROM ?DataBase.`?PrimaryView` WHERE Label REGEXP ?str ORDER BY ?Key ?OrderType";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?PrimaryView", Val = Views.PrimaryView },

                    new MySqlParm() { Name = "?str", Val = str },
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }

        /// <summary>
        /// 获得指定类型的文本ID列表(排序指定,长度限制)
        /// </summary>
        /// <param name="Type">类型集合</param>
        /// <param name="Key">被排序键</param>
        /// <param name="OrderType">排序类型</param>
        /// <param name="Length">取用长度</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Type> Type, string Key, string OrderType, int Length)
        {
            try
            {
                List<int> IDList = new List<int>();

                string str = "";
                foreach (Type a in Type)
                {
                    str += a.Val + "|";
                }
                str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                string SQL = "SELECT ID FROM ?DataBase.`?IndexView` WHERE Type REGEXP ?str ORDER BY ?Key ?OrderType LIMIT 0,?Length";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?IndexView", Val = Views.IndexView },

                    new MySqlParm() { Name = "?str", Val = str },
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 获得指定归档的文本ID列表(排序指定,长度限制)
        /// </summary>
        /// <param name="Archiv">归档集合</param>
        /// <param name="Key">被排序键</param>
        /// <param name="OrderType">排序类型</param>
        /// <param name="Length">取用长度</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Archiv> Archiv, string Key, string OrderType, int Length)
        {
            try
            {
                List<int> IDList = new List<int>();

                string str = "";
                foreach (Archiv a in Archiv)
                {
                    str += a.Val + "|";
                }
                str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                string SQL = "SELECT ID FROM ?DataBase.`?PrimaryView` WHERE Archiv REGEXP ?str ORDER BY ?Key ?OrderType LIMIT 0,?Length";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?PrimaryView", Val = Views.PrimaryView },

                    new MySqlParm() { Name = "?str", Val = str },
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 获得指定标签的文本ID列表(排序指定,长度限制)
        /// </summary>
        /// <param name="Label">标签集合</param>
        /// <param name="Key">被排序键</param>
        /// <param name="OrderType">排序类型</param>
        /// <param name="Length">取用长度</param>
        /// <returns></returns>
        public virtual List<int> GetIDList(List<Label> Label, string Key, string OrderType, int Length)
        {
            try
            {
                List<int> IDList = new List<int>();

                string str = "";
                foreach (Label a in Label)
                {
                    str += a.Val + "|";
                }
                str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                string SQL = "SELECT ID FROM ?DataBase.`?PrimaryView` WHERE Label REGEXP ?str ORDER BY ?Key ?OrderType LIMIT 0,?Length";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?PrimaryView", Val = Views.PrimaryView },

                    new MySqlParm() { Name = "?str", Val = str },
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?OrderType", Val = OrderType },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
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
            try
            {
                List<int> IDList = new List<int>();

                string SQL = "SELECT ID FROM ?DataBase.`?PrimaryView` WHERE ?Key REGEXP ?Val";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase",Val=DataBase },
                    new MySqlParm() { Name = "?PrimaryView", Val = Views.PrimaryView },

                    new MySqlParm() { Name = "?Key", Val = OBJ.GetType().ToString() },
                    new MySqlParm() { Name = "?Val", Val = OBJ.Val.ToString() }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }

        /// <summary>
        /// 文章标题(Title)匹配器
        /// </summary>
        /// <param name="Val">匹配文本</param>
        /// <returns>返回符合匹配文本的ID集合</returns>
        public virtual List<int> MatchTitle(string Val)
        {
            try
            {
                List<int> IDList = new List<int>();

                string SQL = "CALL ?DataBase.`match<title`( ?Val )";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?Val", Val = Val }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 文章概要(Summary)匹配器
        /// </summary>
        /// <param name="Val">匹配文本</param>
        /// <returns>返回符合匹配文本的ID集合</returns>
        public virtual List<int> MatchSummary(string Val)
        {
            try
            {
                List<int> IDList = new List<int>();

                string SQL = "CALL ?DataBase.`match<summary`( ?Val )";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?Val", Val = Val }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 文章内容(Content)匹配器
        /// </summary>
        /// <param name="Val">匹配文本</param>
        /// <returns>返回符合匹配文本的ID集合</returns>
        public virtual List<int> MatchContent(string Val)
        {
            try
            {
                List<int> IDList = new List<int>();

                string SQL = "CALL ?DataBase.`match<content`( ?Val )";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?Val", Val = Val }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataTable result = MySqlConnH.GetTable(MySqlCommand);

                    foreach (DataRow Row in result.Rows)
                    {
                        IDList.Add(Convert.ToInt32(Row[0]));
                    }
                }
                return IDList;
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }


        /// <summary>
        /// 通过ID获取Index表中的目标文章数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public virtual Post GetIndex(int ID)
        {
            try
            {
                string SQL = "CALL ?DataBase.`get>index`( ?ID )";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    DataRow Row = MySqlConnH.GetRow(MySqlCommand);

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
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
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
            try
            {
                string SQL = "SELECT ?Key FROM ?DataBase.`?IndexView` WHERE ID = ?ID";

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?IndexView", Val = Views.IndexView },
                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
                {
                    return MySqlConnH.GetRow(MySqlCommand)[0].ToString();
                }
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }

        /// <summary>
        /// 通过ID获取Primary表中的目标文章数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public virtual Post GetPrimary(int ID)
        {
            try
            {
                string SQL = "CALL ?DataBase.`get>primary`( ?ID )";

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
                {
                    DataRow Row = MySqlConnH.GetRow(MySqlCommand);

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
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 取得Primary表的一个键值
        /// </summary>
        /// <param name="ID">文章序列号</param>
        /// <param name="Key">键名</param>
        /// <returns></returns>
        public virtual string GetPrimary(int ID, string Key)
        {
            try
            {
                string SQL = "SELECT ?Key FROM ?DataBase.`?PrimaryView` WHERE ID = ?ID";

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?PrimaryView", Val = Views.PrimaryView },

                    new MySqlParm() { Name = "?Key", Val = Key },
                    new MySqlParm() { Name = "?ID", Val = ID }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
                {
                    return MySqlConnH.GetRow(MySqlCommand)[0].ToString();
                }
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }


        /// <summary>
        /// 取得文本标题(Title)
        /// </summary>
        /// <param name="ID">文本序列号</param>
        /// <returns></returns>
        public virtual string GetTitle(int ID)
        {
            try
            {
                string SQL = "SELECT Title FROM ?DataBase.`?PrimaryView` WHERE ID = ?ID";

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?PrimaryView", Val = Views.PrimaryView },

                    new MySqlParm() { Name = "?ID", Val = ID }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
                {
                    return MySqlConnH.GetRow(MySqlCommand)[0].ToString();
                }
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 取得文本概要(Summary)
        /// </summary>
        /// <param name="ID">文本序列号</param>
        /// <returns></returns>
        public virtual string GetSummary(int ID)
        {
            try
            {
                string SQL = "SELECT Summary FROM ?DataBase.`?PrimaryView` WHERE ID = ?ID";

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?PrimaryView", Val = Views.PrimaryView },

                    new MySqlParm() { Name = "?ID", Val = ID }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
                {
                    return MySqlConnH.GetRow(MySqlCommand)[0].ToString();
                }
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 取得文本正文(Content)
        /// </summary>
        /// <param name="ID">文本序列号</param>
        /// <returns></returns>
        public virtual string GetContent(int ID)
        {
            try
            {
                string SQL = "SELECT Content FROM ?DataBase.`?PrimaryView` WHERE ID = ?ID";

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?PrimaryView", Val = Views.PrimaryView },

                    new MySqlParm() { Name = "?ID", Val = ID }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
                {
                    return MySqlConnH.GetRow(MySqlCommand)[0].ToString();
                }
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }

        /// <summary>
        /// 按长度取得文本标题(Title)
        /// </summary>
        /// <param name="ID">文本序列号</param>
        /// <param name="Length">取得的文章内容长度</param>
        /// <returns></returns>
        public virtual string GetTitle(int ID, int Length)
        {
            try
            {
                string SQL = "SELECT SUBSTRING(( SELECT Title FROM ?DataBase.`?PrimaryView` WHERE ID = ?ID ) FROM 1 FOR ?Length)";

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?PrimaryView", Val = Views.PrimaryView },

                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
                {
                    return MySqlConnH.GetRow(MySqlCommand)[0].ToString();
                }
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 按长度取得文本概要(Summary)
        /// </summary>
        /// <param name="ID">文本序列号</param>
        /// <param name="Length">取得的文章内容长度</param>
        /// <returns></returns>
        public virtual string GetSummary(int ID, int Length)
        {
            try
            {
                string SQL = "SELECT SUBSTRING(( SELECT Summary FROM ?DataBase.`?PrimaryView` WHERE ID = ?ID ) FROM 1 FOR ?Length)";

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?PrimaryView", Val = Views.PrimaryView },

                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
                {
                    return MySqlConnH.GetRow(MySqlCommand)[0].ToString();
                }
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 按长度取得文本正文(Content)
        /// </summary>
        /// <param name="ID">文本序列号</param>
        /// <param name="Length">取得的文章内容长度</param>
        /// <returns></returns>
        public virtual string GetContent(int ID, int Length)
        {
            try
            {
                string SQL = "SELECT SUBSTRING(( SELECT Content FROM ?DataBase.`?PrimaryView` WHERE ID = ?ID ) FROM 1 FOR ?Length)";

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?PrimaryView", Val = Views.PrimaryView },

                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?Length", Val = Length }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
                {
                    return MySqlConnH.GetRow(MySqlCommand)[0].ToString();
                }
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }


        /// <summary>
        /// 取得目标文本ID 的下一个文本ID（按ID升序查找）
        /// </summary>
        /// <param name="ID">目标文本ID</param>
        /// <returns>错误返回-1</returns>
        public virtual int NextID(int ID)
        {
            try
            {
                /* 取得比当前 ID 大的一行，实现对下一条数据的抓取 */
                string SQL = "SELECT ID FROM ?DataBase.`?IndexView` " +
                    "WHERE ID =( SELECT min(ID) FROM ?DataBase.`?IndexView` WHERE ID > ?ID );";

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?IndexView", Val = Views.IndexView },

                    new MySqlParm() { Name = "?ID", Val = ID }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
                {
                    foreach (DataRow Row in MySqlConnH.GetTable(MySqlCommand).Rows)/* 遍历查询到的表以取得唯一的数据行 */
                    {
                        return Convert.ToInt32(Row[0]);/* 直接返回该ID */
                    }
                }
                return -1;
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 取得目标文本ID 的下一个文本ID(重载二::按指定键升序查找)
        /// </summary>
        /// <param name="ID">目标文本ID</param>
        /// <param name="Key">指定键</param>
        /// <returns>错误返回-1</returns>
        public virtual int NextID(int ID, string Key)
        {
            try
            {
                /* 取得比当前 ID 大的一行，实现对下一条数据的抓取 */
                string SQL = "SELECT ID FROM ?DataBase.`?IndexView` WHERE " +
                             "?Key =( SELECT min(?Key) FROM ?DataBase.`?IndexView` WHERE " +
                             "?Key >( SELECT ?Key FROM ?DataBase.`?IndexView` WHERE ID = ?ID ))";

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?IndexView", Val = Views.IndexView },

                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?Key", Val = Key }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
                {
                    foreach (DataRow Row in MySqlConnH.GetTable(MySqlCommand).Rows)/* 遍历查询到的表以取得唯一的数据行 */
                    {
                        return Convert.ToInt32(Row[0]);/* 直接返回该ID */
                    }
                }
                return -1;
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }

        /// <summary>
        /// 取得指定目标文本ID 的上一个文本ID（按ID升序查找）
        /// </summary>
        /// <param name="ID">目标文本ID</param>
        /// <returns>错误返回-1</returns>
        public virtual int PrevID(int ID)
        {
            try
            {
                /* 取得比当前 ID 小的一行，实现对上一条数据的抓取 */
                string SQL = "SELECT ID FROM ?DataBase.`?IndexView` WHERE " +
                             "ID =( SELECT max(ID) FROM ?DataBase.`?IndexView` WHERE ID < ?ID );";

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?IndexView", Val = Views.IndexView },

                    new MySqlParm() { Name = "?ID", Val = ID }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    foreach (DataRow Row in MySqlConnH.GetTable(MySqlCommand).Rows)
                    {
                        return Convert.ToInt32(Row[0]);
                    }
                }
                return -1;
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 取得指定目标文本ID 的上一个文本ID(重载二::按指定键升序查找)
        /// </summary>
        /// <param name="ID">目标文本ID</param>
        /// <param name="Key">指定键</param>
        /// <returns>错误返回-1</returns>
        public virtual int PrevID(int ID, string Key)
        {
            try
            {
                /* 取得比当前 ID 小的一行，实现对上一条数据的抓取 */
                string SQL = "SELECT ID FROM ?DataBase.`?IndexView` WHERE " +
                             "?Key =( SELECT max(?Key) FROM ?DataBase.`?IndexView` WHERE " +
                             "?Key <( SELECT ?Key FROM ?DataBase.`?IndexView` WHERE ID = ?ID ))";

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?IndexView", Val = Views.IndexView },

                    new MySqlParm() { Name = "?ID", Val = ID },
                    new MySqlParm() { Name = "?Key", Val = Key }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    foreach (DataRow Row in MySqlConnH.GetTable(MySqlCommand).Rows)
                    {
                        return Convert.ToInt32(Row[0]);
                    }
                }
                return -1;
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
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
        /// <returns>错误返回-1</returns>
        private int GetMaxID()
        {
            try
            {
                string SQL = "SELECT max(ID) FROM ?DataBase.`?IndexTable`";

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?DataBase", Val = DataBase },
                    new MySqlParm() { Name = "?IndexTable", Val = Tables.IndexTable }
                };

                using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
                {
                    foreach (DataRow Row in MySqlConnH.GetTable(MySqlCommand).Rows)
                    {
                        return Convert.ToInt32(Row[0]);
                    }
                }
                return -1;
            }
            finally
            {
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }

        /// <summary>
        /// 初始化TextListH
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
        /// <returns></returns>
        public bool RegPost(Post Post)
        {
            DateTime t = DateTime.Now;

            string SQL =
                "INSERT INTO ?DataBase.`?IndexTable`" +
                " (`ID`, `GUID`, `Time`, `Mode`, `Type`, `User`, `UVCount`, `StarCount`) VALUES" +
                " ('?ID','?GUID', '?CT','?Mode','?Type','?User','?UVCount','?StarCount');"
                +
                "INSERT INTO ?DataBase.`?PrimaryTable`" +
                " (`ID`, `GUID`, `Time`, `Title`, `Summary`, `Content`, `Archiv`, `Label`, `Cover`) VALUES" +
                " ('?ID','?GUID','?LCT','?Title','?Summary','?Content','?Archiv','?Label','?Cover');";


            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "?DataBase", Val = DataBase },
                new MySqlParm() { Name = "?IndexTable", Val = Tables.IndexTable },
                new MySqlParm() { Name = "?PrimaryTable", Val = Tables.PrimaryTable },

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

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))
            {
                DataTable result = MySqlConnH.GetTable(MySqlCommand);

                foreach (DataRow Row in result.Rows)
                {

                }
            }

            return false;
        }


        /* 风险性功能，需要周全的开发逻辑以防止生产用数据库的意外灾难 */
        /// <summary>
        /// 注销文章（删除所有副本）
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        public bool DisposeReg(int ID) { return false; }
        /// <summary>
        /// 删除副本（仅单个副本）
        /// </summary>
        /// <param name="GUID">目标文章的GUID</param>
        /// <returns></returns>
        public bool DeletePost(int GUID) { return false; }

        #region 状态管理

        /// <summary>
        /// 将目标文章隐藏
        /// </summary>
        /// <param name="ID">目标文章的ID</param>
        /// <returns></returns>
        public bool Close(int ID) { return false; }
        /// <summary>
        /// 将目标文章列入计划状态
        /// </summary>
        /// <param name="ID">目标文章的ID</param>
        /// <returns></returns>
        public bool Schedule(int ID) { return false; }
        /// <summary>
        /// 将目标文章归档
        /// </summary>
        /// <param name="ID">目标文章的ID</param>
        /// <param name="Val">归档名</param>
        /// <returns></returns>
        public bool Archiv(int ID, string Val) { return false; }

        #endregion

        /// <summary>
        /// 通用文章键更改器
        /// </summary>
        /// <typeparam name="T">继承自IKey的类型</typeparam>
        /// <param name="OBJ">继承自IKey的对象实例</param>
        /// <returns></returns>
        public bool UpdateKey<T>(T OBJ) where T : IKey
        {
            try
            {
                //初始化键定位
                MySqlKey MySqlKey = new MySqlKey
                {
                    DataBase = DataBase,
                    Table = Tables.PrimaryTable,
                    Name = "ID",
                    Val = OBJ.ID.ToString()
                };
                return MySqlConnH.SetColumnValue(MySqlKey, OBJ.GetType().Name, OBJ.Val.ToString());
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }

        /// <summary>
        /// 更改文章标题(Title)
        /// </summary>
        /// <param name="ID">文章序列号</param>
        /// <param name="Val">新标题</param>
        /// <returns></returns>
        public bool UpdateTitle(int ID, string Val)
        {
            try
            {
                //初始化键定位
                MySqlKey MySqlKey = new MySqlKey
                {
                    DataBase = DataBase,
                    Table = Tables.PrimaryTable,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlConnH.SetColumnValue(MySqlKey, "Title", Val);
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 更改文章概要(Summary)
        /// </summary>
        /// <param name="ID">文章序列号</param>
        /// <param name="Val">新概要</param>
        /// <returns></returns>
        public bool UpdateSummary(int ID, string Val)
        {
            try
            {
                MySqlKey MySqlKey = new MySqlKey
                {
                    DataBase = DataBase,
                    Table = Tables.PrimaryTable,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlConnH.SetColumnValue(MySqlKey, "Summary", Val);
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 更改文章内容(Content)
        /// </summary>
        /// <param name="ID">文章序列号</param>
        /// <param name="Val">新内容</param>
        /// <returns></returns>
        public bool UpdateContent(int ID, string Val)
        {
            try
            {
                MySqlKey MySqlKey = new MySqlKey
                {
                    DataBase = DataBase,
                    Table = Tables.PrimaryTable,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlConnH.SetColumnValue(MySqlKey, "Content", Val);
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }

        /// <summary>
        /// 设置浏览计数(UVCount)
        /// </summary>
        /// <parmm name="ID">文本ID</parmm>
        /// <parmm name="Value">值</parmm>
        /// <returns></returns>
        public bool UpdateUVCount(int ID, int Val)
        {
            try
            {
                //初始化键定位
                MySqlKey MySqlKey = new MySqlKey
                {
                    DataBase = DataBase,
                    Table = Tables.IndexTable,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlConnH.SetColumnValue(MySqlKey, "UVCount", Val.ToString());
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }
        /// <summary>
        /// 设置星星计数(StarCount)
        /// </summary>
        /// <parmm name="ID">文本ID</parmm>
        /// <parmm name="Value">值</parmm>
        /// <returns></returns>
        public bool UpdateStarCount(int ID, int Val)
        {
            try
            {
                MySqlKey MySqlKey = new MySqlKey
                {
                    DataBase = DataBase,
                    Table = Tables.IndexTable,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlConnH.SetColumnValue(MySqlKey, "StarCount", Val.ToString());
            }
            finally
            {
                MySqlConnH.CloseHConnection();
                MySqlConnH.NullHCommand();
                MySqlConnH.DisposeHCommand();
            }
        }


        /// <summary>
        /// 检测ID、GUID是否匹配，之后合并Post数据表
        /// </summary>
        /// <parmm name="IndexTable">索引表Post实例</parmm>
        /// <parmm name="PrimaryTable">主表Post实例</parmm>
        /// <returns>错误则返回Content为ERROR的Post实例</returns>
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

                    Cover = IndexTable.Cover
                };
            }
            else
            {
                return new Post { Content = "ERROR" };
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

                Cover = IndexTable.Cover
            };
        }
    }
}