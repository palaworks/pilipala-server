using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LibStructs;
using PILIPALA.pala_services;
using PILIPALA.pala_content.themes.fen.services;

namespace PILIPALA.pala_content.themes.fen
{
    public partial class index : System.Web.UI.Page
    {
        protected SLS SLS { get; set; }

        public List<PaText> List_text_index_page;/* 页面索引列 */
        public List<PaText> List_text_index_post;/* 文章索引列 */

        public int rqst_text_id;/* 请求的文章ID */
        public PaText PaText;/* 文章数据 */

        public void Page_Load(object sender, EventArgs e)
        {
            SLS = new SLS();

            if (Request.QueryString["guide"] == "1")/* 如果导航加载 */
            {
                List_text_index_page = SLS.getTextList("page");/* 导航列不加载 */
            }


            if (Request.QueryString["text_id"] == null)/* 如果没有文本ID参数 */
            {
                if (Request.QueryString["row"] != null)/* 如果推进加载行参数不为空 */
                {/* 以推进式加载数据赋值索引列 */
                    int row = Convert.ToInt32(Request.QueryString["row"]);
                    List_text_index_post = SLS.stepTextList(row, 6, "post");
                }
                else
                {/* 以全部数据赋值索引列 */
                    List_text_index_post = SLS.getTextList("post");
                }

            }
            else if (Request.QueryString["text_id"] != null)
            {/* 加载到文本详情 */
                rqst_text_id = Convert.ToInt32(Request.Params["text_id"]);
                PaText = SLS.fill(SLS.getTextMain(rqst_text_id), SLS.getTextSub(rqst_text_id));
            }
        }
    }
}