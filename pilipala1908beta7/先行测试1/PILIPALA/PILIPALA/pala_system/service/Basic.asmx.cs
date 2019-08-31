using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

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

            /*
            mysqlConn.user = "pala_database_user";
            mysqlConn.password = "pala_database_user_password";
            mysqlConn.dataSource = "localhost";
            mysqlConn.port = "3306";
            paDB.MySqlConnH.start(mysqlConn);
            paDB.dataBase = "pala_db";//测试库
            */

            mysqlConn.user = "thaumy0tdymy";
            mysqlConn.password = "177BDE5";
            mysqlConn.dataSource = "localhost";
            mysqlConn.port = "3306";
            paDB.MySqlConnH.start(mysqlConn);
            paDB.dataBase = "thaumy0tdymy";//生产库
            

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
        public List<int> getTextIDList(string text_type)
        {
            return TextListH.getTextIDList(text_type);
        }

        public List<int> stepTextIDList(int row, int rowLength)
        {
            return TextListH.stepTextIDList(row, rowLength);
        }
        public List<int> stepTextIDList(int row, int rowLength, string text_type)
        {
            return TextListH.stepTextIDList(row, rowLength, text_type);
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

        public static PaText fill(PaText TextMain, PaText TextSub)
        {
            return PaFn.fill(TextMain, TextSub);
        }
        public static string toMD5(string str)
        {
            return BasicMethod.toMD5(str);
        }
    }
}
