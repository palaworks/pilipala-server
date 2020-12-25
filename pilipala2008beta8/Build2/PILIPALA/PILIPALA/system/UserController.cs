using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;

using WaterLibrary.Tools;
using WaterLibrary.pilipala;
using WaterLibrary.pilipala.Entity.PostProperty;
using WaterLibrary.pilipala.Entity;
using WaterLibrary.pilipala.Components;
using WaterLibrary.MySQL;

using PILIPALA.Models.UserModel;

namespace PILIPALA.system
{
    using WaterLibrary.pilipala.Components;

    [EnableCors("DefaultPolicy")]
    public class UserController : Controller
    {
        private Authentication Authentication;
        private Reader Reader;
        private Writer Writer;
        private Counter Counter;
        private CommentLake CommentLake;
        private User User;

        private ICORE CORE;

        public UserController(ICORE CORE, Authentication Authentication = null, Reader Reader = null, Writer Writer = null, Counter Counter = null, CommentLake CommentLake = null, User User = null)
        {
            this.CORE = CORE;
            CORE.SetTables();
            CORE.SetViews(PosUnion: "pos>dirty>union", NegUnion: "neg>dirty>union");

            this.Authentication = Authentication;
            this.Reader = Reader;
            this.Writer = Writer;
            this.Counter = Counter;
            this.CommentLake = CommentLake;
            this.User = User;
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="UserName">登录用户名</param>
        /// <param name="UserPWD">登录用户密码</param>
        /// <returns></returns>
        public string Login(string UserAccount, string UserPWD)
        {
            ComponentFactory ComponentFactory = new();
            CORE.CoreReady += ComponentFactory.Ready;
            CORE.Run(UserAccount, UserPWD);

            Authentication = ComponentFactory.GenAuthentication();
            KeyPair KeyPair = new KeyPair(2048, true);
            Authentication.SetPrivateKey(KeyPair.PrivateKey);
            Authentication.UpdateTokenTime();

            return KeyPair.PublicKey;
        }

        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <returns></returns>
        public string Get_user_data(string Token)
        {
            return Authentication.Auth(Token, () =>
            {
                return User.ToJSON();
            });
        }
        /// <summary>
        /// 取得内核版本
        /// </summary>
        /// <returns>返回内核版本</returns>
        public string Get_core_version(string Token)
        {
            return Authentication.Auth(Token, () =>
            {
                return WaterLibrary.Assembly.Version;
            });
        }

        /* 评论管理 */
        /// <summary>
        /// 取得被评论的文章的定制数据列表
        /// </summary>
        public string Get_commented_posts(string Token)
        {
            return Authentication.Auth(Token, () =>
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
            });
        }
        /// <summary>
        /// 取得指定文章的评论列表
        /// </summary>
        public string Get_comments_by_PostID(string Token, int PostID)
        {
            return Authentication.Auth(Token, () =>
                 CommentLake.GetComments(PostID).ToJSON());
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="CommentID"></param>
        public bool Delete_comment_by_CommentID(string Token, int CommentID)
        {
            return Authentication.Auth(Token, () =>
                  CommentLake.DeleteComment(CommentID));
        }


        /* 读文章管理 */
        /// <summary>
        /// 取得计数
        /// </summary>
        public string Get_counts(string Token)
        {
            return Authentication.Auth(Token, () =>
               JsonConvert.SerializeObject(new Hashtable()
              {
                { "PostCount", Counter.TotalPostCount },
                { "CopyCount",  Counter.BackupCount },
                { "HiddenCount",  Counter.HiddenCount },
                { "OnDisplayCount",  Counter.OnDisplayCount },
                { "ArchivedCount",  Counter.ArchivedCount },
                { "ScheduledCount",  Counter.ScheduledCount },
                { "CommentCount",   CommentLake.TotalCommentCount},
              }));
        }

        /// <summary>
        /// 取得文章列表
        /// </summary>
        public string Get_posts(string Token)
        {
            return Authentication
                .Auth(Token, () =>
                   Reader.GetPost<ID>("^")
                      .ForEach((item) =>
                      {
                          item.PropertyContainer = new Hashtable()
                          {
                              { "CommentCount", CommentLake.GetCommentCount(item.ID) },
                              { "MD5", item.MD5() }
                          };
                      }).ToJSON());
        }

        /// <summary>
        /// 取得文章数据
        /// </summary>
        /// <param name="ID"></param>
        public string Get_post_by_PostID(string Token, int PostID)
        {
            return Authentication.Auth(Token, () => Reader.GetPost(PostID).ToJSON());
        }
        /// <summary>
        /// 取得备份列表
        /// </summary>
        public string Get_neg_posts_by_PostID(string Token, int PostID)
        {
            return Authentication.Auth(Token, () =>
             Reader.GetPost<ID>(PostID.ToString(), true)
                 .ForEach((item) =>
                 {
                     item.PropertyContainer.Add("MD5", item.MD5());
                 }
                 ).ToJSON());
        }

        /* 写文章管理 */
        public bool Reg_post(string Token, PostModel PostModel)
        {
            return Authentication.Auth(Token, () => Writer.Reg(new Post
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
            }));
        }
        public bool Dispose_post_by_PostID(string Token, int PostID)
        {
            return Authentication.Auth(Token, () => Writer.Dispose(PostID));
        }
        public bool Update_post_by_PostID(string Token, PostModel PostModel)
        {
            return Authentication.Auth(Token, () => Writer.Update(new Post
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
            }));
        }


        public bool Delete_post_by_GUID(string Token, string GUID)
        {
            return Authentication.Auth(Token, () => Writer.Delete(GUID));
        }
        public bool Apply_post_by_GUID(string Token, string GUID)
        {
            return Authentication.Auth(Token, () => Writer.Apply(GUID));
        }
        public bool Rollback_post_by_PostID(string Token, int PostID)
        {
            return Authentication.Auth(Token, () => Writer.Rollback(PostID));
        }
        public bool Release_post_by_PostID(string Token, int PostID)
        {
            return Authentication.Auth(Token, () => Writer.Release(PostID));
        }
    }
}
