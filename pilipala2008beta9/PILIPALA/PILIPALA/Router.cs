using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


namespace PILIPALA
{
    public partial class Startup
    {
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


            app.UseEndpoints(endpoints =>/* 用户API（为兼容保留，已过时） */
            {
                endpoints.MapControllerRoute(
                    name: "API.User",
                    pattern: "user/{action}",
                    defaults: new { controller = "Dashboard" });
            });

            app.UseEndpoints(endpoints =>/* 访客API */
            {
                endpoints.MapControllerRoute(
                    name: "API.Guest",
                    pattern: "guest/{action}",
                    defaults: new { controller = "Guest" });
            });
            app.UseEndpoints(endpoints =>/* 访客API */
            {
                endpoints.MapControllerRoute(
                    name: "API.Dashboard",
                    pattern: "dashboard/{action}",
                    defaults: new { controller = "Dashboard" });
            });

            app.UseEndpoints(endpoints =>/* 非ajax文章列表 */
            {
                endpoints.MapControllerRoute(
                    name: "List",
                    pattern: "",
                    defaults: new { controller = "Panel", action = "List", ajax = false });
            });
            app.UseEndpoints(endpoints =>/* 非ajax文章内容 */
            {
                endpoints.MapControllerRoute(
                    name: "Content",
                    pattern: "{ID}",
                    defaults: new { controller = "Panel", action = "Content", ajax = false });
            });
            app.UseEndpoints(endpoints =>/* ajax文章列表 */
            {
                endpoints.MapControllerRoute(
                    name: "@List",
                    pattern: "@/-1",
                    defaults: new { controller = "Panel", action = "List", ajax = true });
            });
            app.UseEndpoints(endpoints =>/* ajax文章内容 */
            {
                endpoints.MapControllerRoute(
                    name: "@Content",
                    pattern: "@/{ID}",
                    defaults: new { controller = "Panel", action = "Content", ajax = true });
            });
        }
    }
}