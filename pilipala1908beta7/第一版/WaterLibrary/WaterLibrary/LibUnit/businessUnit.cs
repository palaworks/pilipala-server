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
            PaRoot.Tables Tables { get; }
            /// <summary>
            /// 文本视图
            /// </summary>
            PaRoot.Views Views { get; }
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
            public Tables objTables { get; }
            /// <summary>
            /// 
            /// </summary>
            public Views objViews { get; }
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

                    string SQL = "call " + dataBase + ".`get_root`( ?root_id )";
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
                public static string text_index = "view>index";
                public static string text_index_page = "view>index.page";
                public static string text_index_post = "view>index.post";

                public static string text_main = "view>main";
                public static string text_sub = "view>sub";
            }

            /// <summary>
            /// 自定义pala数据表
            /// </summary>
            public struct Tables
            {
                /// <summary>
                /// 初始化表名结构
                /// </summary>
                /// <parmm name="root"></parmm>
                /// <parmm name="text_index"></parmm>
                /// <parmm name="text_main"></parmm>
                /// <parmm name="text_sub"></parmm>
                public Tables(string root, string text_index, string text_main, string text_sub) : this()
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
            public struct Views
            {
                /// <summary>
                /// 初始化视图名结构
                /// </summary>
                /// <parmm name="text_main"></parmm>
                /// <parmm name="text_sub"></parmm>
                /// <parmm name="text_index_post"></parmm>
                /// <parmm name="text_index_page"></parmm>
                public Views(string text_main, string text_sub, string text_index_post, string text_index_page) : this()
                {
                    this.text_index_page = text_index_page;
                    this.text_index_post = text_index_post;
                    this.text_main = text_main;
                    this.text_sub = text_sub;
                }
                /// <summary>
                /// 
                /// </summary>
                public string text_index { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string text_index_page { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string text_index_post { get; set; }
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
            /// 以默认值定义表名（重载一::直接更改Tables）
            /// </summary>
            public static void defaultTables(ref Tables Tables)
            {
                Tables.root = dftTables.root;
                Tables.text_index = dftTables.text_index;
                Tables.text_main = dftTables.text_main;
                Tables.text_sub = dftTables.text_sub;
            }
            /// <summary>
            /// 以默认值定义视图名（重载一::直接更改Views）
            /// </summary>
            public static void defaultViews(ref Views Views)
            {
                Views.text_main = dftViews.text_main;
                Views.text_sub = dftViews.text_sub;
                Views.text_index_post = dftViews.text_index_post;
                Views.text_index_page = dftViews.text_index_page;
            }

            /// <summary>
            /// 以默认值定义表名（重载二::返回默认Tables）
            /// </summary>
            /// <returns></returns>
            public static Tables defaultTables()
            { return new Tables(dftTables.root, dftTables.text_index, dftTables.text_main, dftTables.text_sub); }
            /// <summary>
            /// 以默认值定义视图名（重载二::返回默认Views）
            /// </summary>
            /// <returns></returns>
            public static Views defaultViews()
            { return new Views(dftViews.text_main, dftViews.text_sub, dftViews.text_index_post, dftViews.text_index_page); }
        }

        /// <summary>
        /// 文本列表控制接口
        /// </summary>
        interface ITextListH : IPaRoot
        {
            /// <summary>
            /// 获得文本索引表（重载一::得到全文本列表）
            /// </summary>
            /// <returns></returns>
            List<PaText> getTextList();
            /// <summary>
            /// 获得文本索引表（重载二::得到指定类型的文本列表）
            /// </summary>
            /// <parmm name="text_type">自定义文本类型</parmm>
            /// <returns></returns>
            List<PaText> getTextList(string text_type);

            /// <summary>
            /// 步进式获得文本索引表（重载一::得到全文本列表）
            /// </summary>
            /// <parmm name="row">步进起始行（包含该行）</parmm>
            /// <parmm name="rowLength">加载行数</parmm>
            /// <returns></returns>
            List<PaText> stepTextList(int row, int rowLength);
            /// <summary>
            /// 步进式获得文本索引表（重载一::得到指定类型的文本列表）
            /// </summary>
            /// <parmm name="row">步进起始行（包含该行）</parmm>
            /// <parmm name="rowLength">加载行数</parmm>
            /// <parmm name="text_type">自定义文本类型</parmm>
            /// <returns></returns>
            List<PaText> stepTextList(int row, int rowLength, string text_type);
        }
        /// <summary>
        /// 文本控制接口
        /// </summary>
        interface ITextH : IPaRoot
        {
            /// <summary>
            /// 获得指定ID的文本主表行
            /// </summary>
            /// <parmm name="text_id">文本ID</parmm>
            /// <returns></returns>
            PaText getTextMain(int text_id);
            /// <summary>
            /// 获得指定ID的文本副表行
            /// </summary>
            /// <parmm name="text_id">文本ID</parmm>
            /// <returns></returns>
            PaText getTextSub(int text_id);

            /// <summary>
            /// 取得文本标题
            /// </summary>
            /// <parmm name="text_id">文本序列号</parmm>
            PaText getTextTitle(int text_id);
            /// <summary>
            /// 取得文本概要
            /// </summary>
            /// <parmm name="text_id">文本序列号</parmm>
            PaText getTextSummary(int text_id);
            /// <summary>
            /// 取得文本正文
            /// </summary>
            /// <parmm name="text_id">文本序列号</parmm>
            PaText getTextContent(int text_id);

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

            /// <summary>
            /// 随机获得文本索引表行
            /// </summary>
            /// <parmm name="excluded_text_id">不参与随机操作的文本ID</parmm>
            /// <parmm name="text_type">自定义文本类型</parmm>
            /// <returns></returns>
            PaText rdmTextIndex(int excluded_text_id, string text_type);
        }

        namespace UI
        {
            /// <summary>
            /// 文本列表控制器
            /// 方法注释参见ITextListH接口
            /// </summary>
            public class TextListH : ITextListH
            {
                /// <summary>
                /// 
                /// </summary>
                public string dataBase { get; }
                /// <summary>
                /// 
                /// </summary>
                public PaRoot.Tables Tables { get; }
                /// <summary>
                /// 
                /// </summary>
                public PaRoot.Views Views { get; }
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
                /// 
                /// </summary>
                public virtual List<PaText> getTextList()
                {
                    try
                    {
                        List<PaText> List_PaText = new List<PaText>();

                        using (DataTable pala_text_index = MySqlConnH.getTable("select * from " + dataBase + "." + "`" + Views.text_index + "`"))
                        {
                            foreach (DataRow Row in pala_text_index.Rows)/* 遍历数据库表行，逐行取得数据 */
                            {
                                PaText PaText = new PaText
                                {
                                    text_id = Convert.ToInt32(Row["text_id"]),
                                    text_type = Convert.ToString(Row["text_type"])
                                };
                                List_PaText.Add(PaText);/* 每次循环完成，将获取到的文本数据添加至文本列表 */
                            }
                        }
                        return List_PaText;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 
                /// </summary>
                public virtual List<PaText> getTextList(string text_type)
                {
                    try
                    {
                        List<PaText> List_PaText = new List<PaText>();

                        DataTable pala_text_index;
                        if (text_type == "page")/* 对文本类型判断以选择查询目标 */
                        { pala_text_index = MySqlConnH.getTable("select * from " + dataBase + "." + "`" + Views.text_index_page + "`"); }
                        else if (text_type == "post")
                        { pala_text_index = MySqlConnH.getTable("select * from " + dataBase + "." + "`" + Views.text_index_post + "`"); }
                        else { return null; }

                        using (pala_text_index)
                        {
                            foreach (DataRow Row in pala_text_index.Rows)
                            {
                                PaText PaText = new PaText
                                {
                                    text_id = Convert.ToInt32(Row["text_id"]),
                                    text_type = Convert.ToString(Row["text_type"])
                                };
                                List_PaText.Add(PaText);
                            }
                        }
                        return List_PaText;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /// <summary>
                /// 
                /// </summary>
                public virtual List<PaText> stepTextList(int row, int rowLength)
                {
                    try
                    {
                        List<PaText> List_PaText = new List<PaText>();

                        string SQL = "select * from " + dataBase + "." + "`" + Views.text_index + "`" + " LIMIT ?row , ?rowLength";
                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?row", parmValue = row },
                            new mysqlParm() { parmName = "?rowLength", parmValue = rowLength }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable pala_text_index = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in pala_text_index.Rows)/* 遍历数据库表行，逐行取得数据 */
                            {
                                PaText PaText = new PaText
                                {
                                    text_id = Convert.ToInt32(Row["text_id"]),
                                    text_type = Convert.ToString(Row["text_type"])
                                };

                                List_PaText.Add(PaText);/* 每次循环完成，将获取到的文本数据添加至文本列表 */
                            }
                        }
                        return List_PaText;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
                /// <summary>
                /// 
                /// </summary>
                public virtual List<PaText> stepTextList(int row, int rowLength, string text_type)
                {
                    try
                    {
                        string SQL;
                        if (text_type == "page")/* 对文本类型判断以选择查询目标 */
                        { SQL = "select * from " + dataBase + "." + "`" + Views.text_index_page + "`" + " LIMIT ?row , ?rowLength"; }
                        else if (text_type == "post")
                        { SQL = "select * from " + dataBase + "." + "`" + Views.text_index_post + "`" + " LIMIT ?row , ?rowLength"; }
                        else { return null; }

                        List<PaText> List_PaText = new List<PaText>();

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>
                        {
                            new mysqlParm() { parmName = "?row", parmValue = row },
                            new mysqlParm() { parmName = "?rowLength", parmValue = rowLength }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))
                        {
                            DataTable pala_text_index = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in pala_text_index.Rows)
                            {
                                PaText PaText = new PaText
                                {
                                    text_id = Convert.ToInt32(Row["text_id"]),
                                    text_type = Convert.ToString(Row["text_type"])
                                };

                                List_PaText.Add(PaText);
                            }
                        }
                        return List_PaText;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /// <summary>
                /// 
                /// </summary>
                public virtual List<PaText> getTextMap()
                {
                    try
                    {
                        string SQL =
                            "SELECT " +
                            "`" + Views.text_main + "`.text_id," +
                            "`" + Views.text_main + "`.text_title" +
                            " FROM " +
                            dataBase + ".`" + Views.text_index + "`," +
                            dataBase + ".`" + Views.text_main + "`" +
                            " WHERE `" +
                            Views.text_main + "`.text_id=`" + Views.text_index + "`.text_id";

                        List<PaText> List_PaText = new List<PaText>();

                        using (DataTable pala_text_index = MySqlConnH.getTable(SQL))
                        {
                            foreach (DataRow Row in pala_text_index.Rows)/* 遍历数据库表行，逐行取得数据 */
                            {
                                PaText PaText = new PaText
                                {
                                    text_id = Convert.ToInt32(Row["text_id"]),
                                    text_type = Convert.ToString(Row["text_title"])
                                };
                                List_PaText.Add(PaText);/* 每次循环完成，将获取到的文本数据添加至文本列表 */
                            }
                        }
                        return List_PaText;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /// <summary>
                /// 文章匹配器
                /// </summary>
                /// <typeparmm name="T">继承自ITextBasic的类型</typeparmm>
                /// <parmm name="obj">继承自ITextBasic的对象实例</parmm>
                /// <returns></returns>
                public virtual List<int> matchText<T>(T obj) where T : ITextBasic
                {
                    if (typeof(T) == typeof(TextTitle))
                    {
                        return matchTextTitle(obj.innerText);
                    }
                    else if (typeof(T) == typeof(TextSummary))
                    {
                        return matchTextSummary(obj.innerText);
                    }
                    else if (typeof(T) == typeof(TextContent))
                    {
                        return matchTextContent(obj.innerText);
                    }
                    else
                    {
                        return null;
                    }
                }
                /// <summary>
                /// 文章标题匹配器
                /// </summary>
                /// <parmm name="str"></parmm>
                /// <returns></returns>
                public virtual List<int> matchTextTitle(string str)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string SQL = "call " + dataBase + ".`match<main.title`( ?str )";
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
                /// 文章概要匹配器
                /// </summary>
                /// <parmm name="str"></parmm>
                /// <returns></returns>
                public virtual List<int> matchTextSummary(string str)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string SQL = "call " + dataBase + ".`match<main.summary`( ?str )";
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
                /// 文章内容匹配器
                /// </summary>
                /// <parmm name="str"></parmm>
                /// <returns></returns>
                public virtual List<int> matchTextContent(string str)
                {
                    try
                    {
                        List<int> List_text_id = new List<int>();

                        string SQL = "call " + dataBase + ".`match<main.content`( ?str )";
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
            /// 文本控制器
            /// 方法注释参见ITextH接口
            /// </summary>
            public class TextH : ITextH
            {
                /// <summary>
                /// 
                /// </summary>
                public string dataBase { get; }
                /// <summary>
                /// 
                /// </summary>
                public PaRoot.Tables Tables { get; }
                /// <summary>
                /// 
                /// </summary>
                public PaRoot.Views Views { get; }
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
                /// 
                /// </summary>
                public virtual PaText getTextMain(int text_id)
                {
                    try
                    {
                        PaText PaText = new PaText();

                        string SQL = "call " + dataBase + ".`get_text>main`( ?text_id )";
                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable pala_text_main = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in pala_text_main.Rows)/* 遍历数据库表以取得唯一的数据行 */
                            {
                                PaText.text_id = Convert.ToInt32(Row["text_id"]);
                                PaText.text_title = Convert.ToString(Row["text_title"]);
                                PaText.text_summary = Convert.ToString(Row["text_summary"]);
                                PaText.text_content = Convert.ToString(Row["text_content"]);
                            }
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
                /// 
                /// </summary>
                public virtual PaText getTextSub(int text_id)
                {
                    try
                    {
                        PaText PaText = new PaText();

                        string SQL = "call " + dataBase + ".`get_text>sub`( ?text_id )";
                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable pala_text_main = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in pala_text_main.Rows)/* 遍历数据库表行，逐行取得数据 */
                            {
                                PaText.text_id = Convert.ToInt32(Row["text_id"]);
                                PaText.text_class = Convert.ToString(Row["text_class"]);
                                PaText.text_editor = Convert.ToString(Row["text_editor"]);

                                PaText.date_created = Convert.ToDateTime(Row["date_created"]);
                                PaText.date_changed = Convert.ToDateTime(Row["date_changed"]);

                                PaText.count_pv = Convert.ToInt32(Row["count_pv"]);
                                PaText.count_comment = Convert.ToInt32(Row["count_comment"]);
                                PaText.count_star = Convert.ToInt32(Row["count_star"]);

                                PaText.tags = Convert.ToString(Row["tags"]);
                                PaText.cover_url = Convert.ToString(Row["cover_url"]);
                            }
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
                /// 
                /// </summary>
                public virtual PaText getTextTitle(int text_id)
                {
                    try
                    {
                        PaText PaText = new PaText();

                        string SQL = "SELECT " +
                            "`" + Views.text_main + "`.text_title" +
                            " FROM " +
                            dataBase + ".`" + Views.text_index + "`," +
                            dataBase + ".`" + Views.text_main + "`" +
                            " WHERE `" +
                            Views.text_index + "`.text_id =`" + Views.text_main + "`.text_id" +
                            " AND `" +
                            Views.text_main + "`.text_id = ?text_id";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                            {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                            };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in table.Rows)/* 遍历数据库表以取得唯一的数据行 */
                            {
                                PaText.text_title = Convert.ToString(Row["text_title"]);
                            }
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
                /// 
                /// </summary>
                public virtual PaText getTextSummary(int text_id)
                {
                    try
                    {
                        PaText PaText = new PaText();

                        string SQL = "SELECT " +
                            "`" + Views.text_main + "`.text_summary" +
                            " FROM " +
                            dataBase + ".`" + Views.text_index + "`," +
                            dataBase + ".`" + Views.text_main + "`" +
                            " WHERE `" +
                            Views.text_index + "`.text_id =`" + Views.text_main + "`.text_id" +
                            " AND `" +
                            Views.text_main + "`.text_id = ?text_id";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                            {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                            };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in table.Rows)/* 遍历数据库表以取得唯一的数据行 */
                            {
                                PaText.text_summary = Convert.ToString(Row["text_summary"]);
                            }
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
                /// 
                /// </summary>
                public virtual PaText getTextContent(int text_id)
                {
                    try
                    {
                        PaText PaText = new PaText();

                        string SQL = "SELECT " +
                            "`" + Views.text_main + "`.text_content" +
                            " FROM " +
                            dataBase + ".`" + Views.text_index + "`," +
                            dataBase + ".`" + Views.text_main + "`" +
                            " WHERE `" +
                            Views.text_index + "`.text_id =`" + Views.text_main + "`.text_id" +
                            " AND `" +
                            Views.text_main + "`.text_id = ?text_id";

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                            {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                            };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable table = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in table.Rows)/* 遍历数据库表以取得唯一的数据行 */
                            {
                                PaText.text_content = Convert.ToString(Row["text_content"]);
                            }
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
                /// 
                /// </summary>
                public virtual int nextTextID(int current_text_id)
                {
                    try
                    {
                        /* 取得比当前 text_id 大的一行，实现对下一条数据的抓取 */
                        string SQL = "SELECT * FROM " + dataBase + "." + Tables.text_index + " WHERE text_id = " +
                        "(SELECT min(text_id) FROM " + dataBase + "." + Tables.text_index + " WHERE text_id> ?current_text_id);";

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
                /// 
                /// </summary>
                public virtual int prevTextID(int current_text_id)
                {
                    try
                    {
                        /* 取得比当前 text_id 小的一行，实现对上一条数据的抓取 */
                        string SQL = "SELECT * FROM " + dataBase + "." + Tables.text_index + " WHERE text_id = " +
                        "(SELECT max(text_id) FROM " + dataBase + "." + Tables.text_index + " WHERE text_id< ?current_text_id);";

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
                /// 
                /// </summary>
                public virtual PaText rdmTextIndex(int excluded_text_id, string text_type)
                {
                    try
                    {
                        string SQL;
                        if (text_type == "page")/* 对文本类型判断以选择查询目标 */
                        { SQL = "call " + dataBase + ".`random_text>index.page`( ?excluded_text_id )"; }
                        else if (text_type == "post")
                        { SQL = "call " + dataBase + ".`random_text>index.post`( ?excluded_text_id )"; }
                        else { return new PaText(); }/* 类型识别失败返回空结构 */

                        PaText PaText = new PaText();

                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?excluded_text_id", parmValue = excluded_text_id }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable pala_text_main = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in pala_text_main.Rows)/* 遍历数据库表以取得唯一的数据行 */
                            {
                                PaText.text_id = Convert.ToInt32(Row["text_id"]);
                                PaText.text_type = Convert.ToString(Row["text_type"]);
                            }
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
            }
        }

        namespace Edit
        {
            /// <summary>
            /// 文本列表控制器
            /// 方法注释参见ITextListH接口
            /// </summary>
            public class TextListH : UI.TextListH
            {
                /// <summary>
                /// 初始化TextListH
                /// </summary>
                /// <parmm name="PaDB">啪啦数据库信息</parmm>
                public TextListH(PaDB PaDB) : base(PaDB) { }

                /// <summary>
                /// 
                /// </summary>
                public override List<PaText> getTextList()
                {
                    try
                    {
                        List<PaText> List_PaText = new List<PaText>();

                        using (DataTable pala_text_index = MySqlConnH.getTable("select * from " + dataBase + "." + Tables.text_index))
                        {
                            foreach (DataRow Row in pala_text_index.Rows)/* 遍历数据库表行，逐行取得数据 */
                            {
                                PaText PaText = new PaText
                                {
                                    text_id = Convert.ToInt32(Row["text_id"]),
                                    text_type = Convert.ToString(Row["text_type"])
                                };
                                List_PaText.Add(PaText);/* 每次循环完成，将获取到的文本数据添加至文本列表 */
                            }
                        }
                        return List_PaText;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }

                /// <summary>
                /// 
                /// </summary>
                public override List<PaText> getTextMap()
                {
                    try
                    {
                        string SQL =
                            "SELECT " +
                            "`" + Views.text_main + "`.text_id," +
                            "`" + Views.text_main + "`.text_title" +
                            " FROM " +
                            dataBase + ".`" + Views.text_index + "`," +
                            dataBase + ".`" + Views.text_main + "`" +
                            " WHERE `" +
                            Views.text_main + "`.text_id=`" + Views.text_index + "`.text_id";

                        List<PaText> List_PaText = new List<PaText>();

                        using (DataTable pala_text_index = MySqlConnH.getTable(SQL))
                        {
                            foreach (DataRow Row in pala_text_index.Rows)/* 遍历数据库表行，逐行取得数据 */
                            {
                                PaText PaText = new PaText
                                {
                                    text_id = Convert.ToInt32(Row["text_id"]),
                                    text_type = Convert.ToString(Row["text_title"])
                                };
                                List_PaText.Add(PaText);/* 每次循环完成，将获取到的文本数据添加至文本列表 */
                            }
                        }
                        return List_PaText;
                    }
                    finally
                    {
                        MySqlConnH.nullHCommand();
                        MySqlConnH.disposeHCommand();
                    }
                }
            }

            /// <summary>
            /// 文本控制器
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
                /// 
                /// </summary>
                public override PaText getTextMain(int text_id)
                {
                    try
                    {
                        PaText PaText = new PaText();

                        string SQL = "SELECT * FROM " + dataBase + "." + Tables.text_main + " WHERE text_id = ?text_id";
                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable pala_text_main = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in pala_text_main.Rows)/* 遍历数据库表以取得唯一的数据行 */
                            {
                                PaText.text_id = Convert.ToInt32(Row["text_id"]);
                                PaText.text_title = Convert.ToString(Row["text_title"]);
                                PaText.text_summary = Convert.ToString(Row["text_summary"]);
                                PaText.text_content = Convert.ToString(Row["text_content"]);
                            }
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
                /// 
                /// </summary>
                public override PaText getTextSub(int text_id)
                {
                    try
                    {
                        PaText PaText = new PaText();

                        string SQL = "SELECT * FROM " + dataBase + "." + Tables.text_sub + " WHERE text_id = ?text_id";
                        List<mysqlParm> List_mysqlParm = new List<mysqlParm>/* 为参数化查询添加元素 */
                        {
                            new mysqlParm() { parmName = "?text_id", parmValue = text_id }
                        };

                        using (MySqlCommand MySqlCommand = MySqlConnH.parmQueryCmd(SQL, List_mysqlParm))/* 参数化查询 */
                        {
                            DataTable pala_text_main = MySqlConnH.getTable(MySqlCommand);

                            foreach (DataRow Row in pala_text_main.Rows)/* 遍历数据库表行，逐行取得数据 */
                            {
                                PaText.text_id = Convert.ToInt32(Row["text_id"]);
                                PaText.text_class = Convert.ToString(Row["text_class"]);
                                PaText.text_editor = Convert.ToString(Row["text_editor"]);

                                PaText.date_created = Convert.ToDateTime(Row["date_created"]);
                                PaText.date_changed = Convert.ToDateTime(Row["date_changed"]);

                                PaText.count_pv = Convert.ToInt32(Row["count_pv"]);
                                PaText.count_comment = Convert.ToInt32(Row["count_comment"]);
                                PaText.count_star = Convert.ToInt32(Row["count_star"]);

                                PaText.tags = Convert.ToString(Row["tags"]);
                                PaText.cover_url = Convert.ToString(Row["cover_url"]);
                            }
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
                /// 文章更改器
                /// </summary>
                /// <typeparam name="T">继承自ITextBasic的类型</typeparam>
                /// <param name="obj">继承自ITextBasic的对象实例</param>
                /// <returns></returns>
                public bool updateTextBasic<T>(T obj) where T : ITextBasic
                {
                    if (typeof(T) == typeof(TextTitle))
                    {
                        return updateTextTitle(obj.text_id,obj.innerText);
                    }
                    else if (typeof(T) == typeof(TextSummary))
                    {
                        return updateTextSummary(obj.text_id, obj.innerText);
                    }
                    else if (typeof(T) == typeof(TextContent))
                    {
                        return updateTextContent(obj.text_id, obj.innerText);
                    }
                    else
                    {
                        return false;
                    }
                }
                /// <summary>
                /// 更改文章标题
                /// </summary>
                /// <param name="text_id">文章序列号</param>
                /// <param name="text_title">新标题</param>
                /// <returns></returns>
                public bool updateTextTitle(int text_id, string text_title)
                {
                    /* 待更新 */
                    return false;
                }
                /// <summary>
                /// 更改文章概要
                /// </summary>
                /// <param name="text_id">文章序列号</param>
                /// <param name="text_summary">新概要</param>
                /// <returns></returns>
                public bool updateTextSummary(int text_id, string text_summary)
                {
                    /* 待更新 */
                    return false;
                }
                /// <summary>
                /// 更改文章内容
                /// </summary>
                /// <param name="text_id">文章序列号</param>
                /// <param name="text_content">新内容</param>
                /// <returns></returns>
                public bool updateTextContent(int text_id, string text_content)
                {
                    /* 待更新 */
                    return false;
                }


                /// <summary>
                /// 设置count_pv
                /// </summary>
                /// <parmm name="text_id">文本ID</parmm>
                /// <parmm name="value">值</parmm>
                /// <returns></returns>
                public bool update_count_pv(int text_id, int value)
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
                /// 设置count_star
                /// </summary>
                /// <parmm name="text_id">文本ID</parmm>
                /// <parmm name="value">值</parmm>
                /// <returns></returns>
                public bool update_count_star(int text_id, int value)
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
        /// 基本函数
        /// </summary>
        public class PaBasic
        {
            /// <summary>
            /// 合并主副表PaText数据
            /// </summary>
            /// <parmm name="TextMain">主表PaText</parmm>
            /// <parmm name="TextSub">副表PaText</parmm>
            /// <returns></returns>
            public static PaText fill(PaText TextMain, PaText TextSub)
            {
                return new PaText
                {
                    /* 主表数据填充 */
                    text_id = TextMain.text_id,
                    text_title = TextMain.text_title,
                    text_summary = TextMain.text_summary,
                    text_content = TextMain.text_content,

                    /* 副表数据填充 */
                    text_class = TextSub.text_class,
                    text_editor = TextSub.text_editor,
                    date_created = TextSub.date_created,
                    date_changed = TextSub.date_changed,
                    count_pv = TextSub.count_pv,
                    count_comment = TextSub.count_comment,
                    count_star = TextSub.count_star,
                    tags = TextSub.tags,
                    cover_url = TextSub.cover_url,
                };
            }
        }
    }
}