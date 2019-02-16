using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI;
using System.Data;

//将StdLib全部引用
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
    private GetPost getpost;
    private SetPost setpost;
    public webioBridge()
    {
        cS.dataSource = "localhost";
        cS.userName = "root";
        cS.password = "1246265280";
        cS.port = "3306";

        //启动MySql数据库控制器
        MyCH.start(cS);

        getpost = new GetPost(MyCH, "dbname");//启动Post获得器
        setpost = new SetPost(MyCH, "dbname");//启动Post编辑器
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
    public List<postData> comnPostLst_get()
    {
        return getpost.comnPost();
    }

    /// <summary>
    /// 获取置顶文章数据 注意，只允许一个置顶文章出现在数据库中，一个以上的置顶文章会造成取文错误
    /// </summary>
    /// <returns></returns>
    public postData topPost_get()
    {
        return getpost.topPost();
    }

    /// <summary>
    /// 获取菜单文列数据
    /// </summary>
    /// <returns></returns>
    public List<postData> menuPostLst_get()
    {
        return getpost.menuPost();
    }

    /// <summary>
    /// 获得post页面的文章数据
    /// </summary>
    /// <returns></returns>
    public postData postPost_get(string post_id)
    {
        return getpost.postPost(post_id);
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
        ls.dataBaseName = getpost.dataBaseName;
        ls.tableName = "st_posts";
        ls.whereColumnName = "post_id";
        ls.targetColumnName = "count_read";

        int newValue = Convert.ToInt32(MyCH.getColumnValue(ls, post_id.ToString())) + increaseValue;

        return setpost.count_read(post_id, newValue);
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
        ls.dataBaseName = getpost.dataBaseName;
        ls.tableName = "st_posts";
        ls.whereColumnName = "post_id";
        ls.targetColumnName = "count_like";
        
        int newValue = Convert.ToInt32(MyCH.getColumnValue(ls, post_id.ToString())) + increaseValue;

        return setpost.count_like(post_id, newValue);
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
    /// 文章条带颜色字符串转样式
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string stripClr(string str)
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