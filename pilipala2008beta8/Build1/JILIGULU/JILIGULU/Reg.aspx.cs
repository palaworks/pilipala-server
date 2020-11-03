using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WaterLibrary.stru.pilipala;

namespace JILIGULU
{
    public partial class Reg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CommitBtn_Click(System.Object sender, System.EventArgs e)
        {
            Core.Sys Sys = new Core.Sys();
            Sys.INIT();

            Post Post = new Post()
            {
                Title = Title.Value,
                Mode = Mode.Value,
                Type = Type.Value,

                Summary = Summary.Value,
                Archiv = Archiv.Value,
                Label = Label.Value,
                Content = Content.Value,

                StarCount = Convert.ToInt32(StarCount.Value),
                UVCount = Convert.ToInt32(UVCount.Value),
                Cover = Cover.Value,
            };

            Sys.PLDU.Reg(Post);

        }
    }
}