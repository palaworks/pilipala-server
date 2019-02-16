using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Web.UI;
using LibStructs;
using sukiUnit;
using jarwUnit.pilipala;
using jarwUnit.pilipala.UI;

/// <summary>
/// SLS 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class SLS : System.Web.Services.WebService
{
    private PalaRoot PalaRoot;
    private ConnSign cS;
    private PalaDB PalaDB;

    private TextListH TextListH;
    private TextH TextH;

    private CookieHandler CookieHandler;
    public SLS()
    {
        CookieHandler = new CookieHandler();
        PalaDB.MySqlConnH = new MySqlConnH();

        
        cS.user = "thaumy0tdymy";
        cS.password = "177BDE5";
        cS.dataSource = "localhost";
        cS.port = "3306";
        PalaDB.MySqlConnH.start(cS);
        PalaDB.dataBase = "thaumy0tdymy";

        /*
        cS.user = "pala_database_user";
        cS.password = "pala_database_user_password";
        cS.dataSource = "localhost";
        cS.port = "3306";
        PalaDB.MySqlConnH.start(cS);
        PalaDB.dataBase = "pala_database";
        */

        PalaDB.Tables = PalaRoot.defaultTables();/* 以默认值确定表名 */
        PalaDB.Views = PalaRoot.defaultViews();/* 以默认值确定视图名 */

        PalaRoot = new PalaRoot(2, PalaDB);
        TextListH = new TextListH(PalaDB);
        TextH = new TextH(PalaDB);

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    #region 基本操作

    /// <summary>
    /// 文件转字符串
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public string exstring(string url)
    {
        FileHandler FH = new FileHandler();
        return FH.fileToStr(Server.MapPath("") + url);
    }
    /// <summary>
    /// 时间转换
    /// </summary>
    /// <param name="DateTime">时间对象</param>
    /// <returns></returns>
    [WebMethod]
    public static string extime(DateTime DateTime)
    {
        //年份只取后二位：2099=>99
        return Convert.ToString(DateTime.Year).Substring(2, 2) + "/" + DateTime.Month + "/" + DateTime.Day + " " + DateTime.Hour + ":" + DateTime.Minute;
    }
    /// <summary>
    /// 条带颜色样式
    /// </summary>
    /// <param name="color">颜色代码</param>
    /// <returns></returns>
    [WebMethod]
    public static string stripStyle(string strip_color)
    {
        switch (strip_color)
        {
            case "org": return "OrgStrip";
            case "blu": return "BluStrip";
            case "prp": return "PrpStrip";
            default: return null;
        }
    }
    /// <summary>
    /// 标签文本转标签集合
    /// </summary>
    /// <param name="tags">标签文本</param>
    /// <returns></returns>
    [WebMethod]
    public static List<string> extags(string tags)
    {
        List<string> list_tags = new List<string>();
        foreach (string tag in tags.Split('$'))
        {
            list_tags.Add(tag);
        }
        return list_tags;
    }

    #endregion

    /// <summary>
    /// Btn_like刷新
    /// </summary>
    /// <param name="text_id"></param>
    /// <returns></returns>
    [WebMethod]
    public string refre_Btn_like(int text_id)
    {

        if (CookieHandler.cookie<bool>("isLike" + text_id.ToString()) == false)//如果客户端不喜欢
        {
            CookieHandler.setCookie("isLike" + text_id.ToString(), true);//设置喜欢

            int count_like = TextH.getTextSub(text_id).count_like;
            return Convert.ToString(count_like);
        }
        else
        {
            CookieHandler.setCookie("isLike" + text_id.ToString(), false);//设置不喜欢

            int count_like = TextH.getTextSub(text_id).count_like;
            return Convert.ToString(count_like);
        }
    }

    /// <summary>
    /// count_like刷新
    /// </summary>
    /// <param name="text_id"></param>
    /// <param name="old_count_like"></param>
    /// <returns></returns>
    [WebMethod]
    public int refre_count_like(int text_id, int old_count_like)
    {

        if (CookieHandler.cookie<bool>("isLike" + text_id.ToString()) == false)//如果客户端不喜欢
        {
            TextH.update_count_pv(text_id, 1);//数据库计数加一
            return old_count_like + 1;
        }
        else
        {
            TextH.update_count_pv(text_id, -1);
            return old_count_like - 1;
        }
    }

    /// <summary>
    /// 得到权限数据
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public PaRoot getRoot()
    {
        return PalaRoot.getRoot();
    }

    #region PaText操作

    [WebMethod]
    public List<PaText> getTextList()
    {
        return TextListH.getTextList();
    }
    [WebMethod]
    public List<PaText> getTextList(string text_type)
    {
        return TextListH.getTextList(text_type);
    }

    [WebMethod]
    public List<PaText> stepTextList(int row, int rowLength)
    {
        return TextListH.stepTextList(row, rowLength);
    }
    [WebMethod]
    public List<PaText> stepTextList(int row, int rowLength, string text_type)
    {
        return TextListH.stepTextList(row, rowLength, text_type);
    }

    [WebMethod]
    public PaText getTextMain(int text_id)
    {
        return TextH.getTextMain(text_id);
    }
    [WebMethod]
    public PaText getTextSub(int text_id)
    {
        return TextH.getTextSub(text_id);
    }
    [WebMethod]
    public PaText rdmTextIndex(int excluded_text_id, string text_type)
    {
        return TextH.rdmTextIndex(excluded_text_id, text_type);
    }

    [WebMethod]
    public PaText fill(PaText TextMain, PaText TextSub)
    {
        return TextH.fill(TextMain, TextSub);
    }

    [WebMethod]
    public bool update_count_pv(int text_id, int value)
    {
        return TextH.update_count_pv(text_id, value);
    }
    [WebMethod]
    public bool update_count_like(int text_id, int value)
    {
        return TextH.update_count_like(text_id, value);
    }

    #endregion
}
