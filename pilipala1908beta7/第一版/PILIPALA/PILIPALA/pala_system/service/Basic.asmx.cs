using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using LibStruct.MySql;
using LibStruct.pilipala;
using dataUnit;
using businessUnit.pilipala;
using businessUnit.pilipala.Edit;

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
        private mysqlConn mysqlConn;
        private PaDB paDB;
        private PaRoot paRoot;

        private TextListH TextListH;
        private TextH TextH;

        public Basic()
        {
            paDB.MySqlConnH = new MySqlConnH();


            mysqlConn.user = "pala_database_user";
            mysqlConn.password = "pala_database_user_password";
            mysqlConn.dataSource = "localhost";
            mysqlConn.port = "3306";
            paDB.MySqlConnH.start(mysqlConn);
            paDB.dataBase = "pala_db";/* 测试库 */


            paDB.Tables = PaRoot.defaultTables();/* 以默认值确定表名 */
            paDB.Views = PaRoot.defaultViews();/* 以默认值确定视图名 */

            paRoot = new PaRoot(2, paDB);
            TextListH = new TextListH(paDB);
            TextH = new TextH(paDB);

            //如果使用设计的组件，请取消注释以下行 
            //InitializeComponent(); 
        }

        [WebMethod]
        public List<PaText> getTextList()
        {
            return TextListH.getTextList();
        }
        [WebMethod]
        public List<PaText> getTextList(string text_type)
        {
            return new businessUnit.pilipala.UI.TextListH(paDB).getTextList();
        }

        [WebMethod]
        public List<PaText> stepTextList(int row, int rowLength)
        {
            return TextListH.stepTextList(row, rowLength);
        }
        [WebMethod]
        public List<PaText> stepTextList(int row, int rowLength, string text_type)
        {
            return TextListH.stepTextList(row, rowLength, text_type);
        }

        [WebMethod]
        public PaText getTextMain(int text_id)
        {
            return TextH.getTextMain(text_id);
        }
        [WebMethod]
        public PaText getTextSub(int text_id)
        {
            return TextH.getTextSub(text_id);
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
    }
}
