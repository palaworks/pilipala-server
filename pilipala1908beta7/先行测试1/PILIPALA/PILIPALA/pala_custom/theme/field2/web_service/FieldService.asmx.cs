using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using PILIPALA.pala_system.service;

namespace PILIPALA.pala_custom.theme.field2.web_service
{
    /// <summary>
    /// FieldService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    [System.Web.Script.Services.ScriptService]

    public class FieldService : System.Web.Services.WebService
    {

        /// <summary>
        /// 到 日期
        /// </summary>
        /// <param name="DateTime">时间对象</param>
        [WebMethod]
        public static string toTime1(DateTime DateTime, string str)
        {
            //年份只取后二位：2099=>99
            return Convert.ToString(DateTime.Year).Substring(2, 2) + str + DateTime.Month + str + DateTime.Day;
        }
        /// <summary>
        /// 到 周几+时间
        /// </summary>
        /// <param name="DateTime">时间对象</param>
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
        /// count_star计数减一
        /// </summary>
        /// <param name="text_id">文章序列号</param>
        [WebMethod]
        public int subs_countStar(int text_id)
        {
            Basic BasicService = new Basic();
            int count_star = BasicService.getTextSub(text_id).count_star;

            BasicService.update_countStar(text_id, count_star - 1);

            return count_star - 1;
        }
        /// <summary>
        /// count_star计数加一
        /// </summary>
        /// <param name="text_id">文章序列号</param>
        [WebMethod]
        public int plus_countStar(int text_id)
        {
            Basic BasicService = new Basic();
            int count_star = BasicService.getTextSub(text_id).count_star;

            BasicService.update_countStar(text_id, count_star + 1);

            return count_star + 1;
        }
        /// <summary>
        /// count_pv计数加一
        /// </summary>
        /// <param name="text_id">文章序列号</param>
        [WebMethod]
        public int plus_countPv(int text_id)
        {
            Basic BasicService = new Basic();
            int count_pv = BasicService.getTextSub(text_id).count_pv;

            BasicService.update_countPv(text_id, count_pv + 1);

            return count_pv + 1;
        }

        /// <summary>
        /// 数据库标签数据转字符串集合
        /// </summary>
        /// <param name="tags">标签文本</param>
        [WebMethod]
        public static List<string> toTags(string tags)
        {
            List<string> temp = new List<string>();
            foreach (string tag in tags.Split('$'))
            {
                temp.Add(tag);
            }
            return temp;
        }
    }
}
