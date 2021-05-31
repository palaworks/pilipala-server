using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PILIPALA.Controllers
{
    using PILIPALA.Theme;
    using WaterLibrary.Utils;
    using WaterLibrary.pilipala.Entity;
    using WaterLibrary.pilipala.Component;

    public class PanelController : Controller
    {
        private Reader Reader;
        private Writer Writer;
        private Counter Counter;
        private Pluginer Pluginer;
        private CommentLake CommentLake;
        private ThemeHandler ThemeHandler;

        string LightningLinkUUID;

        public PanelController(ComponentFactory compoFty, ThemeHandler ThemeHandler)
        {
            Reader = compoFty.GenReader(Reader.ReadMode.CleanRead);
            Writer = compoFty.GenWriter();
            Counter = compoFty.GenCounter();
            Pluginer = compoFty.GenPluginer();
            CommentLake = compoFty.GenCommentLake();

            this.ThemeHandler = ThemeHandler;

            //load ll plugin
            string pluginRoot = "./.pilipala/.plugin";
            LightningLinkUUID = Pluginer.LoadPlugin(
                pluginRoot + "/LightningLink/LightningLink.dll",
                "piliplugin.LightningLink",
                new[] { pluginRoot + "/LightningLink/LightningLink.json" });
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
                el.Cover = ContentProcessPipeline(el.Cover);//ll plugin
                el.Content = ContentProcessPipeline(el.Content);//ll plugin 
                PostSet置顶.Add(el);
            }

            ViewBag.置顶文章 = PostSet置顶;

            PostSet PostSet其他 = new PostSet();
            foreach (Post el in Reader.GetPost(PostProp.ArchiveName, REGEXP("DefaultArchive")))
            {
                el.PropertyContainer.Add("CommentCount", CommentLake.GetCommentCount(el.PostID));
                el.Cover = ContentProcessPipeline(el.Cover);//ll plugin
                el.Content = ContentProcessPipeline(el.Content);//ll plugin 
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

            var Post = Reader.GetPost(ID);

            Post.Cover = ContentProcessPipeline(Post.Cover);//ll plugin
            Post.Content = ContentProcessPipeline(Post.Content);//ll plugin 

            ViewBag.Post = Post; //文章数据
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

        //内容处理管道，用于暂时接替WL管道功能
        private string ContentProcessPipeline(string content)
        {
            content = (string)Pluginer.Invoke(LightningLinkUUID, "ApplyLink", new object[] { content });//ll plugin
            return content;
        }
    }
}
