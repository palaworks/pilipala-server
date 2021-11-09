using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Cors;

using WaterLibrary.Utils;
using WaterLibrary.pilipala;
using WaterLibrary.pilipala.Entity;
using WaterLibrary.pilipala.Component;


namespace PILIPALA.API
{
    using PILIPALA.Models.Form;
    using static WaterLibrary.pilipala.Entity.PostRecord;

    [EnableCors("DefaultPolicy")]
    public class Dashboard : Controller
    {
        private readonly ComponentFactory fac;
        private Auth Auth;
        private readonly Reader Reader, BackUpReader;
        private readonly Counter Counter;
        private readonly CommentLake CommentLake;
        private new User User;

        public Dashboard(ComponentFactory fac, Models.UserModel UserModel)
        {
            if ((DateTime.Now - Convert.ToDateTime(PiliPala.MySqlManager.GetKey($"SELECT TokenTime FROM {PiliPala.Tables.User} WHERE GroupType = 'user'"))).TotalMinutes <= 120)
            {
                User = fac.GenUser(UserModel.Account, UserModel.PWD);
                Auth = fac.GenAuthentication(User);
                Reader = fac.GenReader(Reader.ReadMode.DirtyRead);
                BackUpReader = fac.GenReader(Reader.ReadMode.DirtyRead, true);
                Counter = fac.GenCounter();
                CommentLake = fac.GenCommentLake();
            }
            this.fac = fac;
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="UserName">登录用户名</param>
        /// <param name="UserPWD">登录用户密码</param>
        /// <returns></returns>
        public string Login(string UserAccount, string UserPWD)
        {
            User = fac.GenUser(UserAccount, UserPWD);
            Auth = fac.GenAuthentication(User);
            KeyPair KeyPair = new KeyPair(2048);
            Auth.SetPrivateKey(KeyPair.PrivateKey);
            Auth.UpdateTokenTime();

            return KeyPair.PublicKey;
        }

        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <returns></returns>
        public string Get_user_data(string Token)
        {
            return Auth.CheckAuth(Token, () =>
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
            return Auth.CheckAuth(Token, () =>
            {
                return JsonConvert.SerializeObject(new Hashtable()
                {
                    { "pilipala_version", "BETA8"  },
                    { "core_version",  WaterLibrary.Assembly.Version},
                    { "auth",  "启用"},
                    { "auth_end_time",  Auth.GetTokenTime().AddHours(2).ToString() },
                });
            });
        }

        /* 评论管理 */
        /// <summary>
        /// 取得被评论的文章的定制数据列表
        /// </summary>
        public string Get_commented_posts(string Token)
        {
            return Auth.CheckAuth(Token, () =>
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
            return Auth.CheckAuth(Token, () =>
                 ((IJsonSerializable)CommentLake.GetComments(PostID)).ToJson());
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="CommentID"></param>
        public bool Delete_comment_by_CommentID(string Token, int CommentID)
        {
            return Auth.CheckAuth(Token, () =>
                  CommentLake.DelComment(CommentID));
        }


        /* 读文章管理 */
        /// <summary>
        /// 取得计数
        /// </summary>
        public string Get_counts(string Token)
        {
            return Auth.CheckAuth(Token, () =>
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
            return Auth
                .CheckAuth(Token, () =>
                   Reader.GetPostStacks(PostProp.PostID, "^")
                      .Map((item) =>
                      {
                          item.Peek.PropertyContainer.Add("CommentCount", CommentLake.GetCommentCount((int)item.Peek.ID));
                      }).ToJSON());
        }

        /// <summary>
        /// 取得文章数据
        /// </summary>
        /// <param name="ID"></param>
        public string Get_post_by_PostID(string Token, int PostID)
        {
            return Auth.CheckAuth(Token, () => ((IJsonSerializable)new PostStack((uint)PostID).Peek).ToJson());
        }
        /// <summary>
        /// 取得备份列表
        /// </summary>
        public string Get_neg_posts_by_PostID(string Token, int PostID)
        {
            return Auth.CheckAuth(Token, () =>
             BackUpReader.GetPostStacks(PostProp.PostID, PostID.ToString())
                 .Map((item) =>
                 {
                     item.Peek.PropertyContainer.Add("MD5", item.Peek.MD5());
                 }
                 ).ToJSON());
        }

        /* 写文章管理 */
        public bool Reg_post(string Token, PostModel PostModel)
        {
            return Auth.CheckAuth(Token, () =>
            {
                var ps = new PostStack();
                var pk = ps.Peek;

                pk.Mode = (ModeState)Enum.Parse(typeof(ModeState), PostModel.Mode);
                pk.Type = (TypeState)Enum.Parse(typeof(TypeState), PostModel.Type);
                pk.User = PostModel.User;

                pk.UVCount = (uint)PostModel.UVCount;
                pk.StarCount = (uint)PostModel.StarCount;

                pk.Title = PostModel.Title;
                pk.Summary = PostModel.Summary;
                pk.Content = PostModel.Content;

                //pk.ArchiveName = PostModel.ArchiveName,
                pk.Label = PostModel.Label;
                pk.Cover = PostModel.Cover;

                return true;//
            });
        }
        public bool Dispose_post_by_PostID(string Token, uint PostID)
        {
            return Auth.CheckAuth(Token, () => new PostStack(PostID).Dispose());
        }
        public bool Update_post_by_PostID(string Token, PostModel PostModel)
        {
            return Auth.CheckAuth(Token, () =>
            {
                var postStack = new PostStack((uint)PostModel.PostID);
                var item = new PostRecord((uint)postStack.ID)
                {
                    Mode = (ModeState)Enum.Parse(typeof(ModeState), PostModel.Mode),
                    Type = (TypeState)Enum.Parse(typeof(TypeState), PostModel.Type),
                    User = PostModel.User,

                    UVCount = (uint)PostModel.UVCount,
                    StarCount = (uint)PostModel.StarCount,

                    Title = PostModel.Title,
                    Summary = PostModel.Summary,
                    Content = PostModel.Content,

                    //item.ArchiveName = PostModel.ArchiveName;//TODO
                    Label = PostModel.Label,
                    Cover = PostModel.Cover
                };

                postStack.Push(item);//其实应返回成功/失败的Bool值
                return true;//TODO，此处原来有失败情况
            }
            );
        }


        public bool Delete_post_by_GUID(string Token, uint ID, string GUID)
        {
            return Auth.CheckAuth(Token, () => new PostStack(ID).Delete(GUID));
        }
        /*public bool Apply_post_by_GUID(string Token, uint ID, string GUID)
        {
            return Auth.CheckAuth(Token, () => new PostStack(ID).RePeek(GUID));
        }*/
        public bool Rollback_post_by_PostID(string Token, uint ID)
        {
            return Auth.CheckAuth(Token, () => { new PostStack(ID).Pop(); return true;/*TODO，返回值逻辑*/ });
        }
        public bool Release_post_by_PostID(string Token, uint ID, int PostID)
        {
            return Auth.CheckAuth(Token, () => new PostStack(ID).Clean());
        }
    }
}
