using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;/* MySqlDB */
using System.Data;/* ADO.NET */
using System.Web.UI;
using System.Web;

using sukiUnit;
using LibStructs;

namespace jarwUnit
{
    namespace pilipala
    {
        interface IPalaRoot
        {
            /// <summary>
            /// Pala数据表所在数据库
            /// </summary>
            string dataBase { get; }
            /// <summary>
            /// 文本表
            /// </summary>
            PalaRoot.Tables Tables { get; }
            /// <summary>
            /// 文本视图
            /// </summary>
            PalaRoot.Views Views { get; }
            /// <summary>
            /// 数据库管理器实例
            /// </summary>
            MySqlConnH MySqlConnH { get; set; }
        }

        /// <summary>
        /// 啪啦权限控制器
        /// </summary>
        public class PalaRoot
        {
            public string dataBase { get; }
            public Tables objTables { get; }
            public Views objViews { get; }
            public MySqlConnH MySqlConnH { get; set; }
            int root_id;/* 权限ID */

            /// <summary>
            /// 初始化PalaRoot
            /// </summary>
            /// <param name="root_id">权限ID</param>
            /// <param name="PalaDB">啪啦数据库信息</param>
            public PalaRoot(int root_id, PalaDB PalaDB)
            {
                this.root_id = root_id;
                objTables = PalaDB.Tables;
                objViews = PalaDB.Views;
                dataBase = PalaDB.dataBase;
                MySqlConnH = PalaDB.MySqlConnH;
            }

