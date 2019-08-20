using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using PILIPALA.pala_system.service;

namespace PILIPALA.pala_custom.theme.field2.web_service
{
    /// <summary>
    /// WebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        /// <summary>
        /// 到 日期
        /// </summary>
        /// <param name="DateTime">时间对象</param>
        /// <returns></returns>
        [WebMethod]
        public static string toTime1(DateTime DateTime)
        {
            //年份只取后二位：2099=>99
            return Convert.ToString(DateTime.Year).Substring(2, 2) + "/" + DateTime.Month + "/" + DateTime.Day;
        }
        /// <summary>
        /// 到 周几+时间
        /// </summary>
        /// <param name="DateTime">时间对象</param>
        /// <returns></returns>
        [WebMethod]
        public static string toTime2(DateTime DateTime)
        {
            //年份只取后二位：2099=>99
            string DayInCN = "未知";
            switch (DateTime.DayOfWeek.ToString())
            {
                case "Monday": DayInCN = "周一"; break;
                case "Tuesday": DayInCN = "周二"; break;
                case "Wednesday": DayInCN = "周三"; break;
                case "Thursday": DayInCN = "周四"; break;
                case "Friday": DayInCN = "周五"; break;
                case "Saturday": DayInCN = "周六"; break;
                case "Sunday": DayInCN = "周日"; break;

                default: break;
            }
            return DayInCN + " " + DateTime.Hour + ":" + DateTime.Minute;
        }

        /// <summary>
        /// count_like计数减一
        /// </summary>
        /// <param name="text_id">文章序列号</param>
        /// <returns></returns>
        [WebMethod]
        public bool redu_countStar(int text_id)
        {
            Basic BasicService = new Basic();
            int count_like = BasicService.getTextSub(text_id).count_like;

            return BasicService.update_countStar(text_id, count_like - 1);

        }
        /// <summary>
        /// count_like计数加一
        /// </summary>
        /// <param name="text_id">文章序列号</param>
        /// <returns></returns>
        [WebMethod]
        public bool add_countStar(int text_id)
        {
            Basic BasicService = new Basic();
            int count_like = BasicService.getTextSub(text_id).count_like;

            return BasicService.update_countStar(text_id, count_like + 1);
        }
        /// <summary>
        /// count_pv计数加一
        /// </summary>
        /// <param name="text_id">文章序列号</param>
        /// <returns></returns>
        [WebMethod]
        public bool add_countPv(int text_id)
        {
            Basic BasicService = new Basic();
            int count_pv = BasicService.getTextSub(text_id).count_pv;

            return BasicService.update_countPv(text_id, count_pv + 1);
        }
    }
}
