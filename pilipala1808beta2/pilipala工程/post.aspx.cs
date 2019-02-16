using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;

//将StdLib全部引用
using StdLib;
using StdLib.DataHandler;
using StdLib.FrameHandler;
using StdLib.LogicHandler;
using StdLib.ViewHandler.pilipala;

public partial class post : Page
{
    public postData postData;
    private webioBridge wb_b = new webioBridge();


    protected void Page_Load(object sender, EventArgs e)
    {
        //从链接请求获得该文章对应的postData
        postData = wb_b.postPost_get(Request.Params["post_id"]);

        //当前文章阅读计数增加1
        iniStateRead();
        if (Request.Cookies["state_read"][postData.post_id.ToString()] == "false")
        {
            wb_b.increase_count_read(postData.post_id, 1);
            Response.Cookies["state_read"][postData.post_id.ToString()] = "true";
        }

        #region 点赞功能

        //将当前文章的点赞计数写入Session对象
        Session["count_like"] = postData.count_like;
        iniLikeStateCookies();//初始化点赞状态
        //将当前文章的客户端点赞状态写入Session对象
        Session["state_like"] = Request.Cookies["count_like"][postData.post_id.ToString()];
        #endregion



    }
    protected void likeBtn_Click(object sender, EventArgs e)
    {
        if (Request.Cookies["count_like"][postData.post_id.ToString()] == "true")//如果Cookies点赞状态为true
        {
            /* 与下方注释原理相同，此处为已经点赞则减一记录，并更改为不点赞
             * “不点赞”表示不处于“点赞”的状态，并非具有差评功能
             */
            wb_b.increase_count_like(postData.post_id, -1);

            int count_like = Convert.ToInt32(Session["count_like"]) - 1;
            Session["count_like"] = count_like;

            Response.Cookies["count_like"][postData.post_id.ToString()] = "false";

            Session["state_like"] = "false";
        }
        else
        {
            //数据库点赞计数加一
            wb_b.increase_count_like(postData.post_id, 1);

            //取Session对象中的点赞计数，加一
            int count_like = Convert.ToInt32(Session["count_like"]) + 1;
            //将点赞计数存入Session对象，供稍后页面刷新时读取
            Session["count_like"] = count_like;

            //将点赞计数存入Cookies，以便保持客户端点赞状态
            Response.Cookies["count_like"][postData.post_id.ToString()] = "true";

            Session["state_like"] = "true";
        }
    }

    public string likeBtnState()
    {
        if (Session["state_like"].ToString() == "true")
        {
            return "已支持该文章";
        }
        else
        {
            return "支持这篇文章";
        }
    }

    /// <summary>
    /// 初始化Cookies点赞状态
    /// </summary>
    protected void iniLikeStateCookies()
    {
        try//试图读取客户端点赞状态，若不为true（null或false），则设置为false
        {
            if (Request.Cookies["count_like"][postData.post_id.ToString()] != "true")
            {
                Response.Cookies["count_like"][postData.post_id.ToString()] = "false";
            }
        }
        catch//点赞状态未设置导致报错，此时将点赞状态初始化为false
        {
            Response.Cookies["count_like"][postData.post_id.ToString()] = "false";
        }
        
    }
    protected void iniLikeStateSession()
    {
        try//试图读取客户端点赞状态，若不为true（null或false），则设置为false
        {
            if (Session["state_like"].ToString() != "true")
            {
                Session["state_like"] = "false";
            }
        }
        catch//点赞状态未设置导致报错，此时将点赞状态初始化为false
        {
            Session["state_like"] = "false";
        }

    }
    protected void iniStateRead()
    {
        try//试图读取客户端点赞状态，若不为true（null或false），则设置为false
        {
            if (Request.Cookies["state_read"][postData.post_id.ToString()] != "true")
            {
                Response.Cookies["state_read"][postData.post_id.ToString()] = "false";
            }
        }
        catch//点赞状态未设置导致报错，此时将点赞状态初始化为false
        {
            Response.Cookies["state_read"][postData.post_id.ToString()] = "false";
        }

    }
}