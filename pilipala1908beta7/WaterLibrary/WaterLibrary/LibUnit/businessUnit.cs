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
            /// <param name="root_id">权限ID</param>
            /// <param name="PaDB">啪啦数据库信息</param>
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
                    List<Para> List_Para = new List<Para>/* 为参数化查询添加元素 */
                    {
                        new Para() { paraName = "?root_id", paraValue = root_id }
                    };

                    using (MySqlCommand MySqlCommand = MySqlConnH.paraQueryCmd(SQL, List_Para))/* 参数化查询 */
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
                /// <param name="root"></param>
                /// <param name="text_index"></param>
                /// <param name="text_main"></param>
                /// <param name="text_sub"></param>
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
                /// <param name="text_main"></param>
                /// <param name="text_sub"></param>
                /// <param name="text_index_post"></param>
                /// <param name="text_index_page"></param>
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
            /// <param name="text_type">自定义文本类型</param>
            /// <returns></returns>
            List<PaText> getTextList(string text_type);

            /// <summary>
            /// 步进式获得文本索引表（重载一::得到全文本列表）
            /// </summary>
            /// <param name="row">步进起始行（包含该行）</param>
            /// <param name="rowLength">加载行数</param>
            /// <returns></returns>
            List<PaText> stepTextList(int row, int rowLength);
            /// <summary>
            /// 步进式获得文本索引表（重载一::得到指定类型的文本列表）
            /// </summary>
            /// <param name="row">步进起始行（包含该行）</param>
            /// <param name="rowLength">加载行数</param>
            /// <param name="text_type">自定义文本类型</param>
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
            /// <param name="text_id">文本ID</param>
            /// <returns></returns>
            PaText getTextMain(int text_id);
            /// <summary>
            /// 获得指定ID的文本副表行
            /// </summary>
            /// <param name="text_id">文本ID</param>
            /// <returns></returns>
            PaText getTextSub(int text_id);

            /// <summary>
            /// 返回当前文本 text_id 的下一个文本 text_id
            /// </summary>
            /// <param name="current_text_id">当前文本序列号</param>
            /// <returns>错误返回 -1</returns>
            int nextTextID(int current_text_id);
            /// <summary>
            /// 返回当前文本 text_id 的上一个文本 text_id
            /// </summary>
            /// <param name="current_text_id">当前文本序列号</param>
            /// <returns>错误返回 -1</returns>
            int prevTextID(int current_text_id);

            /// <summary>
            /// 随机获得文本索引表行
            /// </summary>
            /// <param name="excluded_text_id">不参与随机操作的文本ID</param>
            /// <param name="text_type">自定义文本类型</param>
            /// <returns></returns>
            PaText rdmTextIndex(int excluded_text_id, string text_type);
        }

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
            /// <param name="PaDB">啪啦数据库信息</param>
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
            public List<PaText> getTextList()
            {
                try
                {
                    string SQL =
                        "SELECT " +
                        Tables.text_main + ".text_id," +
                        Tables.text_main + ".text_title" +
                        " FROM " +
                        dataBase + "." + Tables.text_index + "," +
                        dataBase + "." + Tables.text_main +
                        " WHERE " +
                        Tables.text_main + ".text_id=" + Tables.text_index + ".text_id" +
                        " AND " +
                        Tables.text_index + ".text_mode='onshow'";

                    List<PaText> List_PaText = new List<PaText>();

                    using (DataTable pala_text_index = MySqlConnH.getTable(SQL))
                    {
                        foreach (DataRow Row in pala_text_index.Rows)/* 遍历数据库表行，逐行取得数据 */
                        {
                            PaText PaText = new PaText
                            {
                                text_id = Convert.ToInt32(Row["text_id"]),
                                text_mode = Convert.ToString(Row["text_mode"]),
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
            public List<PaText> getTextList(string text_type)
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
                        foreach (DataRow Row in pala_text_index.Rows)/* 遍历数据库表行，逐行取得数据 */
                        {
                            PaText PaText = new PaText
                            {
                                text_id = Convert.ToInt32(Row["text_id"]),
                                text_mode = Convert.ToString(Row["text_mode"]),
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
            public List<PaText> stepTextList(int row, int rowLength)
            {
                try
                {
                    List<PaText> List_PaText = new List<PaText>();

                    string SQL = "select * from " + dataBase + "." + "`" + Tables.text_index + "`" + " LIMIT ?row , ?rowLength";
                    List<Para> List_Para = new List<Para>/* 为参数化查询添加元素 */
                        {
                            new Para() { paraName = "?row", paraValue = row },
                            new Para() { paraName = "?rowLength", paraValue = rowLength }
                        };

                    using (MySqlCommand MySqlCommand = MySqlConnH.paraQueryCmd(SQL, List_Para))/* 参数化查询 */
                    {
                        DataTable pala_text_index = MySqlConnH.getTable(MySqlCommand);

                        foreach (DataRow Row in pala_text_index.Rows)/* 遍历数据库表行，逐行取得数据 */
                        {
                            PaText PaText = new PaText
                            {
                                text_id = Convert.ToInt32(Row["text_id"]),
                                text_mode = Convert.ToString(Row["text_mode"]),
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
            public List<PaText> stepTextList(int row, int rowLength, string text_type)
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

                    List<Para> List_Para = new List<Para>
                        {
                            new Para() { paraName = "?row", paraValue = row },
                            new Para() { paraName = "?rowLength", paraValue = rowLength }
                        };

                    using (MySqlCommand MySqlCommand = MySqlConnH.paraQueryCmd(SQL, List_Para))
                    {
                        DataTable pala_text_index = MySqlConnH.getTable(MySqlCommand);

                        foreach (DataRow Row in pala_text_index.Rows)
                        {
                            PaText PaText = new PaText
                            {
                                text_id = Convert.ToInt32(Row["text_id"]),
                                text_mode = Convert.ToString(Row["text_mode"]),
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
            public List<PaText> getTextMap()
            {
                try
                {
                    List<PaText> List_PaText = new List<PaText>();

                    string SQL = "select from " + dataBase + ".`match<main.title`( ?str )";

                    using (DataTable table = MySqlConnH.getTable(SQL))
                    {
                        foreach (DataRow Row in table.Rows)
                        {
                            PaText PaText = new PaText
                            {
                                text_id = Convert.ToInt32(Row["text_id"]),
                                text_title = Convert.ToString(Row["text_title"])
                            };

                            List_PaText.Add(PaText);
                        }
                    }
                    return List_PaText;
                }
                finally
                {
                    MySqlConnH.closeHConnection();
                    MySqlConnH.nullHCommand();
                    MySqlConnH.disposeHCommand();
                }
            }

            /// <summary>
            /// 文章匹配器
            /// </summary>
            /// <typeparam name="T">对象类型</typeparam>
            /// <param name="obj">继承自ITextBasic的对象实例</param>
            /// <returns></returns>
            public List<int> matchText<T>(T obj) where T : ITextBasic
            {
                if (typeof(T) == typeof(TextTitle))
                {
                    return matchTextTitle(obj.innerText);
                }
                if (typeof(T) == typeof(TextSummary))
                {
                    return matchTextSummary(obj.innerText);
                }
                if (typeof(T) == typeof(TextContent))
                {
                    return matchTextContent(obj.innerText);
                }
                return null;
            }
            /// <summary>
            /// 文章标题匹配器
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public List<int> matchTextTitle(string str)
            {
                try
                {
                    List<int> List_text_id = new List<int>();

                    string SQL = "call " + dataBase + ".`match<main.title`( ?str )";
                    List<Para> List_Para = new List<Para>
                    {
                        new Para() { paraName = "?str", paraValue = str }
                    };

                    using (MySqlCommand MySqlCommand = MySqlConnH.paraQueryCmd(SQL, List_Para))/* 参数化查询 */
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
            /// <param name="str"></param>
            /// <returns></returns>
            public List<int> matchTextSummary(string str)
            {
                try
                {
                    List<int> List_text_id = new List<int>();

                    string SQL = "call " + dataBase + ".`match<main.summary`( ?str )";
                    List<Para> List_Para = new List<Para>
                    {
                        new Para() { paraName = "?str", paraValue = str }
                    };

                    using (MySqlCommand MySqlCommand = MySqlConnH.paraQueryCmd(SQL, List_Para))/* 参数化查询 */
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
            /// <param name="str"></param>
            /// <returns></returns>
            public List<int> matchTextContent(string str)
            {
                try
                {
                    List<int> List_text_id = new List<int>();

                    string SQL = "call " + dataBase + ".`match<main.content`( ?str )";
                    List<Para> List_Para = new List<Para>
                    {
                        new Para() { paraName = "?str", paraValue = str }
                    };

                    using (MySqlCommand MySqlCommand = MySqlConnH.paraQueryCmd(SQL, List_Para))/* 参数化查询 */
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
            /// <param name="PaDB">啪啦数据库信息</param>
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
            public PaText getTextMain(int text_id)
            {
                PaText PaText = new PaText();

                string SQL = "call " + dataBase + ".`get_text>main`( ?text_id )";
                List<Para> List_Para = new List<Para>/* 为参数化查询添加元素 */
                        {
                            new Para() { paraName = "?text_id", paraValue = text_id }
                        };

                using (MySqlCommand MySqlCommand = MySqlConnH.paraQueryCmd(SQL, List_Para))/* 参数化查询 */
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
            /// <summary>
            /// 
            /// </summary>
            public PaText getTextSub(int text_id)
            {
                try
                {
                    PaText PaText = new PaText();

                    string SQL = "call " + dataBase + ".`get_text>sub`( ?text_id )";
                    List<Para> List_Para = new List<Para>/* 为参数化查询添加元素 */
                        {
                            new Para() { paraName = "?text_id", paraValue = text_id }
                        };

                    using (MySqlCommand MySqlCommand = MySqlConnH.paraQueryCmd(SQL, List_Para))/* 参数化查询 */
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
            public PaText getTextTitle(int text_id)
            {

            }
            /// <summary>
            /// 
            /// </summary>
            public PaText getTextSummary(int text_id)
            {

            }
            /// <summary>
            /// 
            /// </summary>
            public PaText getTextContent(int text_id)
            {

            }

            /// <summary>
            /// 
            /// </summary>
            public int nextTextID(int current_text_id)
            {
                /* 取得比当前 text_id 大的一行，实现对下一条数据的抓取 */
                /* text_mode='onshow' 文本显示开启，防止遍历到数据库内不公开的 text_id */
                string SQL = "SELECT * FROM " + dataBase + ".`" + Tables.text_index + "`" + " WHERE text_id = " +
                    "(SELECT min(text_id) FROM " + dataBase + ".`" + Tables.text_index + "`" + " WHERE text_id> ?current_text_id and text_mode='onshow');";

                List<Para> List_Para = new List<Para>/* 为参数化查询添加元素 */
                        {
                            new Para() { paraName = "?current_text_id", paraValue = current_text_id }
                        };

                using (MySqlCommand MySqlCommand = MySqlConnH.paraQueryCmd(SQL, List_Para))/* 参数化查询 */
                {
                    foreach (DataRow Row in MySqlConnH.getTable(MySqlCommand).Rows)/* 遍历查询到的表以取得唯一的数据行 */
                    {
                        return Convert.ToInt32(Row["text_id"]);/* 直接返回该text_id */
                    }
                    return -1;/* 错误返回-1 */
                }
            }
            /// <summary>
            /// 
            /// </summary>
            public int prevTextID(int current_text_id)
            {
                /* 取得比当前 text_id 小的一行，实现对上一条数据的抓取 */
                /* text_mode='onshow' 文本显示开启，防止遍历到数据库内不公开的 text_id */
                string SQL = "SELECT * FROM " + dataBase + ".`" + Tables.text_index + "`" + " WHERE text_id = " +
                    "(SELECT max(text_id) FROM " + dataBase + ".`" + Tables.text_index + "`" + " WHERE text_id< ?current_text_id and text_mode='onshow');";

                List<Para> List_Para = new List<Para>/* 为参数化查询添加元素 */
                        {
                            new Para() { paraName = "?current_text_id", paraValue = current_text_id }
                        };

                using (MySqlCommand MySqlCommand = MySqlConnH.paraQueryCmd(SQL, List_Para))/* 参数化查询 */
                {
                    foreach (DataRow Row in MySqlConnH.getTable(MySqlCommand).Rows)/* 遍历查询到的表以取得唯一的数据行 */
                    {
                        return Convert.ToInt32(Row["text_id"]);/* 直接返回该text_id */
                    }
                    return -1;/* 错误返回-1 */
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public PaText rdmTextIndex(int excluded_text_id, string text_type)
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

                    List<Para> List_Para = new List<Para>/* 为参数化查询添加元素 */
                        {
                            new Para() { paraName = "?excluded_text_id", paraValue = excluded_text_id }
                        };

                    using (MySqlCommand MySqlCommand = MySqlConnH.paraQueryCmd(SQL, List_Para))/* 参数化查询 */
                    {
                        DataTable pala_text_main = MySqlConnH.getTable(MySqlCommand);

                        foreach (DataRow Row in pala_text_main.Rows)/* 遍历数据库表以取得唯一的数据行 */
                        {
                            PaText.text_id = Convert.ToInt32(Row["text_id"]);
                            PaText.text_mode = Convert.ToString(Row["text_mode"]);
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

            /// <summary>
            /// 合并主副表PaText数据
            /// </summary>
            /// <param name="TextMain">主表PaText</param>
            /// <param name="TextSub">副表PaText</param>
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

            /// <summary>
            /// 设置count_pv
            /// </summary>
            /// <param name="text_id">文本ID</param>
            /// <param name="value">值</param>
            /// <returns></returns>
            public bool update_count_pv(int text_id, int value)
            {
                try
                {
                    //初始化目标行定位数据
                    Posi Posi = new Posi
                    {
                        dataBase = dataBase,
                        table = Tables.text_sub,
                        whereColumn = "text_id",
                        targetColumn = "count_pv"
                    };
                    return MySqlConnH.setColumnValue(Posi, text_id.ToString(), value.ToString());
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
            /// <param name="text_id">文本ID</param>
            /// <param name="value">值</param>
            /// <returns></returns>
            public bool update_count_star(int text_id, int value)
            {
                try
                {
                    Posi Posi = new Posi
                    {
                        dataBase = dataBase,
                        table = Tables.text_sub,
                        whereColumn = "text_id",
                        targetColumn = "count_star"
                    };
                    return MySqlConnH.setColumnValue(Posi, text_id.ToString(), value.ToString());
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
}