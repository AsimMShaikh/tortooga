using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using TortoogaApp.Data;
using TortoogaApp.Messaging.Emails;
using TortoogaApp.Security;
using TortoogaApp.Services;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace TortoogaApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            //Configure automapper profile
            MapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfileConfig>();
            });

            MapperConfig.AssertConfigurationIsValid();

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            HostingEnvironment = env;
        }

        public IConfigurationRoot Configuration { get; }
        public MapperConfiguration MapperConfig { get; set; }
        public IHostingEnvironment HostingEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DbConnection")));

            //Setup AppSettings for application wide access
            services.Configure<AppSettings>(appSettings =>
            {
                appSettings.BaseUrl = Configuration["AppSettings:BaseUrl"];
                appSettings.ContentRootPath = HostingEnvironment.ContentRootPath;
                appSettings.StorageConnectionString = Configuration["AppSettings:StorageConnectionString"];
                appSettings.ProfileImageContainer = Configuration["AppSettings:ProfileImageContainer"];
                appSettings.ImageBlobContainerPath = Configuration["AppSettings:ImageBlobContainerPath"];
                appSettings.CompanyLogoContainer = Configuration["AppSettings:CompanyLogoContainer"];
                appSettings.MailRoot = Environment.GetEnvironmentVariable("MAILROOT");
                appSettings.DevelopmentEmailCredential = Environment.GetEnvironmentVariable("DEVELOPMENT_EMAIL_CREDENTIAL");
            });

            services.AddIdentity<ApplicationUser, Role>(
                config =>
                {
                    config.User.RequireUniqueEmail = true;

                    config.Password.RequireDigit = false;
                    config.Password.RequireLowercase = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext, Guid>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, Role, ApplicationDbContext, Guid>>()
                .AddRoleStore<RoleStore<Role, ApplicationDbContext, Guid>>();

            services.AddMvc();

            // Add application services.
            services.AddSingleton<IMapper>(v => MapperConfig.CreateMapper());
            if (HostingEnvironment.IsDevelopment())
            {
                services.AddTransient<IEmailSender, DevelopmentMessageSender>();
            }
            else
            {
                services.AddTransient<IEmailSender, AuthMessageSender>();
            }
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddScoped<IDbService, EFDbService>();
            services.AddScoped<IEmailNotificationFactory, EmailNotificationFactory>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddTransient<TortoogaSeedData>();

            services.AddScoped<IViewRenderingService, ViewRenderingService>();

            services.AddScoped<IDashboardService, DashboardService>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddSingleton<ICommonservice, CommonService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, TortoogaSeedData seeder)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            seeder.DevEnvSeedData();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}