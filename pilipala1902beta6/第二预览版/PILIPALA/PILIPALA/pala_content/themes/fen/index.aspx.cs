using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LibStructs;

public partial class T_fen_index : Page
{
    /* 公有初始化 */
    public SLS SLS { get; set; }
    public List<PaText> List_text_index_page;

    /* indexPage初始化 */
    public List<PaText> List_text_index_post;

    /* infoPage初始化 */
    public int rqsted_text_id;
    public PaText PaText;

    protected void Page_Load(object sender, EventArgs e)
    {
        SLS = new SLS();
        List_text_index_page = SLS.getTextList("page");


        if (Request.QueryString["text_id"] == null)
        {
            List_text_index_post = SLS.getTextList("post");
        }
        else
        {
            rqsted_text_id = Convert.ToInt32(Request.Params["text_id"]);
            PaText = SLS.fill(SLS.getTextMain(rqsted_text_id), SLS.getTextSub(rqsted_text_id));
        }
    }

}