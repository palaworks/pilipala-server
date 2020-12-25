using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WaterLibrary.pilipala.Components;
using WaterLibrary.pilipala;
using PILIPALA.Models;
using WaterLibrary.MySQL;
using WaterLibrary.pilipala.Database;

namespace PILIPALA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            using MySqlManager MySqlManager = new MySqlManager(new MySqlConnMsg
            {
                DataSource = Configuration.GetSection("AppSettings:MySQL:DataSource").Value,
                DataBase = Configuration.GetSection("AppSettings:MySQL:DataBase").Value,
                Port = Configuration.GetSection("AppSettings:MySQL:Port").Value,
                User = Configuration.GetSection("AppSettings:MySQL:User").Value,
                PWD = Configuration.GetSection("AppSettings:MySQL:PWD").Value
            });

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddTransient<ICORE>(x => new CORE(new PLDatabase { MySqlManager = MySqlManager }));

            MySqlManager.Open();

            if ((DateTime.Now - Convert.ToDateTime(MySqlManager.GetKey("SELECT TokenTime FROM pl_user WHERE GroupType = 'user'"))).TotalMinutes < 120)
            {
                CORE CORE = new CORE(new PLDatabase { MySqlManager = MySqlManager });

                var ComponentFactory = new ComponentFactory();

                CORE.SetTables();
                CORE.SetViews(PosUnion: "pos>dirty>union", NegUnion: "neg>dirty>union");

                CORE.CoreReady += ComponentFactory.Ready;

                var UserAccount = Configuration.GetSection("AppSettings:User:Account").Value;
                var UserPWD = Configuration.GetSection("AppSettings:User:PWD").Value;

                var User = CORE.Run(UserAccount, UserPWD);

                var Authentication = ComponentFactory.GenAuthentication();
                var Reader = ComponentFactory.GenReader();
                var Writer = ComponentFactory.GenWriter();
                var Counter = ComponentFactory.GenCounter();
                var CommentLake = ComponentFactory.GenCommentLake();

                services.AddTransient(x => Authentication);
                services.AddTransient(x => Reader);
                services.AddTransient(x => Writer);
                services.AddTransient(x => Counter);
                services.AddTransient(x => CommentLake);
                services.AddTransient(x => User);
            }

            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "UserAPI",
                    pattern: "user/{action}",
                    defaults: new { controller = "User" });
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "GuestAPI",
                    pattern: "guest/{action}",
                    defaults: new { controller = "Guest" });
            });
            /* PannelÂ·ÓÉ */
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "List",
                    pattern: "",
                    defaults: new { controller = "Panel", action = "List", ajax = false });
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Content",
                    pattern: "{ID}",
                    defaults: new { controller = "Panel", action = "Content", ajax = false });
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "@List",
                    pattern: "@/-1",
                    defaults: new { controller = "Panel", action = "List", ajax = true });
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "@Content",
                    pattern: "@/{ID}",
                    defaults: new { controller = "Panel", action = "Content", ajax = true });
            });
        }
    }
}
