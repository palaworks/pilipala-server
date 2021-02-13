using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Cors;

using WaterLibrary.pilipala;
using WaterLibrary.pilipala.Entity;
using WaterLibrary.pilipala.Component;


namespace PILIPALA.API
{
    using PILIPALA.Models.Form;

    public class Guest : Controller
    {
        public Reader Reader;
        public Writer Writer;
        public Counter Counter;
        public CommentLake CommentLake;

        public Guest()
        {
            Reader = ComponentFactory.Instance.GenReader(Reader.ReadMode.DirtyRead, true);
            Writer = ComponentFactory.Instance.GenWriter();
            Counter = ComponentFactory.Instance.GenCounter();
            CommentLake = ComponentFactory.Instance.GenCommentLake();
        }

        /// <summary>
        /// 星星计数减一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [HttpPost]
        public int Decrease_StarCount_by_PostID(int PostID)
        {
            int StarCount = Convert.ToInt32(Reader.GetPostProp(PostID, PostProp.StarCount));

            Counter.SetStarCount(PostID, StarCount - 1);

            return StarCount - 1;
        }
        /// <summary>
        /// 星星计数加一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [HttpPost]
        public int Increase_StarCount_by_PostID(int PostID)
        {
            int StarCount = Convert.ToInt32(Reader.GetPostProp(PostID, PostProp.StarCount));

            Counter.SetStarCount(PostID, StarCount + 1);

            return StarCount + 1;

        }
        /// <summary>
        /// 浏览计数加一
        /// </summary>
        /// <param name="ID">文章序列号</param>

        [HttpPost]
        public int Increase_UVCount_by_PostID(int PostID)
        {

            int UVCount = Convert.ToInt32(Reader.GetPostProp(PostID, PostProp.UVCount));

            Counter.SetUVCount(PostID, UVCount + 1);

            return UVCount + 1;
        }

        /// <summary>
        /// 取得内核版本
        /// </summary>
        /// <returns>返回内核版本</returns>
        [HttpPost]
        public string Get_core_version()
        {
            return WaterLibrary.Assembly.Version;
        }

        /// <summary>
        /// 评论验证
        /// </summary>
        /// <param name="PostID"></param>
        /// <param name="HEAD"></param>
        /// <param name="Content"></param>
        /// <param name="User"></param>
        /// <param name="Email"></param>
        /// <param name="WebSite"></param>
        /// <returns></returns>
        [HttpPost]
        public string CommentLakeCaptcha(CommentModel CommentModel)
        {
            CommentLake.AddComment(new Comment()
            {
                PostID = CommentModel.PostID,
                User = CommentModel.User,
                Email = CommentModel.Email,
                Content = CommentModel.Content,
                WebSite = CommentModel.WebSite,
                HEAD = CommentModel.HEAD
            });

            return "CommentLake : 提交成功";
        }
    }
}
