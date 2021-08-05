using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.IO;

using WaterLibrary.pilipala;
using WaterLibrary.pilipala.Component;


namespace PILIPALA
{
    using PILIPALA.Theme;
    using PILIPALA.Models;
    using PILIPALA.pilipala.plugin;
    

    public partial class Startup
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

            //配置集
            var AppSettings = Configuration.GetSection("AppSettings");//主节点
            var ThemeSection = AppSettings.GetSection("Theme");//主题信息节点
            var UserSection = AppSettings.GetSection("User");//用户信息节点
            //用户模型注入
            services.AddTransient(x => new UserModel()
            {
                Account = UserSection.GetSection("Account").Value,
                PWD = UserSection.GetSection("PWD").Value
            });
            //主题管理器注入
            services.AddTransient(x => new ThemeHandler(new ThemeConfigModel()
            {
                Path = ThemeSection.GetSection("Path").Value,
            }));

            PiliPala.INIT(File.ReadAllText(@".pilipala/config.yml", System.Text.Encoding.UTF8));//内核单例初始化

            var fac = new ComponentFactory();

            //插件管理器注入
            services.AddTransient(x => new PluginManager(fac));
            //组件工厂注入
            services.AddTransient(x => fac);

            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                    });
            });
        }
    }
}
