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
using StdLib.ViewHandler.WebSite;

/// <summary>
/// 为StdLib与网页沟通提供IO桥接
/// </summary>
public class webioBridge : Page
{
    connStr cS;
    MySqlConnectionHandler mysqlCH = new MySqlConnectionHandler();

    public webioBridge()
    {
        cS.dataSource = "localhost";
        cS.userName = "thaumymysql";
        cS.password = "0863A03";
        cS.port = "3306";

        //启动MySql数据库控制器
        mysqlCH.start(cS);
    }

    /// <summary>
    /// 以字符串形式输出文件（1.1可用取代）
    /// </summary>
    /// <param name="filePath">站点根目录下的文件位置,例如"//scripts//main_pg.st.script"</param>
    /// <returns>返回字符串</returns>
    public string simpleGet(string filePath)
    {
        PageHandler p = new PageHandler(null);
        return p.outputFileString(Server.MapPath("") + filePath);//获取到文件位置并返回字符串
    }

    /// <summary>
    /// 获取文列的postData List
    /// </summary>
    /// <returns>返回普通文章postData List</returns>
    public List<postData> pstlst_comn_get()
    {
        //定义postData List
        List<postData> postDataList = new List<postData>();

        //获得展示可用的文列文章postData表
        DataTable postDataTable = mysqlCH.getTable("select `st_posts`.`post_id` AS `post_id`,`st_posts`.`post_title` AS `post_title`,thaumymysql.st_posts.`post_summary` AS `post_summary`,`st_posts`.`post_archive` AS `post_archive`,`st_posts`.`date_created` AS `date_created`,`st_posts`.`count_browse` AS `count_browse`,`st_posts`.`count_review` AS `count_review`,`st_posts`.`count_like` AS `count_like`,`st_posts`.`tagA` AS `tagA`,`st_posts`.`tagB` AS `tagB`,`st_posts`.`tagC` AS `tagC`,`st_posts`.`color_strip` AS `color_strip` from thaumymysql.st_posts where ((`st_posts`.`post_position` like 'comn') and (`st_posts`.`post_isShow` = 1))");

        foreach (DataRow r in postDataTable.Rows)
        {
            //定义单postData来组成postData List
            postData e_postData = new postData();

            //赋值文章列表单文章信息（1.1可用取代）
            e_postData.post_id = Convert.ToInt32(r["post_id"]);
            e_postData.post_title = Convert.ToString(r["post_title"]);
            e_postData.post_summary = Convert.ToString(r["post_summary"]);
            e_postData.post_archive = Convert.ToString(r["post_archive"]);

            e_postData.date_created = Convert.ToDateTime(r["date_created"]);
            e_postData.count_browse = Convert.ToInt32(r["count_browse"]);
            e_postData.count_review = Convert.ToInt32(r["count_review"]);
            e_postData.count_like = Convert.ToInt32(r["count_like"]);

            e_postData.tagA = Convert.ToString(r["tagA"]);
            e_postData.tagB = Convert.ToString(r["tagB"]);
            e_postData.tagC = Convert.ToString(r["tagC"]);

            e_postData.color_strip = Convert.ToString(r["color_strip"]);

            postDataList.Add(e_postData);
        }

        return postDataList;
    }

    /// <summary>
    /// 获取置顶文章postData 注意，只允许一个置顶文章出现在数据库中，一个以上的置顶文章会造成取文错误
    /// </summary>
    /// <returns>返回置顶文章postData List</returns>
    public postData pst_top_get()
    {
        postData postData = new postData();

        //获得展示可用的置顶文章postData表
        DataTable postDataTable = mysqlCH.getTable("select `st_posts`.`post_id` AS `post_id`,`st_posts`.`post_title` AS `post_title`,thaumymysql.st_posts.`count_browse` AS `count_browse`,`st_posts`.`count_review` AS `count_review`,`st_posts`.`color_strip` AS `color_strip` from thaumymysql.st_posts where ((`st_posts`.`post_position` like 'top') and (`st_posts`.`post_isShow` = 1))");

        foreach (DataRow r in postDataTable.Rows)
        {
            //赋值文章列表单文章信息（1.1可用取代）
            postData.post_id = Convert.ToInt32(r["post_id"]);
            postData.post_title = Convert.ToString(r["post_title"]);

            postData.count_browse = Convert.ToInt32(r["count_browse"]);
            postData.count_review = Convert.ToInt32(r["count_review"]);

            postData.color_strip = Convert.ToString(r["color_strip"]);
        }

        return postData;
    }

