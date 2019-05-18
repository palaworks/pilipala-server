using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using LibStructs;
using sukiUnit;
using jarwUnit.pilipala;
using jarwUnit.pilipala.UI;

namespace PILIPALA.pala_services
{
    /// <summary>
    /// SLS 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class SLS : System.Web.Services.WebService
    {
        private PalaRoot PalaRoot;
        private ConnSign cS;
        private PalaDB PalaDB;

        private TextListH TextListH;
        private TextH TextH;


        public SLS()
        {
            PalaDB.MySqlConnH = new MySqlConnH();

            
            cS.user = "thaumy0tdymy";
            cS.password = "177BDE5";
            cS.dataSource = "localhost";
            cS.port = "3306";
            PalaDB.MySqlConnH.start(cS);
            PalaDB.dataBase = "thaumy0tdymy";


            /*
            cS.user = "pala_database_user";
            cS.password = "pala_database_user_password";
            cS.dataSource = "localhost";
            cS.port = "3306";
            PalaDB.MySqlConnH.start(cS);
            PalaDB.dataBase = "pala_database";
            */


            PalaDB.Tables = PalaRoot.defaultTables();/* 以默认值确定表名 */
            PalaDB.Views = PalaRoot.defaultViews();/* 以默认值确定视图名 */

            PalaRoot = new PalaRoot(2, PalaDB);
            TextListH = new TextListH(PalaDB);
            TextH = new TextH(PalaDB);

            //如果使用设计的组件，请取消注释以下行 
            //InitializeComponent(); 
        }

        /// <summary>
        /// 获得权限数据
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public PaRoot getRoot()
        {
            return PalaRoot.getRoot();
        }

        #region 文本数据操作

        [WebMethod]
        public List<PaText> getTextList()
        {
            return TextListH.getTextList();
        }
        [WebMethod]
        public List<PaText> getTextList(string text_type)
        {
            return TextListH.getTextList(text_type);
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
        public PaText rdmTextIndex(int excluded_text_id, string text_type)
        {
            return TextH.rdmTextIndex(excluded_text_id, text_type);
        }
        [WebMethod]
        public bool update_count_pv(int text_id, int value)
        {
            return TextH.update_count_pv(text_id, value);
        }
        [WebMethod]
        public bool update_count_like(int text_id, int value)
        {
            return TextH.update_count_like(text_id, value);
        }
        [WebMethod]
        public static PaText fill(PaText TextMain, PaText TextSub)
        {
            return TextH.fill(TextMain, TextSub);
        }
        #endregion
    }
}
