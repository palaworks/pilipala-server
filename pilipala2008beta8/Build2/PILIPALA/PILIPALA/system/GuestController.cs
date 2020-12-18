using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Cors;

using WaterLibrary.pilipala.Entity.PostProperty;
using WaterLibrary.pilipala;
using WaterLibrary.CommentLake;
using WaterLibrary.pilipala.Components;

using PILIPALA.Models.Guest;

namespace PILIPALA.system
{
    public class GuestController : Controller
    {
        public Reader Reader = new Reader();
        public Writer Writer = new Writer();
        public Counter Counter = new Counter();

        public CommentLake CommentLake = new CommentLake();

        public GuestController(ICORE CORE)
        {
            CORE.SetTables();
            CORE.SetViews(PosUnion: "pos>dirty>union", NegUnion: "neg>dirty>union");

            CORE.LinkOn += Reader.Ready;
            CORE.LinkOn += Writer.Ready;
            CORE.LinkOn += Counter.Ready;
            CORE.LinkOn += CommentLake.Ready;

            /* 启动内核 */
            CORE.Run();
        }

        /// <summary>
        /// 星星计数减一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [HttpPost]
        public int Decrease_StarCount_by_PostID(int PostID)
        {
            int StarCount = Convert.ToInt32(Reader.GetProperty<StarCount>(PostID));

            Writer.UpdateIndex<StarCount>(PostID, StarCount - 1);

            return StarCount - 1;
        }
        /// <summary>
        /// 星星计数加一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [HttpPost]
        public int Increase_StarCount_by_PostID(int PostID)
        {
            int StarCount = Convert.ToInt32(Reader.GetProperty<StarCount>(PostID));

            Writer.UpdateIndex<StarCount>(PostID, StarCount + 1);

            return StarCount + 1;

        }
        /// <summary>
        /// 浏览计数加一
        /// </summary>
        /// <param name="ID">文章序列号</param>

        [HttpPost]
        public int Increase_UVCount_by_PostID(int PostID)
        {

            int UVCount = Convert.ToInt32(Reader.GetProperty<UVCount>(PostID));

            Writer.UpdateIndex<UVCount>(PostID, UVCount + 1);

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