    /// <summary>
    /// 获取菜单文列的postData List
    /// </summary>
    /// <returns>返回菜单文列的postData List</returns>
    public List<postData> pstlst_menu_get()
    {
        List<postData> postDataList = new List<postData>();

        //获得展示可用的菜单文章postData表
        DataTable postDataTable = mysqlCH.getTable("select `st_posts`.`post_id` AS `post_id`,`st_posts`.`post_title` AS `post_title` from thaumymysql.st_posts where ((`st_posts`.`post_position` like 'menu') and (`st_posts`.`post_isShow` = 1))");

        foreach (DataRow r in postDataTable.Rows)
        {
            //定义单postData来组成postData清单
            postData e_postData = new postData();

            //赋值文章列表单文章信息（1.1可用取代）
            e_postData.post_id = Convert.ToInt32(r["post_id"]);
            e_postData.post_title = Convert.ToString(r["post_title"]);

            postDataList.Add(e_postData);
        }

        return postDataList;
    }

    /// <summary>
    /// 格式化文章下栏时间
    /// </summary>
    /// <param name="DateTime">文列时间原始时间戳数据</param>
    /// <returns>返回被标准化的文列时间字符串</returns>
    public string pst_time_arrange(DateTime DateTime)
    {
        return DateTime.Year + "/" + DateTime.Month + "/" + DateTime.Day + " " + DateTime.TimeOfDay;
    }

    /// <summary>
    /// 将数据库获取的文章条带颜色代码转换为style代码
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string pst_stripClr_arrange(string str)
    {
        switch (str)
        {
            case "blu": return Resources.global.strip_blu;
            case "org": return Resources.global.strip_org;
            case "prp": return Resources.global.strip_prp;
            default: return null;
        }
    }



    public postData post_pDget(string post_id)
    {
        postData postData = new postData();

        //获得展示可用的文章postData
        DataTable postDataTable = mysqlCH.getTable("select `st_posts`.`post_id` AS `post_id`,`st_posts`.`post_title` AS `post_title`,`st_posts`.`post_summary` AS `post_summary`,`st_posts`.`post_content` AS `post_content`,`st_posts`.`post_archive` AS `post_archive`,`st_posts`.`date_created` AS `date_created`,`st_posts`.`date_changed` AS `date_changed`,`st_posts`.`count_browse` AS `count_browse`,`st_posts`.`count_review` AS `count_review`,`st_posts`.`count_like` AS `count_like`,`st_posts`.`tagA` AS `tagA`,`st_posts`.`tagB` AS `tagB`,`st_posts`.`tagC` AS `tagC`,`st_posts`.`color_strip` AS `color_strip` from thaumymysql.st_posts where ((`st_posts`.`post_isShow` = 1)AND(`st_posts`.`post_id` = " + post_id+ "))");

        foreach (DataRow r in postDataTable.Rows)
        {
            //赋值文章列表单文章信息（1.1可用取代）
            postData.post_id = Convert.ToInt32(r["post_id"]);
            postData.post_title = Convert.ToString(r["post_title"]);
            postData.post_summary = Convert.ToString(r["post_summary"]);
            postData.post_content = Convert.ToString(r["post_content"]);
            postData.post_archive = Convert.ToString(r["post_archive"]);

            postData.date_created = Convert.ToDateTime(r["date_created"]);
            postData.date_changed = Convert.ToDateTime(r["date_changed"]);
            postData.count_browse = Convert.ToInt32(r["count_browse"]);
            postData.count_review = Convert.ToInt32(r["count_review"]);
            postData.count_like = Convert.ToInt32(r["count_like"]);

            postData.tagA = Convert.ToString(r["tagA"]);
            postData.tagB = Convert.ToString(r["tagB"]);
            postData.tagC = Convert.ToString(r["tagC"]);

            postData.color_strip = Convert.ToString(r["color_strip"]);
        }

        return postData;
    }
}