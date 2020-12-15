using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Reflection;
using Markdig;


namespace WaterLibrary.Tools
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

        /// <summary>
        /// 二分法检索(重载一),适用于整型检索
        /// </summary>
        /// <param name="value">被检索值</param>
        /// <param name="array">数组,顺序由小到大</param>
        /// <returns>若数组存在被检索值,则返回值在数组中的位置,若不存在则返回-1</returns>
        public static int BinarySearch(int value, int[] array)
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
        /// <summary>
        /// 二分法检索(重载二),适用于双精度浮点检索
        /// </summary>
        /// <param name="value">被检索值</param>
        /// <param name="array">数组，顺序由小到大</param>
        /// <returns>若数组存在被检索值,则返回值在数组中的位置,若不存在则返回-1</returns>
        public static double BinarySearch(double value, double[] array)
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

        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="array">被排序的数组</param>
        /// <returns>通常返回有序数组(由小到大)</returns>
        public static T[] BubbleSort<T>(T[] array) where T : IComparable
        {
            for (int path = 0; path < array.Length; path++)//正被有序的起始位
            {
                for (int i = 0; i < array.Length; i++)//临近元素排序
                {
                    if (i + 1 < array.Length)//元素交换
                    {
                        T tmp;
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

        /// <summary>
        /// 生成RSA密钥对(BASE64)
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetRSAKeyPair()
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            return new Dictionary<string, string>
            {
                { "PUBLIC", Convert.ToBase64String(RSA.ExportCspBlob(false)) },
                { "PRIVATE", Convert.ToBase64String(RSA.ExportCspBlob(true)) }
            };
        }

        /// <summary>
        /// RSA加密(BASE64)
        /// </summary>
        /// <param name="PublicKey">RSA公钥(BASE64)</param>
        /// <param name="PlainText">明文</param>
        /// <returns></returns>
        public static string RSAEncrypt(string PublicKey, string PlainText)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.ImportCspBlob(Convert.FromBase64String(PublicKey));

            return Convert.ToBase64String(RSA.Encrypt(Encoding.UTF8.GetBytes(PlainText), false));
        }
        /// <summary>
        /// RSA解密(BASE64)
        /// </summary>
        /// <param name="PrivateKey">RSA公钥(BASE64)</param>
        /// <param name="CipherText">密文(BASE64)</param>
        /// <returns></returns>
        public static string RSADecrypt(string PrivateKey, string CipherText)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.ImportCspBlob(Convert.FromBase64String(PrivateKey));

            return Encoding.UTF8.GetString(RSA.Decrypt(Convert.FromBase64String(CipherText), false));
        }

        /// <summary>
        /// 将对象加密到MD5
        /// </summary>
        /// <param name="obj">待处理对象</param>
        /// <returns>返回处理后得到的字符串</returns>
        public static string MD5(object obj)
        {
            byte[] source = Encoding.Default.GetBytes(Convert.ToString(obj));/* 将对象转换成string后转换成字节数组 */
            byte[] data = System.Security.Cryptography.MD5.Create().ComputeHash(source);/* 创建SHA1对象进行散列计算 */

            StringBuilder md5 = new StringBuilder();/* 创建一个新的Stringbuilder收集字节 */
            foreach (var temp in data)/* 遍历每个字节的散列数据 */
            {
                md5.Append(temp.ToString("X2"));/* 格式每一个十六进制字符串 */
            }

            return md5.ToString();
        }
        /// <summary>
        /// 将对象加密到SHA1
        /// </summary>
        /// <param name="obj">待处理对象</param>
        /// <returns>返回处理后得到的字符串</returns>
        public static string SHA1(object obj)
        {
            byte[] source = Encoding.Default.GetBytes(Convert.ToString(obj));
            byte[] data = System.Security.Cryptography.SHA1.Create().ComputeHash(source);

            StringBuilder sha1 = new StringBuilder();
            foreach (var temp in data)
            {
                sha1.Append(temp.ToString("X2"));
            }

            return sha1.ToString();
        }
        /// <summary>
        /// 将对象加密到SHA256
        /// </summary>
        /// <param name="obj">待处理对象</param>
        /// <returns>返回处理后得到的字符串</returns>
        public static string SHA256(object obj)
        {
            byte[] source = Encoding.Default.GetBytes(Convert.ToString(obj));
            byte[] data = System.Security.Cryptography.SHA256.Create().ComputeHash(source);

            StringBuilder sha256 = new StringBuilder();
            foreach (var temp in data)
            {
                sha256.Append(temp.ToString("X2"));
            }

            return sha256.ToString();
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
        public static List<string> StringToList(string str, char Delimiter)
        {
            List<string> StringList = new List<string>();
            foreach (string el in str.Split(Delimiter))
            {
                StringList.Add(el);
            }
            return StringList;
        }
        /// <summary>
        /// 将字符串集合按照分隔符合并为一个字符串
        /// </summary>
        /// <param name="List">待合并string集合</param>
        /// <param name="Delimiter">分隔符</param>
        /// <returns></returns>
        public static string ListToString(List<string> List, char Delimiter)
        {
            string Result = "";
            foreach (string temp in List)
            {
                Result += temp + Delimiter;
            }
            return Result[0..^1];
        }
        /// <summary>
        /// 将可遍历对象的元素值按照分隔符合并为一个字符串
        /// </summary>
        /// <param name="List">可遍历对象，其中的元素需能通过指定属性获取值</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="Delimiter">分隔符</param>
        /// <returns></returns>
        public static string ListToString(dynamic List, string PropertyName, char Delimiter)
        {
            string Result = "";
            PropertyInfo info = List[0].GetType().GetProperty(PropertyName);
            foreach (dynamic temp in List)
            {
                Result += info.GetValue(temp) + Delimiter;
            }
            return Result[0..^1];
        }

        /// <summary>
        /// Html过滤器
        /// </summary>
        /// <param name="HtmlText">Html文本</param>
        /// <returns></returns>
        public static string HtmlFilter(string HtmlText)
        {
            if (string.IsNullOrEmpty(HtmlText))
            {
                return "";
            }

            HtmlText = Regex.Replace(HtmlText, "<style[^>]*?>[\\s\\S]*?<\\/style>", "");
            HtmlText = Regex.Replace(HtmlText, "<script[^>]*?>[\\s\\S]*?<\\/script>", "");
            HtmlText = Regex.Replace(HtmlText, "<[^>]+>", "");
            HtmlText = Regex.Replace(HtmlText, "\\s*|\t|\r|\n", "");
            HtmlText = Regex.Replace(HtmlText, "&(#\\d*)?\\w*;", "");
            HtmlText = HtmlText.Replace(" ", "");

            return HtmlText.Trim();
        }
        /// <summary>
        /// Markdown转Html
        /// </summary>
        /// <param name="MarkdownText">Markdown文本</param>
        /// <returns></returns>
        public static string MarkdownToHtml(string MarkdownText)
        {
            var builder = new MarkdownPipelineBuilder();//添加对表格的解析支持
            builder.Extensions.Add(new Markdig.Extensions.Tables.PipeTableExtension());
            builder.Extensions.Add(new Markdig.Extensions.EmphasisExtras.EmphasisExtraExtension());

            var pipeline = builder.Build();

            return Markdown.ToHtml(MarkdownText, pipeline);
        }
    }

    /// <summary>
    /// RSA密钥对
    /// </summary>
    public class KeyPair
    {
        /// <summary>
        /// 初始化密钥对
        /// </summary>
        /// <param name="IS_INIT">是否在初始化时自动填充密钥对，默认为false</param>
        public KeyPair(bool IS_INIT = false)
        {
            if (IS_INIT == true)
            {
                Dictionary<string, string> KeyPair = MathH.GetRSAKeyPair();
                PublicKey = KeyPair["PUBLIC"];
                PrivateKey = KeyPair["PRIVATE"];
            }
            else
            {
                PublicKey = "";
                PrivateKey = "";
            }
        }
        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey { get; set; }
    }
}
