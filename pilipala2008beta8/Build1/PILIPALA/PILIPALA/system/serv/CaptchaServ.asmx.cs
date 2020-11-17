using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Web.Configuration;

/* 阿里云云盾测试引用项目 */
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.afs.Model.V20180112;

using WaterLibrary.com.CommentLake;
using WaterLibrary.stru.CommentLake;
using WaterLibrary.stru.MySQL;

namespace PILIPALA.system.serv
{
    /// <summary>
    /// Captcha 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    [System.Web.Script.Services.ScriptService]

    public class CaptchaServ : System.Web.Services.WebService
    {
        [WebMethod]
        public string CommentLakeCaptcha(string Token, string SessionId, string Sig, int PostID, int HEAD, string Content, string User, string Email, string WebSite)
        {
            string ACCESS_KEY = WebConfigurationManager.AppSettings["ACCESS_KEY"];
            string ACCESS_SECRET = WebConfigurationManager.AppSettings["ACCESS_SECRET"];

            //YOUR ACCESS_KEY、YOUR ACCESS_SECRET请替换成您的阿里云accesskey id和secret 
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", ACCESS_KEY, ACCESS_SECRET);
            IAcsClient client = new DefaultAcsClient(profile);

            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", "afs", "afs.aliyuncs.com");

            AuthenticateSigRequest request = new AuthenticateSigRequest
            {
                Token = Token,// 必填参数，从前端获取，不可更改
                SessionId = SessionId,// 必填参数，从前端获取，不可更改，android和ios只传这个参数即可
                Sig = Sig,// 必填参数，从前端获取，不可更改

                Scene = "nc_bbs",// 必填参数，从前端获取，不可更改
                AppKey = "FFFF0N0000000000987D",// 必填参数，后端填写
                RemoteIp = HttpContext.Current.Request.Url.Host// 必填参数，后端填写
            };

            try
            {
                AuthenticateSigResponse response = client.GetAcsResponse(request);// 返回code 100表示验签通过，900表示验签失败
                // TODO
                if (response.Code == 100)
                {
                    CommentLake CommentLake = new CommentLake(new MySqlConn
                    {
                        DataSource = WebConfigurationManager.AppSettings["DataSource"],
                        DataBase = WebConfigurationManager.AppSettings["DataBase"],
                        Port = WebConfigurationManager.AppSettings["Port"],
                        User = WebConfigurationManager.AppSettings["User"],
                        PWD = WebConfigurationManager.AppSettings["PWD"]
                    }, "comment_lake");


                    CommentLake.AddComment(new Comment()
                    {
                        PostID = PostID,
                        User = User,
                        Email = Email,
                        Content = Content,
                        WebSite = WebSite,
                        HEAD = HEAD
                    });

                    return "CommentLake : 提交成功";
                }
                else if (response.Code == 900)
                {
                    return "CommentLake : 安全验证未通过";
                }
                else
                {
                    return "CommentLake : 未知错误";
                }

            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}
