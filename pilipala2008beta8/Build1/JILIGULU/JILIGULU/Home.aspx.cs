using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Configuration;

using CommentLake;
using CommentLake.stru;
using WaterLibrary.stru.MySQL;

namespace JILIGULU
{
    public partial class Home : System.Web.UI.Page
    {
        public int PostCount;
        public int CopyCount;
        public int CommentCount;

        public int OnDisplayCount;
        public int ScheduledCount;
        public int HiddenCount;
        public int ArchivedCount;

        public CommentLake.CommentLake CommentLake;
        protected void Page_Load(object sender, EventArgs e)
        {
            Core.Sys Sys = new Core.Sys();
            Sys.INIT();

            PostCount = Sys.PLCH.PostCount;
            CopyCount = Sys.PLCH.CopyCount;
            CommentCount = Sys.PLCH.CommentCount;

            OnDisplayCount = Sys.PLCH.OnDisplayCount;
            ScheduledCount = Sys.PLCH.ScheduledCount;
            HiddenCount = Sys.PLCH.HiddenCount;
            ArchivedCount = Sys.PLCH.ArchivedCount;
        }
    }
}