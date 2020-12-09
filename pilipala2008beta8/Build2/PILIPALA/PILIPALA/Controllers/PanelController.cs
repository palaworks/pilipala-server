using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PILIPALA;
using WaterLibrary.stru.pilipala.Post;
using WaterLibrary.stru.pilipala.Post.Property;

namespace PILIPALA.Controllers
{
    public class PanelController : Controller
    {
        public ActionResult List(bool ajax)
        {
            ViewBag.置顶文章 = Program.Reader.GetPost<Archiv>("置顶");
            ViewBag.其他文章 = Program.Reader.GetPost<Archiv>("技术|生活");
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

            ViewBag.Post = Program.Reader.GetPost(ID);//文章数据
            ViewBag.CommentList = Program.CommentLake.GetCommentList(ID);//评论数据

            /* 前后文章标题赋值 */
            ViewBag.PrevID = Program.Reader.Smaller<ID>(ID, "生活|技术", typeof(Archiv));
            ViewBag.NextID = Program.Reader.Bigger<ID>(ID, "生活|技术", typeof(Archiv));



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