            /// <summary>
            /// 获得权限数据
            /// </summary>
            /// <returns></returns>
            public PaRoot getRoot()
            {
                try
                {
                    PaRoot PaRoot = new PaRoot();/* 定义权限数据 */

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
                            PaRoot.root_id = Convert.ToInt32(Row["root_id"]);
                            PaRoot.root_definer = Convert.ToString(Row["root_definer"]);
                            PaRoot.site_debug = Convert.ToBoolean(Row["site_debug"]);
                            PaRoot.site_access = Convert.ToBoolean(Row["site_access"]);
                            PaRoot.site_url = Convert.ToString(Row["site_url"]);
                            PaRoot.site_title = Convert.ToString(Row["site_title"]);
                            PaRoot.site_summary = Convert.ToString(Row["site_summary"]);
                        }
                        return PaRoot;
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
                public static string text_index_page = "view_text>index.page";
                public static string text_index_post = "view_text>index.post";
                public static string text_main = "view_text>main";
                public static string text_sub = "view_text>sub";
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

                public string root { get; set; }
                public string text_index { get; set; }
                public string text_main { get; set; }
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
                public string text_index_page { get; set; }
                public string text_index_post { get; set; }
                public string text_main { get; set; }
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

        namespace UI
        {
            /// <summary>
            /// 文本列表控制接口
            /// </summary>
            interface ITextListH : IPalaRoot
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
            interface ITextH : IPalaRoot
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
                int prviousTextID(int current_text_id);

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
            /// </summary>
            public class TextListH : ITextListH
            {
                public string dataBase { get; }
                public PalaRoot.Tables Tables { get; }
                public PalaRoot.Views Views { get; }
                public MySqlConnH MySqlConnH { get; set; }

                /// <summary>
                /// 初始化TextListH
                /// </summary>
                /// <param name="PalaDB">啪啦数据库信息</param>
                public TextListH(PalaDB PalaDB)
                {
                    dataBase = PalaDB.dataBase;
                    Tables = PalaDB.Tables;
                    Views = PalaDB.Views;
                    MySqlConnH = PalaDB.MySqlConnH;
                }

                public List<PaText> getTextList()
                {
                    try
                    {
                        List<PaText> List_PaText = new List<PaText>();/* 定义文本列表 */

                        using (DataTable pala_text_index = MySqlConnH.getTable("select * from " + dataBase + "." + "`" + Tables.text_index + "`"))
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
                public List<PaText> getTextList(string text_type)
                {
                    try
                    {
                        List<PaText> List_PaText = new List<PaText>();/* 定义文本列表 */

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

                public List<PaText> stepTextList(int row, int rowLength)
                {
                    try
                    {
                        List<PaText> List_PaText = new List<PaText>();/* 定义文本列表 */

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

                        List<PaText> List_PaText = new List<PaText>();/* 定义文本列表 */

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
            }
            /// <summary>
            /// 文本控制器
            /// </summary>
            public class TextH : ITextH
            {
                public string dataBase { get; }
                public PalaRoot.Tables Tables { get; }
                public PalaRoot.Views Views { get; }
                public MySqlConnH MySqlConnH { get; set; }

                /// <summary>
                /// 初始化TextH
                /// </summary>
                /// <param name="PalaDB">啪啦数据库信息</param>
                public TextH(PalaDB PalaDB)
                {
                    dataBase = PalaDB.dataBase;
                    Tables = PalaDB.Tables;
                    Views = PalaDB.Views;
                    MySqlConnH = PalaDB.MySqlConnH;
                }

                public PaText getTextMain(int text_id)
                {
                    PaText PaText = new PaText();/* 定义文本列表 */

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
                public PaText getTextSub(int text_id)
                {
                    try
                    {
                        PaText PaText = new PaText();/* 定义文本列表 */

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
                                PaText.count_like = Convert.ToInt32(Row["count_like"]);

                                PaText.tags = Convert.ToString(Row["tags"]);
                                PaText.cover_url = Convert.ToString(Row["cover_url"]);
                                PaText.strip_color = Convert.ToString(Row["strip_color"]);
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

                public int nextTextID(int current_text_id)
                {
                    /* 取得比当前 text_id 大的一行，实现对下一条数据的抓取 */
                    /* text_mode='onshow' 文本显示开启，防止遍历到数据库内不公开的 text_id */
                    string SQL = "SELECT * FROM " + dataBase + "`" + Tables.text_index + "`" + " WHERE text_id = " +
                        "(SELECT min(text_id) FROM " + dataBase + "`" + Tables.text_index + "`" + " WHERE text_id> ?current_text_id and text_mode='onshow');";

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
                public int prviousTextID(int current_text_id)
                {
                    /* 取得比当前 text_id 小的一行，实现对上一条数据的抓取 */
                    /* text_mode='onshow' 文本显示开启，防止遍历到数据库内不公开的 text_id */
                    string SQL = "SELECT * FROM " + dataBase + "`" + Tables.text_index + "`" + " WHERE text_id = " +
                        "(SELECT min(text_id) FROM " + dataBase + "`" + Tables.text_index + "`" + " WHERE text_id< ?current_text_id and text_mode='onshow');";

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

                        PaText PaText = new PaText();/* 定义文本列表 */

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
                        count_like = TextSub.count_like,
                        tags = TextSub.tags,
                        cover_url = TextSub.cover_url,
                        strip_color = TextSub.strip_color
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
                /// 设置count_like
                /// </summary>
                /// <param name="text_id">文本ID</param>
                /// <param name="value">值</param>
                /// <returns></returns>
                public bool update_count_like(int text_id, int value)
                {
                    try
                    {
                        Posi Posi = new Posi
                        {
                            dataBase = dataBase,
                            table = Tables.text_sub,
                            whereColumn = "text_id",
                            targetColumn = "count_like"
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

        /// <summary>
        /// Cookie控制器
        /// </summary>
        public class CookieHandler : Page
        {
            /// <summary>
            /// 判断Cookie对象是否存在
            /// </summary>
            /// <param name="CookieName">被判断Cookie对象的名称</param>
            /// <returns>存在返回true，反之false</returns>
            public bool isCookiesExist(string CookieName)
            {
                if (HttpContext.Current.Request.Cookies[CookieName] == null)//如果Cookie对象为null（不存在）
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            /// <summary>
            /// 判断Cookie对象是否存在（重载二：判断索引是否存在）
            /// </summary>
            /// <param name="CookieName">被判断Cookie对象的名称</param>
            /// <param name="keyName">索引名，属于被判断的Cookie</param>
            /// <returns>存在返回true，反之false</returns>
            public bool isCookiesExist(string CookieName, string keyName)
            {
                if (isCookiesExist(CookieName) == true)//如果Cookie对象存在
                {
                    if (HttpContext.Current.Request.Cookies[CookieName][keyName].ToString() != "")//如果Cookie中的keyName索引键值为空字符串
                    {
                        return false;//不存在
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }

            }

            /// <summary>
            /// 读取Cookie对象到指定类型
            /// </summary>
            /// <typeparam name="T">指定泛型</typeparam>
            /// <param name="CookieName">被读取Cookie对象名</param>
            /// <returns>返回泛型值，读取失败则返回泛型默认值</returns>
            public T cookie<T>(string CookieName)
            {
                if (isCookiesExist(CookieName) == true)//如果Cookie对象存在
                {
                    if (HttpContext.Current.Request.Cookies[CookieName].Value == "")
                    {
                        return default(T);//如果Cookie对象值为空字符串（null），返回泛型默认值
                    }
                    else
                    {
                        if (HttpContext.Current.Request.Cookies[CookieName].Value == null)
                        {
                            return default(T);
                        }
                        else
                        {
                            //如果Cookie对象的索引值不为null，转换为泛型返回
                            return (T)Convert.ChangeType(HttpContext.Current.Request.Cookies[CookieName].Value, typeof(T));
                        }
                    }
                }
                else
                {
                    return default(T);
                }

            }
            /// <summary>
            /// 读取Cookie对象的指定索引值到指定类型
            /// </summary>
            /// <typeparam name="T">指定泛型</typeparam>
            /// <param name="CookieName">被读取Cookie对象名</param>
            /// <param name="keyName">索引名，属于当前Cookie对象</param>
            /// <returns>返回泛型值，读取失败则返回泛型默认值</returns>
            public T cookie<T>(string CookieName, string keyName)
            {
                if (isCookiesExist(CookieName, keyName) == true)
                {
                    //如果索引键值不为空字符串，转换为泛型返回
                    return (T)Convert.ChangeType(HttpContext.Current.Request.Cookies[CookieName][keyName].ToString(), typeof(T));
                }
                else
                {
                    return default(T);
                }

            }

            /// <summary>
            /// 设置Cookie值
            /// </summary>
            /// <param name="CookieName">Cookie名，承担该操作</param>
            /// <param name="value">设置值</param>
            /// <returns>设置成功返回true，反之false</returns>
            public bool setCookie(string CookieName, object value)
            {
                HttpContext.Current.Response.Cookies[CookieName].Value = value.ToString();
                return true;
            }
            /// <summary>
            /// 设置Cookie值（重载二：索引设置）
            /// </summary>
            /// <param name="CookieName">Cookie名，承担该操作</param>
            /// <param name="keyName">索引名，属于承担该操作的Cookie</param>
            /// <param name="value">设置值</param>
            /// <returns>设置成功返回true，反之false</returns>
            public bool setCookie(string CookieName, string keyName, object value)
            {
                HttpContext.Current.Response.Cookies[CookieName][keyName] = value.ToString();
                return true;
            }
        }

        /// <summary>
        /// Session控制器
        /// </summary>
        public class SessionHandler : Page
        {
            /// <summary>
            /// 读取Session对象到指定类型
            /// </summary>
            /// <typeparam name="T">指定泛型</typeparam>
            /// <param name="varName">变量名，属于Session对象，承担该操作</param>
            /// <returns>返回泛型值，读取失败则返回泛型默认值</returns>
            public T session<T>(string varName)
            {
                //测试表明，Session对象始终存在，并且不为null，所以可以不加判断直接转换
                return (T)Convert.ChangeType(HttpContext.Current.Session[varName], typeof(T));//转换为自定义类型
            }

            /// <summary>
            /// 设置Session对象的变量值
            /// </summary>
            /// <param name="varName">变量名，属于Session对象，承担该操作</param>
            /// <param name="value">设置值</param>
            /// <returns>设置成功返回true，反之false</returns>
            public bool setSession(string varName, object value)
            {
                HttpContext.Current.Session[varName] = value;
                return true;
            }
        }
    }
}