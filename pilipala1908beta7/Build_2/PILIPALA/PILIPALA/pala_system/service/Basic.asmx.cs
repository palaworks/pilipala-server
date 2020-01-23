using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Web.Configuration;

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
            if (getTextSummary(text_id) == "")
            {
                return htmlFilter(getTextContent(text_id, 120));
            }
            else
            {
                return getTextSummary(text_id);
            }
        }

        public static PaText fill(PaText TextMain, PaText TextSub)
        {
            return PaFn.fill(TextMain, TextSub);
        }
        public static string htmlFilter(string str)
        {
            return PaFn.htmlFilter(str);
        }
        public static string timeFromNow(DateTime dateTime)
        {
            return PaFn.timeFromNow(dateTime);
        }

        public static string toMD5(string str)
        {
            return BasicMethod.toMD5(str);
        }
        #endregion
    }
}
