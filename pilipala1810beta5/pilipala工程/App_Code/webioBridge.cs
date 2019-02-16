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
    #region 字段
    private connStr cS;
    private MySqlConnectionHandler MyCH = new MySqlConnectionHandler();

    private palaFunction palaF;
    private palaPlan palaPlan;
    private palaUser palaUser;

    private GetPost getPost;
    private SetPost setPost;
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

        getPost = new GetPost(MyCH, palaRoot.palaDataBase);//启动Post获得器
        setPost = new SetPost(MyCH, palaRoot.palaDataBase);//启动Post编辑器
    }

    /// <summary>
    /// 文件转字符串输出
    /// </summary>
    /// <returns></returns>
    public string fileToStr(string url)
    {
        FileHandler FH = new FileHandler();
        return FH.fileToStr(Server.MapPath("") + url);
    }


    #region index页数据

    /// <summary>
    /// 获取普通文列数据
    /// </summary>
    /// <returns></returns>
    public List<post> idxComnPost_get()
    {
        return getPost.idx_comnPost();
    }
    /// <summary>
    /// 获取普通文列数据（重载二：推进式获取）
    /// </summary>
    /// <returns></returns>
    public List<post> idxComnPost_get(int beginPlace, int length)
    {
        return getPost.idx_comnPost(beginPlace, length);
    }

    /// <summary>
    /// 获取置顶文章数据 注意，只允许一个置顶文章出现在数据库中，一个以上的置顶文章会造成取文错误
    /// </summary>
    /// <returns></returns>
    public post idxTopPost_get()
    {
        return getPost.idx_topPost();
    }

    /// <summary>
    /// 获取菜单文列数据
    /// </summary>
    /// <returns></returns>
    public List<post> idxMenuPost_get()
    {
        return getPost.idx_menuPost();
    }

    #endregion

    #region word页数据

    /// <summary>
    /// 获得word页面的文章数据
    /// </summary>
    /// <returns></returns>
    public post wrdPostData_get(int post_id)
    {
        return getPost.pst_post(post_id);
    }

    /// <summary>
    /// 获得word页面的计数数据
    /// </summary>
    /// <returns></returns>
    public postEx wrdPostCount_get(int post_id)
    {
        return palaF.postEx(post_id);
    }

    #endregion



    #region pala函数

    /// <summary>
    /// 随机取一个展示可用的文章数据（序列号和标题）
    /// </summary>
    /// <returns></returns>
    public DataRow ramdom(int post_id)
    {
        return palaF.random(post_id);
    }

    /// <summary>
    /// 取得文章插件数据
    /// </summary>
    /// <returns></returns>
    public postEx postEx(int post_id)
    {
        return palaF.postEx(post_id);
    }

    #endregion

    #region 浏览计数

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
        ls.tableName = palaRoot.postExTable;
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
        ls.tableName = palaRoot.postExTable;
        ls.whereColumnName = "post_id";
        ls.targetColumnName = "count_like";

        int newValue = Convert.ToInt32(MyCH.getColumnValue(ls, post_id.ToString())) + increaseValue;

        return setPost.count_like(post_id, newValue);
    }

    #endregion

    #region 转义操作

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

    #endregion



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
}