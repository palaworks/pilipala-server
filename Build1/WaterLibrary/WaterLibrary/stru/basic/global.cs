using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WaterLibrary.com.basic;

namespace WaterLibrary.stru.basic
{
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
