using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Web;
using System.Web.UI;
using System.Security.Cryptography;

namespace basicUnit
{
    /// <summary>
    /// 该类包含编程常用的基本方法
    /// </summary>
    public class BasicMethod
    {
        /// <summary>
        /// 交换值的方法，引用类型
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public static void exch(ref int i, ref int j)
        {
            int temp = i;
            i = j;
            j = temp;
        }
        /// <summary>
        /// 比较大小的方法
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static bool less(int i, int j)
        {
            return i.CompareTo(j) < 0;
        }

        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="array">被排序的数组</param>
        /// <returns>通常返回有序数组(由小到大)，报错则返回null</returns>
        public static T[] easySort<T>(T[] array) where T : IComparable
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
        public static int[] shellSort(int[] array)
        {
            int N = array.Length;
            int h = 1;

            while (h < N / 3)
                h = 3 * h + 1;
            while (h >= 1)
            {
                for (int i = h; i < N; i++)
                {
                    for (int j = i; j >= h && less(array[j], array[j - h]); j -= h)
                        exch(ref array[j], ref array[j - h]);
                }
                h = h / 3;
            }

            return array;
        }

        /// <summary>
        /// 二分法检索(重载一),适用于整型检索
        /// </summary>
        /// <param name="value">被检索值</param>
        /// <param name="array">数组,顺序由小到大</param>
        /// <returns>若数组存在被检索值,则返回值在数组中的位置,若不存在则返回-1,报错则返回-2</returns>
        public static int binarySearch(int value, int[] array)
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
        public static double binarySearch(double value, double[] array)
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

