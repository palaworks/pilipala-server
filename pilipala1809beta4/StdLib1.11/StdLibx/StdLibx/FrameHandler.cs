using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;//系统IO命名空间
using System.Net;//网页操作命名空间
using System.Drawing;//GDI+命名空间
using System.Web.Script.Serialization;//json解析

namespace StdLib
{
    namespace FrameHandler//版本控制
    {
        /// <summary>
        /// 版本控制限定
        /// </summary>
        public interface IStdLibFrame
        {
            /// <summary>
            /// 版本
            /// </summary>
            int projectVer
            {
                get;
            }

            /// <summary>
            /// 版本名字对象
            /// </summary>
            string projectMoniker
            {
                get;
            }

            /// <summary>
            /// 版本类型
            /// </summary>
            string editionType
            {
                get;
            }

            /// <summary>
            /// 步进
            /// </summary>
            string stepping
            {
                get;
            }

            /// <summary>
            /// 目标框架
            /// </summary>
            string targetFramework
            {
                get;
            }

            /// <summary>
            /// 目标框架名字对象
            /// </summary>
            string targetFrameworkMoniker
            {
                get;
            }

            /// <summary>
            /// 对最近一次pub版本的全局兼容性
            /// </summary>
            bool compat
            {
                get;
            }

            /// <summary>
            /// 适用平台
            /// </summary>
            string platform
            {
                get;
            }
        }

        /// <summary>
        /// 用于获取类库信息的类
        /// </summary>
        public sealed class LibInformation : IStdLibFrame
        {
            #region 字段 私有
            private static string spareInfUrl;//替补URL，内置HlinfURL失效时用于替补

            private static string _HlinfURL = "https://thaumy.github.io/StdLib1x/xplore/st111_Hlinf.html";

            private int _projectVer = 111;//1.11

            private string _projectMoniker = "st111";

            private string _editionType = "public";

            private string _stepping = "a0";

            private string _targetFramework = ".NET Framework";

            private string _targetFrameworkMoniker = "net452";

            private bool _compat = false;

            private string _paltform = "x86";

            //非接口次要内容
            private string _isNewVer;//使用Hlinf获得的该字段为string值，使用Jlinf则为bool值

            private string _newVerDownloadURL;

            private string _thisVerDownloadURL;
            #endregion
            #region 字段 访问器

            /// <summary>
            /// 版本
            /// </summary>
            public int projectVer
            {
                get { return _projectVer; }
            }

            /// <summary>
            /// 版本名字对象
            /// </summary>
            public string projectMoniker
            {
                get { return _projectMoniker; }
            }

            /// <summary>
            /// 版本类型
            /// </summary>
            public string editionType
            {
                get { return _editionType; }
            }

            /// <summary>
            /// 步进
            /// </summary>
            public string stepping
            {
                get { return _stepping; }
            }

            /// <summary>
            /// 类库的目标框架
            /// </summary>
            public string targetFramework
            {
                get { return _targetFramework; }
            }

            /// <summary>
            /// 类库的目标框架名字对象
            /// </summary>
            public string targetFrameworkMoniker
            {
                get { return _targetFrameworkMoniker; }
            }

            /// <summary>
            /// 针对最近一次发行版的全局兼容性
            /// </summary>
            public bool compat
            {
                get { return _compat; }
            }

            /// <summary>
            /// 适用平台
            /// </summary>
            public string platform
            {
                get { return _paltform; }
            }

            /// <summary>
            /// 是否为最新pub版本
            /// </summary>
            public string isNewVer
            {
                get { return _isNewVer; }
            }

            /// <summary>
            /// 最新pub版本下载URL
            /// </summary>
            public string newVerDownloadURL
            {
                get { return _newVerDownloadURL; }
            }

            /// <summary>
            /// 当前版本下载URL
            /// </summary>
            public string thisVerDownloadURL
            {
                get { return _thisVerDownloadURL; }
            }

            /// <summary>
            /// 内置于当前版本的Hlinf信息获取URL
            /// </summary>
            public string HlinfURL
            {
                get { return _HlinfURL; }
            }

            /// <summary>
            /// 获取到像素化的StdLib_logo
            /// </summary>
            public Bitmap logo
            {
                get { return StdLibx.Resource1.StdLib_logo; }
            }

            #endregion

            /// <summary>
            /// 初始化LibInformation
            /// </summary>
            /// <param name="spareInfUrl">必须的值：用于内置联网信息获取Url失效时进行替补的Url</param>
            public LibInformation(string spareInfUrl)
            {
                LibInformation.spareInfUrl = spareInfUrl;

                _isNewVer = getHtmlLibInformation()[0];//得到是否为最新pub版本
                _newVerDownloadURL = getHtmlLibInformation()[1];//得到最新版本下载URL

                _thisVerDownloadURL = getHtmlLibInformation()[2];//得到当前版本下载URL
            }

