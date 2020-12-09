using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Collections;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using PILIPALA.Models;
using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala.Post.Property;
using WaterLibrary.stru.pilipala.DB;
using WaterLibrary.stru.pilipala.Post;
using WaterLibrary.stru.CommentLake;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;
using WaterLibrary.com.pilipala.Components;
using WaterLibrary.com.CommentLake;

using PILIPALA.Models.UserModel;

namespace PILIPALA.system
{
    public class UserController : Controller
    {
        public CORE CORE;
        public Reader Reader = new Reader();
        public Writer Writer = new Writer();
        public Counter Counter = new Counter();

        public CommentLake CommentLake = new CommentLake();



        public UserController(IOptions<AppSettings> config)
        {
            /* 初始化噼里啪啦数据库信息和MySql控制器 */
            PLDB PLDB = new PLDB
            {
                Views = new PLViews() { PosUnion = "pos>dirty>union", NegUnion = "neg>dirty>union" },
                MySqlManager = new MySqlManager(new MySqlConnMsg
                {
                    DataSource = config.Value.DataSource,
                    DataBase = config.Value.DataBase,
                    Port = config.Value.Port,
                    User = config.Value.User,
                    PWD = config.Value.PWD
                })
            };

            string UserName = config.Value.UserName;
            string UserPWD = config.Value.UserPWD;

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
        public string Get_commented_posts()
        {
            var data = new List<Hashtable>();
            foreach (int ID in CommentLake.GetCommentedPostID())
            {

                /* 评论列表 */
                var CommentSet = CommentLake.GetCommentList(ID);

                string Title = Convert.ToString(Reader.GetProperty<Title>(ID));

                var item = new Hashtable
                {
                    { "ID", ID },
                    { "Title", Title },
                    { "Content",Title == ""?Reader.GetProperty<Content>(ID):"" },
                    { "CommentCount",  CommentSet.Count},
                    { "MonthCommentCount", CommentSet.WithinMonthCount() },
                    { "WeekCommentCount", CommentSet.WithinWeekCount() },
                    { "LatestCommentTime", CommentSet.Last().Time }
                };

                data.Add(item);
            }

            return JsonConvert.SerializeObject(data, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }

        /// <summary>
        /// 取得指定文章的评论列表
        /// </summary>
        public string Get_comments_by_PostID(int PostID)
        {
            return CommentLake.GetCommentList(PostID).ToJSON();
        }
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="CommentID"></param>
        public bool Delete_comment_by_CommentID(int CommentID)
        {
            return CommentLake.DeleteComment(CommentID);
        }

        /* 读文章管理 */
        /// <summary>
        /// 取得计数
        /// </summary>
        public string Get_counts()
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

            return JsonConvert.SerializeObject(data);
        }
        /// <summary>
        /// 取得文章列表
        /// </summary>
        public string Get_posts()
        {
            var data = new PostSet();

            foreach (Post item in Reader.GetPost<ID>("^"))
            {
                item.PropertyContainer.Add("CommentCount", CommentLake.GetCommentCount(item.ID));
                item.PropertyContainer.Add("MD5", item.MD5());
                data.Add(item);
            }

            return data.ToJSON();

        }
        /// <summary>
        /// 取得文章数据
        /// </summary>
        /// <param name="ID"></param>
        public string Get_post_by_PostID(int PostID)
        {
            return Reader.GetPost(PostID).ToJSON();

        }
        /// <summary>
        /// 取得备份列表
        /// </summary>
        public string Get_neg_posts_by_PostID(int PostID)
        {
            var data = new PostSet();

            foreach (Post item in Reader.GetPost<ID>(PostID.ToString(), true))
            {
                item.PropertyContainer.Add("MD5", item.MD5());
                data.Add(item);
            }

            return data.ToJSON();

        }

        /* 写文章管理 */

        public bool Reg_post(PostModel PostModel)
        {
            return Writer.Reg(new Post
            {
                Mode = PostModel.Mode,
                Type = PostModel.Type,
                User = PostModel.User,

                UVCount = PostModel.UVCount,
                StarCount = PostModel.StarCount,

                Title = PostModel.Title,
                Summary = PostModel.Summary,
                Content = PostModel.Content,

                Archiv = PostModel.Archiv,
                Label = PostModel.Label,
                Cover = PostModel.Cover
            });

        }

        public bool Dispose_post_by_PostID(int PostID)
        {
            return Writer.Dispose(PostID);
        }

        public bool Update_post_by_PostID(PostModel PostModel)
        {
            return Writer.Update(new Post
            {
                ID = PostModel.PostID,
                Mode = PostModel.Mode,
                Type = PostModel.Type,

                UVCount = PostModel.UVCount,
                StarCount = PostModel.StarCount,

                Title = PostModel.Title,
                Summary = PostModel.Summary,
                Content = PostModel.Content,

                Archiv = PostModel.Archiv,
                Label = PostModel.Label,
                Cover = PostModel.Cover
            });

        }


        public bool Delete_post_by_GUID(string GUID)
        {
            return Writer.Delete(GUID);

        }

        public bool Apply_post_by_GUID(string GUID)
        {
            return Writer.Apply(GUID);

        }

        public bool Rollback_post_by_PostID(int PostID)
        {
            return Writer.Rollback(PostID);

        }

        public bool Release_post_by_PostID(int PostID)
        {
            return Writer.Release(PostID);

        }

    }
}
