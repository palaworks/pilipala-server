using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PILIPALA.Models;
using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala.DB;
using WaterLibrary.com.CommentLake;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;
using WaterLibrary.com.pilipala.Components;

namespace PILIPALA
{
    public class Program
    {
        public static WaterLibrary.stru.pilipala.User User;
        /* 定义内核 */
        public static CORE CORE;
        /* 初始化配件 */
        public static Reader Reader = new Reader();
        public static Writer Writer = new Writer();
        public static Counter Counter = new Counter();
        public static CommentLake CommentLake = new CommentLake();

        public static void Main(string[] args)
        {
            /* 初始化噼里啪啦数据库信息和MySql控制器 */
            PLDB PLDB = new PLDB
            {
                MySqlManager = new MySqlManager(new MySqlConnMsg
                {
                    DataSource = "localhost",
                    DataBase = "pilipala",
                    Port = "3306",
                    User = "root",
                    PWD = "65a1561425f744e2b541303f628963f8"
                })
            };

            /* 初始化内核 */
            string UserName = "Thaumy";
            string UserPWD = "1238412384";

            CORE CORE = new CORE(PLDB, UserName, UserPWD);
            CORE.SetTables();
            CORE.SetViews();

            /* 设置内核准备完成后需要为其安装哪些配件 */
            CORE.LinkOn += Reader.Ready;
            CORE.LinkOn += Writer.Ready;
            CORE.LinkOn += Counter.Ready;
            CORE.LinkOn += CommentLake.Ready;

            /* 启动内核 */
            User = CORE.Run();


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
