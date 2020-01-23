using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LibStruct.pilipala;

namespace PILIPALA.pala_custom.theme.field2.cut
{
    public partial class CONTENT : System.Web.UI.Page
    {
        /* 创建pala基本服务 */
        protected pala_system.service.Basic BS { get; set; }
        protected PaText PaText;

        public int text_id;

        public int prevTextID;
        public int nextTextID;

        public string prevTextTitle;
        public string nextTextTitle;

        protected void Page_Load(object sender, EventArgs e)
        {
            /* 如果请求的text_id不为空 */
            if (Request.QueryString["text_id"] != null)
            {
                /* 获得请求的text_id */
                text_id = Convert.ToInt32(Request.Params["text_id"]);

                /* 初始化基本服务 */
                BS = new pala_system.service.Basic();
                /* 合并主次表 */
                PaText = pala_system.service.Basic.fill(BS.getTextMain(text_id), BS.getTextSub(text_id));

                /* 前后文章标题赋值 */
                prevTextID = BS.prevTextID(text_id);
                nextTextID = BS.nextTextID(text_id);

                /* 前后文章标题赋值 */
                prevTextTitle = BS.getTextTitle(prevTextID);
                nextTextTitle = BS.getTextTitle(nextTextID);
            }
        }
    }
}