            /// <summary>
            /// 被重写的WebClient
            /// </summary>
            public class OWebClient : WebClient
            {
                /// <summary>
                /// WebRequest请求访问的超时时间
                /// </summary>
                public int timeOut { get; set; }

                /// <summary>
                /// 得到WebRwquest请求实例并附加超时时间的方法
                /// </summary>
                /// <param name="uri">统一资源标识符对象</param>
                /// <returns>返回WebRequests实例(注意！该部分代码尚不安全，因为它没有建立报错处理机制)</returns>
                protected override WebRequest GetWebRequest(Uri uri)
                {
                    WebRequest request = base.GetWebRequest(uri);
                    request.Timeout = timeOut;//设置超时时间

                    return request;
                }
            }

            /// <summary>
            /// 获取url指定的OWebClient所赋值的StreamReader对象（重载一）（UTF8编码模式）
            /// </summary>
            /// <param name="url">被OWebClient指定的url</param>
            /// <returns>返回StreamReader，报错返回null</returns>
            public static StreamReader getStreamReader(string url)
            {
                try
                {
                    //安全套接字协议
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    using (OWebClient Client = new OWebClient())//调用完成释放资源
                    {
                        Client.timeOut = 5000;
                        return new StreamReader(Client.OpenRead(url), Encoding.UTF8);//使用UTF8编码模式解码，然后返回流对象
                    }
                }
                catch
                {
                    return null;
                }

            }

            /// <summary>
            /// 获取url指定的OWebClient所赋值的StreamReader对象（重载二）
            /// </summary>
            /// <param name="url">被OWebClient指定的url</param>
            /// <param name="encodingType">使用的编码模式</param>
            /// <returns>返回StreamReader，报错返回null</returns>
            public static StreamReader getStreamReader(string url, string encodingType)
            {
                try
                {
                    //安全套接字协议
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    using (OWebClient Client = new OWebClient())//调用完成释放资源
                    {
                        Client.timeOut = 5000;
                        return new StreamReader(Client.OpenRead(url), Encoding.GetEncoding(encodingType));//使用UTF8编码模式解码，然后返回流对象
                    }
                }
                catch
                {
                    return null;
                }

            }

            /// <summary>
            /// 获取联网信息
            /// （注意：一旦取到空值，该方法将自动返回自定义URL所取得的信息（重载二被执行））
            /// </summary>
            /// <returns>返回获得的信息，报错返回null</returns>
            private static string[] getHtmlLibInformation()
            {
                try
                {
                    string[] result = getStreamReader(_HlinfURL).ReadToEnd().Split('$');

                    if (result == null || result[0] == null || result[1] == null || result[2] == null)//如果获取到空值
                    {
                        return getHtmlLibInformation(spareInfUrl);//返回重载二
                    }
                    else { return result; }//返回查询结果
                }
                catch
                {
                    return getHtmlLibInformation(spareInfUrl);//返回重载二
                }

            }

            /// <summary>
            /// 获取联网信息（重载二）
            /// </summary>
            /// <param name="url">信息所在的URL地址</param>
            /// <returns>返回获得的信息，报错返回“string[] { "null", "null" }”数组</returns>
            private static string[] getHtmlLibInformation(string url)
            {
                try
                {
                    return getStreamReader(url).ReadToEnd().Split('$'); ;
                }
                catch
                {
                    return new string[] { "null", "null" };
                }
            }

            /// <summary>
            /// 通过json文件获得类库信息（重载一）
            /// </summary>
            ///<param name="url">json文件所在url</param>
            /// <returns>返回存有类库信息的JlinfObject，错误则返回null</returns>
            public JlinfObject getJsonLibInformation(string url)
            {
                try
                {
                    //序列化/反序列化对象JavaScriptSerializer
                    //转换json为JlinfObject对象并返回
                    return new JavaScriptSerializer().Deserialize<JlinfObject>(getStreamReader(url).ReadToEnd());
                }
                catch
                {
                    return null;
                }
            }

            /// <summary>
            /// 通过json文件获得类库信息（重载二）
            /// </summary>
            /// <param name="filePath">json文件所在的本地物理路径</param>
            /// <param name="bufferSize">文件流缓冲区大小，默认值可填4096</param>
            /// <param name="useAsync">使用异步初始化文件流，缺乏设计的异步调用会慢于串行调用</param>
            /// <returns>返回存有类库信息的JlinfObject，错误则返回null</returns>
            public JlinfObject getJsonLibInformation(string filePath, int bufferSize, bool useAsync)
            {
                try
                {
                    using (
                        StreamReader StreamReader = new StreamReader//流读取对象
                        (new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Write, bufferSize, useAsync)//文件流对象
                        , Encoding.GetEncoding("unicode"))//指定编码模式，为保证兼容，这里使用unicode
                            )
                    {
                        //序列化/反序列化对象JavaScriptSerializer
                        //转换json为JlinfObject对象并返回
                        return new JavaScriptSerializer().Deserialize<JlinfObject>(StreamReader.ReadToEnd().ToString());
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

    }
}
