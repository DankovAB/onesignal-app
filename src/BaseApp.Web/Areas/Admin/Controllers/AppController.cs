using BaseApp.Application.App.Interfaces;
using BaseApp.Application.App.Models;
using BaseApp.Common;
using BaseApp.Web.Areas.Admin.Components.App;
using BaseApp.Web.Areas.Admin.Models.App;
using BaseApp.Web.Code.Infrastructure.Api;
using BaseApp.Web.Code.Infrastructure.BaseControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BaseApp.Web.Areas.Admin.Controllers
{
    public class AppController : ControllerBaseAdminOrOperatorRequired
    {
        private readonly IAppService _appService;

        public AppController(IAppService appsService)
        {
            _appService = appsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAllApps()
        {
            return ViewComponent(nameof(AppListViewComponent));
        }

        [Authorize(Policy = Constants.Policy.Admin)]
        public IActionResult Create()
        {
            return View("Edit", new EditAppModel());
        }

        [Authorize(Policy = Constants.Policy.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(EditAppModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            var request = Mapper.Map<CreateAppRequest>(model);
            
            try
            {
                await _appService.CreateApp(request);
            }
            catch (Refit.ApiException exception)
            {
                var errorMessage = JsonConvert.DeserializeObject<OneSignalApiErrorMessage>(exception.Content);
                foreach (var item in errorMessage.Errors)
                {
                    ModelState.AddModelError(string.Empty, item);
                }

                return View("Edit", model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Policy = Constants.Policy.Admin)]
        public async Task<IActionResult> Edit(Guid appId)
        {
            var response = await _appService.GetApp(appId);
            if (response == null)
            {
                return NotFound();
            }

            var model = Mapper.Map<EditAppModel>(response);

            return View(model);
        }

        [Authorize(Policy = Constants.Policy.Admin)]
        [HttpPost]
        public async Task<IActionResult> Edit(EditAppModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = Mapper.Map<UpdateAppRequest>(model);

            try
            {
                await _appService.EditApp(request);
            }
            catch (Refit.ApiException exception)
            {
                var errorMessage = JsonConvert.DeserializeObject<OneSignalApiErrorMessage>(exception.Content);
                foreach (var item in errorMessage.Errors)
                {
                    ModelState.AddModelError(string.Empty, item);
                }

                return View("Edit", model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return RedirectToAction("Index");
        }
    }
}