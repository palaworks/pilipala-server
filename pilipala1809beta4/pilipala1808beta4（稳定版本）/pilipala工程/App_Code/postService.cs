using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using StdLib;
using StdLib.DataHandler;
using StdLib.FrameHandler;
using StdLib.LogicHandler;
using StdLib.ViewHandler.pilipala;

/// <summary>
/// post页面服务
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
[System.Web.Script.Services.ScriptService]
public class postService : System.Web.Services.WebService
{

    #region 字段初始化

    private webioBridge serviceWb = new webioBridge();
    private SetStatus setStatus = new SetStatus();
    private GetStatus getStatus = new GetStatus();

    #endregion


    public postService()
    {
        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]//喜欢按钮文本刷新
    public string txt_LikeBtn_Refresh(int post_id)
    {
        if (getStatus.cookie<bool>("isLike" + post_id.ToString()) == false)//如果客户端不喜欢
        {
            setStatus.setCookie("isLike" + post_id.ToString(), true);//设置喜欢
            return "您已表示支持";
        }
        else
        {
            setStatus.setCookie("isLike" + post_id.ToString(), false);//设置不喜欢
            return "支持这篇文章";
        }
    }

    [WebMethod]//喜欢计数刷新
    public int countLike_Refresh(int post_id, int old_count_like)
    {
        if (getStatus.cookie<bool>("isLike" + post_id.ToString()) == false)//如果客户端不喜欢
        {
            serviceWb.increase_count_like(post_id, 1);//数据库计数加一
            return old_count_like + 1;
        }
        else
        {
            serviceWb.increase_count_like(post_id, -1);
            return old_count_like - 1;
        }
    }
}