        /// <summary>
        /// 以字符串形式输出文件（重载一）（UTF8编码模式）
        /// </summary>
        /// <param name="url">文件所在的本地网络路径</param>
        /// <returns>返回字符串</returns>
        public static string fileToString(string url)
        {
            //读取url文件到文件尾，然后返回
            return LibFrame.LibFrameInfo.getStreamReader(url).ReadToEnd();
        }
        /// <summary>
        /// 以字符串形式输出文件（重载二）
        /// </summary>
        /// <param name="url">文件所在的本地网络路径</param>
        /// /// <param name="encodingType">解析文件所用的编码模式</param>
        /// <returns>返回字符串</returns>
        public static string fileToString(string url, string encodingType)
        {
            //读取url文件到文件尾，然后返回
            return LibFrame.LibFrameInfo.getStreamReader(url, encodingType).ReadToEnd();
        }
        /// <summary>
        /// 以字符串形式输出文件（重载三）（UTF8编码模式）
        /// </summary>
        /// <param name="filePath">文件所在的本地物理路径</param>
        /// <param name="bufferSize">文件流缓冲区大小，默认值可填4096</param>
        /// <param name="useAsync">使用异步初始化文件流，缺乏设计的异步调用会慢于串行调用</param>
        /// <returns>返回字符串</returns>
        public static string fileToString(string filePath, int bufferSize, bool useAsync)
        {

            using (
                StreamReader StreamReader = new StreamReader//流读取对象
                (new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Write, bufferSize, useAsync)//文件流对象
                , Encoding.GetEncoding("UTF8"))//指定编码模式
                    )
            {
                //以字符串形式输出文件
                return StreamReader.ReadToEnd().ToString();
            }
        }
        /// <summary>
        /// 以字符串形式输出文件（重载四）
        /// </summary>
        /// <param name="filePath">文件所在的本地物理路径</param>
        /// <param name="bufferSize">文件流缓冲区大小，默认值可填4096</param>
        /// <param name="useAsync">使用异步初始化文件流，缺乏设计的异步调用会慢于串行调用</param>
        /// <param name="encodingType">解析文件所用的编码模式</param>
        /// <returns>返回字符串</returns>
        public static string fileToString(string filePath, int bufferSize, bool useAsync, string encodingType)
        {

            using (
                StreamReader StreamReader = new StreamReader//流读取对象
                (new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Write, bufferSize, useAsync)//文件流对象
                , Encoding.GetEncoding(encodingType))//指定编码模式
                    )
            {
                //以字符串形式输出文件
                return StreamReader.ReadToEnd().ToString();
            }

        }

        /// <summary>
        /// 对象输出到MD5
        /// </summary>
        /// <typeparam name="T">待转换对象对应的类型</typeparam>
        /// <param name="obj">待转换对象</param>
        /// <returns>返回处理后得到的字符串</returns>
        public static string toMD5<T>(T obj)
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
        public static string toSHA1<T>(T obj)
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
        public static string toSHA256<T>(T obj)
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

    /// <summary>
    /// 该类包含web编程常用的基本方法
    /// </summary>
    public class WebMethod : Page
    {
        /* cookie操作 */
        /// <summary>
        /// 判断Cookie对象是否存在
        /// </summary>
        /// <param name="CookieName">被判断Cookie对象的名称</param>
        /// <returns>存在返回true，反之false</returns>
        public bool isCookiesExist(string CookieName)
        {
            if (HttpContext.Current.Request.Cookies[CookieName] == null)//如果Cookie对象为null（不存在）
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        /// <summary>
        /// 判断Cookie对象是否存在（重载二：判断索引是否存在）
        /// </summary>
        /// <param name="CookieName">被判断Cookie对象的名称</param>
        /// <param name="keyName">索引名，属于被判断的Cookie</param>
        /// <returns>存在返回true，反之false</returns>
        public bool isCookiesExist(string CookieName, string keyName)
        {
            if (isCookiesExist(CookieName) == true)//如果Cookie对象存在
            {
                if (HttpContext.Current.Request.Cookies[CookieName][keyName].ToString() != "")//如果Cookie中的keyName索引键值为空字符串
                {
                    return false;//不存在
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 读取Cookie对象到指定类型
        /// </summary>
        /// <typeparam name="T">指定泛型</typeparam>
        /// <param name="CookieName">被读取Cookie对象名</param>
        /// <returns>返回泛型值，读取失败则返回泛型默认值</returns>
        public T cookie<T>(string CookieName)
        {
            if (isCookiesExist(CookieName) == true)//如果Cookie对象存在
            {
                if (HttpContext.Current.Request.Cookies[CookieName].Value == "")
                {
                    /* 如果Cookie对象值为空字符串（null），返回泛型默认值 */
                    /* 在当前版本C#(7.3)中，default(T)可以简化为default */
                    return default;
                }
                else
                {
                    if (HttpContext.Current.Request.Cookies[CookieName].Value == null)
                    {
                        return default;
                    }
                    else
                    {
                        //如果Cookie对象的索引值不为null，转换为泛型返回
                        return (T)Convert.ChangeType(HttpContext.Current.Request.Cookies[CookieName].Value, typeof(T));
                    }
                }
            }
            else
            {
                return default;
            }

        }
        /// <summary>
        /// 读取Cookie对象的指定索引值到指定类型
        /// </summary>
        /// <typeparam name="T">指定泛型</typeparam>
        /// <param name="CookieName">被读取Cookie对象名</param>
        /// <param name="keyName">索引名，属于当前Cookie对象</param>
        /// <returns>返回泛型值，读取失败则返回泛型默认值</returns>
        public T cookie<T>(string CookieName, string keyName)
        {
            if (isCookiesExist(CookieName, keyName) == true)
            {
                //如果索引键值不为空字符串，转换为泛型返回
                return (T)Convert.ChangeType(HttpContext.Current.Request.Cookies[CookieName][keyName].ToString(), typeof(T));
            }
            else
            {
                return default;
            }

        }

        /// <summary>
        /// 设置Cookie值
        /// </summary>
        /// <param name="CookieName">Cookie名，承担该操作</param>
        /// <param name="value">设置值</param>
        /// <returns>设置成功返回true，反之false</returns>
        public bool setCookie(string CookieName, object value)
        {
            HttpContext.Current.Response.Cookies[CookieName].Value = value.ToString();
            return true;
        }
        /// <summary>
        /// 设置Cookie值（重载二：索引设置）
        /// </summary>
        /// <param name="CookieName">Cookie名，承担该操作</param>
        /// <param name="keyName">索引名，属于承担该操作的Cookie</param>
        /// <param name="value">设置值</param>
        /// <returns>设置成功返回true，反之false</returns>
        public bool setCookie(string CookieName, string keyName, object value)
        {
            HttpContext.Current.Response.Cookies[CookieName][keyName] = value.ToString();
            return true;
        }

        /* session操作 */
        /// <summary>
        /// 读取Session对象到指定类型
        /// </summary>
        /// <typeparam name="T">指定泛型</typeparam>
        /// <param name="varName">变量名，属于Session对象，承担该操作</param>
        /// <returns>返回泛型值，读取失败则返回泛型默认值</returns>
        public T session<T>(string varName)
        {
            //测试表明，Session对象始终存在，并且不为null，所以可以不加判断直接转换
            return (T)Convert.ChangeType(HttpContext.Current.Session[varName], typeof(T));//转换为自定义类型
        }
        /// <summary>
        /// 设置Session对象的变量值
        /// </summary>
        /// <param name="varName">变量名，属于Session对象，承担该操作</param>
        /// <param name="value">设置值</param>
        /// <returns>设置成功返回true，反之false</returns>
        public bool setSession(string varName, object value)
        {
            HttpContext.Current.Session[varName] = value;
            return true;
        }
    }
}