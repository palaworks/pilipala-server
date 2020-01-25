using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Data;

using dataUnit;
using LibStruct.MySql;
using LibStruct.pilipala;
using LibStruct.Interface;

using System.Text.RegularExpressions;

namespace businessUnit
{
    namespace pilipala
    {
        /// <summary>
        /// 权限接口
        /// </summary>
        interface IPaRoot
        {
            /// <summary>
            /// Pala数据表所在数据库
            /// </summary>
            string dataBase { get; }
            /// <summary>
            /// 文本表
            /// </summary>
            PaRoot.PalaSysTables Tables { get; }
            /// <summary>
            /// 文本视图
            /// </summary>
            PaRoot.PalaSysViews Views { get; }
            /// <summary>
            /// 数据库管理器实例
            /// </summary>
            MySqlConnH MySqlConnH { get; set; }
        }

        /// <summary>
        /// 权限控制器
        /// </summary>
        public class PaRoot
        {
            /// <summary>
            /// 
            /// </summary>
            public string dataBase { get; }
            /// <summary>
            /// 
            /// </summary>
            public PalaSysTables objTables { get; }
            /// <summary>
            /// 
            /// </summary>
            public PalaSysViews objViews { get; }
            /// <summary>
            /// 
            /// </summary>
            public MySqlConnH MySqlConnH { get; set; }

            int root_id;/* 权限ID */

            /// <summary>
            /// 初始化PaRoot
            /// </summary>
            /// <parmm name="root_id">权限ID</parmm>
            /// <parmm name="PaDB">啪啦数据库信息</parmm>
            public PaRoot(int root_id, PaDB PaDB)
            {
                this.root_id = root_id;
                objTables = PaDB.Tables;
                objViews = PaDB.Views;
                dataBase = PaDB.dataBase;
                MySqlConnH = PaDB.MySqlConnH;
            }

            /// <summary>
            /// 获得权限数据
            /// </summary>
            /// <returns></returns>
            public PaUser getRoot()
            {
                try
                {
                    PaUser PaUser = new PaUser();/* 定义权限数据 */

                    string SQL = "CALL " + dataBase + ".`get_root`( ?root_id )";
                    List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                    {
                        new mysqlParm() { parmName = "?root_id", parmValue = root_id }
                    };

                    using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                    {
                        DataTable pala_root = MySqlConnH.getTable(MySqlCommand);

                        foreach (DataRow Row in pala_root.Rows)/* 遍历数据库表以取得唯一的数据行 */
                        {
                            PaUser.root_id = Convert.ToInt32(Row["root_id"]);
                            PaUser.root_definer = Convert.ToString(Row["root_definer"]);
                            PaUser.site_debug = Convert.ToBoolean(Row["site_debug"]);
                            PaUser.site_access = Convert.ToBoolean(Row["site_access"]);
                            PaUser.site_url = Convert.ToString(Row["site_url"]);
                            PaUser.site_title = Convert.ToString(Row["site_title"]);
                            PaUser.site_summary = Convert.ToString(Row["site_summary"]);
                        }
                        return PaUser;
                    }
                }
                finally
                {
                    MySqlConnH.closeHConnection();
                    MySqlConnH.nullHCommand();
                    MySqlConnH.disposeHCommand();
                }
            }

            /// <summary>
            /// 默认pala数据表
            /// </summary>
            private struct dftTables
            {
                public static string root = "pala_root";
                public static string text_index = "pala_text_index";
                public static string text_main = "pala_text_main";
                public static string text_sub = "pala_text_sub";
            }
            /// <summary>
            /// 默认pala数据视图
            /// </summary>
            private struct dftViews
            {
                public static string view_index = "view>index";
                public static string view_main = "view>main";
                public static string view_sub = "view>sub";
            }

            /// <summary>
            /// 自定义pala数据表
            /// </summary>
            public struct PalaSysTables
            {
                /// <summary>
                /// 初始化表名结构
                /// </summary>
                /// <parmm name="root"></parmm>
                /// <parmm name="text_index"></parmm>
                /// <parmm name="text_main"></parmm>
                /// <parmm name="text_sub"></parmm>
                public PalaSysTables(string root, string text_index, string text_main, string text_sub) : this()
                {
                    this.root = root;
                    this.text_index = text_index;
                    this.text_main = text_main;
                    this.text_sub = text_sub;
                }

