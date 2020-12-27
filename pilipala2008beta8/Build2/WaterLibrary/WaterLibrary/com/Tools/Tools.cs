using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
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
        /// 生成RSA密钥对
        /// </summary>
        /// <param name="keySize">密钥位数</param>
        /// <returns></returns>
        public static Dictionary<string, string> GenRSAKeyPair(int keySize)
        {
            var rsa = new RSA(keySize);

            return new Dictionary<string, string>
            {
                { "PRIVATE",  rsa.ToPEM().ToPEM_PKCS1()},
                { "PUBLIC",  rsa.ToPEM().ToPEM_PKCS8(true)}
            };
        }
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="PublicKey">RSA公钥（PEM）</param>
        /// <param name="PlainText">明文</param>
        /// <returns></returns>
        public static string RSAEncrypt(string PublicKey, string PlainText)
        {
            var rsa = new RSA(PublicKey);
            return rsa.Encode(PlainText);
        }
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="PrivateKey">RSA公私钥（PEM）</param>
        /// <param name="CipherText">密文</param>
        /// <returns></returns>
        public static string RSADecrypt(string PrivateKey, string CipherText)
        {
            var rsa = new RSA(PrivateKey);
            return rsa.DecodeOrNull(CipherText);
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
    /// RSA操作类
    /// GitHub: https://github.com/xiangyuecn/RSA-csharp
    /// </summary>
    internal class RSA
    {
        /// <summary>
        /// 导出XML格式密钥对，如果convertToPublic含私钥的RSA将只返回公钥，仅含公钥的RSA不受影响
        /// </summary>
        public string ToXML(bool convertToPublic = false)
        {
            return rsa.ToXmlString(!rsa.PublicOnly && !convertToPublic);
        }
        /// <summary>
        /// 将密钥对导出成PEM对象，如果convertToPublic含私钥的RSA将只返回公钥，仅含公钥的RSA不受影响
        /// </summary>
        public RSA_PEM ToPEM(bool convertToPublic = false)
        {
            return new RSA_PEM(rsa, convertToPublic);
        }




        /// <summary>
        /// 加密字符串（utf-8），出错抛异常
        /// </summary>
        public string Encode(string str)
        {
            return Convert.ToBase64String(Encode(Encoding.UTF8.GetBytes(str)));
        }
        /// <summary>
        /// 加密数据，出错抛异常
        /// </summary>
        public byte[] Encode(byte[] data)
        {
            int blockLen = rsa.KeySize / 8 - 11;
            if (data.Length <= blockLen)
            {
                return rsa.Encrypt(data, false);
            }

            using var dataStream = new MemoryStream(data);
            using var enStream = new MemoryStream();
            Byte[] buffer = new Byte[blockLen];
            int len = dataStream.Read(buffer, 0, blockLen);

            while (len > 0)
            {
                Byte[] block = new Byte[len];
                Array.Copy(buffer, 0, block, 0, len);

                Byte[] enBlock = rsa.Encrypt(block, false);
                enStream.Write(enBlock, 0, enBlock.Length);

                len = dataStream.Read(buffer, 0, blockLen);
            }

            return enStream.ToArray();
        }
        /// <summary>
        /// 解密字符串（utf-8），解密异常返回null
        /// </summary>
        public string DecodeOrNull(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return null;
            }
            byte[] byts = null;
            try { byts = Convert.FromBase64String(str); } catch { }
            if (byts == null)
            {
                return null;
            }
            var val = DecodeOrNull(byts);
            if (val == null)
            {
                return null;
            }
            return Encoding.UTF8.GetString(val);
        }
        /// <summary>
        /// 解密数据，解密异常返回null
        /// </summary>
        public byte[] DecodeOrNull(byte[] data)
        {
            try
            {
                int blockLen = rsa.KeySize / 8;
                if (data.Length <= blockLen)
                {
                    return rsa.Decrypt(data, false);
                }

                using var dataStream = new MemoryStream(data);
                using var deStream = new MemoryStream();
                Byte[] buffer = new Byte[blockLen];
                int len = dataStream.Read(buffer, 0, blockLen);

                while (len > 0)
                {
                    Byte[] block = new Byte[len];
                    Array.Copy(buffer, 0, block, 0, len);

                    Byte[] deBlock = rsa.Decrypt(block, false);
                    deStream.Write(deBlock, 0, deBlock.Length);

                    len = dataStream.Read(buffer, 0, blockLen);
                }

                return deStream.ToArray();
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 对str进行签名，并指定hash算法（如：SHA256）
        /// </summary>
        public string Sign(string hash, string str)
        {
            return Convert.ToBase64String(Sign(hash, Encoding.UTF8.GetBytes(str)));
        }
        /// <summary>
        /// 对data进行签名，并指定hash算法（如：SHA256）
        /// </summary>
        public byte[] Sign(string hash, byte[] data)
        {
            return rsa.SignData(data, hash);
        }
        /// <summary>
        /// 验证字符串str的签名是否是sgin，并指定hash算法（如：SHA256）
        /// </summary>
        public bool Verify(string hash, string sgin, string str)
        {
            byte[] byts = null;
            try { byts = Convert.FromBase64String(sgin); } catch { }
            if (byts == null)
            {
                return false;
            }
            return Verify(hash, byts, Encoding.UTF8.GetBytes(str));
        }
        /// <summary>
        /// 验证data的签名是否是sgin，并指定hash算法（如：SHA256）
        /// </summary>
        public bool Verify(string hash, byte[] sgin, byte[] data)
        {
            try
            {
                return rsa.VerifyData(data, hash, sgin);
            }
            catch
            {
                return false;
            }
        }




        private readonly RSACryptoServiceProvider rsa;
        /// <summary>
        /// 最底层的RSACryptoServiceProvider对象
        /// </summary>
        public RSACryptoServiceProvider RSAObject
        {
            get
            {
                return rsa;
            }
        }

        /// <summary>
        /// 密钥位数
        /// </summary>
        public int KeySize
        {
            get
            {
                return rsa.KeySize;
            }
        }
        /// <summary>
        /// 是否包含私钥
        /// </summary>
        public bool HasPrivate
        {
            get
            {
                return !rsa.PublicOnly;
            }
        }

        /// <summary>
        /// 用指定密钥大小创建一个新的RSA，出错抛异常
        /// </summary>
        public RSA(int keySize)
        {
            var rsaParams = new CspParameters
            {
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            rsa = new RSACryptoServiceProvider(keySize, rsaParams);
        }
        /// <summary>
        /// 通过一个pem文件创建RSA，pem为公钥或私钥，出错抛异常
        /// </summary>
        public RSA(string pem)
        {
            rsa = RSA_PEM.FromPEM(pem).GetRSA();
        }
        /// <summary>
        /// 通过一个pem对象创建RSA，pem为公钥或私钥，出错抛异常
        /// </summary>
        public RSA(RSA_PEM pem)
        {
            rsa = pem.GetRSA();
        }
        /// <summary>
        /// 本方法会先生成RSA_PEM再创建RSA：通过公钥指数和私钥指数构造一个PEM，会反推计算出P、Q但和原始生成密钥的P、Q极小可能相同
        /// 注意：所有参数首字节如果是0，必须先去掉
        /// 出错将会抛出异常
        /// </summary>
        /// <param name="modulus">必须提供模数</param>
        /// <param name="exponent">必须提供公钥指数</param>
        /// <param name="dOrNull">私钥指数可以不提供，导出的PEM就只包含公钥</param>
        public RSA(byte[] modulus, byte[] exponent, byte[] dOrNull)
        {
            rsa = new RSA_PEM(modulus, exponent, dOrNull).GetRSA();
        }
        /// <summary>
        /// 本方法会先生成RSA_PEM再创建RSA：通过全量的PEM字段数据构造一个PEM，除了模数modulus和公钥指数exponent必须提供外，其他私钥指数信息要么全部提供，要么全部不提供（导出的PEM就只包含公钥）
        /// 注意：所有参数首字节如果是0，必须先去掉
        /// </summary>
        public RSA(byte[] modulus, byte[] exponent, byte[] d, byte[] p, byte[] q, byte[] dp, byte[] dq, byte[] inverseQ)
        {
            rsa = new RSA_PEM(modulus, exponent, d, p, q, dp, dq, inverseQ).GetRSA();
        }
    }
    /// <summary>
    /// RSA PEM格式密钥对的解析和导出
    /// GitHub: https://github.com/xiangyuecn/RSA-csharp
    /// </summary>
    internal class RSA_PEM
    {
        /// <summary>
        /// modulus 模数n，公钥、私钥都有
        /// </summary>
        public byte[] Key_Modulus;
        /// <summary>
        /// publicExponent 公钥指数e，公钥、私钥都有
        /// </summary>
        public byte[] Key_Exponent;
        /// <summary>
        /// privateExponent 私钥指数d，只有私钥的时候才有
        /// </summary>
        public byte[] Key_D;

        //以下参数只有私钥才有 https://docs.microsoft.com/zh-cn/dotnet/api/system.security.cryptography.rsaparameters?redirectedfrom=MSDN&view=netframework-4.8
        /// <summary>
        /// prime1
        /// </summary>
        public byte[] Val_P;
        /// <summary>
        /// prime2
        /// </summary>
        public byte[] Val_Q;
        /// <summary>
        /// exponent1
        /// </summary>
        public byte[] Val_DP;
        /// <summary>
        /// exponent2
        /// </summary>
        public byte[] Val_DQ;
        /// <summary>
        /// coefficient
        /// </summary>
        public byte[] Val_InverseQ;

        private RSA_PEM() { }

        /// <summary>
        /// 通过RSA中的公钥和私钥构造一个PEM，如果convertToPublic含私钥的RSA将只读取公钥，仅含公钥的RSA不受影响
        /// </summary>
        public RSA_PEM(RSACryptoServiceProvider rsa, bool convertToPublic = false)
        {
            var isPublic = convertToPublic || rsa.PublicOnly;
            var param = rsa.ExportParameters(!isPublic);

            Key_Modulus = param.Modulus;
            Key_Exponent = param.Exponent;

            if (!isPublic)
            {
                Key_D = param.D;

                Val_P = param.P;
                Val_Q = param.Q;
                Val_DP = param.DP;
                Val_DQ = param.DQ;
                Val_InverseQ = param.InverseQ;
            }
        }
        /// <summary>
        /// 通过全量的PEM字段数据构造一个PEM，除了模数modulus和公钥指数exponent必须提供外，其他私钥指数信息要么全部提供，要么全部不提供（导出的PEM就只包含公钥）
        /// 注意：所有参数首字节如果是0，必须先去掉
        /// </summary>
        public RSA_PEM(byte[] modulus, byte[] exponent, byte[] d, byte[] p, byte[] q, byte[] dp, byte[] dq, byte[] inverseQ)
        {
            Key_Modulus = modulus;
            Key_Exponent = exponent;
            Key_D = BigL(d, modulus.Length);

            int keyLen = modulus.Length / 2;
            Val_P = BigL(p, keyLen);
            Val_Q = BigL(q, keyLen);
            Val_DP = BigL(dp, keyLen);
            Val_DQ = BigL(dq, keyLen);
            Val_InverseQ = BigL(inverseQ, keyLen);
        }
        /// <summary>
        /// 通过公钥指数和私钥指数构造一个PEM，会反推计算出P、Q但和原始生成密钥的P、Q极小可能相同
        /// 注意：所有参数首字节如果是0，必须先去掉
        /// 出错将会抛出异常
        /// </summary>
        /// <param name="modulus">必须提供模数</param>
        /// <param name="exponent">必须提供公钥指数</param>
        /// <param name="dOrNull">私钥指数可以不提供，导出的PEM就只包含公钥</param>
        public RSA_PEM(byte[] modulus, byte[] exponent, byte[] dOrNull)
        {
            Key_Modulus = modulus;//modulus
            Key_Exponent = exponent;//publicExponent

            if (dOrNull != null)
            {
                Key_D = BigL(dOrNull, modulus.Length);//privateExponent

                //反推P、Q
                BigInteger n = BigX(modulus);
                BigInteger e = BigX(exponent);
                BigInteger d = BigX(dOrNull);
                BigInteger p = FindFactor(e, d, n);
                BigInteger q = n / p;
                if (p.CompareTo(q) > 0)
                {
                    BigInteger t = p;
                    p = q;
                    q = t;
                }
                BigInteger exp1 = d % (p - BigInteger.One);
                BigInteger exp2 = d % (q - BigInteger.One);
                BigInteger coeff = BigInteger.ModPow(q, p - 2, p);

                int keyLen = modulus.Length / 2;
                Val_P = BigL(BigB(p), keyLen);//prime1
                Val_Q = BigL(BigB(q), keyLen);//prime2
                Val_DP = BigL(BigB(exp1), keyLen);//exponent1
                Val_DQ = BigL(BigB(exp2), keyLen);//exponent2
                Val_InverseQ = BigL(BigB(coeff), keyLen);//coefficient
            }
        }

        /// <summary>
        /// 密钥位数
        /// </summary>
        public int KeySize
        {
            get
            {
                return Key_Modulus.Length * 8;
            }
        }
        /// <summary>
        /// 是否包含私钥
        /// </summary>
        public bool HasPrivate
        {
            get
            {
                return Key_D != null;
            }
        }
        /// <summary>
        /// 将PEM中的公钥私钥转成RSA对象，如果未提供私钥，RSA中就只包含公钥
        /// </summary>
        public RSACryptoServiceProvider GetRSA()
        {
            var rsaParams = new CspParameters
            {
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            var rsa = new RSACryptoServiceProvider(rsaParams);

            var param = new RSAParameters
            {
                Modulus = Key_Modulus,
                Exponent = Key_Exponent
            };
            if (Key_D != null)
            {
                param.D = Key_D;
                param.P = Val_P;
                param.Q = Val_Q;
                param.DP = Val_DP;
                param.DQ = Val_DQ;
                param.InverseQ = Val_InverseQ;
            }
            rsa.ImportParameters(param);
            return rsa;
        }
        /// <summary>
        /// 转成正整数，如果是负数，需要加前导0转成正整数
        /// </summary>
        static public BigInteger BigX(byte[] bigb)
        {
            if (bigb[0] > 0x7F)
            {
                byte[] c = new byte[bigb.Length + 1];
                Array.Copy(bigb, 0, c, 1, bigb.Length);
                bigb = c;
            }
            return new BigInteger(bigb.Reverse().ToArray());//C#的二进制是反的
        }
        /// <summary>
        /// BigInt导出byte整数首字节>0x7F的会加0前导，保证正整数，因此需要去掉0
        /// </summary>
        static public byte[] BigB(BigInteger bigx)
        {
            byte[] val = bigx.ToByteArray().Reverse().ToArray();//C#的二进制是反的
            if (val[0] == 0)
            {
                byte[] c = new byte[val.Length - 1];
                Array.Copy(val, 1, c, 0, c.Length);
                val = c;
            }
            return val;
        }
        /// <summary>
        /// 某些密钥参数可能会少一位（32个byte只有31个，目测是密钥生成器的问题，只在c#生成的密钥中发现这种参数，java中生成的密钥没有这种现象），直接修正一下就行；这个问题与BigB有本质区别，不能动BigB
        /// </summary>
        static public byte[] BigL(byte[] bytes, int keyLen)
        {
            if (keyLen - bytes.Length == 1)
            {
                byte[] c = new byte[bytes.Length + 1];
                Array.Copy(bytes, 0, c, 1, bytes.Length);
                bytes = c;
            }
            return bytes;
        }
        /// <summary>
        /// 由n e d 反推 P Q 
        /// 资料： https://stackoverflow.com/questions/43136036/how-to-get-a-rsaprivatecrtkey-from-a-rsaprivatekey
        /// https://v2ex.com/t/661736
        /// </summary>
        static private BigInteger FindFactor(BigInteger e, BigInteger d, BigInteger n)
        {
            BigInteger edMinus1 = e * d - BigInteger.One;
            int s = -1;
            if (edMinus1 != BigInteger.Zero)
            {
                s = (int)(BigInteger.Log(edMinus1 & -edMinus1) / BigInteger.Log(2));
            }
            BigInteger t = edMinus1 >> s;

            long now = DateTime.Now.Ticks;
            for (int aInt = 2; true; aInt++)
            {
                if (aInt % 10 == 0 && DateTime.Now.Ticks - now > 3000 * 10000)
                {
                    throw new Exception("推算RSA.P超时");//测试最多循环2次，1024位的速度很快 8ms
                }

                BigInteger aPow = BigInteger.ModPow(new BigInteger(aInt), t, n);
                for (int i = 1; i <= s; i++)
                {
                    if (aPow == BigInteger.One)
                    {
                        break;
                    }
                    if (aPow == n - BigInteger.One)
                    {
                        break;
                    }
                    BigInteger aPowSquared = aPow * aPow % n;
                    if (aPowSquared == BigInteger.One)
                    {
                        return BigInteger.GreatestCommonDivisor(aPow - BigInteger.One, n);
                    }
                    aPow = aPowSquared;
                }
            }
        }











        /// <summary>
        /// 用PEM格式密钥对创建RSA，支持PKCS#1、PKCS#8格式的PEM
        /// 出错将会抛出异常
        /// </summary>
        static public RSA_PEM FromPEM(string pem)
        {
            RSA_PEM param = new RSA_PEM();

            var base64 = _PEMCode.Replace(pem, "");
            byte[] data = null;
            try { data = Convert.FromBase64String(base64); } catch { }
            if (data == null)
            {
                throw new Exception("PEM内容无效");
            }
            var idx = 0;

            //读取长度
            int readLen(byte first)
            {
                if (data[idx] == first)
                {
                    idx++;
                    if (data[idx] == 0x81)
                    {
                        idx++;
                        return data[idx++];
                    }
                    else if (data[idx] == 0x82)
                    {
                        idx++;
                        return (((int)data[idx++]) << 8) + data[idx++];
                    }
                    else if (data[idx] < 0x80)
                    {
                        return data[idx++];
                    }
                }
                throw new Exception("PEM未能提取到数据");
            }
            //读取块数据
            byte[] readBlock()
            {
                var len = readLen(0x02);
                if (data[idx] == 0x00)
                {
                    idx++;
                    len--;
                }
                var val = new byte[len];
                for (var i = 0; i < len; i++)
                {
                    val[i] = data[idx + i];
                }
                idx += len;
                return val;
            }
            //比较data从idx位置开始是否是byts内容
            bool eq(byte[] byts)
            {
                for (var i = 0; i < byts.Length; i++, idx++)
                {
                    if (idx >= data.Length)
                    {
                        return false;
                    }
                    if (byts[i] != data[idx])
                    {
                        return false;
                    }
                }
                return true;
            }




            if (pem.Contains("PUBLIC KEY"))
            {
                /****使用公钥****/
                //读取数据总长度
                readLen(0x30);

                //检测PKCS8
                var idx2 = idx;
                if (eq(_SeqOID))
                {
                    //读取1长度
                    readLen(0x03);
                    idx++;//跳过0x00
                          //读取2长度
                    readLen(0x30);
                }
                else
                {
                    idx = idx2;
                }

                //Modulus
                param.Key_Modulus = readBlock();

                //Exponent
                param.Key_Exponent = readBlock();
            }
            else if (pem.Contains("PRIVATE KEY"))
            {
                /****使用私钥****/
                //读取数据总长度
                readLen(0x30);

                //读取版本号
                if (!eq(_Ver))
                {
                    throw new Exception("PEM未知版本");
                }

                //检测PKCS8
                var idx2 = idx;
                if (eq(_SeqOID))
                {
                    //读取1长度
                    readLen(0x04);
                    //读取2长度
                    readLen(0x30);

                    //读取版本号
                    if (!eq(_Ver))
                    {
                        throw new Exception("PEM版本无效");
                    }
                }
                else
                {
                    idx = idx2;
                }

                //读取数据
                param.Key_Modulus = readBlock();
                param.Key_Exponent = readBlock();
                int keyLen = param.Key_Modulus.Length;
                param.Key_D = BigL(readBlock(), keyLen);
                keyLen /= 2;
                param.Val_P = BigL(readBlock(), keyLen);
                param.Val_Q = BigL(readBlock(), keyLen);
                param.Val_DP = BigL(readBlock(), keyLen);
                param.Val_DQ = BigL(readBlock(), keyLen);
                param.Val_InverseQ = BigL(readBlock(), keyLen);
            }
            else
            {
                throw new Exception("pem需要BEGIN END标头");
            }

            return param;
        }
        static private readonly Regex _PEMCode = new Regex(@"--+.+?--+|\s+");
        static private readonly byte[] _SeqOID = new byte[] { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
        static private readonly byte[] _Ver = new byte[] { 0x02, 0x01, 0x00 };







        /// <summary>
        /// 将RSA中的密钥对转换成PEM PKCS#1格式
        /// 。convertToPublic：等于true时含私钥的RSA将只返回公钥，仅含公钥的RSA不受影响
        /// 。公钥如：-----BEGIN RSA PUBLIC KEY-----，私钥如：-----BEGIN RSA PRIVATE KEY-----
        /// 。似乎导出PKCS#1公钥用的比较少，PKCS#8的公钥用的多些，私钥#1#8都差不多
        /// </summary>
        public string ToPEM_PKCS1(bool convertToPublic = false)
        {
            return ToPEM(convertToPublic, false, false);
        }
        /// <summary>
        /// 将RSA中的密钥对转换成PEM PKCS#8格式
        /// 。convertToPublic：等于true时含私钥的RSA将只返回公钥，仅含公钥的RSA不受影响
        /// 。公钥如：-----BEGIN PUBLIC KEY-----，私钥如：-----BEGIN PRIVATE KEY-----
        /// </summary>
        public string ToPEM_PKCS8(bool convertToPublic = false)
        {
            return ToPEM(convertToPublic, true, true);
        }
        /// <summary>
        /// 将RSA中的密钥对转换成PEM格式
        /// 。convertToPublic：等于true时含私钥的RSA将只返回公钥，仅含公钥的RSA不受影响
        /// 。privateUsePKCS8：私钥的返回格式，等于true时返回PKCS#8格式（-----BEGIN PRIVATE KEY-----），否则返回PKCS#1格式（-----BEGIN RSA PRIVATE KEY-----），返回公钥时此参数无效；两种格式使用都比较常见
        /// 。publicUsePKCS8：公钥的返回格式，等于true时返回PKCS#8格式（-----BEGIN PUBLIC KEY-----），否则返回PKCS#1格式（-----BEGIN RSA PUBLIC KEY-----），返回私钥时此参数无效；一般用的多的是true PKCS#8格式公钥，PKCS#1格式似乎比较少见公钥
        /// </summary>
        public string ToPEM(bool convertToPublic, bool privateUsePKCS8, bool publicUsePKCS8)
        {
            //https://www.jianshu.com/p/25803dd9527d
            //https://www.cnblogs.com/ylz8401/p/8443819.html
            //https://blog.csdn.net/jiayanhui2877/article/details/47187077
            //https://blog.csdn.net/xuanshao_/article/details/51679824
            //https://blog.csdn.net/xuanshao_/article/details/51672547

            var ms = new MemoryStream();
            //写入一个长度字节码
            void writeLenByte(int len)
            {
                if (len < 0x80)
                {
                    ms.WriteByte((byte)len);
                }
                else if (len <= 0xff)
                {
                    ms.WriteByte(0x81);
                    ms.WriteByte((byte)len);
                }
                else
                {
                    ms.WriteByte(0x82);
                    ms.WriteByte((byte)(len >> 8 & 0xff));
                    ms.WriteByte((byte)(len & 0xff));
                }
            }
            //写入一块数据
            void writeBlock(byte[] byts)
            {
                var addZero = (byts[0] >> 4) >= 0x8;
                ms.WriteByte(0x02);
                var len = byts.Length + (addZero ? 1 : 0);
                writeLenByte(len);

                if (addZero)
                {
                    ms.WriteByte(0x00);
                }
                ms.Write(byts, 0, byts.Length);
            }
            //根据后续内容长度写入长度数据
            byte[] writeLen(int index, byte[] byts)
            {
                var len = byts.Length - index;

                ms.SetLength(0);
                ms.Write(byts, 0, index);
                writeLenByte(len);
                ms.Write(byts, index, len);

                return ms.ToArray();
            }
            void writeAll(MemoryStream stream, byte[] byts)
            {
                stream.Write(byts, 0, byts.Length);
            }
            string TextBreak(string text, int line)
            {
                var idx = 0;
                var len = text.Length;
                var str = new StringBuilder();
                while (idx < len)
                {
                    if (idx > 0)
                    {
                        str.Append('\n');
                    }
                    if (idx + line >= len)
                    {
                        str.Append(text[idx..]);
                    }
                    else
                    {
                        str.Append(text.Substring(idx, line));
                    }
                    idx += line;
                }
                return str.ToString();
            }


            if (Key_D == null || convertToPublic)
            {
                /****生成公钥****/

                //写入总字节数，不含本段长度，额外需要24字节的头，后续计算好填入
                ms.WriteByte(0x30);
                var index1 = (int)ms.Length;

                //PKCS8 多一段数据
                int index2 = -1, index3 = -1;
                if (publicUsePKCS8)
                {
                    //固定内容
                    // encoded OID sequence for PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
                    writeAll(ms, _SeqOID);

                    //从0x00开始的后续长度
                    ms.WriteByte(0x03);
                    index2 = (int)ms.Length;
                    ms.WriteByte(0x00);

                    //后续内容长度
                    ms.WriteByte(0x30);
                    index3 = (int)ms.Length;
                }

                //写入Modulus
                writeBlock(Key_Modulus);

                //写入Exponent
                writeBlock(Key_Exponent);


                //计算空缺的长度
                var byts = ms.ToArray();

                if (index2 != -1)
                {
                    byts = writeLen(index3, byts);
                    byts = writeLen(index2, byts);
                }
                byts = writeLen(index1, byts);


                var flag = " PUBLIC KEY";
                if (!publicUsePKCS8)
                {
                    flag = " RSA" + flag;
                }
                return "-----BEGIN" + flag + "-----\n" + TextBreak(Convert.ToBase64String(byts), 64) + "\n-----END" + flag + "-----";
            }
            else
            {
                /****生成私钥****/

                //写入总字节数，后续写入
                ms.WriteByte(0x30);
                int index1 = (int)ms.Length;

                //写入版本号
                writeAll(ms, _Ver);

                //PKCS8 多一段数据
                int index2 = -1, index3 = -1;
                if (privateUsePKCS8)
                {
                    //固定内容
                    writeAll(ms, _SeqOID);

                    //后续内容长度
                    ms.WriteByte(0x04);
                    index2 = (int)ms.Length;

                    //后续内容长度
                    ms.WriteByte(0x30);
                    index3 = (int)ms.Length;

                    //写入版本号
                    writeAll(ms, _Ver);
                }

                //写入数据
                writeBlock(Key_Modulus);
                writeBlock(Key_Exponent);
                writeBlock(Key_D);
                writeBlock(Val_P);
                writeBlock(Val_Q);
                writeBlock(Val_DP);
                writeBlock(Val_DQ);
                writeBlock(Val_InverseQ);


                //计算空缺的长度
                var byts = ms.ToArray();

                if (index2 != -1)
                {
                    byts = writeLen(index3, byts);
                    byts = writeLen(index2, byts);
                }
                byts = writeLen(index1, byts);


                var flag = " PRIVATE KEY";
                if (!privateUsePKCS8)
                {
                    flag = " RSA" + flag;
                }
                return "-----BEGIN" + flag + "-----\n" + TextBreak(Convert.ToBase64String(byts), 64) + "\n-----END" + flag + "-----";
            }
        }















        /// <summary>
        /// 将XML格式密钥转成PEM，支持公钥xml、私钥xml
        /// 出错将会抛出异常
        /// </summary>
        static public RSA_PEM FromXML(string xml)
        {
            RSA_PEM rtv = new RSA_PEM();

            Match xmlM = xmlExp.Match(xml);
            if (!xmlM.Success)
            {
                throw new Exception("XML内容不符合要求");
            }

            Match tagM = xmlTagExp.Match(xmlM.Groups[1].Value);
            while (tagM.Success)
            {
                string tag = tagM.Groups[1].Value;
                string b64 = tagM.Groups[2].Value;
                byte[] val = Convert.FromBase64String(b64);
                switch (tag)
                {
                    case "Modulus": rtv.Key_Modulus = val; break;
                    case "Exponent": rtv.Key_Exponent = val; break;
                    case "D": rtv.Key_D = val; break;

                    case "P": rtv.Val_P = val; break;
                    case "Q": rtv.Val_Q = val; break;
                    case "DP": rtv.Val_DP = val; break;
                    case "DQ": rtv.Val_DQ = val; break;
                    case "InverseQ": rtv.Val_InverseQ = val; break;
                }
                tagM = tagM.NextMatch();
            }

            if (rtv.Key_Modulus == null || rtv.Key_Exponent == null)
            {
                throw new Exception("XML公钥丢失");
            }
            if (rtv.Key_D != null)
            {
                if (rtv.Val_P == null || rtv.Val_Q == null || rtv.Val_DP == null || rtv.Val_DQ == null || rtv.Val_InverseQ == null)
                {
                    return new RSA_PEM(rtv.Key_Modulus, rtv.Key_Exponent, rtv.Key_D);
                }
            }

            return rtv;
        }
        static private readonly Regex xmlExp = new Regex("\\s*<RSAKeyValue>([<>\\/\\+=\\w\\s]+)</RSAKeyValue>\\s*");
        static private readonly Regex xmlTagExp = new Regex("<(.+?)>\\s*([^<]+?)\\s*</");





        /// <summary>
        /// 将RSA中的密钥对转换成XML格式
        /// ，如果convertToPublic含私钥的RSA将只返回公钥，仅含公钥的RSA不受影响
        /// </summary>
        public string ToXML(bool convertToPublic)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<RSAKeyValue>");
            str.Append("<Modulus>" + Convert.ToBase64String(Key_Modulus) + "</Modulus>");
            str.Append("<Exponent>" + Convert.ToBase64String(Key_Exponent) + "</Exponent>");
            if (Key_D == null || convertToPublic)
            {
                /****生成公钥****/
                //NOOP
            }
            else
            {
                /****生成私钥****/
                str.Append("<P>" + Convert.ToBase64String(Val_P) + "</P>");
                str.Append("<Q>" + Convert.ToBase64String(Val_Q) + "</Q>");
                str.Append("<DP>" + Convert.ToBase64String(Val_DP) + "</DP>");
                str.Append("<DQ>" + Convert.ToBase64String(Val_DQ) + "</DQ>");
                str.Append("<InverseQ>" + Convert.ToBase64String(Val_InverseQ) + "</InverseQ>");
                str.Append("<D>" + Convert.ToBase64String(Key_D) + "</D>");
            }
            str.Append("</RSAKeyValue>");
            return str.ToString();
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
        /// 将可迭代对象的元素值按照分隔符合并为一个字符串
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
        /// <param name="keySize">密钥位数</param>
        /// <param name="IS_INIT">是否在初始化时自动填充密钥对</param>
        public KeyPair(int keySize = 2048, bool IS_INIT = false)
        {
            if (IS_INIT == true)
            {
                Dictionary<string, string> KeyPair = MathH.GenRSAKeyPair(keySize);
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
