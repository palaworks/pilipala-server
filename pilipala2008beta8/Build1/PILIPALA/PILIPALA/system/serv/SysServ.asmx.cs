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
using WaterLibrary.stru.pilipala.PostKey;
using WaterLibrary.stru.pilipala.DB;
using WaterLibrary.stru.pilipala;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;


using pla_Type = WaterLibrary.stru.pilipala.PostKey.Type;
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
        public CORE CORE;
        public PLDR PLDR;
        public PLDU PLDU;
        public PLDC PLDC;

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

            CORE = new CORE(PLDB);
            CORE.SetTables();
            CORE.SetViews();

            /* 启动内核 */
            CORE.Run();

            PLDR = new PLDR(CORE);
            PLDU = new PLDU(CORE);
            PLDC = new PLDC(CORE);
        }


        /// <summary>
        /// count_star计数减一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [WebMethod]
        public int StarCount_subs(int ID)
        {
            SysServ SysServ = new SysServ();
            int StarCount = SysServ.PLDR.GetIndex(ID).StarCount;

            SysServ.PLDU.UpdateIndex<StarCount>(ID, StarCount - 1);

            return StarCount - 1;
        }
        /// <summary>
        /// count_star计数加一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [WebMethod]
        public int StarCount_plus(int ID)
        {
            SysServ SysServ = new SysServ();
            int StarCount = SysServ.PLDR.GetIndex(ID).StarCount;

            SysServ.PLDU.UpdateIndex<StarCount>(ID, StarCount + 1);

            return StarCount + 1;
        }
        /// <summary>
        /// count_pv计数加一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [WebMethod]
        public int UVCount_plus(int ID)
        {
            SysServ SysServ = new SysServ();
            int UVCount = SysServ.PLDR.GetIndex(ID).UVCount;

            SysServ.PLDU.UpdateIndex<UVCount>(ID, UVCount + 1);

            return UVCount + 1;
        }
    }
}
