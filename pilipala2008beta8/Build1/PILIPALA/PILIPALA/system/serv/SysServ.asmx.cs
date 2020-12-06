using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

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


namespace PILIPALA.system.serv
{
    /// <summary>
    /// SysServ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]

    public class SysServ : System.Web.Services.WebService
    {
        /* 定义内核 */
        public CORE CORE;
        /* 初始化配件 */
        public Reader Reader = new Reader();
        public Writer Writer = new Writer();
        public Counter Counter = new Counter();
        public CommentLake CommentLake = new CommentLake();

        public WaterLibrary.stru.pilipala.User Thaumy;

        public SysServ()
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
            Thaumy = CORE.Run();
        }


        /// <summary>
        /// 星星计数减一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [WebMethod]
        public void Decrease_StarCount_by_PostID(int PostID)
        {
            SysServ SysServ = new SysServ();
            uint StarCount = SysServ.Reader.GetProperty<StarCount>(PostID);

            SysServ.Writer.UpdateIndex<StarCount>(PostID, StarCount - 1);

            Context.Response.Write(StarCount - 1);
            Context.Response.End();
        }
        /// <summary>
        /// 星星计数加一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [WebMethod]
        public void Increase_StarCount_by_PostID(int PostID)
        {
            SysServ SysServ = new SysServ();
            uint StarCount = SysServ.Reader.GetProperty<StarCount>(PostID);

            SysServ.Writer.UpdateIndex<StarCount>(PostID, StarCount + 1);

            Context.Response.Write(StarCount + 1);
            Context.Response.End();
        }
        /// <summary>
        /// 浏览计数加一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [WebMethod]
        public void Increase_UVCount_by_PostID(int PostID)
        {
            SysServ SysServ = new SysServ();
            uint UVCount = SysServ.Reader.GetProperty<UVCount>(PostID);

            SysServ.Writer.UpdateIndex<UVCount>(PostID, UVCount + 1);

            Context.Response.Write(UVCount + 1);
            Context.Response.End();
        }

        /// <summary>
        /// 取得内核版本
        /// </summary>
        /// <returns>返回内核版本</returns>
        [WebMethod]
        public void Get_core_version()
        {
            Context.Response.Write(WaterLibrary.Assembly.Version);
            Context.Response.End();
        }
    }
}
