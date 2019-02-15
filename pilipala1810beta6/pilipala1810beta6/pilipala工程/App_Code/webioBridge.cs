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
using StdLib.ViewHandler.pilipala.palaUI;

/// <summary>
/// 为StdLib与网页沟通提供IO桥接
/// </summary>
/// webioBridge ///
/// 网页输入输出桥。用于将网站与StdLib数据进行桥接
/// 全部内部方法的详细注释见相应StdLib方法的相应注释
public class webioBridge : Page
{
    #region 字段
    private connStr cS;
    private MySqlConnectionHandler MyCH = new MySqlConnectionHandler();

    private palaFunction palaF;
    private palaPlan palaPlan;
    private palaUser palaUser;

    private palaPage palaPage;
    private palaPost palaPost;
    private palaList palaList;
    #endregion

    /// <summary>
    /// 初始化io桥
    /// </summary>
    public webioBridge()
    {
        cS.userName = "pala_user";
        cS.password = "pala_password";
        cS.dataSource = "localhost";
        cS.port = "3306";

        //初始化pala元数据
        palaRoot.defaultSet();

        //启动MySql数据库控制器
        MyCH.start(cS);

        palaF = new palaFunction(MyCH, palaRoot.palaDataBase);//启动站点函数
        palaPlan = new palaPlan(1, MyCH, palaRoot.palaDataBase);//启动计划控制器
        palaUser = new palaUser(MyCH, palaRoot.palaDataBase);//用户信息初始化

        //初始化页面对象
        palaPage = new palaPage(MyCH, palaRoot.palaDataBase);
        //初始化文章对象
        palaPost = new palaPost(MyCH, palaRoot.palaDataBase);
        //初始化列表对象
        palaList = new palaList(MyCH, palaRoot.palaDataBase);

    }

    #region 其他函数

    /// <summary>
    /// 文件转字符串输出
    /// </summary>
    /// <returns></returns>
    public string fileToStr(string url)
    {
        FileHandler FH = new FileHandler();
        return FH.fileToStr(Server.MapPath("") + url);
    }

    /// <summary>
    /// 时间数据转字符串
    /// </summary>
    /// <param name="DateTime">文列时间原始时间戳数据</param>
    /// <returns></returns>
    public static string timeToStr(DateTime DateTime)
    {
        //年份字符串只取后二位，例：2099=>99
        return Convert.ToString(DateTime.Year).Substring(2, 2) + "/" + DateTime.Month + "/" + DateTime.Day + " " + DateTime.Hour + ":" + DateTime.Minute;
    }

    /// <summary>
    /// 文章条带样式（非READONLY）判断输出
    /// </summary>
    /// <param name="str">条带颜色码</param>
    /// <returns>返回颜色码对应的条带样式</returns>
    public static string stripStyle(string str)
    {
        switch (str)
        {
            case "blu": return Resources.global.strip_blu;
            case "org": return Resources.global.strip_org;
            case "prp": return Resources.global.strip_prp;
            default: return null;
        }
    }

    /// <summary>
    /// 用户签名获得
    /// </summary>
    /// <returns></returns>
    public string user_word_get(int user_id)
    {
        //以id为1000的用户初始化
        palaUser.start(1000);
        //返回签名
        return palaUser.user_word;
    }

    #endregion

    #region pala列表

    //文章列表获取重载
    public List<paPost> postL()
    {
        return palaList.postL();
    }
    public List<paPost> postL(string position)
    {
        return palaList.postL(position);

    }
    public List<paPost> postL(int beginPlace, int length)
    {
        return palaList.postL(beginPlace, length);
    }
    public List<paPost> postL(int beginPlace, int length, string position)
    {
        return palaList.postL(beginPlace, length, position);
    }

    //页面列表获取重载
    public List<paPage> pageL()
    {
        return palaList.pageL();
    }

    #endregion

    #region pala文章

    //文章获取重载
    public paPost paPost(int post_id)
    {
        return palaPost.post(post_id);
    }

    /// <summary>
    /// 取得文章插件数据
    /// </summary>
    /// <returns></returns>
    public pluPaPost pluPaPost(int post_id)
    {
        return palaPost.pluPaPost(post_id);
    }

    //文章随机获取重载
    public paPost randomPost(int post_id)
    {
        return palaPost.post(post_id);
    }
    public paPost randomPost(int post_id,string position)
    {
        return palaPost.randomPost(post_id, position);
    }

    /// <summary>
    /// 页面浏览计数增加
    /// </summary>
    /// <param name="post_id">文章序列号</param>
    /// <param name="value">设置值</param>
    /// <returns></returns>
    public bool SET_page_count_read(int page_id, int increaseValue)
    {
        locateStr ls;
        ls.dataBaseName = palaPost.dataBaseName;
        ls.tableName = palaRoot.pluPost_Table;
        ls.whereColumnName = "page_id";
        ls.targetColumnName = "count_read";

        int newValue = Convert.ToInt32(MyCH.getColumnValue(ls, page_id.ToString())) + increaseValue;

        return palaPage.SET_count_read(page_id, newValue);
    }
    /// <summary>
    /// 页面浏览计数增加
    /// </summary>
    /// <param name="post_id">文章序列号</param>
    /// <param name="value">设置值</param>
    /// <returns></returns>
    public bool SET_page_count_like(int page_id, int increaseValue)
    {
        locateStr ls;
        ls.dataBaseName = palaPost.dataBaseName;
        ls.tableName = palaRoot.pluPost_Table;
        ls.whereColumnName = "page_id";
        ls.targetColumnName = "count_like";

        int newValue = Convert.ToInt32(MyCH.getColumnValue(ls, page_id.ToString())) + increaseValue;

        return palaPage.SET_count_like(page_id, newValue);
    }

    #endregion

    #region pala页面

    //页面获取重载
    public paPage paPage(int page_id)
    {
        return palaPage.page(page_id);
    }

    /// <summary>
    /// 取得文章插件数据
    /// </summary>
    /// <returns></returns>
    public pluPaPage pluPaPage(int post_id)
    {
        return palaPage.pluPaPage(post_id);
    }

    //页面随机获取重载
    public paPage randomPage(int page_id)
    {
        return palaPage.randomPage(page_id);
    }


    /// <summary>
    /// 页面浏览计数增加
    /// </summary>
    /// <param name="post_id">文章序列号</param>
    /// <param name="value">设置值</param>
    /// <returns></returns>
    public bool SET_post_count_read(int post_id, int increaseValue)
    {
        locateStr ls;
        ls.dataBaseName = palaPost.dataBaseName;
        ls.tableName = palaRoot.pluPost_Table;
        ls.whereColumnName = "post_id";
        ls.targetColumnName = "count_read";

        int newValue = Convert.ToInt32(MyCH.getColumnValue(ls, post_id.ToString())) + increaseValue;

        return palaPage.SET_count_read(post_id, newValue);
    }
    /// <summary>
    /// 页面浏览计数增加
    /// </summary>
    /// <param name="post_id">文章序列号</param>
    /// <param name="value">设置值</param>
    /// <returns></returns>
    public bool SET_post_count_like(int post_id, int increaseValue)
    {
        locateStr ls;
        ls.dataBaseName = palaPost.dataBaseName;
        ls.tableName = palaRoot.pluPost_Table;
        ls.whereColumnName = "post_id";
        ls.targetColumnName = "count_like";

        int newValue = Convert.ToInt32(MyCH.getColumnValue(ls, post_id.ToString())) + increaseValue;

        return palaPage.SET_count_like(post_id, newValue);
    }

    #endregion

}