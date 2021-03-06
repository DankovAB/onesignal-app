using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using BaseApp.Common.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BaseApp.Data.DataContext;
using BaseApp.Web.Code.Extensions;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Code.Infrastructure.Logs;
using BaseApp.Web.Code.Scheduler.Queue;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BaseApp.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostEnv;
        private readonly List<Exception> _startupExceptions = new();

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            _hostEnv = env;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddMemoryCache();
                services.AddOptions();

                services.AddDbContext<DBData>(options =>
                    {
                        options.UseLazyLoadingProxies().UseSqlServer(
                            Configuration["Data:DefaultConnection:ConnectionString"]
                            , b => b.MigrationsAssembly("BaseApp.Data.ProjectMigration"));
                    }
                );

                services.AddAppWeb(Configuration);
                services.AddAppWebSecurity(_hostEnv);

                services
                    .AddControllersWithViews()
                    .SetCompatibilityVersion(CompatibilityVersion.Latest)
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        //options.JsonSerializerOptions.IgnoreNullValues = true;
                    });
                
            }
            catch (Exception ex)
            {
                _startupExceptions.Add(ex);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!_startupExceptions.Any())
                {
                    UseAppInner(app, env);
                }
            }
            catch (Exception ex)
            {
                _startupExceptions.Add(ex);
                LogHolder.MainLog.Error(ex);
            }

            if (_startupExceptions.Any())
            {
                try
                {
                    _startupExceptions.ForEach(ex => LogHolder.MainLog.Error(ex));
                }
                catch (Exception ex)
                {
                    new StartupLogger(env.ContentRootPath).ErrorException(ex);
                }
                
                RenderStartupErrors(app);
            }
        }

        private static void UseAppInner(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AppDependencyResolver.Init(app.ApplicationServices);
            app.UseStatusCodePagesWithReExecute("/Errors/Statuses/{0}");
                       

            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Errors/Index");
            }

            app.UseMiddleware<AjaxExceptionHandlerMiddleware>();

            app.ApplicationServices.GetRequiredService<WorkersQueue>().Init();

            app.UseStaticFiles();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization(); //todo: check it

            app.UseEndpoints(routes =>
               {
                   routes.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                   routes.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
               });
        }

        private void RenderStartupErrors(IApplicationBuilder app)
        {
            app.Run(
                async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "text/plain";

                    foreach (var ex in _startupExceptions)
                    {
                        await context.Response.WriteAsync($"Error on startup {ex.Message}").ConfigureAwait(false);
                    }
                });
        }
    }    
}
