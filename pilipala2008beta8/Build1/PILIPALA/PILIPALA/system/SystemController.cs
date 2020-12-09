using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;

using PILIPALA.Models;
using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala.Post.Property;
using WaterLibrary.stru.pilipala.DB;
using WaterLibrary.stru.CommentLake;
using WaterLibrary.com.CommentLake;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;
using WaterLibrary.com.pilipala.Components;

using PILIPALA.Models.Guest;

namespace PILIPALA.system
{
    public class SystemController : Controller
    {
        public CORE CORE;
        public Reader Reader = new Reader();
        public Writer Writer = new Writer();
        public Counter Counter = new Counter();

        public CommentLake CommentLake = new CommentLake();

        public SystemController(IOptions<AppSettings> config)
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
