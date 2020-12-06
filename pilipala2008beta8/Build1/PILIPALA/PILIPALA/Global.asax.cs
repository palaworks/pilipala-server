using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using System.Web.Configuration;
using System.Collections;
using System.Text;

using Newtonsoft.Json;

using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala.Post.Property;
using WaterLibrary.stru.pilipala.DB;
using WaterLibrary.com.CommentLake;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;
using WaterLibrary.com.pilipala.Components;


using pla_Type = WaterLibrary.stru.pilipala.Post.Property.Type;
using sys_Type = System.Type;

namespace PILIPALA
{
    public class Global : System.Web.HttpApplication
    {
        /* 定义内核 */
        public static CORE CORE;
        /* 初始化配件 */
        public static Reader Reader = new Reader();
        public static Writer Writer = new Writer();
        public static Counter Counter = new Counter();
        public static CommentLake CommentLake = new CommentLake();

        public static new WaterLibrary.stru.pilipala.User User;

        protected void Application_Start(object sender, EventArgs e)
        {
            /* 初始化噼里啪啦数据库信息和MySql控制器 */
            PLDB PLDB = new PLDB
            {
                MySqlManager = new MySqlManager(new MySqlConnMsg
                {
                    DataSource = WebConfigurationManager.AppSettings["DataSource"],
                    DataBase = WebConfigurationManager.AppSettings["DataBase"],
                    Port = WebConfigurationManager.AppSettings["Port"],
                    User = WebConfigurationManager.AppSettings["User"],
                    PWD = WebConfigurationManager.AppSettings["PWD"]
                })
            };

            /* 初始化内核 */
            string UserName = WebConfigurationManager.AppSettings["UserName"];
            string UserPWD = WebConfigurationManager.AppSettings["UserPWD"];

            CORE CORE = new CORE(PLDB, UserName, UserPWD);
            CORE.SetTables();
            CORE.SetViews();

            /* 设置内核准备完成后需要为其安装哪些配件 */
            CORE.LinkOn += Reader.Ready;
            CORE.LinkOn += Writer.Ready;
            CORE.LinkOn += Counter.Ready;
            CORE.LinkOn += CommentLake.Ready;

            /* 启动内核 */
            User = CORE.Run();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}