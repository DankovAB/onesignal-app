using BaseApp.Common.Logs;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace BaseApp.Web.Code.Infrastructure.Api
{
    public class RefitApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is Refit.ApiException { Content: { } } apiException)
            {
                var errorMessage = JsonConvert.DeserializeObject<OneSignalApiErrorMessage>(apiException.Content);
                foreach (var item in errorMessage.Errors)
                {
                    context.ModelState.AddModelError(string.Empty, item);
                }

                LogHolder.MainLog.Error(context.Exception, $"Refit.ApiException error: {errorMessage}");
            }

            base.OnException(context);
        }
    }
}
