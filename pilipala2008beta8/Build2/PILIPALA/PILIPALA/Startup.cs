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

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddTransient<ICORE>(x => new CORE(new PLDatabase
            {
                MySqlManager = new MySqlManager(new MySqlConnMsg
                {
                    DataSource = Configuration.GetSection("AppSettings:DataSource").Value,
                    DataBase = Configuration.GetSection("AppSettings:DataBase").Value,
                    Port = Configuration.GetSection("AppSettings:Port").Value,
                    User = Configuration.GetSection("AppSettings:User").Value,
                    PWD = Configuration.GetSection("AppSettings:PWD").Value
                })
            }, Configuration.GetSection("AppSettings:UserName").Value, Configuration.GetSection("AppSettings:UserPWD").Value));
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
