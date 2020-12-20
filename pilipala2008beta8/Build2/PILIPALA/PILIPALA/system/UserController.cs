using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Cors;

using WaterLibrary.pilipala;
using WaterLibrary.pilipala.Entity.PostProperty;
using WaterLibrary.pilipala.Entity;
using WaterLibrary.CommentLake;
using WaterLibrary.pilipala.Components;

using PILIPALA.Models.UserModel;

namespace PILIPALA.system
{
    [EnableCors("DefaultPolicy")]
    public class UserController : Controller
    {
        public Reader Reader;
        public Writer Writer;
        public Counter Counter;
        private ComponentFactory ComponentFactory = new ComponentFactory();

        public CommentLake CommentLake = new CommentLake();

        public UserController(ICORE CORE)
        {
            CORE.SetTables();
            CORE.SetViews(PosUnion: "pos>dirty>union", NegUnion: "neg>dirty>union");

            CORE.LinkOn += ComponentFactory.Ready;
            CORE.LinkOn += CommentLake.Ready;

            /* 启动内核 */
            CORE.Run();

            Reader = ComponentFactory.GenReader();
            Writer = ComponentFactory.GenWriter();
            Counter = ComponentFactory.GenCounter();
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
                var CommentSet = CommentLake.GetComments(ID);

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
            return CommentLake.GetComments(PostID).ToJSON();
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
            return Reader.GetPost<ID>("^")
                .ForEach((item) =>
                {
                    item.PropertyContainer = new Hashtable()
                    {
                        { "CommentCount", CommentLake.GetCommentCount(item.ID) },
                        { "MD5", item.MD5() }
                    };
                }).ToJSON();
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
            return Reader.GetPost<ID>(PostID.ToString(), true)
                .ForEach((item) =>
                {
                    item.PropertyContainer.Add("MD5", item.MD5());
                }
                ).ToJSON();
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


        public bool Delete_post_by_GUID(string GUID) => Writer.Delete(GUID);
        public bool Apply_post_by_GUID(string GUID) => Writer.Apply(GUID);
        public bool Rollback_post_by_PostID(int PostID) => Writer.Rollback(PostID);
        public bool Release_post_by_PostID(int PostID) => Writer.Release(PostID);
    }
}
