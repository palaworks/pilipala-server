using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Web.Configuration;
using System.Collections;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala.Post.Property;
using WaterLibrary.stru.pilipala.DB;
using WaterLibrary.stru.pilipala.Post;
using WaterLibrary.stru.CommentLake;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;
using WaterLibrary.com.pilipala.Components;
using WaterLibrary.com.CommentLake;

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
        public Reader Reader = new Reader();
        public Writer Writer = new Writer();
        public Counter Counter = new Counter();

        public CommentLake CommentLake = new CommentLake();

        /* JSON序列化时间格式重置 */
        readonly IsoDateTimeConverter iso = new IsoDateTimeConverter
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
        };

        public User()
        {
            /* 初始化噼里啪啦数据库信息和MySql控制器 */
            PLDB PLDB = new PLDB
            {
                Views = new PLViews() { PosUnion = "pos>dirty>union", NegUnion = "neg>dirty>union" },
                MySqlManager = new MySqlManager(new MySqlConnMsg
                {
                    DataSource = WebConfigurationManager.AppSettings["DataSource"],
                    DataBase = WebConfigurationManager.AppSettings["DataBase"],
                    Port = WebConfigurationManager.AppSettings["Port"],
                    User = WebConfigurationManager.AppSettings["User"],
                    PWD = WebConfigurationManager.AppSettings["PWD"]
                })
            };

            string UserName = WebConfigurationManager.AppSettings["UserName"];
            string UserPWD = WebConfigurationManager.AppSettings["UserPWD"];

            CORE CORE = new CORE(PLDB, UserName, UserPWD);
            CORE.SetTables();

            CORE.LinkOn += Reader.Ready;
            CORE.LinkOn += Writer.Ready;
            CORE.LinkOn += Counter.Ready;
            CORE.LinkOn += CommentLake.Ready;

            /* 启动内核 */
            CORE.Run();
        }



        /* 评论管理 */
        /// <summary>
        /// 取得被评论的文章的定制数据列表
        /// </summary>
        [WebMethod]
        public void Get_commented_posts()
        {
            var data = new List<Hashtable>();
            foreach (int ID in CommentLake.GetCommentedPostID())
            {
                /* 月计数和周计数 */
                int MonthCommentCount = 0;
                int WeekCommentCount = 0;
                foreach (Comment Comment in CommentLake.GetCommentList(ID))
                {
                    if (Comment.Time > DateTime.Now.AddMonths(-1))
                    {
                        MonthCommentCount++;
                    }
                    if (Comment.Time > DateTime.Now.AddDays(-7))
                    {
                        WeekCommentCount++;
                    }
                }
                /* 评论列表 */
                var CommentList = CommentLake.GetCommentList(ID);

                string Title = Reader.GetProperty<Title>(ID);

                var item = new Hashtable
                {
                    { "ID", ID },
                    { "Title", Title },
                    { "Content",Title == ""?Reader.GetProperty<Content>(ID):"" },
                    { "CommentCount",  CommentList.Count},
                    { "MonthCommentCount", MonthCommentCount },
                    { "WeekCommentCount", WeekCommentCount },
                    { "LatestCommentTime", CommentList.Last().Time }
                };

                data.Add(item);
            }

            Context.Response.Write(JsonConvert.SerializeObject(data, iso));
            Context.Response.End();
        }
        /// <summary>
        /// 取得指定文章的评论列表
        /// </summary>
        [WebMethod]
        public void Get_comments_by_PostID(int PostID)
        {
            var data = CommentLake.GetCommentList(PostID);

            Context.Response.Write(JsonConvert.SerializeObject(data, iso));
            Context.Response.End();
        }
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="CommentID"></param>
        [WebMethod]
        public void Delete_comment_by_CommentID(int CommentID)
        {
            Context.Response.Write(JsonConvert.SerializeObject(CommentLake.DeleteComment(CommentID), iso));
            Context.Response.End();
        }

        /* 读文章管理 */
        /// <summary>
        /// 取得计数
        /// </summary>
        [WebMethod]
        public void Get_counts()
        {
            Hashtable data = new Hashtable()
            {
                { "PostCount", Counter.TotalPostCount },
                { "CopyCount",  Counter.BackupCount },
                { "HiddenCount",  Counter.HiddenCount },
                { "OnDisplayCount",  Counter.OnDisplayCount },
                { "ArchivedCount",  Counter.ArchivedCount },
                { "ScheduledCount",  Counter.ScheduledCount },
                { "CommentCount",   CommentLake.TotalCommentCount},
            };

            Context.Response.Write(JsonConvert.SerializeObject(data));
            Context.Response.End();
        }
        /// <summary>
        /// 取得文章列表
        /// </summary>
        [WebMethod]
        public void Get_posts()
        {
            List<Post> data = new List<Post>();

            foreach (Post item in Reader.GetPost<ID>("^"))
            {
                item.PropertyContainer.Add("CommentCount", CommentLake.GetCommentCount(item.ID));
                data.Add(item);
            }

            Context.Response.Write(JsonConvert.SerializeObject(data, iso));
            Context.Response.End();
        }
        /// <summary>
        /// 取得文章数据
        /// </summary>
        /// <param name="ID"></param>
        [WebMethod]
        public void Get_post_by_PostID(int PostID)
        {
            Post data = Reader.GetPost(PostID);

            Context.Response.Write(JsonConvert.SerializeObject(data, iso));
            Context.Response.End();
        }
        /// <summary>
        /// 取得备份列表
        /// </summary>
        [WebMethod]
        public void Get_neg_posts_by_PostID(int PostID)
        {
            List<Post> data = Reader.GetPost<ID>(PostID.ToString(),true);

            Context.Response.Write(JsonConvert.SerializeObject(data, iso));
            Context.Response.End();
        }

        /* 写文章管理 */
        [WebMethod]
        public void Reg_post
            (
            string Mode, string Type, string User,
            int UVCount, int StarCount,
            string Title, string Summary, string Content,
            string Archiv, string Label, string Cover
            )
        {
            Context.Response.Write(Writer.Reg(new Post
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
        public void Dispose_post_by_PostID(int PostID)
        {
            Context.Response.Write(Writer.Dispose(PostID));
            Context.Response.End();
        }
        [WebMethod]
        public void Update_post_by_PostID
            (
            int PostID, string Mode, string Type,
            int UVCount, int StarCount,
            string Title, string Summary, string Content,
            string Archiv, string Label, string Cover
            )
        {
            Context.Response.Write(Writer.Update(new Post
            {
                ID = PostID,
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
        public void Delete_post_by_GUID(string GUID)
        {
            Context.Response.Write(Writer.Delete(GUID));
            Context.Response.End();
        }
        [WebMethod]
        public void Apply_post_by_GUID(string GUID)
        {
            Context.Response.Write(Writer.Apply(GUID));
            Context.Response.End();
        }
        [WebMethod]
        public void Rollback_post_by_PostID(int PostID)
        {
            Context.Response.Write(Writer.Rollback(PostID));
            Context.Response.End();
        }
        [WebMethod]
        public void Release_post_by_PostID(int PostID)
        {
            Context.Response.Write(Writer.Release(PostID));
            Context.Response.End();
        }
    }
}