                /// <summary>
                /// 
                /// </summary>
                public string root { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string text_index { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string text_main { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string text_sub { get; set; }
            }
            /// <summary>
            /// 自定义pala数据视图
            /// </summary>
            public struct PalaSysViews
            {
                /// <summary>
                /// 初始化视图名结构
                /// </summary>
                /// <param name="text_index"></param>
                /// <param name="text_main"></param>
                /// <param name="text_sub"></param>
                public PalaSysViews(string text_index, string text_main, string text_sub) : this()
                {
                    this.view_index = text_index;
                    this.view_main = text_main;
                    this.view_sub = text_sub;
                }
                /// <summary>
                /// 
                /// </summary>
                public string view_index { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string view_main { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string view_sub { get; set; }
            }

            /// <summary>
            /// 以默认值定义表名（重载一::直接更改Tables）
            /// </summary>
            public static void defaultTables(ref PalaSysTables Tables)
            {
                Tables.root = dftTables.root;
                Tables.text_index = dftTables.text_index;
                Tables.text_main = dftTables.text_main;
                Tables.text_sub = dftTables.text_sub;
            }
            /// <summary>
            /// 以默认值定义视图名（重载一::直接更改Views）
            /// </summary>
            public static void defaultViews(ref PalaSysViews Views)
            {
                Views.view_index = dftViews.view_index;
                Views.view_main = dftViews.view_main;
                Views.view_sub = dftViews.view_sub;
            }

            /// <summary>
            /// 以默认值定义表名（重载二::返回默认Tables）
            /// </summary>
            /// <returns></returns>
            public static PalaSysTables defaultTables()
            { return new PalaSysTables(dftTables.root, dftTables.text_index, dftTables.text_main, dftTables.text_sub); }
            /// <summary>
            /// 以默认值定义视图名（重载二::返回默认Views）
            /// </summary>
            /// <returns></returns>
            public static PalaSysViews defaultViews()
            { return new PalaSysViews(dftViews.view_index, dftViews.view_main, dftViews.view_sub); }
        }

        /// <summary>
        /// 文本列表控制接口
        /// </summary>
        interface ITextListH : IPaRoot
        {
            /// <summary>
            /// 获得全部文本ID列表
            /// </summary>
            /// <returns></returns>
            List<int> getTextIDList();

            /// <summary>
            /// 获得全部文本ID列表(步进式)
            /// </summary>
            /// <parmm name="start">步进起始行（包含该行）</parmm>
            /// <parmm name="length">加载行数</parmm>
            /// <returns></returns>
            List<int> stepTextIDList(int start, int length);
            /// <summary>
            /// 获得指定类型的文本ID列表(步进式)
            /// </summary>
            /// <parmm name="start">步进起始行（包含该行）</parmm>
            /// <parmm name="length">加载行数</parmm>
            /// <parmm name="text_type">自定义文本类型</parmm>
            /// <returns></returns>
            List<int> stepTextIDList(int start, int length, string text_type);

            /// <summary>
            /// 文章标题(text_title)匹配器
            /// </summary>
            /// <param name="str">匹配文本</param>
            /// <returns>返回符合匹配文本的text_id集合</returns>
            List<int> matchTextTitle(string str);
            /// <summary>
            /// 文章标题(text_summary)匹配器
            /// </summary>
            /// <param name="str">匹配文本</param>
            /// <returns>返回符合匹配文本的text_id集合</returns>
            List<int> matchTextSummary(string str);
            /// <summary>
            /// 文章标题(text_content)匹配器
            /// </summary>
            /// <param name="str">匹配文本</param>
            /// <returns>返回符合匹配文本的text_id集合</returns>
            List<int> matchTextContent(string str);
        }
        /// <summary>
        /// 单一文本控制接口
        /// </summary>
        interface ITextH : IPaRoot
        {
            /// <summary>
            /// 获得符合text_id的文本索引数据
            /// </summary>
            /// <parmm name="text_id">文本序列号</parmm>
            /// <returns></returns>
            PaText getTextIndex(int text_id);
            /// <summary>
            /// 获得符合text_id的文本数据
            /// </summary>
            /// <parmm name="text_id">文本序列号</parmm>
            /// <returns></returns>
            PaText getTextMain(int text_id);
            /// <summary>
            ///  获得符合text_id的文本参数数据
            /// </summary>
            /// <parmm name="text_id">文本序列号</parmm>
            /// <returns></returns>
            PaText getTextSub(int text_id);

            /// <summary>
            /// 取得当前文本 text_id 的下一个文本 text_id
            /// </summary>
            /// <parmm name="current_text_id">当前文本序列号</parmm>
            /// <returns>错误返回 -1</returns>
            int nextTextID(int current_text_id);
            /// <summary>
            /// 取得当前文本 text_id 的上一个文本 text_id
            /// </summary>
            /// <parmm name="current_text_id">当前文本序列号</parmm>
            /// <returns>错误返回 -1</returns>
            int prevTextID(int current_text_id);
        }

        namespace UI
        {
            /// <summary>
            /// 文本列表控制器
            /// </summary>
            public abstract class TextListH : ITextListH
            {
                /// <summary>
                /// 
                /// </summary>
                public string dataBase { get; }
                /// <summary>
                /// 
                /// </summary>
                public PaRoot.PalaSysTables Tables { get; }
                /// <summary>
                /// 
                /// </summary>
                public PaRoot.PalaSysViews Views { get; }
                /// <summary>
                /// 
                /// </summary>
                public MySqlConnH MySqlConnH { get; set; }

                /// <summary>
                /// 初始化TextListH
                /// </summary>
                /// <parmm name="PaDB">啪啦数据库信息</parmm>
                public TextListH(PaDB PaDB)
                {
                    dataBase = PaDB.dataBase;
                    Tables = PaDB.Tables;
                    Views = PaDB.Views;
                    MySqlConnH = PaDB.MySqlConnH;
                }


                /// <summary>
                /// ITextListH接口的基础实现，详见其接口注释
                /// </summary>
                public virtual List<int> getTextIDList()
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        using (DataTable text_id_table
                            = MySqlConnH.getTable("SELECT text_id FROM " + dataBase + ".`" + Views.view_index + "`"))
                        {
                            foreach (DataRow Row in text_id_table.Rows)/* 遍历数据库表行，逐行取得数据 */
                            {
                                List_text_id.Add(Convert.ToInt32(Row["text_id"]));/* 每次循环完成，将获取到的文本数据添加至文本列表 */
                            }
                        }
                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /*未经迭代式维护*/
                /// <summary>
                /// ITextListH接口的基础实现，详见其接口注释
                /// </summary>
                public virtual List<int> stepTextIDList(int start, int length)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string SQL = "SELECT `" +
                                     Views.view_index + "`.text_id" +
                                     " FROM " +
                                     dataBase + ".`" + Views.view_index +
                                     "` LIMIT " +
                                     "?start , ?length";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?start", parmValue = start },
                            new mysqlParm() { parmName = "?length", parmValue = length }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable pala_text_index = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in pala_text_index.Rows)/* 遍历数据库表行，逐行取得数据 */
                            {
                                List_text_id.Add(Convert.ToInt32(Row["text_id"]));/* 每次循环完成，将获取到的文本数据添加至文本列表 */
                            }
                        }
                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// ITextListH接口的基础实现，详见其接口注释
                /// </summary>
                public virtual List<int> stepTextIDList(int start, int length, string text_type)
                {
                    try
                    {
                        string SQL = "SELECT `" +
                                     Views.view_index + "`.text_id" +
                                     " FROM " +
                                     dataBase + ".`" + Views.view_index +
                                     "` WHERE " +
                                     "text_type = ?text_type" +
                                     " LIMIT " +
                                     "?start , ?length";

                        List<int> List_text_id = new List<int>();

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {
                            new mysqlParm() { parmName = "?text_type", parmValue = text_type },
                            new mysqlParm() { parmName = "?start", parmValue = start },
                            new mysqlParm() { parmName = "?length", parmValue = length }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))
                        {
                            DataTable pala_text_index = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in pala_text_index.Rows)
                            {
                                List_text_id.Add(Convert.ToInt32(Row["text_id"]));
                            }
                        }
                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /*未经迭代式维护*/
                /// <summary>
                /// 获取总文本排行(无截止长度)
                /// </summary>
                /// <param name="key">键名</param>
                /// <param name="OrderType">asc(升序)或desc(降序)</param>
                /// <returns></returns>
                public virtual List<int> getTextIDList(string key, string OrderType)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string SQL = "SELECT text_id FROM " + dataBase + ".`" + Views.view_sub + "` ORDER BY ?key ?OrderType";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {
                            new mysqlParm() { parmName = "?key", parmValue = key },
                            new mysqlParm() { parmName = "?OrderType", parmValue = OrderType },
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))
                        {
                            DataTable text_id_table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in text_id_table.Rows)
                            {
                                List_text_id.Add(Convert.ToInt32(Row["text_id"]));
                            }
                        }
                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 获取总文本排行(有截止长度)
                /// </summary>
                /// <param name="key">键名</param>
                /// <param name="OrderType">asc(升序)或desc(降序)</param>
                /// <param name="length">截止长度</param>
                /// <returns></returns>
                public virtual List<int> getTextIDList(string key, string OrderType, int length)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string SQL = "SELECT text_id FROM " + dataBase + ".`" + Views.view_sub + "` ORDER BY ?key ?OrderType LIMIT 0,?length";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {

                            new mysqlParm() { parmName = "?key", parmValue = key },
                            new mysqlParm() { parmName = "?OrderType", parmValue = OrderType },
                            new mysqlParm() { parmName = "?length", parmValue = length }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))
                        {
                            DataTable text_id_table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in text_id_table.Rows)
                            {
                                List_text_id.Add(Convert.ToInt32(Row["text_id"]));
                            }
                        }
                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /// <summary>
                /// 获得指定类型的文本ID列表
                /// </summary>
                /// <param name="text_type">类型集合</param>
                /// <returns></returns>
                public virtual List<int> getTextIDList(List<TextType> text_type)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string str = "";
                        foreach (TextType a in text_type)
                        {
                            str += a.val + "|";
                        }
                        str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                        string SQL = "SELECT text_id FROM " + dataBase + ".`" + Views.view_index + "`" + " WHERE text_type REGEXP ?str";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {
                            new mysqlParm() { parmName = "?str", parmValue = str },
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))
                        {
                            DataTable text_id_table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in text_id_table.Rows)
                            {
                                List_text_id.Add(Convert.ToInt32(Row["text_id"]));
                            }
                        }

                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 获得指定归档的文本ID列表
                /// </summary>
                /// <param name="text_archiv">归档集合</param>
                /// <returns></returns>
                public virtual List<int> getTextIDList(List<TextArchiv> text_archiv)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string str = "";
                        foreach (TextArchiv a in text_archiv)
                        {
                            str += a.val + "|";
                        }
                        str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                        string SQL = "SELECT text_id FROM " + dataBase + ".`" + Views.view_main + "`" + " WHERE text_archiv REGEXP ?str";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {
                            new mysqlParm() { parmName = "?str", parmValue = str },
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))
                        {
                            DataTable text_id_table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in text_id_table.Rows)
                            {
                                List_text_id.Add(Convert.ToInt32(Row["text_id"]));
                            }
                        }

                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 获得指定标签的文本ID列表
                /// </summary>
                /// <param name="text_tag">标签集合</param>
                /// <returns></returns>
                public virtual List<int> getTextIDList(List<TextTag> text_tag)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string str = "";
                        foreach (TextTag a in text_tag)
                        {
                            str += a.val + "|";
                        }
                        str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                        string SQL = "SELECT text_id FROM " + dataBase + ".`" + Views.view_main + "`" + " WHERE text_tag REGEXP ?str";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {
                            new mysqlParm() { parmName = "?str", parmValue = str },
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))
                        {
                            DataTable text_id_table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in text_id_table.Rows)
                            {
                                List_text_id.Add(Convert.ToInt32(Row["text_id"]));
                            }
                        }

                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /// <summary>
                /// 获得指定类型的文本ID列表(重载二:排序指定)
                /// </summary>
                /// <param name="text_type">类型集合</param>
                /// <param name="key">被排序键</param>
                /// <param name="OrderType">排序类型</param>
                /// <returns></returns>
                public virtual List<int> getTextIDList(List<TextType> text_type, string key, string OrderType)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string str = "";
                        foreach (TextType a in text_type)
                        {
                            str += a.val + "|";
                        }
                        str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                        string SQL = "SELECT text_id FROM " + dataBase + ".`" + Views.view_index + "`" + " WHERE text_type REGEXP ?str ORDER BY ?key ?OrderType";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {
                            new mysqlParm() { parmName = "?str", parmValue = str },
                            new mysqlParm() { parmName = "?key", parmValue = key },
                            new mysqlParm() { parmName = "?OrderType", parmValue = OrderType },
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))
                        {
                            DataTable text_id_table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in text_id_table.Rows)
                            {
                                List_text_id.Add(Convert.ToInt32(Row["text_id"]));
                            }
                        }

                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 获得指定归档的文本ID列表(重载二:排序指定)
                /// </summary>
                /// <param name="text_archiv">归档集合</param>
                /// <param name="key">被排序键</param>
                /// <param name="OrderType">排序类型</param>
                /// <returns></returns>
                public virtual List<int> getTextIDList(List<TextArchiv> text_archiv, string key, string OrderType)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string str = "";
                        foreach (TextArchiv a in text_archiv)
                        {
                            str += a.val + "|";
                        }
                        str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                        string SQL = "SELECT text_id FROM " + dataBase + ".`" + Views.view_main + "`" + " WHERE text_archiv REGEXP ?str ORDER BY ?key ?OrderType";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {
                            new mysqlParm() { parmName = "?str", parmValue = str },
                            new mysqlParm() { parmName = "?key", parmValue = key },
                            new mysqlParm() { parmName = "?OrderType", parmValue = OrderType },
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))
                        {
                            DataTable text_id_table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in text_id_table.Rows)
                            {
                                List_text_id.Add(Convert.ToInt32(Row["text_id"]));
                            }
                        }

                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 获得指定标签的文本ID列表(重载二:排序指定)
                /// </summary>
                /// <param name="text_tag">标签集合</param>
                /// <param name="key">被排序键</param>
                /// <param name="OrderType">排序类型</param>
                /// <returns></returns>
                public virtual List<int> getTextIDList(List<TextTag> text_tag, string key, string OrderType)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string str = "";
                        foreach (TextTag a in text_tag)
                        {
                            str += a.val + "|";
                        }
                        str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                        string SQL = "SELECT text_id FROM " + dataBase + ".`" + Views.view_main + "`" + " WHERE text_tag REGEXP ?str ORDER BY ?key ?OrderType";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {
                            new mysqlParm() { parmName = "?str", parmValue = str },
                            new mysqlParm() { parmName = "?key", parmValue = key },
                            new mysqlParm() { parmName = "?OrderType", parmValue = OrderType },
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))
                        {
                            DataTable text_id_table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in text_id_table.Rows)
                            {
                                List_text_id.Add(Convert.ToInt32(Row["text_id"]));
                            }
                        }

                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /// <summary>
                /// 获得指定类型的文本ID列表(重载三:排序指定,有限长度)
                /// </summary>
                /// <param name="text_type">类型集合</param>
                /// <param name="key">被排序键</param>
                /// <param name="OrderType">排序类型</param>
                /// <param name="length">取用长度</param>
                /// <returns></returns>
                public virtual List<int> getTextIDList(List<TextType> text_type, string key, string OrderType, int length)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string str = "";
                        foreach (TextType a in text_type)
                        {
                            str += a.val + "|";
                        }
                        str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                        string SQL = "SELECT text_id FROM " + dataBase + ".`" + Views.view_index + "`" + " WHERE text_type REGEXP ?str ORDER BY ?key ?OrderType LIMIT 0,?length";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {
                            new mysqlParm() { parmName = "?str", parmValue = str },
                            new mysqlParm() { parmName = "?key", parmValue = key },
                            new mysqlParm() { parmName = "?OrderType", parmValue = OrderType },
                            new mysqlParm() { parmName = "?length", parmValue = length },
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))
                        {
                            DataTable text_id_table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in text_id_table.Rows)
                            {
                                List_text_id.Add(Convert.ToInt32(Row["text_id"]));
                            }
                        }

                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 获得指定归档的文本ID列表(重载三:排序指定,有限长度)
                /// </summary>
                /// <param name="text_archiv">归档集合</param>
                /// <param name="key">被排序键</param>
                /// <param name="OrderType">排序类型</param>
                /// <param name="length">取用长度</param>
                /// <returns></returns>
                public virtual List<int> getTextIDList(List<TextArchiv> text_archiv, string key, string OrderType, int length)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string str = "";
                        foreach (TextArchiv a in text_archiv)
                        {
                            str += a.val + "|";
                        }
                        str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                        string SQL = "SELECT text_id FROM " + dataBase + ".`" + Views.view_main + "`" + " WHERE text_archiv REGEXP ?str ORDER BY ?key ?OrderType LIMIT 0,?length";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {
                            new mysqlParm() { parmName = "?str", parmValue = str },
                            new mysqlParm() { parmName = "?key", parmValue = key },
                            new mysqlParm() { parmName = "?OrderType", parmValue = OrderType },
                            new mysqlParm() { parmName = "?length", parmValue = length },
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))
                        {
                            DataTable text_id_table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in text_id_table.Rows)
                            {
                                List_text_id.Add(Convert.ToInt32(Row["text_id"]));
                            }
                        }

                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 获得指定标签的文本ID列表(重载三:排序指定,有限长度)
                /// </summary>
                /// <param name="text_tag">标签集合</param>
                /// <param name="key">被排序键</param>
                /// <param name="OrderType">排序类型</param>
                /// <param name="length">取用长度</param>
                /// <returns></returns>
                public virtual List<int> getTextIDList(List<TextTag> text_tag, string key, string OrderType, int length)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string str = "";
                        foreach (TextTag a in text_tag)
                        {
                            str += a.val + "|";
                        }
                        str = str.Substring(0, str.Length - 1);//将最后一个或符号删除

                        string SQL = "SELECT text_id FROM " + dataBase + ".`" + Views.view_main + "`" + " WHERE text_tag REGEXP ?str ORDER BY ?key ?OrderType LIMIT 0,?length";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {
                            new mysqlParm() { parmName = "?str", parmValue = str },
                            new mysqlParm() { parmName = "?key", parmValue = key },
                            new mysqlParm() { parmName = "?OrderType", parmValue = OrderType },
                            new mysqlParm() { parmName = "?length", parmValue = length },
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))
                        {
                            DataTable text_id_table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in text_id_table.Rows)
                            {
                                List_text_id.Add(Convert.ToInt32(Row["text_id"]));
                            }
                        }

                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /// <summary>
                /// 文本匹配器
                /// </summary>
                /// <typeparmm name="T">继承自ITextBasic的类型</typeparmm>
                /// <parmm name="obj">继承自ITextBasic的对象实例</parmm>
                /// <returns></returns>
                public virtual List<int> matchText<T>(T obj) where T : ITextBasic
                {
                    if (typeof(T) == typeof(TextTitle))
                    {
                        return matchTextTitle(obj.val);
                    }
                    else if (typeof(T) == typeof(TextSummary))
                    {
                        return matchTextSummary(obj.val);
                    }
                    else if (typeof(T) == typeof(TextContent))
                    {
                        return matchTextContent(obj.val);
                    }
                    else
                    {
                        return null;
                    }
                }
                /// <summary>
                /// ITextListH接口的基础实现，详见其接口注释
                /// </summary>
                public virtual List<int> matchTextTitle(string str)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string SQL = "CALL " + dataBase + ".`match<main.title`( ?str )";
                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {
                            new mysqlParm() { parmName = "?str", parmValue = str }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in table.Rows)
                            {
                                int text_id = Convert.ToInt32(Row["text_id"]);

                                List_text_id.Add(text_id);
                            }
                        }
                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// ITextListH接口的基础实现，详见其接口注释
                /// </summary>
                public virtual List<int> matchTextSummary(string str)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string SQL = "CALL " + dataBase + ".`match<main.summary`( ?str )";
                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {
                            new mysqlParm() { parmName = "?str", parmValue = str }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in table.Rows)
                            {
                                int text_id = Convert.ToInt32(Row["text_id"]);

                                List_text_id.Add(text_id);
                            }
                        }
                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// ITextListH接口的基础实现，详见其接口注释
                /// </summary>
                public virtual List<int> matchTextContent(string str)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string SQL = "CALL " + dataBase + ".`match<main.content`( ?str )";
                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {
                            new mysqlParm() { parmName = "?str", parmValue = str }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in table.Rows)
                            {
                                int text_id = Convert.ToInt32(Row["text_id"]);

                                List_text_id.Add(text_id);
                            }
                        }
                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

            }

            /// <summary>
            /// 单一文本控制器
            /// </summary>
            public abstract class TextH : ITextH
            {
                /// <summary>
                /// 
                /// </summary>
                public string dataBase { get; }
                /// <summary>
                /// 
                /// </summary>
                public PaRoot.PalaSysTables Tables { get; }
                /// <summary>
                /// 
                /// </summary>
                public PaRoot.PalaSysViews Views { get; }
                /// <summary>
                /// 
                /// </summary>
                public MySqlConnH MySqlConnH { get; set; }

                /// <summary>
                /// 初始化TextH
                /// </summary>
                /// <parmm name="PaDB">啪啦数据库信息</parmm>
                public TextH(PaDB PaDB)
                {
                    dataBase = PaDB.dataBase;
                    Tables = PaDB.Tables;
                    Views = PaDB.Views;
                    MySqlConnH = PaDB.MySqlConnH;
                }

                /// <summary>
                /// ITextH接口的基础实现，详见其接口注释
                /// </summary>
                public virtual PaText getTextIndex(int text_id)
                {
                    try
                    {
                        PaText PaText = new PaText();

                        string SQL = "CALL " + dataBase + ".`get>index`( ?text_id )";
                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataRow Row = MySqlConnH.getRow(MySqlCommand);

                            PaText.text_id = Convert.ToInt32(Row["text_id"]);
                            PaText.text_guid = Convert.ToString(Row["text_guid"]);
                            PaText.time_set = Convert.ToDateTime(Row["time_set"]);
                            PaText.text_mode = Convert.ToString(Row["text_mode"]);
                            PaText.text_type = Convert.ToString(Row["text_type"]);
                            PaText.text_user = Convert.ToString(Row["text_user"]);

                            return PaText;
                        }
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 取得Index表的一个键值
                /// </summary>
                /// <param name="text_id">文章序列号</param>
                /// <param name="key">键名</param>
                /// <returns></returns>
                public virtual string getTextIndex(int text_id, string key)
                {
                    try
                    {
                        string SQL = "SELECT " +
                                     "`" + Views.view_index + "`.?key" +
                                     " FROM " +
                                     dataBase + ".`" + Views.view_index + "`" +
                                     " WHERE `" +
                                     Views.view_index + "`.text_id = ?text_id";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?key", parmValue = key },
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataRow Row = MySqlConnH.getRow(MySqlCommand);

                            return Convert.ToString(Convert.ToInt32(Row[key]));
                        }
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /// <summary>
                /// ITextH接口的基础实现，详见其接口注释
                /// </summary>
                public virtual PaText getTextMain(int text_id)
                {
                    try
                    {
                        PaText PaText = new PaText();

                        string SQL = "CALL " + dataBase + ".`get>main`( ?text_id )";
                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataRow Row = MySqlConnH.getRow(MySqlCommand);

                            PaText.time_change = Convert.ToDateTime(Row["time_change"]);
                            PaText.text_title = Convert.ToString(Row["text_title"]);
                            PaText.text_summary = Convert.ToString(Row["text_summary"]);
                            PaText.text_content = Convert.ToString(Row["text_content"]);
                            PaText.text_archiv = Convert.ToString(Row["text_archiv"]);
                            PaText.text_tag = Convert.ToString(Row["text_tag"]);

                            return PaText;
                        }
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 取得Main表的一个键值
                /// </summary>
                /// <param name="text_id">文章序列号</param>
                /// <param name="key">键名</param>
                /// <returns></returns>
                public virtual string getTextMain(int text_id, string key)
                {
                    try
                    {
                        string SQL = "SELECT " +
                                     "`" + Views.view_main + "`.?key" +
                                     " FROM " +
                                     dataBase + ".`" + Views.view_main + "`" +
                                     " WHERE `" +
                                     Views.view_main + "`.text_id = ?text_id";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?key", parmValue = key },
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataRow Row = MySqlConnH.getRow(MySqlCommand);

                            return Convert.ToString(Convert.ToInt32(Row[key]));
                        }
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /// <summary>
                /// ITextH接口的基础实现，详见其接口注释
                /// </summary>
                public virtual PaText getTextSub(int text_id)
                {
                    try
                    {
                        PaText PaText = new PaText();

                        string SQL = "CALL " + dataBase + ".`get>sub`( ?text_id )";
                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataRow Row = MySqlConnH.getRow(MySqlCommand);

                            PaText.count_pv = Convert.ToInt32(Row["count_pv"]);
                            PaText.count_comment = Convert.ToInt32(Row["count_comment"]);
                            PaText.count_star = Convert.ToInt32(Row["count_star"]);
                            PaText.before_html = Convert.ToString(Row["before_html"]);

                            return PaText;
                        }
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 取得Sub表的一个键值
                /// </summary>
                /// <param name="text_id">文章序列号</param>
                /// <param name="key">键名</param>
                /// <returns></returns>
                public virtual string getTextSub(int text_id, string key)
                {
                    try
                    {
                        string SQL = "SELECT " +
                                     "`" + Views.view_sub + "`.?key" +
                                     " FROM " +
                                     dataBase + ".`" + Views.view_index + "`," +
                                     " WHERE `" +
                                     Views.view_sub + "`.text_id = ?text_id";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?key", parmValue = key },
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataRow Row = MySqlConnH.getRow(MySqlCommand);

                            return Convert.ToString(Convert.ToInt32(Row[key]));
                        }
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /// <summary>
                /// 取得文本标题(text_title)
                /// </summary>
                /// <param name="text_id">文本序列号</param>
                /// <returns></returns>
                public virtual string getTextTitle(int text_id)
                {
                    try
                    {
                        string SQL = "SELECT " +
                                     "`" + Views.view_main + "`.text_title" +
                                     " FROM " +
                                     dataBase + ".`" + Views.view_index + "`," +
                                     dataBase + ".`" + Views.view_main + "`" +
                                     " WHERE `" +
                                     Views.view_index + "`.text_guid =`" + Views.view_main + "`.text_guid" +
                                     " AND `" +
                                     Views.view_main + "`.text_id = ?text_id";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                            {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                            };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in table.Rows)/* 遍历数据库表以取得唯一的数据行 */
                            {
                                return Convert.ToString(Row["text_title"]);
                            }
                        }
                        return null;
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 取得文本概要(text_summary)
                /// </summary>
                /// <param name="text_id">文本序列号</param>
                /// <returns></returns>
                public virtual string getTextSummary(int text_id)
                {
                    try
                    {
                        string SQL = "SELECT " +
                                     "`" + Views.view_main + "`.text_summary" +
                                     " FROM " +
                                     dataBase + ".`" + Views.view_index + "`," +
                                     dataBase + ".`" + Views.view_main + "`" +
                                     " WHERE `" +
                                     Views.view_index + "`.text_guid =`" + Views.view_main + "`.text_guid" +
                                     " AND `" +
                                     Views.view_main + "`.text_id = ?text_id";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                            {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                            };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in table.Rows)/* 遍历数据库表以取得唯一的数据行 */
                            {
                                return Convert.ToString(Row["text_summary"]);
                            }
                        }
                        return null;
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 取得文本正文(text_content)
                /// </summary>
                /// <param name="text_id">文本序列号</param>
                /// <returns></returns>
                public virtual string getTextContent(int text_id)
                {
                    try
                    {
                        string SQL = "SELECT " +
                                     "`" + Views.view_main + "`.text_content" +
                                     " FROM " +
                                     dataBase + ".`" + Views.view_index + "`," +
                                     dataBase + ".`" + Views.view_main + "`" +
                                     " WHERE `" +
                                      Views.view_index + "`.text_guid =`" + Views.view_main + "`.text_guid" +
                                     " AND `" +
                                     Views.view_main + "`.text_id = ?text_id";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                            {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                            };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in table.Rows)/* 遍历数据库表以取得唯一的数据行 */
                            {
                                return Convert.ToString(Row["text_content"]);
                            }
                        }
                        return null;
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /// <summary>
                /// 按长度取得文本标题(text_title)
                /// </summary>
                /// <param name="text_id">文本序列号</param>
                /// <param name="length">取得的文章内容长度</param>
                /// <returns></returns>
                public virtual string getTextTitle(int text_id, int length)
                {
                    try
                    {
                        string SQL = "SELECT SUBSTRING((" +

                                     "SELECT " +
                                     "`" + Views.view_main + "`.text_title" +
                                     " FROM " +
                                     dataBase + ".`" + Views.view_index + "`," +
                                     dataBase + ".`" + Views.view_main + "`" +
                                     " WHERE `" +
                                     Views.view_index + "`.text_guid =`" + Views.view_main + "`.text_guid" +
                                     " AND `" +
                                     Views.view_main + "`.text_id = ?text_id" +

                                     ") FROM 1 FOR ?length)";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                            {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id },
                            new mysqlParm() { parmName = "?length", parmValue = length }
                            };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            return MySqlConnH.getRow(MySqlCommand)[0].ToString();
                        }
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 按长度取得文本概要(text_summary)
                /// </summary>
                /// <param name="text_id">文本序列号</param>
                /// <param name="length">取得的文章内容长度</param>
                /// <returns></returns>
                public virtual string getTextSummary(int text_id, int length)
                {
                    try
                    {
                        string SQL = "SELECT SUBSTRING((" +

                                     "SELECT " +
                                     "`" + Views.view_main + "`.text_summary" +
                                     " FROM " +
                                     dataBase + ".`" + Views.view_index + "`," +
                                     dataBase + ".`" + Views.view_main + "`" +
                                     " WHERE `" +
                                     Views.view_index + "`.text_guid =`" + Views.view_main + "`.text_guid" +
                                     " AND `" +
                                     Views.view_main + "`.text_id = ?text_id" +

                                     ") FROM 1 FOR ?length)";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                            {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id },
                            new mysqlParm() { parmName = "?length", parmValue = length }
                            };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            return MySqlConnH.getRow(MySqlCommand)[0].ToString();
                        }
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 按长度取得文本正文(text_content)
                /// </summary>
                /// <param name="text_id">文本序列号</param>
                /// <param name="length">取得的文章内容长度</param>
                /// <returns></returns>
                public virtual string getTextContent(int text_id, int length)
                {
                    try
                    {
                        string SQL = "SELECT SUBSTRING((" +

                                     "SELECT " +
                                     "`" + Views.view_main + "`.text_content" +
                                     " FROM " +
                                     dataBase + ".`" + Views.view_index + "`," +
                                     dataBase + ".`" + Views.view_main + "`" +
                                     " WHERE `" +
                                     Views.view_index + "`.text_guid =`" + Views.view_main + "`.text_guid" +
                                     " AND `" +
                                     Views.view_main + "`.text_id = ?text_id" +

                                     ") FROM 1 FOR ?length)";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                            {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id },
                            new mysqlParm() { parmName = "?length", parmValue = length }
                            };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            return MySqlConnH.getRow(MySqlCommand)[0].ToString();
                        }
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /// <summary>
                /// ITextH接口的基础实现，详见其接口注释
                /// </summary>
                public virtual int nextTextID(int current_text_id)
                {
                    try
                    {
                        /* 取得比当前 text_id 大的一行，实现对下一条数据的抓取 */
                        string SQL = "SELECT *" +
                                     " FROM " +
                                     dataBase + ".`" + Views.view_index +
                                     "` WHERE " +
                                     "text_id = (SELECT min(text_id) FROM " + dataBase + ".`" + Views.view_index + "` WHERE text_id > ?current_text_id);";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?current_text_id", parmValue = current_text_id }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            foreach (DataRow Row in MySqlConnH.getTable(MySqlCommand).Rows)/* 遍历查询到的表以取得唯一的数据行 */
                            {
                                return Convert.ToInt32(Row["text_id"]);/* 直接返回该text_id */
                            }
                            return -1;/* 错误返回-1 */
                        }
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// ITextH接口的基础实现，详见其接口注释
                /// </summary>
                public virtual int prevTextID(int current_text_id)
                {
                    try
                    {
                        /* 取得比当前 text_id 小的一行，实现对上一条数据的抓取 */
                        string SQL = "SELECT *" +
                                     " FROM " +
                                     dataBase + ".`" + Views.view_index +
                                     "` WHERE " +
                                     "text_id = (SELECT max(text_id) FROM " + dataBase + ".`" + Views.view_index + "` WHERE text_id < ?current_text_id);";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?current_text_id", parmValue = current_text_id }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            foreach (DataRow Row in MySqlConnH.getTable(MySqlCommand).Rows)/* 遍历查询到的表以取得唯一的数据行 */
                            {
                                return Convert.ToInt32(Row["text_id"]);/* 直接返回该text_id */
                            }
                            return -1;/* 错误返回-1 */
                        }
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }


                #region 抽象类
                /*未经迭代式维护*/
                /// <summary>
                /// 文章更改器
                /// </summary>
                public abstract bool updateTextBasic<T>(T obj) where T : ITextBasic;
                /// <summary>
                /// 更改文章标题(text_title)
                /// </summary>
                public abstract bool updateTextTitle(int text_id, string text_title);
                /// <summary>
                /// 更改文章概要(text_summary)
                /// </summary>
                public abstract bool updateTextSummary(int text_id, string text_summary);
                /// <summary>
                /// 更改文章内容(text_content)
                /// </summary>
                public abstract bool updateTextContent(int text_id, string text_content);
                /// <summary>
                /// 设置浏览计数(count_pv)
                /// </summary>
                public abstract bool update_countPv(int text_id, int value);
                /// <summary>
                /// 设置星星计数(count_star)
                /// </summary>
                public abstract bool update_countStar(int text_id, int value);

                #endregion
            }
        }

        namespace Edit
        {
            /// <summary>
            /// 文本列表控制器
            /// </summary>
            public class TextListH : UI.TextListH
            {
                /// <summary>
                /// 初始化TextListH
                /// </summary>
                /// <parmm name="PaDB">啪啦数据库信息</parmm>
                public TextListH(PaDB PaDB) : base(PaDB) { }


                /// <summary>
                /// ITextListH接口的基础实现，详见其接口注释
                /// </summary>
                public new List<int> getTextIDList()
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        using (DataTable text_id_table
                            = MySqlConnH.getTable("SELECT text_id FROM " + dataBase + "." + Tables.text_index))
                        {
                            foreach (DataRow Row in text_id_table.Rows)/* 遍历数据库表行，逐行取得数据 */
                            {
                                List_text_id.Add(Convert.ToInt32(Row["text_id"]));/* 每次循环完成，将获取到的文本数据添加至文本列表 */
                            }
                        }
                        return List_text_id;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
            }

            /// <summary>
            /// 单一文本控制器
            /// 方法注释参见ITextH接口
            /// </summary>
            public class TextH : UI.TextH
            {
                /// <summary>
                /// 初始化TextH
                /// </summary>
                /// <parmm name="PaDB">啪啦数据库信息</parmm>
                public TextH(PaDB PaDB) : base(PaDB) { }

                /// <summary>
                /// 根据text_id取得数据，无论展示与否
                /// </summary>
                public PaText getTextData(int text_id)
                {
                    try
                    {
                        PaText PaText = new PaText();

                        string SQL1 = "SELECT * FROM " +
                             dataBase + "." + Tables.text_index + "," +
                             dataBase + "." + Tables.text_sub +
                             " WHERE " +
                             Tables.text_index + ".text_guid = " + Tables.text_sub + ".text_guid" +
                             " AND " +
                             Tables.text_index + ".text_id = ?text_id";


                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL1, List_mysqlParm))/* 参数化查询 */
                        {
                            DataRow Row1 = MySqlConnH.getRow(MySqlCommand);/*得到查询结果*/

                            /*pala_text_index表*/
                            PaText.text_id = Convert.ToInt32(Row1["text_id"]);
                            PaText.text_guid = Convert.ToString(Row1["text_guid"]);
                            PaText.time_set = Convert.ToDateTime(Row1["time_set"]);
                            PaText.text_mode = Convert.ToString(Row1["text_mode"]);
                            PaText.text_type = Convert.ToString(Row1["text_type"]);
                            PaText.text_user = Convert.ToString(Row1["text_user"]);

                            /*pala_text_sub表*/
                            PaText.count_pv = Convert.ToInt32(Row1["count_pv"]);
                            PaText.count_comment = Convert.ToInt32(Row1["count_comment"]);
                            PaText.count_star = Convert.ToInt32(Row1["count_star"]);
                            PaText.before_html = Convert.ToString(Row1["before_html"]);
                        }

                        #region pala_text_main表

                        string SQL2 = "SELECT * FROM " + dataBase + "." + Tables.text_main + " WHERE text_guid = " + PaText.text_guid;

                        DataRow Row2 = MySqlConnH.getRow(SQL2); ;

                        PaText.time_change = Convert.ToDateTime(Row2["time_change"]);
                        PaText.text_title = Convert.ToString(Row2["text_title"]);
                        PaText.text_summary = Convert.ToString(Row2["text_summary"]);
                        PaText.text_content = Convert.ToString(Row2["text_content"]);
                        PaText.text_archiv = Convert.ToString(Row2["text_archiv"]);
                        PaText.text_tag = Convert.ToString(Row2["text_tag"]);

                        #endregion

                        return PaText;
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /// <summary>
                /// 文章更改器
                /// </summary>
                /// <typeparam name="T">继承自ITextBasic的类型</typeparam>
                /// <param name="obj">继承自ITextBasic的对象实例</param>
                /// <returns></returns>
                public override bool updateTextBasic<T>(T obj)
                {
                    if (typeof(T) == typeof(TextTitle))
                    {
                        return updateTextTitle(obj.text_id, obj.val);
                    }
                    else if (typeof(T) == typeof(TextSummary))
                    {
                        return updateTextSummary(obj.text_id, obj.val);
                    }
                    else if (typeof(T) == typeof(TextContent))
                    {
                        return updateTextContent(obj.text_id, obj.val);
                    }
                    else
                    {
                        return false;
                    }
                }
                /// <summary>
                /// 更改文章标题(text_title)
                /// </summary>
                /// <param name="text_id">文章序列号</param>
                /// <param name="text_title">新标题</param>
                /// <returns></returns>
                public override bool updateTextTitle(int text_id, string text_title)
                {
                    try
                    {
                        //初始化键定位
                        mysqlKey mysqlKey = new mysqlKey
                        {
                            dataBase = dataBase,
                            table = Tables.text_main,
                            primaryKeyName = "text_id",
                            primaryKeyValue = text_id.ToString()
                        };
                        return MySqlConnH.setColumnValue(mysqlKey, "text_title", text_title.ToString());
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 更改文章概要(text_summary)
                /// </summary>
                /// <param name="text_id">文章序列号</param>
                /// <param name="text_summary">新概要</param>
                /// <returns></returns>
                public override bool updateTextSummary(int text_id, string text_summary)
                {
                    try
                    {
                        mysqlKey mysqlKey = new mysqlKey
                        {
                            dataBase = dataBase,
                            table = Tables.text_main,
                            primaryKeyName = "text_id",
                            primaryKeyValue = text_id.ToString()
                        };
                        return MySqlConnH.setColumnValue(mysqlKey, "text_summary", text_summary.ToString());
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 更改文章内容(text_content)
                /// </summary>
                /// <param name="text_id">文章序列号</param>
                /// <param name="text_content">新内容</param>
                /// <returns></returns>
                public override bool updateTextContent(int text_id, string text_content)
                {
                    try
                    {
                        mysqlKey mysqlKey = new mysqlKey
                        {
                            dataBase = dataBase,
                            table = Tables.text_main,
                            primaryKeyName = "text_id",
                            primaryKeyValue = text_id.ToString()
                        };
                        return MySqlConnH.setColumnValue(mysqlKey, "text_content", text_content.ToString());
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /// <summary>
                /// 设置浏览计数(count_pv)
                /// </summary>
                /// <parmm name="text_id">文本ID</parmm>
                /// <parmm name="value">值</parmm>
                /// <returns></returns>
                public override bool update_countPv(int text_id, int value)
                {
                    try
                    {
                        //初始化键定位
                        mysqlKey mysqlKey = new mysqlKey
                        {
                            dataBase = dataBase,
                            table = Tables.text_sub,
                            primaryKeyName = "text_id",
                            primaryKeyValue = text_id.ToString()
                        };
                        return MySqlConnH.setColumnValue(mysqlKey, "count_pv", value.ToString());
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 设置星星计数(count_star)
                /// </summary>
                /// <parmm name="text_id">文本ID</parmm>
                /// <parmm name="value">值</parmm>
                /// <returns></returns>
                public override bool update_countStar(int text_id, int value)
                {
                    try
                    {
                        mysqlKey mysqlKey = new mysqlKey
                        {
                            dataBase = dataBase,
                            table = Tables.text_sub,
                            primaryKeyName = "text_id",
                            primaryKeyValue = text_id.ToString()
                        };
                        return MySqlConnH.setColumnValue(mysqlKey, "count_star", value.ToString());
                    }
                    finally
                    {
                        MySqlConnH.closeHConnection();
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
            }
        }

        /// <summary>
        /// 啪啦函数
        /// </summary>
        public class PaFn
        {
            /// <summary>
            /// 合并PaText数据表
            /// </summary>
            /// <parmm name="TextMain">主表PaText</parmm>
            /// <parmm name="TextSub">副表PaText</parmm>
            /// <returns></returns>
            public static PaText fill(PaText index, PaText main, PaText sub)
            {
                return new PaText
                {
                    /*pala_text_index表*/
                    text_id = index.text_id,
                    text_guid = index.text_guid,
                    time_set = index.time_set,
                    text_mode = index.text_mode,
                    text_type = index.text_type,
                    text_user = index.text_user,

                    /*pala_text_sub表*/
                    count_pv = sub.count_pv,
                    count_comment = sub.count_comment,
                    count_star = sub.count_star,
                    before_html = sub.before_html,

                    /*pala_text_main表*/
                    time_change = main.time_change,
                    text_title = main.text_title,
                    text_summary = main.text_summary,
                    text_content = main.text_content,
                    text_archiv = main.text_archiv,
                    text_tag = main.text_tag

                };
            }


            /// <summary>
            /// html过滤器
            /// </summary>
            /// <param name="str">待过滤的字符串</param>
            /// <returns></returns>
            public static string htmlFilter(string str)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return "";
                }

                string regEx_style = "<style[^>]*?>[\\s\\S]*?<\\/style>";
                string regEx_script = "<script[^>]*?>[\\s\\S]*?<\\/script>";
                string regEx_html = "<[^>]+>";

                str = Regex.Replace(str, regEx_style, "");
                str = Regex.Replace(str, regEx_script, "");
                str = Regex.Replace(str, regEx_html, "");
                str = Regex.Replace(str, "\\s*|\t|\r|\n", "");
                str = str.Replace(" ", "");

                return str.Trim();
            }

            /// <summary>
            /// 距现在字符串生成器
            /// </summary>
            /// <param name="dateTime">要计算的时间</param>
            /// <returns></returns>
            public static string timeFromNow(DateTime dateTime)
            {
                //获取当前时间
                DateTime DateTime1 = DateTime.Now;

                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(dateTime.Ticks);

                //时间比较，得出差值
                TimeSpan ts = ts1.Subtract(ts2).Duration();

                if (ts.Days >= 60)
                {
                    return null;
                }
                else
                if (ts.Days >= 28 && ts.Days < 60)
                {
                    return "一月前";
                }
                else
                if (ts.Days >= 21 && ts.Days < 28)
                {
                    return "三周前";
                }
                else
                if (ts.Days >= 14 && ts.Days < 21)
                {
                    return "二周前";
                }
                else
                if (ts.Days >= 7)
                {
                    return "一周前";
                }
                else
                if (ts.Days < 1 && ts.Hours < 6)
                {
                    return "刚刚";
                }
                else
                {
                    switch (ts.Days)
                    {
                        case 0: return "今天";
                        case 1: return "昨天";
                        case 2: return "二天前";
                        case 3: return "三天前";
                        case 4: return "四天前";
                        case 5: return "五天前";
                        case 6: return "六天前";
                        default: return null;
                    }
                }
            }

        }

    }
}
