using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;
using StdLib;
using StdLib.DataHandler;
using StdLib.FrameHandler;
using StdLib.LogicHandler;
using StdLib.ViewHandler.pilipala;
using System.Data;

public partial class post : Page
{

    #region 字段初始化

    public postData postData;
    public pluginData pluginData;

    public webioBridge serverWb = new webioBridge();
    private SetStatus setStatus = new SetStatus();
    private GetStatus getStatus = new GetStatus();

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        //获得请求的文章序列号
        int rqst_post_id = Convert.ToInt32(Request.Params["post_id"]);
        //根据文章序列号获得文章数据和文章插入数据
        postData = serverWb.pstPostData_get(rqst_post_id);
        pluginData = serverWb.pluginData(rqst_post_id);

        //访问状态Cookie过期时间设置
        int timeValue = 10;
        Response.Cookies["isCountReadLock" + postData.post_id.ToString()].Expires = DateTime.Now.AddSeconds(timeValue);
        Response.Cookies["isLike" + postData.post_id.ToString()].Expires = DateTime.Now.AddSeconds(timeValue);

        #region 访问计数增加

        if (getStatus.cookie<bool>("isCountReadLock" + postData.post_id.ToString()) == false)//如果客户端阅读锁未上锁
        {
            //计数加一并上锁
            serverWb.increase_count_read(postData.post_id, 1);
            setStatus.setCookie("isCountReadLock" + postData.post_id.ToString(), true);
        }
        else
        {
            //保持上锁状态
            setStatus.setCookie("isCountReadLock" + postData.post_id.ToString(), true);
        }

        #endregion

        #region 喜欢按钮初始化

        //使用Cookie索引会丢失数据（未知原因）
        if (getStatus.cookie<bool>("isLike" + postData.post_id.ToString()) == true)//如果客户端该文章的喜欢状态为是
        {
            txt_likeBtn.InnerText = "您已表示支持";
            //保持喜欢状态为喜欢
            setStatus.setCookie("isLike" + postData.post_id.ToString(), true);
        }
        else
        {
            txt_likeBtn.InnerText = "支持这篇文章";
            //因为Cookie对象（isLike）为null或者false都被视为不喜欢，所以此处可不必保持喜欢状态为不喜欢
        }

        #endregion

        #region 随机文章按钮初始化

        //目前该功能取到的数据不随机，有待改善
        DataRow r = serverWb.ramdom(postData.post_id);
        randomPostBtn.HRef = "post.aspx?post_id=" + r["post_id"].ToString();
        txt_randomPostBtn.InnerText = r["post_title"].ToString();

        #endregion

        if (postData.post_isReadOnly == true)
        {
            u10097.Style.Value = Resources.global.strip_readOnly;
        }
        else
        {
            u8685.Style.Value = webioBridge.stripStyle(pluginData.color_strip);
        }


        //加载计数
        countRead.InnerText = "阅读" + serverWb.pstPostCount_get(rqst_post_id).count_read.ToString() + "次";
        countLike.InnerText = pluginData.count_like.ToString();

        //设置页面标题
        Title = postData.post_title + "|Thaumy的博客";
    }

    protected void likeBtn_Click(object sender, EventArgs e)//喜欢按钮按下事件
    {
        postService pS = new postService();

        txt_likeBtn.InnerText = pS.txt_LikeBtn_Refresh(postData.post_id);//刷新按钮文本
        countLike.InnerText = pS.countLike_Refresh(postData.post_id, pluginData.count_like).ToString();//刷新喜欢计数
    }
}