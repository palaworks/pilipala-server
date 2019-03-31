using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;/* hash&md5 */
using System.Drawing;/* GDI+ */


namespace sansuUnit
{
    /// <summary>
    /// 加密算法
    /// </summary>
    public class Encryptor
    {
        private delegate string MD5Handler(string Str);//声明用于toMD5的委托

        /// <summary>
        /// MD5方法
        /// </summary>
        /// <param name="str">被加密的字符串</param>
        /// <returns>通常返回MD5加密结果，报错则返回错误信息</returns>
        public virtual string md5(string str)
        {
            MD5Handler Mh = new MD5Handler(pvtToMD5);
            IAsyncResult result = Mh.BeginInvoke(str, null, null);

            return Mh.EndInvoke(result);
        }//实际用于调用的方法

        private string pvtToMD5(string input_str)
        {
            try
            {
                var buffer = Encoding.Default.GetBytes(input_str);
                var data = MD5.Create().ComputeHash(buffer);

                var md5 = new StringBuilder();
                foreach (var temp in data)
                {
                    md5.Append(temp.ToString("X2"));
                }

                return md5.ToString();//返回
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private delegate string HashHandler(string Str);//声明用于toHash的委托

        /// <summary>
        /// 散列方法
        /// </summary>
        /// <param name="str">被加密的字符串</param>
        /// <returns>通常返回散列加密结果，报错则返回错误信息</returns>
        public virtual string hash(string str)
        {
            HashHandler Hh = new HashHandler(pvtToHash);
            IAsyncResult result = Hh.BeginInvoke(str, null, null);

            return Hh.EndInvoke(result);
        }//实际用于调用的方法
        private string pvtToHash(string input_str)
        {
            try
            {
                var buffer = Encoding.UTF8.GetBytes(input_str);//将输入字符串转换成字节数组
                var data = SHA1.Create().ComputeHash(buffer);//创建SHA1对象进行散列计算

                var sha = new StringBuilder();//创建一个新的Stringbuilder收集字节
                foreach (var temp in data)//遍历每个字节的散列数据 
                {
                    sha.Append(temp.ToString("X2"));//格式每一个十六进制字符串
                }

                return sha.ToString();//返回
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }

    /// <summary>
    /// 排序算法
    /// </summary>
    public class Sorter
    {
        /// <summary>
        /// 交换值的方法，引用类型
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        static public void exch(ref int i, ref int j)
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
        static public bool less(int i, int j)
        {
            return i.CompareTo(j) < 0;
        }

        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="array">被排序的数组</param>
        /// <returns>通常返回有序数组(由小到大)，报错则返回null</returns>
        public virtual T[] easySort<T>(T[] array) where T : IComparable
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
        public virtual int[] shellSort(int[] array)
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
    }

    /// <summary>
    /// 检索类算法
    /// </summary>
    public class Searcher
    {
        /// <summary>
        /// 二分法检索(重载一),适用于整型检索
        /// </summary>
        /// <param name="value">被检索值</param>
        /// <param name="array">数组,顺序由小到大</param>
        /// <returns>若数组存在被检索值,则返回值在数组中的位置,若不存在则返回-1,报错则返回-2</returns>
        public virtual int binarySearch(int value, int[] array)
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
        public virtual double binarySearch(double value, double[] array)
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
}