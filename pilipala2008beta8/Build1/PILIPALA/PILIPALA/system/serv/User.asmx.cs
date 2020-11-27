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
using WaterLibrary.stru.pilipala.Post;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;


using pla_Type = WaterLibrary.stru.pilipala.Post.Property.Type;
using sys_Type = System.Type;

namespace PILIPALA.system.serv
{
    /// <summary>
    /// User 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]

    public class User : System.Web.Services.WebService
    {

        public CORE CORE;
        public PLDR PLDR = new PLDR();
        public PLDU PLDU = new PLDU();
        public PLDC PLDC = new PLDC();

        public User()
        {
            /* 初始化噼里啪啦数据库信息和MySql控制器 */
            PLDB PLDB = new PLDB
            {
                Views = new PLViews() { PosUnion = "pos>dirty>union", NegUnion = "neg>dirty>union"},
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

            CORE.LinkOn += PLDR.Ready;
            CORE.LinkOn += PLDU.Ready;
            CORE.LinkOn += PLDC.Ready;

            CORE.Ready();
            /* 启动内核 */
            CORE.Run();
        }


        [WebMethod]
        public void Get_Post_Data(int ID)
        {
            Post data = PLDR.GetPost(ID);

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
        public void Get_CoreVersion()
        {
            Context.Response.Write(WaterLibrary.Assembly.Version);
            Context.Response.End();
        }
        /// <summary>
        /// 取得计数
        /// </summary>
        [WebMethod]
        public void Get_Count_DataList()
        {
            Hashtable data = new Hashtable()
            {
                { "PostCount", PLDC.PostCount },
                { "CopyCount",  PLDC.BackupCount },
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
        public void Get_Post_DataList()
        {
            List<Post> data = PLDR.GetPost<ID>("^");

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
        public void Get_Backup_DataList(int ID)
        {
            List<Post> data = PLDR.GetPost<ID>(ID.ToString());

            var iso = new Newtonsoft.Json.Converters.IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };

            Context.Response.Write(JsonConvert.SerializeObject(data, iso));
            Context.Response.End();
        }



        [WebMethod]
        public void Post_Reg
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
        public void Post_Dispose(int ID)
        {
            Context.Response.Write(PLDU.Dispose(ID));
            Context.Response.End();
        }
        [WebMethod]
        public void Post_Update
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
        public void Post_Delete(string GUID)
        {
            Context.Response.Write(PLDU.Delete(GUID));
            Context.Response.End();
        }
        [WebMethod]
        public void Post_Apply(string GUID)
        {
            Context.Response.Write(PLDU.Apply(GUID));
            Context.Response.End();
        }
        [WebMethod]
        public void Post_Rollback(int ID)
        {
            Context.Response.Write(PLDU.Rollback(ID));
            Context.Response.End();
        }
        [WebMethod]
        public void Post_Release(int ID)
        {
            Context.Response.Write(PLDU.Release(ID));
            Context.Response.End();
        }
    }
}
