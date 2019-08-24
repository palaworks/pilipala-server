using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PILIPALA.pala_custom.theme.field2.cut
{
    public partial class LIST : System.Web.UI.Page
    {
        /* 创建pala基本服务 */
        protected pala_system.service.Basic BS { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            /* 初始化基本服务 */
            BS = new pala_system.service.Basic();
        }
    }
}
