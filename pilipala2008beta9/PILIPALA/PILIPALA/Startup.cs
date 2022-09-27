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

        // This method gets called by the runtime.
        // Use this method to add services to the container.
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

            //���ü�
            var AppSettings = Configuration.GetSection("AppSettings"); //���ڵ�
            var ThemeSection = AppSettings.GetSection("Theme"); //������Ϣ�ڵ�
            var UserSection = AppSettings.GetSection("User"); //�û���Ϣ�ڵ�
            //�û�ģ��ע��
            services.AddTransient(x => new UserModel
            {
                Account = UserSection.GetSection("Account").Value,
                PWD = UserSection.GetSection("PWD").Value
            });
            //���������ע��
            services.AddTransient(x => new ThemeHandler(new ThemeConfigModel()
            {
                Path = ThemeSection.GetSection("Path").Value,
            }));

            PiliPala.INIT(File.ReadAllText(@".pilipala/config.yml", System.Text.Encoding.UTF8)); //�ں˵�����ʼ��

            var fac = new ComponentFactory();

            //���������ע��
            services.AddTransient(x => new PluginManager(fac));
            //�������ע��
            services.AddTransient(x => fac);

            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy",
                    builder => { builder.AllowAnyOrigin(); });
            });
        }
    }
}