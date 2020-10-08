using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace WaterLibrary.com.basic
{
    /// <summary>
    /// 数学管理器
    /// </summary>
    public static class MathH
    {
        /// <summary>
        /// 交换值的方法，引用类型
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public static void Exch(ref int i, ref int j)
        {
            int temp = i;
            i = j;
            j = temp;
        }
        /// <summary>
        /// 判断小于的方法
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>若i小于j返回true</returns>
        public static bool Less(int i, int j)
        {
            return i.CompareTo(j) < 0;
        }
        /// <summary>
        /// 判断奇数的方法
        /// </summary>
        /// <param name="num">待判断的数值</param>
        /// <returns>num为奇数返回true，num为偶数返回false</returns>
        public static bool IsOdd(int num)
        {
            return (num % 2) == 1;
        }

        /// <summary>
        /// 生成GUID
        /// </summary>
        /// <param name="format">生成格式，可选值有N、D、B、P、X</param>
        /// <returns></returns>
        public static string GenerateGUID(string format)
        {
            return Guid.NewGuid().ToString(format);
        }
    }

    /// <summary>
    /// 检索管理器
    /// </summary>
    public static class SearchH
    {
        /// <summary>
        /// 二分法检索(重载一),适用于整型检索
        /// </summary>
        /// <param name="value">被检索值</param>
        /// <param name="array">数组,顺序由小到大</param>
        /// <returns>若数组存在被检索值,则返回值在数组中的位置,若不存在则返回-1,报错则返回-2</returns>
        public static int BinarySearch(int value, int[] array)
        {
            try//二分法主体
            {
                int low = 0;
                int high = array.Length - 1;
                while (low <= high)
                {
                    int mid = (low + high) / 2;

                    if (value == array[mid])
                    {
                        return mid;
                    }
                    if (value > array[mid])
                    {
                        low = mid + 1;
                    }
                    if (value < array[mid])
                    {
                        high = mid - 1;
                    }
                }
                return -1;
            }
            catch
            {
                return -2;
            }
        }
        /// <summary>
        /// 二分法检索(重载二),适用于双精度浮点检索
        /// </summary>
        /// <param name="value">被检索值</param>
        /// <param name="array">数组，顺序由小到大</param>
        /// <returns>若数组存在被检索值,则返回值在数组中的位置,若不存在则返回-1,报错则返回-2</returns>
        public static double BinarySearch(double value, double[] array)
        {
            try//二分法主体
            {
                double low = 0;
                double high = array.Length - 1;
                while (low <= high)
                {
                    int mid = (int)(low + high) / 2;

                    if (value == array[mid])
                    {
                        return mid;
                    }
                    if (value > array[mid])
                    {
                        low = mid + 1;
                    }
                    if (value < array[mid])
                    {
                        high = mid - 1;
                    }
                }
                return -1;
            }
            catch
            {
                return -2;
            }
        }
    }

    /// <summary>
    /// 排序管理器
    /// </summary>
    public static class SortH
    {
        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="array">被排序的数组</param>
        /// <returns>通常返回有序数组(由小到大)，报错则返回null</returns>
        public static T[] BubbleSort<T>(T[] array) where T : IComparable
        {
            try
            {
                for (int path = 0; path < array.Length; path++)//正被有序的起始位
                {
                    for (int i = 0; i < array.Length; i++)//临近元素排序
                    {
                        if (i + 1 < array.Length)//元素交换
                        {
                            T tmp; ;
                            if (array[i].CompareTo(array[i + 1]) > 0)
                            {
                                tmp = array[i];
                                array[i] = array[i + 1];
                                array[i + 1] = tmp;
                            }
                        }

                    }
                }
                return array;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 希尔排序
        /// </summary>
        /// <param name="array">待排序的整数组</param>
        /// <returns>返回排序完成的数组</returns>
        public static int[] ShellSort(int[] array)
        {
            int N = array.Length;
            int h = 1;

            while (h < N / 3)
                h = 3 * h + 1;
            while (h >= 1)
            {
                for (int i = h; i < N; i++)
                {
                    for (int j = i; j >= h && MathH.Less(array[j], array[j - h]); j -= h)
                        MathH.Exch(ref array[j], ref array[j - h]);
                }
                h /= 3;
            }

            return array;
        }
    }

    /// <summary>
    /// 转换管理器
    /// </summary>
    public static class ConvertH
    {
        /// <summary>
        /// 将字符串按照分隔符字符切割
        /// </summary>
        /// <param name="str">待切割字符串</param>
        /// <param name="Delimiter">分隔符</param>
        /// <returns>返回切割结果集</returns>
        public static List<string> ToStringList(string str, char Delimiter)
        {
            List<string> StringList = new List<string>();
            foreach (string el in str.Split(Delimiter))
            {
                StringList.Add(el);
            }
            return StringList;
        }
        /// <summary>
        /// Html过滤器
        /// </summary>
        /// <param name="Html">待过滤的Html字符串</param>
        /// <returns></returns>
        public static string HtmlFilter(string Html)
        {
            if (string.IsNullOrEmpty(Html))
            {
                return "";
            }

            string regEx_style = "<style[^>]*?>[\\s\\S]*?<\\/style>";
            string regEx_script = "<script[^>]*?>[\\s\\S]*?<\\/script>";
            string regEx_html = "<[^>]+>";

            Html = Regex.Replace(Html, regEx_style, "");
            Html = Regex.Replace(Html, regEx_script, "");
            Html = Regex.Replace(Html, regEx_html, "");
            Html = Regex.Replace(Html, "\\s*|\t|\r|\n", "");
            Html = Html.Replace(" ", "");

            return Html.Trim();
        }

        /// <summary>
        /// 对象输出到MD5
        /// </summary>
        /// <typeparam name="T">待转换对象对应的类型</typeparam>
        /// <param name="obj">待转换对象</param>
        /// <returns>返回处理后得到的字符串</returns>
        public static string ToMD5<T>(T obj)
        {
            byte[] source = Encoding.Default.GetBytes(Convert.ToString(obj));/* 将对象转换成string后转换成字节数组 */
            byte[] data = MD5.Create().ComputeHash(source);/* 创建SHA1对象进行散列计算 */

            StringBuilder md5 = new StringBuilder();/* 创建一个新的Stringbuilder收集字节 */
            foreach (var temp in data)/* 遍历每个字节的散列数据 */
            {
                md5.Append(temp.ToString("X2"));/* 格式每一个十六进制字符串 */
            }

            return md5.ToString();
        }
        /// <summary>
        /// 对象输出到SHA1
        /// </summary>
        /// <typeparam name="T">待转换对象对应的类型</typeparam>
        /// <param name="obj">待转换对象</param>
        /// <returns>返回处理后得到的字符串</returns>
        public static string ToSHA1<T>(T obj)
        {
            byte[] source = Encoding.Default.GetBytes(Convert.ToString(obj));
            byte[] data = SHA1.Create().ComputeHash(source);

            StringBuilder sha1 = new StringBuilder();
            foreach (var temp in data)
            {
                sha1.Append(temp.ToString("X2"));
            }

            return sha1.ToString();
        }
        /// <summary>
        /// 对象输出到SHA256
        /// </summary>
        /// <typeparam name="T">待转换对象对应的类型</typeparam>
        /// <param name="obj">待转换对象</param>
        /// <returns>返回处理后得到的字符串</returns>
        public static string ToSHA256<T>(T obj)
        {
            byte[] source = Encoding.Default.GetBytes(Convert.ToString(obj));
            byte[] data = SHA1.Create().ComputeHash(source);

            StringBuilder sha256 = new StringBuilder();
            foreach (var temp in data)
            {
                sha256.Append(temp.ToString("X2"));
            }

            return sha256.ToString();
        }
    }
}
