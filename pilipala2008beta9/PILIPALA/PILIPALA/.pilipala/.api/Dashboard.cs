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

using WaterLibrary.Utils;
using WaterLibrary.pilipala;
using WaterLibrary.pilipala.Entity;
using WaterLibrary.pilipala.Component;


namespace PILIPALA.API
{
    using PILIPALA.Models.Form;

    [EnableCors("DefaultPolicy")]
    public class Dashboard : Controller
    {
        private ComponentFactory compoFty;
        private Authentication Authentication;
        private readonly Reader Reader, BackUpReader;
        private readonly Writer Writer;
        private readonly Counter Counter;
        private readonly CommentLake CommentLake;
        private new User User;

        public Dashboard(ComponentFactory compoFty, Models.UserModel UserModel)
        {
            if ((DateTime.Now - Convert.ToDateTime(CORE.MySqlManager.GetKey($"SELECT TokenTime FROM {CORE.Tables.User} WHERE GroupType = 'user'"))).TotalMinutes <= 120)
            {
                User = compoFty.GenUser(UserModel.Account, UserModel.PWD);
                Authentication = compoFty.GenAuthentication(User);
                Reader = compoFty.GenReader(Reader.ReadMode.DirtyRead);
                BackUpReader = compoFty.GenReader(Reader.ReadMode.DirtyRead, true);
                Writer = compoFty.GenWriter();
                Counter = compoFty.GenCounter();
                CommentLake = compoFty.GenCommentLake();
            }
            this.compoFty = compoFty;
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="UserName">登录用户名</param>
        /// <param name="UserPWD">登录用户密码</param>
        /// <returns></returns>
        public string Login(string UserAccount, string UserPWD)
        {
            User = compoFty.GenUser(UserAccount, UserPWD);
            Authentication = compoFty.GenAuthentication(User);
            KeyPair KeyPair = new KeyPair(2048);
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
        /// 取得系统信息
        /// </summary>
        /// <returns>返回内核版本</returns>
        public string Get_system_info(string Token)
        {
            return Authentication.Auth(Token, () =>
            {
                return JsonConvert.SerializeObject(new Hashtable()
                {
                    { "pilipala_version", "BETA8"  },
                    { "core_version",  WaterLibrary.Assembly.Version},
                    { "auth",  "启用"},
                    { "auth_end_time",  Authentication.GetTokenTime().AddHours(2).ToString() },
                });
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

                    string Title = Convert.ToString(Reader.GetPostProp(ID, PostProp.Title));

                    var item = new Hashtable
                    {
                    { "ID", ID },
                    { "Title", Title },
                    { "Content",Title == ""?Reader.GetPostProp(ID,PostProp.Content):"" },
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
                  CommentLake.DelComment(CommentID));
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
               { "CopyCount",  Counter.StackCount },
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
                   Reader.GetPost(PostProp.PostID, "^")
                      .ForEach((item) =>
                      {
                          item.PropertyContainer = new()
                          {
                              { "CommentCount", CommentLake.GetCommentCount(item.PostID) },
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
             BackUpReader.GetPost(PostProp.PostID, PostID.ToString())
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

                ArchiveName = PostModel.ArchiveName,
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
                PostID = PostModel.PostID,
                Mode = PostModel.Mode,
                Type = PostModel.Type,
                User = PostModel.User,

                UVCount = PostModel.UVCount,
                StarCount = PostModel.StarCount,

                Title = PostModel.Title,
                Summary = PostModel.Summary,
                Content = PostModel.Content,

                ArchiveName = PostModel.ArchiveName,
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
