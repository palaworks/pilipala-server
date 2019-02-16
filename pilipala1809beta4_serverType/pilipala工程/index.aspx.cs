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

    #region 推进式加载参数

    private int beginPlace = 0;//推进起始位置
    private int length = 8;//推进间隔
    static private int i = 0;//推进计数

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        topPostData = serverWb.idxTopPost_get();
        topPluginData = serverWb.pluginData(topPostData.post_id);

        //设置页面标题
        Title = "Thaumy的博客|又一个码农的家";

        
        if (comnPstLst.InnerHtml == "")
        {
            i = beginPlace;//如果div为空，清空推进计数
            comnPstLst.InnerHtml = idx_comnPstLst(beginPlace, length);//如果div为空，先输出一段文章列表
            comnPstLst320.InnerHtml = idx_comnPstLst(i, length);
        }
        
    }
    
    protected void loadBtn_Click(object sender, EventArgs e)//加载文章按钮按下事件
    {
        i+= length;//点击加载按钮后，增加推进计数到下一间隔
        comnPstLst.InnerHtml += idx_comnPstLst(i, length);//将新加载的文章列表添加到列表尾部
        comnPstLst320.InnerHtml += idx_comnPstLst(i, length);
    }

    /// <summary>
    /// 返回comn文章列表html字符串，注意：出于某种未知原因，文章取完后不再返回文章列表数据，恰好符合开发需求。。。
    /// </summary>
    /// <param name="length">向后推进输出的个数（推进间隔）</param>
    public string idx_comnPstLst(int beginPlace, int length)
    {
        //汇总字符串，用于汇总获取到的html数据
        string all = null;
        //替换index_comnPstLst标记文本为实际文章数据，循环输出到客户端
        foreach (postData comnPostData in serverWb.idxComnPost_get(beginPlace, length))
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

            //插入到汇总字符串尾部
            all += temp;
        }

        return all;
    }
}
