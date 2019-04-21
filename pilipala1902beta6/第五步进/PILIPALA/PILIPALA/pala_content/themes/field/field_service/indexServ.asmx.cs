using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using LibStructs;
using sukiUnit;
using jarwUnit.pilipala;
using PILIPALA.pala_services;

namespace PILIPALA.pala_content.themes.field.field_service
{
    /// <summary>
    /// index 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    [System.Web.Script.Services.ScriptService]
    public class indexServ : System.Web.Services.WebService
    {
        public indexServ()
        {
            //如果使用设计的组件，请取消注释以下行 
            //InitializeComponent(); 
        }


        /// <summary>
        /// count_like计数减一
        /// </summary>
        /// <param name="text_id"></param>
        /// <returns></returns>
        [WebMethod]
        public int less_count_like(int text_id)
        {
            SLS SLS = new SLS();
            int count_like = SLS.getTextSub(text_id).count_like;

            SLS.update_count_like(text_id, count_like - 1);
            return count_like - 1;

        }
        /// <summary>
        /// count_like计数加一
        /// </summary>
        /// <param name="text_id"></param>
        /// <returns></returns>
        [WebMethod]
        public int incre_count_like(int text_id)
        {
            SLS SLS = new SLS();
            int count_like = SLS.getTextSub(text_id).count_like;

            SLS.update_count_like(text_id, count_like + 1);
            return count_like + 1;
        }
        /// <summary>
        /// count_pv计数加一
        /// </summary>
        /// <param name="text_id"></param>
        /// <returns></returns>
        [WebMethod]
        public int incre_count_pv(int text_id)
        {
            SLS SLS = new SLS();
            int count_pv = SLS.getTextSub(text_id).count_pv;

            SLS.update_count_pv(text_id, count_pv + 1);
            return count_pv + 1;
        }

        /// <summary>
        /// 文件转字符串
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string exstring(string url)
        {
            FileHandler FH = new FileHandler();
            return FH.fileToStr(Server.MapPath("") + url);
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="DateTime">时间对象</param>
        /// <returns></returns>
        [WebMethod]
        public static string extime(DateTime DateTime)
        {
            //年份只取后二位：2099=>99
            return Convert.ToString(DateTime.Year).Substring(2, 2) + "/" + DateTime.Month + "/" + DateTime.Day + " " + DateTime.Hour + ":" + DateTime.Minute;
        }
        /// <summary>
        /// 条带颜色样式
        /// </summary>
        /// <param name="color">颜色代码</param>
        /// <returns></returns>
        [WebMethod]
        public static string stripStyle(string strip_color)
        {
            switch (strip_color)
            {
                case "org": return "OrgQian";
                case "blu": return "BluQian";
                case "prp": return "PrpQian";
                default: return null;
            }
        }
        /// <summary>
        /// 标签文本转标签集合
        /// </summary>
        /// <param name="tags">标签文本</param>
        /// <returns></returns>
        [WebMethod]
        public static List<string> extags(string tags)
        {
            List<string> list_tags = new List<string>();
            foreach (string tag in tags.Split('$'))
            {
                list_tags.Add(tag);
            }
            return list_tags;
        }
    }
}
