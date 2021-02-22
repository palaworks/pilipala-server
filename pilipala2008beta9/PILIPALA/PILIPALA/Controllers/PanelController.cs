using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WaterLibrary.Utils;
using WaterLibrary.pilipala;
using WaterLibrary.pilipala.Entity;
using WaterLibrary.pilipala.Component;


namespace PILIPALA.Controllers
{
    using PILIPALA.Theme;

    public class PanelController : Controller
    {
        private Reader Reader;
        private Writer Writer;
        private Counter Counter;
        private CommentLake CommentLake;
        private ThemeHandler ThemeHandler;

        public PanelController(ComponentFactory compoFty, ThemeHandler ThemeHandler)
        {
            Reader = compoFty.GenReader(Reader.ReadMode.CleanRead);
            Writer = compoFty.GenWriter();
            Counter = compoFty.GenCounter();
            CommentLake = compoFty.GenCommentLake();

            this.ThemeHandler = ThemeHandler;
        }

        public ActionResult List(bool ajax)
        {
            string REGEXP(string s)
            {
                /* 读取主题配置文件并取得要在本页展示的文章归档 */
                var archive = ThemeHandler.Config["Pannel"][s].ToList();
                return ConvertH.ListToString(archive, '|');
            }

            PostSet PostSet置顶 = new PostSet();
            foreach (Post el in Reader.GetPost(PostProp.ArchiveName, REGEXP("ToppedArchive")))
            {
                el.PropertyContainer.Add("CommentCount", CommentLake.GetCommentCount(el.PostID));
                PostSet置顶.Add(el);
            }
            ViewBag.置顶文章 = PostSet置顶;

            PostSet PostSet其他 = new PostSet();
            foreach (Post el in Reader.GetPost(PostProp.ArchiveName, REGEXP("DefaultArchive")))
            {
                el.PropertyContainer.Add("CommentCount", CommentLake.GetCommentCount(el.PostID));
                PostSet其他.Add(el);
            }
            ViewBag.其他文章 = PostSet其他;

            if (ajax == false)
            {
                ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
                return View();
            }
            else
            {
                ViewBag.Layout = null;
                return View();
            }
        }
        public ActionResult Content(int ID, bool ajax)
        {
            string REGEXP(string s = "DefaultArchive")
            {
                /* 读取主题配置文件并取得要在本页展示的文章归档 */
                var archive = ThemeHandler.Config["Pannel"][s].ToList();
                return ConvertH.ListToString(archive, '|');
            }

            ViewBag.ID = ID;//请求ID

            ViewBag.Post = Reader.GetPost(ID);//文章数据
            ViewBag.Post.PropertyContainer.Add("CommentCount", CommentLake.GetCommentCount(ID));//添加评论计数，可优化


            ViewBag.CommentList = CommentLake.GetComments(ID);//评论数据

            ViewBag.PrevID = Reader.Smaller(ID, PostProp.PostID, REGEXP(), PostProp.ArchiveName);
            ViewBag.PrevTitle = Reader.GetPostProp(ViewBag.PrevID, PostProp.Title);

            ViewBag.NextID = Reader.Bigger(ID, PostProp.PostID, REGEXP(), PostProp.ArchiveName);
            ViewBag.NextTitle = Reader.GetPostProp(ViewBag.NextID, PostProp.Title);



            if (ajax == false)
            {
                ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
                return View();
            }
            else
            {
                ViewBag.Layout = null;
                return View();
            }

        }
    }
}
