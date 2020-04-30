using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Web.Configuration;
using System.Text.RegularExpressions;

using LibStruct.MySql;
using LibStruct.pilipala;
using dataUnit;
using basicUnit;
using businessUnit.pilipala;
using businessUnit.pilipala.UI;

namespace PILIPALA.pala_system.service
{
    /// <summary>
    /// Basic 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Basic : System.Web.Services.WebService
    {
        /* 数据库 */
        private mysqlConn mysqlConn;
        private PaDB paDB;

        /* 文章列表控制器 */
        private TextListH TextListH;
        /* 文章控制器 */
        private TextH TextH;

        public Basic()
        {
            paDB.MySqlConnH = new MySqlConnH();

            /* 初始化数据库信息 */
            mysqlConn.user = WebConfigurationManager.AppSettings["mysql_user"];
            mysqlConn.password = WebConfigurationManager.AppSettings["mysql_password"];
            mysqlConn.dataSource = WebConfigurationManager.AppSettings["mysql_dataSource"];
            mysqlConn.port = WebConfigurationManager.AppSettings["mysql_port"];

            /* 初始化数据库连接 */
            paDB.MySqlConnH.start(mysqlConn);
            paDB.dataBase = WebConfigurationManager.AppSettings["mysql_dataBase"];


            paDB.Tables = PaRoot.defaultTables();/* 以默认值确定表名 */
            paDB.Views = PaRoot.defaultViews();/* 以默认值确定视图名 */

            TextListH = new businessUnit.pilipala.Edit.TextListH(paDB);
            TextH = new businessUnit.pilipala.Edit.TextH(paDB);

            //如果使用设计的组件，请取消注释以下行 
            //InitializeComponent(); 
        }
        #region TextListH

        public List<int> getTextIDList()
        {
            return TextListH.getTextIDList();
        }

        public List<int> stepTextIDList(int row, int rowLength)
        {
            return TextListH.stepTextIDList(row, rowLength);
        }
        public List<int> stepTextIDList(int row, int rowLength, string text_type)
        {
            return TextListH.stepTextIDList(row, rowLength, text_type);
        }

        public List<int> getTextIDList(List<TextArchiv> text_archiv)
        {
            return TextListH.getTextIDList(text_archiv);
        }

        #endregion

        #region TextH
        public PaText getTextIndex(int text_id)
        {
            return TextH.getTextIndex(text_id);
        }
        public PaText getTextMain(int text_id)
        {
            return TextH.getTextMain(text_id);
        }
        public PaText getTextSub(int text_id)
        {
            return TextH.getTextSub(text_id);
        }

        public string getTextTitle(int text_id)
        {
            return TextH.getTextTitle(text_id);
        }
        public string getTextSummary(int text_id)
        {
            return TextH.getTextSummary(text_id);
        }
        public string getTextContent(int text_id)
        {
            return TextH.getTextContent(text_id);
        }

        public string getTextTitle(int text_id, int length)
        {
            return TextH.getTextTitle(text_id, length);
        }
        public string getTextSummary(int text_id, int length)
        {
            return TextH.getTextSummary(text_id, length);
        }
        public string getTextContent(int text_id, int length)
        {
            return TextH.getTextContent(text_id, length);
        }



        public int nextTextID(int current_text_id)
        {
            return TextH.nextTextID(current_text_id);
        }
        public int prevTextID(int current_text_id)
        {
            return TextH.prevTextID(current_text_id);
        }

        [WebMethod]
        public bool update_countPv(int text_id, int value)
        {
            return TextH.update_countPv(text_id, value);
        }
        [WebMethod]
        public bool update_countStar(int text_id, int value)
        {
            return TextH.update_countStar(text_id, value);
        }

        #endregion

        #region PalaFn
        /// <summary>
        /// 生成概要
        /// </summary>
        /// <param name="text_id">文章id</param>
        /// <param name="length">如果文章概要不存在，取文章前一段内容的长度</param>
        /// <returns></returns>
        public string doSummary(int text_id, int length)
        {
            return htmlFilter(getTextContent(text_id, length));
        }

        public static PaText fill(PaText TextIndex, PaText TextMain, PaText TextSub)
        {
            return PaFn.fill(TextIndex, TextMain, TextSub);
        }

        public static string toMD5(string str)
        {
            return BasicMethod.toMD5(str);
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

            /*把MKD中的#符号去掉*/
            str = str.Replace("#", "");

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
            if (ts.Days >= 50 && ts.Days < 80)
            {
                return "两月前";
            }
            else
            if (ts.Days >= 28 && ts.Days < 50)
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
                return "两周前";
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
                    case 2: return "两天前";
                    case 3: return "三天前";
                    case 4: return "四天前";
                    case 5: return "五天前";
                    case 6: return "六天前";
                    default: return null;
                }
            }
        }
        #endregion
    }
}
