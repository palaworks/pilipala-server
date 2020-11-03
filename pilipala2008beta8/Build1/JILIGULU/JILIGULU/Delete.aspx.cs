using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JILIGULU
{
    public partial class Delete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CommitBtn_Click(object sender, EventArgs e)
        {
            Core.Sys Sys = new Core.Sys();
            Sys.INIT();

            Sys.PLDU.Delete(GUID.Value);
        }
    }
}