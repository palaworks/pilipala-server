using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Collections;


namespace PILIPALA.Utils
{
    public static class Formatter
    {
        /// <summary>
        /// 转到中文时间概要（今天、昨天、上个月...）
        /// </summary>
        /// <param name="dateTime">要计算的时间</param>
        /// <returns></returns>
        public static string CN_TimeSummary(DateTime time, Func<DateTime, string> todo)
        {
            var span = (DateTime.Now - time).TotalHours;

            Hashtable Table = new()
            {
                { new[] { 00 * 00, 06 * 01 }, "刚刚" },
                { new[] { 06 * 01, 01 * 24 }, "今天" },
                { new[] { 01 * 24, 02 * 24 }, "昨天" },
                { new[] { 02 * 24, 03 * 24 }, "前天" },
                { new[] { 03 * 24, 04 * 24 }, "三天前" },
                { new[] { 04 * 24, 05 * 24 }, "四天前" },
                { new[] { 05 * 24, 06 * 24 }, "五天前" },
                { new[] { 06 * 24, 07 * 24 }, "六天前" },
                { new[] { 07 * 24, 14 * 24 }, "一周前" },
                { new[] { 14 * 24, 21 * 24 }, "两周前" },
                { new[] { 21 * 24, 28 * 24 }, "三周前" },
                { new[] { 28 * 24, 60 * 24 }, "一月前" },
                { new[] { 60 * 24, 90 * 24 }, "两月前" },
            };

            foreach (int[] el in Table.Keys)
            {
                if (span > el[0] && span < el[1])
                {
                    return (string)Table[el];
                }
            }

            return todo(time);
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
