using BaseApp.Application.App.Interfaces;
using BaseApp.Common.Emails.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Refit;
using System;
using System.Threading.Tasks;

namespace BaseApp.Web.Code.Extensions
{
    public static class RefitServiceExtensions
    {
        public static void AddRefit(this IServiceCollection services, IConfiguration configurationRoot)
        {
            services.Configure<OneSignalOptions>(configurationRoot.GetSection("OneSignalOptions"));
            var oneSignal = new OneSignalOptions();
            configurationRoot.GetSection("OneSignal").Bind(oneSignal);
            
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            services
                .AddRefitClient<IOneSignalApi>(new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer(jsonSettings),
                    AuthorizationHeaderValueGetter = () => Task.FromResult(oneSignal.UserAuthKey)
                })
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(oneSignal.BaseAddress));
        }
    }
}
