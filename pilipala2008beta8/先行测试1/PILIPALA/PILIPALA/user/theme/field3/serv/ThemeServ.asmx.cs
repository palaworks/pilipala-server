using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Text.RegularExpressions;
using PILIPALA.system.serv;

namespace PILIPALA.user.theme.field3.serv
{
    /// <summary>
    /// ThemeServ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]

    [System.Web.Script.Services.ScriptService]

    public class ThemeServ : System.Web.Services.WebService
    {
        /// <summary>
        /// 数据库标签数据转字符串集合
        /// </summary>
        /// <param name="tags">标签文本</param>
        [WebMethod]
        public static List<string> ToLabels(string tags)
        {
            List<string> temp = new List<string>();
            foreach (string tag in tags.Split('$'))
            {
                temp.Add(tag);
            }
            return temp;
        }

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
        /// <summary>
        /// 生成概要
        /// </summary>
        /// <param name="ID">文章id</param>
        /// <param name="length">如果文章概要不存在，取文章前一段内容的长度</param>
        /// <returns></returns>
        public static string doSummary(string Content, int Length)
        {
            return htmlFilter(Content).Substring(0, Length);
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
    }
}
