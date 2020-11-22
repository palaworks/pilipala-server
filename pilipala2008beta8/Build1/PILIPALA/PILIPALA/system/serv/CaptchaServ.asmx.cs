using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Web.Configuration;

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
        public string CommentLakeCaptcha(int PostID, int HEAD, string Content, string User, string Email, string WebSite)
        {
            CommentLake CommentLake = new CommentLake(new MySqlConnMsg
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
    }
}
