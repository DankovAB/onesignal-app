using BaseApp.Web.Code.Extensions;
using BaseApp.Web.Code.Infrastructure.BaseControllers;
using BaseApp.Web.Code.Infrastructure.LogOn;
using BaseApp.Web.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Controllers
{
    public class AccountController : ControllerBaseAuthorizeRequired
    {
        private readonly ILogonManager _logonManager;

        public AccountController(ILogonManager logonManager)
        {
            _logonManager = logonManager;
        }


        [AllowAnonymous]
        public IActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult LogOn(LogOnModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var account = model.AccountAfterValidation;
            _logonManager.SignInViaCookies(new LoggedClaims(account), model.RememberMe);

            return Redirect(Url.Home());
        }

        public IActionResult LogOff()
        {
            _logonManager.SignOutAsCookies();

            return Redirect(Url.Home());
        }
    }
}
