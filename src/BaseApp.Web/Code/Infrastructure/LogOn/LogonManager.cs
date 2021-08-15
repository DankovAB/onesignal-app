using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Security.Claims;

namespace BaseApp.Web.Code.Infrastructure.LogOn
{
    public class LogonManager: ILogonManager
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;

        public LogonManager(IHttpContextAccessor contextAccessor, IMemoryCache memoryCache, IUnitOfWork unitOfWork)
        {
            _contextAccessor = contextAccessor;
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;
        }

        public LoggedClaims LoggedClaims => IsAuthenticated 
            ? new LoggedClaims(_contextAccessor.HttpContext.User.Claims.ToList())
            : null;

        public LoggedUserDbInfo LoggedUserDbInfo
        {
            get
            {
                var claims = LoggedClaims;
                if (claims == null)
                    return null;

                var dbInfo = _memoryCache.GetOrAdd(GetUserDbInfoCacheKey(claims.Login), () => GetUserDbInfo(claims.Login, claims.GeneratedDateTicks));
                if (dbInfo.GeneratedDateTicks != claims.GeneratedDateTicks)
                {
                    /*This mean that our cache out of date. This can occur on Web server farm*/
                    dbInfo = GetUserDbInfo(claims.Login, claims.GeneratedDateTicks);
                    _memoryCache.Set(GetUserDbInfoCacheKey(claims.Login), dbInfo);
                }
                return dbInfo;
            }
        }

        public void SignInViaCookies(LoggedClaims loggedClaims, bool isPersistent)
        {
            var identity = new ClaimsIdentity(loggedClaims.GetAsClaims(), CookieAuthenticationDefaults.AuthenticationScheme);
            
            _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity)
                , new AuthenticationProperties() { IsPersistent = isPersistent })
                .Wait();
        }

        public void SignOutAsCookies()
        {
            _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme)
                .Wait();
        }

        public void RefreshCurrentLoggedUserInfo(bool refreshClaims = true)
        {
            if (LoggedClaims == null)//we are not signed in
                return;
            
            _memoryCache.Remove(GetUserDbInfoCacheKey(LoggedClaims.Login));
            if (refreshClaims)
            {
                var authType = _contextAccessor.HttpContext.User.Identity.AuthenticationType;
                switch (authType)
                {
                    case CookieAuthenticationDefaults.AuthenticationScheme:
                        var newClaims = new LoggedClaims(_unitOfWork.Users.GetAccountByIdOrNull(LoggedClaims.UserId));
                        SignInViaCookies(newClaims, true /*TODO: detect if current cookies persistent or not*/);
                        break;
                    default:
                        throw new Exception($"RefreshCurrentLoggedUserInfo does not support {authType} authentication");
                }
            }
        }

        private static string GetUserDbInfoCacheKey(string login)
        {
            return "userLogon_" + login;
        }

        private LoggedUserDbInfo GetUserDbInfo(string login, long generatedDateTicks)
        {
            var user = _unitOfWork.Users.GetAccountByLoginOrNull(login);
            if (user == null)
                return null;

            return new LoggedUserDbInfo(user.Login, user.FirstName, user.LastName, generatedDateTicks);
        }

        private bool IsAuthenticated => _contextAccessor.HttpContext.User?.Identity?.IsAuthenticated ?? false;
    }
}
