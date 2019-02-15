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

public partial class index : Page
{
    #region 字段初始化

    public postData topPostData;
    public pluginData topPluginData;

    public webioBridge serverWb = new webioBridge();

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        topPostData = serverWb.idxTopPost_get();
        topPluginData = serverWb.pluginData(topPostData.post_id);

        //设置页面标题
        Title = "Thaumy的博客|又一个码农的家";

        //输出文章列表
        idx_comnPstLst();
    }

    protected void loadBtn_Click(object sender, EventArgs e)//加载文章按钮按下事件
    {
        idx_comnPstLst();
    }

    /// <summary>
    /// 循环输出comn文章以组成列表
    /// </summary>
    public void idx_comnPstLst()
    {
        //替换index_comnPstLst标记文本为实际文章数据，循环输出到客户端
        foreach (StdLib.postData comnPostData in serverWb.idxComnPost_get())
        {
            string temp = Resources.index.index_comnPstLst;

            temp = temp.Replace("<!-- post_title -->", comnPostData.post_title);
            temp = temp.Replace("<!-- post_summary -->", comnPostData.post_summary);
            temp = temp.Replace("<!-- post_archive -->", comnPostData.post_archive);

            //文章标题添加链接
            temp = temp.Replace("<!-- post_title_href -->", "href=\"post.aspx?post_id=" + comnPostData.post_id.ToString() + "\"");

            #region 文章插件数据替换
            pluginData pluginData = serverWb.pluginData(comnPostData.post_id);

            temp = temp.Replace("<!-- count_review -->", pluginData.count_review.ToString());
            temp = temp.Replace("<!-- count_like -->", pluginData.count_like.ToString());
            temp = temp.Replace("<!-- count_read -->", pluginData.count_read.ToString());

            temp = temp.Replace("<!-- date_created -->", webioBridge.timeToStr(comnPostData.date_created));

            temp = temp.Replace("<!-- tagA -->", pluginData.tagA);
            temp = temp.Replace("<!-- tagB -->", pluginData.tagB);
            temp = temp.Replace("<!-- tagC -->", pluginData.tagC);

            //文章条带样式设定
            temp = temp.Replace("<!-- stripStyle -->", "style=\"" + webioBridge.stripStyle(pluginData.color_strip) + "\"");
            #endregion

            //输出到comnPstLst div
            comnPstLst.InnerHtml += temp;
        }
    }
}
