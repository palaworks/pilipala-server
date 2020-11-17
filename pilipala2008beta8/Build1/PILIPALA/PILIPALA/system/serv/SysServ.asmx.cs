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
    // [System.Web.Script.Services.ScriptService]
    [System.Web.Script.Services.ScriptService]

    public class SysServ : System.Web.Services.WebService
    {
        public PLSYS PLSYS;
        public PLDR PLDR;
        public PLDU PLDU;
        public PLDC PLDC;

        public SysServ()
        {
            /* 初始化噼里啪啦数据库信息和MySql控制器 */
            PLDB PLDB = new PLDB
            {
                Tables = PLSYS.DefTables,
                Views = PLSYS.DefViews,
                MySqlManager = new MySqlManager()
            };
            PLSYS = new PLSYS(2, PLDB);

            PLSYS.DefaultSysTables();
            PLSYS.DefaultSysViews();

            /* 初始化数据库连接 */
            PLSYS.DBCHINIT(new MySqlConn
            {
                DataSource = WebConfigurationManager.AppSettings["DataSource"],
                DataBase = WebConfigurationManager.AppSettings["DataBase"],
                Port = WebConfigurationManager.AppSettings["Port"],
                User = WebConfigurationManager.AppSettings["User"],
                PWD = WebConfigurationManager.AppSettings["PWD"]
            });

            PLDR = new PLDR(PLSYS);
            PLDU = new PLDU(PLSYS);
            PLDC = new PLDC(PLSYS);
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



        [WebMethod]
        public void GetPostData(int ID)
        {
            Post data = PLDR.GetTotal(ID);

            var iso = new Newtonsoft.Json.Converters.IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };

            Context.Response.Write(JsonConvert.SerializeObject(data, iso));
            Context.Response.End();
        }

        /// <summary>
        /// 取得内核版本
        /// </summary>
        /// <returns>返回内核版本</returns>
        [WebMethod]
        public void GetCoreVersion()
        {
            Context.Response.Write(WaterLibrary.Assembly.Version);
            Context.Response.End();
        }
        /// <summary>
        /// 取得计数
        /// </summary>
        [WebMethod]
        public void GetCounts()
        {
            Hashtable data = new Hashtable()
            {
                { "PostCount", PLDC.PostCount },
                { "CopyCount",  PLDC.CopyCount },
                { "HiddenCount",  PLDC.HiddenCount },
                { "OnDisplayCount",  PLDC.OnDisplayCount },
                { "ArchivedCount",  PLDC.ArchivedCount },
                { "ScheduledCount",  PLDC.ScheduledCount },
            };

            Context.Response.Write(JsonConvert.SerializeObject(data));
            Context.Response.End();
        }
        /// <summary>
        /// 取得文章列表
        /// </summary>
        [WebMethod]
        public void GetPosts()
        {
            List<Post> data = PLDR.GetList
                (typeof(GUID),
                typeof(Title), typeof(Summary), typeof(Content), typeof(Cover),
                typeof(CT), typeof(LCT), typeof(Mode), typeof(Archiv),
                typeof(UVCount), typeof(StarCount),
                typeof(pla_Type)
                );

            var iso = new Newtonsoft.Json.Converters.IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };

            Context.Response.Write(JsonConvert.SerializeObject(data, iso));
            Context.Response.End();
        }
        /// <summary>
        /// 取得拷贝列表
        /// </summary>
        [WebMethod]
        public void GetCopys(int ID)
        {
            List<Post> data = PLDR.GetCopyList(ID);

            var iso = new Newtonsoft.Json.Converters.IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };

            Context.Response.Write(JsonConvert.SerializeObject(data, iso));
            Context.Response.End();
        }



        [WebMethod]
        public void Reg
            (
            string Mode, string Type, string User,
            int UVCount, int StarCount,
            string Title, string Summary, string Content,
            string Archiv, string Label, string Cover
            )
        {
            Context.Response.Write(PLDU.Reg(new Post
            {
                Mode = Mode,
                Type = Type,
                User = User,

                UVCount = UVCount,
                StarCount = StarCount,

                Title = Title,
                Summary = Summary,
                Content = Content,

                Archiv = Archiv,
                Label = Label,
                Cover = Cover
            }));
            Context.Response.End();
        }
        [WebMethod]
        public void Dispose(int ID)
        {
            Context.Response.Write(PLDU.Dispose(ID));
            Context.Response.End();
        }
        [WebMethod]
        public void Update
            (
            int ID, string Mode, string Type,
            int UVCount, int StarCount,
            string Title, string Summary, string Content,
            string Archiv, string Label, string Cover
            )
        {
            Context.Response.Write(PLDU.Update(new Post
            {
                ID = ID,
                Mode = Mode,
                Type = Type,

                UVCount = UVCount,
                StarCount = StarCount,

                Title = Title,
                Summary = Summary,
                Content = Content,

                Archiv = Archiv,
                Label = Label,
                Cover = Cover
            }));
            Context.Response.End();
        }

        [WebMethod]
        public void Delete(string GUID)
        {
            Context.Response.Write(PLDU.Delete(GUID));
            Context.Response.End();
        }
        [WebMethod]
        public void Apply(string GUID)
        {
            Context.Response.Write(PLDU.Apply(GUID));
            Context.Response.End();
        }
        [WebMethod]
        public void Rollback(int ID)
        {
            Context.Response.Write(PLDU.Rollback(ID));
            Context.Response.End();
        }
        [WebMethod]
        public void Release(int ID)
        {
            Context.Response.Write(PLDU.Release(ID));
            Context.Response.End();
        }
    }
}
