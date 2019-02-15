using System;//Base Lib
using System.Text;//Base Lib

using System.Collections;//泛型集合类命名空间
using System.Collections.Generic;//非泛型集合命名空间
using System.Security.Cryptography;//hash/md5命名空间
using System.Drawing;//GDI+命名空间

//将StdLib全部引用
using StdLib;
using StdLib.DataHandler;
using StdLib.FrameHandler;
using StdLib.LogicHandler;
using StdLib.ViewHandler.WebSite;

namespace StdLib
{
    //逻辑方法要求可被重写
    namespace LogicHandler
    {
        /// <summary>
        /// 加密算法类
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
                MD5Handler Mh = new MD5Handler(toMD5);
                IAsyncResult result = Mh.BeginInvoke(str, null, null);

                return Mh.EndInvoke(result);
            }//实际用于调用的方法
            private string toMD5(string input_str)
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
                HashHandler Hh = new HashHandler(toHash);
                IAsyncResult result = Hh.BeginInvoke(str, null, null);

                return Hh.EndInvoke(result);
            }//实际用于调用的方法
            private string toHash(string input_str)
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
        /// 矩阵算法类
        /// </summary>
        public class ArrayAlgorithm
        {
            private int[,] buf = new int[10, 10];//初始化矩阵，大小为10x10
            private int[] temp = new int[10];//初始化包含10个坐标的字符串数组

            /// <summary>
            /// 存放生成的矩阵
            /// </summary>
            private List<int[,]> NumArray = new List<int[,]>();

            /// <summary>
            /// 矩阵的引用访问器
            /// </summary>
            public List<int[,]> numArray
            {
                get { return NumArray; }//如果算法还没处理就直接取用的话取到的是空的List
            }

            /// <summary>
            /// 提取矩阵有效信息索引的方法
            /// </summary>
            /// <param name="psw">密码(int):共10个整数</param>
            /// <param name="dic">int[] dic = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };:字典集，一共10个字符</param>
            /// <param name="num">int[] num = { 8, 4, 5, 5, 5, 3, 7, 9, 2, 9 };:被加密的10个整数</param>
            /// <returns>通常返回索引数组，报错则返回null</returns>
            public virtual int[] getIndex(int[] psw, int[] dic, int[] num)
            {
                ushort i = 0;//循环值
                try
                {
                    #region 生成矩阵
                    for (int y = 0; y < 10; y++)//y表示纵坐标
                    {
                        for (int x = 0; x < dic.Length; x++)//x表示横坐标
                        {
                            if (x > 10 - psw[y]) { i++; }

                            if (x < 10 - psw[y])
                            {
                                buf[y, x] = dic[x + psw[y]];

                                Console.Write(buf[y, x]);//逐个输出位移数
                            }
                            else
                            {
                                buf[y, x] = dic[i];

                                Console.Write(buf[y, x]);//逐个输出位移数
                            }
                        }

                        i = 0;

                        Console.WriteLine();//满10换行
                    }
                    #endregion

                    NumArray.Add(buf);//给NumArray添加元素

                    #region 循环以检索矩阵中符合要求的值
                    for (int p = 0; p < 10; p++)
                    {
                        for (i = 0; i < 10; i++)
                        {
                            if (buf[p, i] == num[p])
                            {
                                temp[p] = i * 10 + p;//把横纵坐标联合构成单坐标
                            }
                        }
                    }
                    #endregion

                    return temp;//返回temp数组

                    //假设其中一个值为29，那么它的坐标即为(2,9)，假如出现了单数，设其值为6，则坐标为(0,6)
                }
                catch
                {
                    return null;//算法主方法(即矩阵处理)过程中发生致命性错误，可能是由于dic数组或num数组错误传参导致
                }
            }//Main_Srr

        }

        /// <summary>
        /// PixelGraphic转码类
        /// </summary>
        public class PixelGraphic
        {
            //异步多线程解码
            private delegate string dePixelsHandler(string stream);//声明用于dePixels委托
            /// <summary>
            /// PixelGraphic解码方法
            /// </summary>
            /// <param name="stream">文件流指定</param>
            /// <returns>通常返回解码结果，报错则返回"StdLibError ec4580"</returns>
            public virtual string dePixels(string stream)
            {
                dePixelsHandler deHandler = new dePixelsHandler(de);//建立委托deHandler
                IAsyncResult result = deHandler.BeginInvoke(stream, null, null);//异步请求结果

                return deHandler.EndInvoke(result);//返回异步结果
            }//实际用于调用的方法

            //异步多线程编码
            private delegate Bitmap toPixelsHandler_normal(string hex, string stream);//声明用于hexToBMP的委托
            /// <summary>
            /// PixelGraphic加密方法:(第一重载)注意hex至少由4个16进制字符组成
            /// </summary>
            /// <param name="hex">被加密的16进制text(不带空格)</param>
            /// <param name="stream">bmp模板流</param>
            /// <returns>通常返回编译后的bmp，报错则返回null</returns>
            public virtual Bitmap toPixels(string hex, string stream)
            {
                toPixelsHandler_normal toHandler = new toPixelsHandler_normal(hexToBMP);//建立委托toHandler
                IAsyncResult result = toHandler.BeginInvoke(hex, stream, null, null);

                return toHandler.EndInvoke(result);
            }//实际用于调用的方法

            private delegate Bitmap toPixelsHandler_pro(object obj, string stream, string type);//声明用于_allToBMP的委托
            /// <summary>
            /// PixelGraphic加密方法:(第二重载)注意hex至少由4个16进制字符组成
            /// </summary>
            /// <param name="obj">被加密的16进制obj:text(不带空格)或者_hex(带空格)或者str[,](hex矩阵)/</param>
            /// <param name="stream">bmp模板流</param>
            /// <param name="type"></param>
            /// <returns>通常返回编译后的bmp，报错则返回null</returns>
            public virtual Bitmap toPixels(object obj, string stream, string type)
            {
                toPixelsHandler_pro toHandler = new toPixelsHandler_pro(allToBMP);//建立委托toHandler
                IAsyncResult result = toHandler.BeginInvoke(obj, stream, type, null, null);

                return toHandler.EndInvoke(result);
            }//实际用于调用的方法


            //用于对图像解码的方法
            /// <summary>
            /// 解码ANSW图像
            /// </summary>
            /// <param name="stream">包含待解码BMP图像的物理路径</param>
            /// <returns>返回解码后的16进制数据，报错则返回null</returns>
            private string de(string stream)
            {
                try
                {
                    Bitmap bp = new Bitmap(stream);//从文件流获取bmp
                    Color c = new Color();
                    string hex = "";

                    #region 解码部分
                    for (int y = 0; y < 100; y++)
                    {
                        for (int x = 0; x < 100; x++)
                        {
                            c = bp.GetPixel(x, y);
                            switch (c.R)
                            {
                                case 000: hex += "0"; break;
                                case 010: hex += "1"; break;
                                case 020: hex += "2"; break;
                                case 030: hex += "3"; break;
                                case 040: hex += "4"; break;
                                case 050: hex += "5"; break;
                                case 060: hex += "6"; break;
                                case 070: hex += "7"; break;
                                case 080: hex += "8"; break;
                                case 090: hex += "9"; break;
                                case 100: hex += "a"; break;
                                case 110: hex += "b"; break;
                                case 120: hex += "c"; break;
                                case 130: hex += "d"; break;
                                case 140: hex += "e"; break;
                                case 150: hex += "f"; break;
                            }
                        }
                    }
                    #endregion

                    return hex;
                }
                catch
                {
                    return null; //方法中发生关键性错误，通常是由于未能成功初始化Bitmap或传递了错误的Bitmap导致
                }
            }

            //用于编译图像的方法
            /// <summary>
            /// 通过选择根据方式来编译ANSW
            /// </summary>
            /// <param name="obj">含有16进制数据的实例</param>
            /// <param name="stream">初始BMP图片模板的物理路径</param>
            /// <param name="type">编译类型，可选填项有："hex"（根据不带空格的16进制文本编译ANSW）、"_hex"（根据带有空格的16进制文本编译ANSW）、"array"（根据16进制文本数据矩阵编译ANSW）</param>
            /// <returns>返回Bitmap实例，错误则返回null</returns>
            private Bitmap allToBMP(object obj, string stream, string type)
            {
                try
                {
                    switch (type)
                    {
                        case "hex": return hexToBMP(obj.ToString(), stream);//根据不带空格的16进制文本编译ANSW
                        case "_hex": return _hexToBMP(obj.ToString(), stream);//根据带有空格的16进制文本编译ANSW
                        case "array": return arrayToBMP((string[,])obj, stream);//根据16进制文本数据矩阵编译ANSW
                        default: return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
            /// <summary>
            /// 根据不带空格的16进制文本编译ANSW
            /// </summary>
            /// <param name="hex">不带空格的16进制文本</param>
            /// <param name="stream">初始BMP图片模板的物理路径</param>
            /// <returns>返回Bitmap实例，错误则返回null</returns>
            private Bitmap hexToBMP(string hex, string stream)
            {
                try
                {
                    //初始化
                    Bitmap outputBP = new Bitmap(stream);
                    ushort i = 0;//临时变量
                    int len = hex.Length;
                    string[,] hexArray = new string[100, 100];

                    #region 将矩阵记空
                    for (int y = 0; y < 100; y++)
                    {
                        for (int x = 0; x < 100; x++)
                        {
                            hexArray[x, y] = null;//循环记空
                        }
                    }
                    #endregion

                    #region 将字符串加空格
                    if (len <= 10000)
                    {
                        for (int a = 1; a < len; a++)
                        {
                            i++;
                            hex = hex.Insert(a + i - 1, " ");
                        }
                    }
                    else
                    {
                        for (int a = 1; a < 10000; a++)
                        {
                            i++;
                            hex = hex.Insert(a + i - 1, " ");
                        }
                    }

                    #endregion

                    #region 把空格字符串变为矩阵
                    i = 0;//临时变量归零
                    for (int y = 0; y < 100; y++)
                    {
                        for (int x = 0; x < 100; x++)
                        {

                            if (i < hex.Split(' ').Length)
                            {
                                hexArray[x, y] = hex.Split(' ')[i];
                            }
                            i++;
                        }
                    }
                    #endregion

                    #region 根据矩阵的值生成bmp图像
                    for (int y = 0; y < 100; y++)
                    {

                        for (int x = 0; x < 100; x++)
                        {
                            switch (hexArray[x, y])
                            {
                                case "0": outputBP.SetPixel(x, y, Color.FromArgb(000, 0, 0)); break;
                                case "1": outputBP.SetPixel(x, y, Color.FromArgb(010, 0, 0)); break;
                                case "2": outputBP.SetPixel(x, y, Color.FromArgb(020, 0, 0)); break;
                                case "3": outputBP.SetPixel(x, y, Color.FromArgb(030, 0, 0)); break;
                                case "4": outputBP.SetPixel(x, y, Color.FromArgb(040, 0, 0)); break;
                                case "5": outputBP.SetPixel(x, y, Color.FromArgb(050, 0, 0)); break;
                                case "6": outputBP.SetPixel(x, y, Color.FromArgb(060, 0, 0)); break;
                                case "7": outputBP.SetPixel(x, y, Color.FromArgb(070, 0, 0)); break;
                                case "8": outputBP.SetPixel(x, y, Color.FromArgb(080, 0, 0)); break;
                                case "9": outputBP.SetPixel(x, y, Color.FromArgb(090, 0, 0)); break;
                                case "a": outputBP.SetPixel(x, y, Color.FromArgb(100, 0, 0)); break;
                                case "b": outputBP.SetPixel(x, y, Color.FromArgb(110, 0, 0)); break;
                                case "c": outputBP.SetPixel(x, y, Color.FromArgb(120, 0, 0)); break;
                                case "d": outputBP.SetPixel(x, y, Color.FromArgb(130, 0, 0)); break;
                                case "e": outputBP.SetPixel(x, y, Color.FromArgb(140, 0, 0)); break;
                                case "f": outputBP.SetPixel(x, y, Color.FromArgb(150, 0, 0)); break;
                                case "A": outputBP.SetPixel(x, y, Color.FromArgb(100, 0, 0)); break;
                                case "B": outputBP.SetPixel(x, y, Color.FromArgb(110, 0, 0)); break;
                                case "C": outputBP.SetPixel(x, y, Color.FromArgb(120, 0, 0)); break;
                                case "D": outputBP.SetPixel(x, y, Color.FromArgb(130, 0, 0)); break;
                                case "E": outputBP.SetPixel(x, y, Color.FromArgb(140, 0, 0)); break;
                                case "F": outputBP.SetPixel(x, y, Color.FromArgb(150, 0, 0)); break;
                            }
                        }
                    }
                    #endregion

                    return outputBP;
                }
                catch
                {
                    return null;
                }
            }
            /// <summary>
            /// 根据带有空格的16进制文本编译ANSW
            /// </summary>
            /// <param name="_hex">带有空格的16进制文本</param>
            /// <param name="stream">初始BMP图片模板的物理路径</param>
            /// <returns>返回Bitmap实例，错误则返回null</returns>
            private Bitmap _hexToBMP(string _hex, string stream)
            {
                try
                {
                    //初始化
                    Bitmap outputBP = new Bitmap(stream);
                    ushort i = 0;
                    string[,] hexArray = new string[100, 100];

                    #region 将矩阵记空
                    for (int y = 0; y < 100; y++)
                    {
                        for (int x = 0; x < 100; x++)
                        {
                            hexArray[x, y] = null;//循环记空
                        }
                    }
                    #endregion

                    #region 把空格字符串变为矩阵
                    for (int y = 0; y < 100; y++)
                    {
                        for (int x = 0; x < 100; x++)
                        {

                            if (i < _hex.Split(' ').Length)
                            {
                                hexArray[x, y] = _hex.Split(' ')[i];
                            }
                            i++;
                        }
                    }
                    #endregion

                    #region 根据矩阵的值生成bmp图像
                    for (int y = 0; y < 100; y++)
                    {
                        for (int x = 0; x < 100; x++)
                        {
                            switch (hexArray[x, y])
                            {
                                case "0": outputBP.SetPixel(x, y, Color.FromArgb(000, 0, 0)); break;
                                case "1": outputBP.SetPixel(x, y, Color.FromArgb(010, 0, 0)); break;
                                case "2": outputBP.SetPixel(x, y, Color.FromArgb(020, 0, 0)); break;
                                case "3": outputBP.SetPixel(x, y, Color.FromArgb(030, 0, 0)); break;
                                case "4": outputBP.SetPixel(x, y, Color.FromArgb(040, 0, 0)); break;
                                case "5": outputBP.SetPixel(x, y, Color.FromArgb(050, 0, 0)); break;
                                case "6": outputBP.SetPixel(x, y, Color.FromArgb(060, 0, 0)); break;
                                case "7": outputBP.SetPixel(x, y, Color.FromArgb(070, 0, 0)); break;
                                case "8": outputBP.SetPixel(x, y, Color.FromArgb(080, 0, 0)); break;
                                case "9": outputBP.SetPixel(x, y, Color.FromArgb(090, 0, 0)); break;
                                case "a": outputBP.SetPixel(x, y, Color.FromArgb(100, 0, 0)); break;
                                case "b": outputBP.SetPixel(x, y, Color.FromArgb(110, 0, 0)); break;
                                case "c": outputBP.SetPixel(x, y, Color.FromArgb(120, 0, 0)); break;
                                case "d": outputBP.SetPixel(x, y, Color.FromArgb(130, 0, 0)); break;
                                case "e": outputBP.SetPixel(x, y, Color.FromArgb(140, 0, 0)); break;
                                case "f": outputBP.SetPixel(x, y, Color.FromArgb(150, 0, 0)); break;
                                case "A": outputBP.SetPixel(x, y, Color.FromArgb(100, 0, 0)); break;
                                case "B": outputBP.SetPixel(x, y, Color.FromArgb(110, 0, 0)); break;
                                case "C": outputBP.SetPixel(x, y, Color.FromArgb(120, 0, 0)); break;
                                case "D": outputBP.SetPixel(x, y, Color.FromArgb(130, 0, 0)); break;
                                case "E": outputBP.SetPixel(x, y, Color.FromArgb(140, 0, 0)); break;
                                case "F": outputBP.SetPixel(x, y, Color.FromArgb(150, 0, 0)); break;
                            }
                        }
                    }
                    #endregion

                    return outputBP;
                }
                catch
                {
                    return null;
                }
            }
            /// <summary>
            /// 根据16进制文本数据矩阵编译ANSW
            /// </summary>
            /// <param name="hexArray">16进制文本数据矩阵</param>
            /// <param name="stream">初始BMP图片模板的物理路径</param>
            /// <returns>返回Bitmap实例，错误则返回null</returns>
            private Bitmap arrayToBMP(string[,] hexArray, string stream)
            {
                try
                {
                    Bitmap outputBP = new Bitmap(stream);//初始化

                    //根据矩阵的值生成bmp图像
                    for (int y = 0; y < 100; y++)
                    {
                        for (int x = 0; x < 100; x++)
                        {
                            switch (hexArray[x, y])
                            {
                                case "0": outputBP.SetPixel(x, y, Color.FromArgb(000, 0, 0)); break;
                                case "1": outputBP.SetPixel(x, y, Color.FromArgb(010, 0, 0)); break;
                                case "2": outputBP.SetPixel(x, y, Color.FromArgb(020, 0, 0)); break;
                                case "3": outputBP.SetPixel(x, y, Color.FromArgb(030, 0, 0)); break;
                                case "4": outputBP.SetPixel(x, y, Color.FromArgb(040, 0, 0)); break;
                                case "5": outputBP.SetPixel(x, y, Color.FromArgb(050, 0, 0)); break;
                                case "6": outputBP.SetPixel(x, y, Color.FromArgb(060, 0, 0)); break;
                                case "7": outputBP.SetPixel(x, y, Color.FromArgb(070, 0, 0)); break;
                                case "8": outputBP.SetPixel(x, y, Color.FromArgb(080, 0, 0)); break;
                                case "9": outputBP.SetPixel(x, y, Color.FromArgb(090, 0, 0)); break;
                                case "a": outputBP.SetPixel(x, y, Color.FromArgb(100, 0, 0)); break;
                                case "b": outputBP.SetPixel(x, y, Color.FromArgb(110, 0, 0)); break;
                                case "c": outputBP.SetPixel(x, y, Color.FromArgb(120, 0, 0)); break;
                                case "d": outputBP.SetPixel(x, y, Color.FromArgb(130, 0, 0)); break;
                                case "e": outputBP.SetPixel(x, y, Color.FromArgb(140, 0, 0)); break;
                                case "f": outputBP.SetPixel(x, y, Color.FromArgb(150, 0, 0)); break;
                                case "A": outputBP.SetPixel(x, y, Color.FromArgb(100, 0, 0)); break;
                                case "B": outputBP.SetPixel(x, y, Color.FromArgb(110, 0, 0)); break;
                                case "C": outputBP.SetPixel(x, y, Color.FromArgb(120, 0, 0)); break;
                                case "D": outputBP.SetPixel(x, y, Color.FromArgb(130, 0, 0)); break;
                                case "E": outputBP.SetPixel(x, y, Color.FromArgb(140, 0, 0)); break;
                                case "F": outputBP.SetPixel(x, y, Color.FromArgb(150, 0, 0)); break;
                            }
                        }
                    }
                    return outputBP;
                }
                catch
                {
                    return null;
                }
            }

        }

        /// <summary>
        /// 排序算法类
        /// </summary>
        public class Sorter
        {
            /// <summary>
            /// 排序方法
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
        }

        /// <summary>
        /// 检索类
        /// </summary>
        public class Searcher
        {
            /// <summary>
            /// 二分法检索(第一重载),适用于整型检索
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
            /// 二分法检索(第二重载),适用于双精度浮点检索
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
}
