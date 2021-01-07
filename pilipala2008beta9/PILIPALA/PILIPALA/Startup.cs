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

            //配置节点集
            var MySQLSection = Configuration.GetSection("AppSettings:MySQL");
            var DatabaseSection = Configuration.GetSection("AppSettings:Database");
            var UserSection = Configuration.GetSection("AppSettings:User");

            var MySqlManager = new MySqlManager(new MySqlConnMsg
            {
                DataSource = MySQLSection.GetSection("DataSource").Value,
                Port = MySQLSection.GetSection("Port").Value,
                User = MySQLSection.GetSection("User").Value,
                PWD = MySQLSection.GetSection("PWD").Value,
                DataBase = DatabaseSection.GetSection("Name").Value
            });

            services.AddTransient(x => new Models.User()
            {
                Account = UserSection.GetSection("Account").Value,
                PWD = UserSection.GetSection("PWD").Value
            });

            var PLDatabase = new PLDatabase
            {
                Tables = new
                (
                    DatabaseSection.GetSection("Tables:User").Value,
                    DatabaseSection.GetSection("Tables:Index").Value,
                    DatabaseSection.GetSection("Tables:Backup").Value,
                    DatabaseSection.GetSection("Tables:Comment").Value
                ),

                ViewsSet = new
                (
                    new(
                        DatabaseSection.GetSection("ViewsSet:CleanViews:PosUnion").Value,
                        DatabaseSection.GetSection("ViewsSet:CleanViews:NegUnion").Value
                        ),
                    new(
                        DatabaseSection.GetSection("ViewsSet:DirtyViews:PosUnion").Value,
                        DatabaseSection.GetSection("ViewsSet:DIrtyViews:NegUnion").Value
                        )
                ),
                MySqlManager = MySqlManager
            };
            services.AddTransient<ICORE>(x => new CORE(PLDatabase));

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
            /* Pannel路由 */
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
