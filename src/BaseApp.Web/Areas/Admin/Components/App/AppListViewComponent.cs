using System.Threading.Tasks;
using BaseApp.Application.App.Interfaces;
using BaseApp.Web.Areas.Admin.Models.App;
using BaseApp.Web.Code.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Areas.Admin.Components.App
{
    public class AppListViewComponent : ViewComponentBase
    {
        private readonly IAppService _appService;

        public AppListViewComponent(IAppService appService)
        {
            _appService = appService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _appService.GetAllAppsAsync();
            var result = Mapper.Map<ListAppViewModel>(items);

            return View(result);
        }
    }
}
