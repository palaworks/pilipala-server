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
using WaterLibrary.stru.pilipala;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;


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
        public PLDR PLDR = new PLDR();
        public PLDU PLDU = new PLDU();
        public PLDC PLDC = new PLDC();

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
            CORE = new CORE(PLDB);
            CORE.SetTables();
            CORE.SetViews();

            /* 设置内核准备完成后需要为其安装哪些配件 */
            CORE.LinkOn += PLDR.Ready;
            CORE.LinkOn += PLDU.Ready;
            CORE.LinkOn += PLDC.Ready;

            /* 准备内核 */
            CORE.Ready();
            /* 启动内核 */
            CORE.Run();
        }


        /// <summary>
        /// count_star计数减一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [WebMethod]
        public uint StarCount_subs(int ID)
        {
            SysServ SysServ = new SysServ();
            uint StarCount = SysServ.PLDR.GetProperty<StarCount>(ID);

            SysServ.PLDU.UpdateIndex<StarCount>(ID, StarCount - 1);

            return StarCount - 1;
        }
        /// <summary>
        /// count_star计数加一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [WebMethod]
        public uint StarCount_plus(int ID)
        {
            SysServ SysServ = new SysServ();
            uint StarCount = SysServ.PLDR.GetProperty<StarCount>(ID);

            SysServ.PLDU.UpdateIndex<StarCount>(ID, StarCount + 1);

            return StarCount + 1;
        }
        /// <summary>
        /// count_pv计数加一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [WebMethod]
        public uint UVCount_plus(int ID)
        {
            SysServ SysServ = new SysServ();
            uint UVCount = SysServ.PLDR.GetProperty<UVCount>(ID);

            SysServ.PLDU.UpdateIndex<UVCount>(ID, UVCount + 1);

            return UVCount + 1;
        }
    }
}
