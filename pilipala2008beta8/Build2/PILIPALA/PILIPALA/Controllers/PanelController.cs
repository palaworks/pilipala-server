using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WaterLibrary.pilipala;
using WaterLibrary.pilipala.Entity;
using WaterLibrary.pilipala.Entity.PostProperty;
using WaterLibrary.pilipala.Components;

namespace PILIPALA.Controllers
{
    public class PanelController : Controller
    {
        public Reader Reader;
        public Writer Writer;
        public Counter Counter;
        public CommentLake CommentLake;

        private readonly ComponentFactory ComponentFactory = new();

        public PanelController(ICORE CORE)
        {
            CORE.SetTables();
            CORE.SetViews();

            CORE.CoreReady += ComponentFactory.Ready;

            /* 启动内核 */
            CORE.Run();

            Reader = ComponentFactory.GenReader();
            Writer = ComponentFactory.GenWriter();
            Counter = ComponentFactory.GenCounter();
            CommentLake = ComponentFactory.GenCommentLake();
        }

        public ActionResult List(bool ajax)
        {

            PostSet PostSet置顶 = new PostSet();
            foreach (Post el in Reader.GetPost<Archiv>("置顶"))
            {
                el.PropertyContainer.Add("CommentCount", CommentLake.GetCommentCount(el.ID));
                PostSet置顶.Add(el);
            }
            ViewBag.置顶文章 = PostSet置顶;

            PostSet PostSet其他 = new PostSet();
            foreach (Post el in Reader.GetPost<Archiv>("技术|生活"))
            {
                el.PropertyContainer.Add("CommentCount", CommentLake.GetCommentCount(el.ID));
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
            ViewBag.ID = ID;//请求ID

            ViewBag.Post = Reader.GetPost(ID);//文章数据
            ViewBag.Post.PropertyContainer.Add("CommentCount", CommentLake.GetCommentCount(ID));//添加评论计数，可优化


            ViewBag.CommentList = CommentLake.GetComments(ID);//评论数据

            ViewBag.PrevID = Reader.Smaller<ID>(ID, "生活|技术", typeof(Archiv));
            ViewBag.PrevTitle = Reader.GetProperty<Title>(ViewBag.PrevID);

            ViewBag.NextID = Reader.Bigger<ID>(ID, "生活|技术", typeof(Archiv));
            ViewBag.NextTitle = Reader.GetProperty<Title>(ViewBag.NextID);



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
