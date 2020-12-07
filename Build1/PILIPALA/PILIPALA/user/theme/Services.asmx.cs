using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Text.RegularExpressions;

using WaterLibrary.com.basic;

namespace PILIPALA.user.theme
{
    /// <summary>
    /// Services 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Services : System.Web.Services.WebService
    {
        /// <summary>
        /// 转到中午时间概要（今天、昨天、上个月...）
        /// </summary>
        /// <param name="dateTime">要计算的时间</param>
        /// <returns></returns>
        public static string CN_TimeSummary(DateTime dateTime)
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
        /// 转到中文星期（星期三、星期五...）
        /// </summary>
        /// <param name="DateTime"></param>
        /// <returns></returns>
        public static string CN_Week(DateTime DateTime)
        {
            string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return Day[Convert.ToInt16(DateTime.DayOfWeek)] + " " + DateTime.Hour + ":" + DateTime.Minute;
        }

        
    }
}
