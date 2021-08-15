using BaseApp.Application.App;
using BaseApp.Application.App.Interfaces;
using BaseApp.Common;
using BaseApp.Common.Emails;
using BaseApp.Common.Emails.Impl;
using BaseApp.Common.Emails.Models;
using BaseApp.Common.Files;
using BaseApp.Common.Files.Impl;
using BaseApp.Common.Files.Models;
using BaseApp.Data.Files;
using BaseApp.Data.Files.Impl;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Code.Infrastructure.CustomRazor;
using BaseApp.Web.Code.Infrastructure.LogOn;
using BaseApp.Web.Code.Infrastructure.Menu;
using BaseApp.Web.Code.Mappers;
using BaseApp.Web.Code.Scheduler;
using BaseApp.Web.Code.Scheduler.Queue;
using BaseApp.Web.Code.Scheduler.Queue.Workers;
using BaseApp.Web.Code.Scheduler.Queue.Workers.Impl;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.IO;

namespace BaseApp.Web.Code.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppWeb(this IServiceCollection services, IConfiguration configurationRoot)
        {
            services.Configure<SiteOptions>(configurationRoot.GetSection("SiteOptions"));

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IPathResolver, PathResolver>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ILoggedUserAccessor, LoggedUserAccessor>();
            services.AddScoped<ILogonManager, LogonManager>();

            services.AddScoped<IMenuBuilderFactory, MenuBuilderFactory>();
            services.AddScoped<ViewDataItems>();

            services.AddSingleton(sp => MapInit.CreateConfiguration().CreateMapper());

            AddFiles(services, configurationRoot);

            services.AddSingleton<ICustomRazorViewService, CustomRazorViewService>();
            services.AddSingleton<IEmailSenderService, EmailSenderService>();
            services.Configure<EmailSenderOptions>(configurationRoot.GetSection("EmailSenderOptions"));

            services.AddTransient<IEmailWorkerService, EmailWorkerService>();
            services.AddTransient<ISchedulerWorkerService, SchedulerWorkerService>();
            services.AddSingleton<WorkersQueue>();
            services.AddSingleton<ISchedulerService, SchedulerService>();

            services.AddRefit(configurationRoot);
            services.AddScoped<IAppService, AppService>();
        }

        private static void AddFiles(IServiceCollection services, IConfiguration configurationRoot)
        {
            services.Configure<FileFactoryOptions>(configurationRoot.GetSection("FileOptions"));
            services.AddSingleton<IFileFactoryService, FileFactoryService>();
            services.AddSingleton<IAttachmentService, AttachmentService>();
        }

        public static void AddAppWebSecurity(this IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(env.ContentRootPath + "\\App_Data\\PersistKeys\\"))
                .SetApplicationName("BaseApp");

            services.AddAuthorization(auth =>
                {
                    auth.AddPolicy(Constants.Policy.Admin,
                        p => p.RequireRole(Constants.Roles.Admin));

                    auth.AddPolicy(Constants.Policy.DataEntryOperator,
                        p => p.RequireRole(Constants.Roles.DataOperator));

                    auth.AddPolicy(Constants.Policy.AdminOrDataEntryOperator,
                        p => p
                            .RequireAssertion(r => r
                                                       .User.IsInRole(Constants.Roles.Admin)
                                                   || r.User.IsInRole(Constants.Roles.DataOperator)));

                })
                .AddAuthentication(o =>
                {
                    o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.Cookie.Name = ".BaseApp.Web-Core-AUTH";
                    options.LoginPath = new PathString("/Account/LogOn/");
                    options.LogoutPath = new PathString("/Account/LogOff/");
                    options.AccessDeniedPath = new PathString("/Account/Forbidden/");
                    options.ExpireTimeSpan = TimeSpan.FromDays(60);
                });
        }
    }
}