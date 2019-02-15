using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI;
using System.Data;
using StdLib;
using StdLib.DataHandler;
using StdLib.FrameHandler;
using StdLib.LogicHandler;
using StdLib.ViewHandler.pilipala;

/// <summary>
/// 为StdLib与网页沟通提供IO桥接
/// </summary>
public class webioBridge : Page
{
    private connStr cS;
    private MySqlConnectionHandler MyCH = new MySqlConnectionHandler();
    private GetPost getPost;
    private SetPost setPost;
    public webioBridge()
    {
        cS.dataSource = "localhost";
        cS.userName = "root";
        cS.password = "1246265280";
        cS.port = "3306";

        //启动MySql数据库控制器
        MyCH.start(cS);

        getPost = new GetPost(MyCH, "dbname");//启动Post获得器
        setPost = new SetPost(MyCH, "dbname");//启动Post编辑器
    }

    public string fileToStr(string url)
    {
        FileHandler FH = new FileHandler();
        return FH.fileToStr(Server.MapPath("") + url);
    }

    /// <summary>
    /// 获取普通文列数据
    /// </summary>
    /// <returns></returns>
    public List<postData> idxComnPost_get()
    {
        return getPost.idx_comnPost();
    }

    /// <summary>
    /// 获取置顶文章数据 注意，只允许一个置顶文章出现在数据库中，一个以上的置顶文章会造成取文错误
    /// </summary>
    /// <returns></returns>
    public postData idxTopPost_get()
    {
        return getPost.idx_topPost();
    }

    /// <summary>
    /// 获取菜单文列数据
    /// </summary>
    /// <returns></returns>
    public List<postData> idxMenuPost_get()
    {
        return getPost.idx_menuPost();
    }

    /// <summary>
    /// 获得post页面的文章数据
    /// </summary>
    /// <returns></returns>
    public postData pstPostData_get(string post_id)
    {
        return getPost.pst_postData(post_id);
    }

    /// <summary>
    /// 获得post页面的计数数据
    /// </summary>
    /// <returns></returns>
    public postData pstPostCount_get(string post_id)
    {
        return getPost.pst_postCount(post_id);
    }

    /// <summary>
    /// 随机取一个展示可用的文章数据（序列号和标题）
    /// </summary>
    /// <returns></returns>
    public DataRow ramdom_post(int post_id)
    {
        return getPost.random_post(post_id);
    }

    /// <summary>
    /// 文章浏览计数增加
    /// </summary>
    /// <param name="post_id">文章序列号</param>
    /// <param name="value">设置值</param>
    /// <returns></returns>
    public bool increase_count_read(int post_id, int increaseValue)
    {
        locateStr ls;
        ls.dataBaseName = getPost.dataBaseName;
        ls.tableName = "st_posts";
        ls.whereColumnName = "post_id";
        ls.targetColumnName = "count_read";

        int newValue = Convert.ToInt32(MyCH.getColumnValue(ls, post_id.ToString())) + increaseValue;

        return setPost.count_read(post_id, newValue);
    }

    /// <summary>
    /// 文章浏览计数增加
    /// </summary>
    /// <param name="post_id">文章序列号</param>
    /// <param name="value">设置值</param>
    /// <returns></returns>
    public bool increase_count_like(int post_id, int increaseValue)
    {
        locateStr ls;
        ls.dataBaseName = getPost.dataBaseName;
        ls.tableName = "st_posts";
        ls.whereColumnName = "post_id";
        ls.targetColumnName = "count_like";

        int newValue = Convert.ToInt32(MyCH.getColumnValue(ls, post_id.ToString())) + increaseValue;

        return setPost.count_like(post_id, newValue);
    }

    /// <summary>
    /// 时间数据转字符串
    /// </summary>
    /// <param name="DateTime">文列时间原始时间戳数据</param>
    /// <returns></returns>
    public string timeToStr(DateTime DateTime)
    {
        //年份字符串只取后二位，例：2099=>99
        return Convert.ToString(DateTime.Year).Substring(2, 2) + "/" + DateTime.Month + "/" + DateTime.Day + " " + DateTime.TimeOfDay;
    }

    /// <summary>
    /// 文章条带样式（非READONLY）判断输出
    /// </summary>
    /// <param name="str">条带颜色码</param>
    /// <returns>返回颜色码对应的条带样式</returns>
    public string stripStyle(string str)
    {
        switch (str)
        {
            case "blu": return Resources.global.strip_blu;
            case "org": return Resources.global.strip_org;
            case "prp": return Resources.global.strip_prp;
            default: return null;
        }
    }
}