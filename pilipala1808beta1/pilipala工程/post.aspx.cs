using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//将StdLib全部引用
using StdLib;
using StdLib.DataHandler;
using StdLib.FrameHandler;
using StdLib.LogicHandler;
using StdLib.ViewHandler.WebSite;

public partial class post : System.Web.UI.Page
{
    public postData bkstg_pD = new postData();

    protected void Page_Load(object sender, EventArgs e)
    {
        webioBridge wb = new webioBridge();

        bkstg_pD = wb.post_pDget(Request.Params["post_id"]);//从链接请求获得该文章对应的postData

        Page.DataBind();//执行数据绑定
    }
}