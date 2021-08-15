namespace BaseApp.Web.Code.Infrastructure.LogOn
{
    public interface ILogonManager
    {
        void SignInViaCookies(LoggedClaims loggedClaims, bool isPersistent);
        void SignOutAsCookies();
        LoggedClaims LoggedClaims { get; }
        LoggedUserDbInfo LoggedUserDbInfo { get; }

        void RefreshCurrentLoggedUserInfo(bool refreshClaims = true);
    }
}
