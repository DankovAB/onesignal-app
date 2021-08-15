namespace BaseApp.Web.Code.Infrastructure.LogOn
{
    public interface ILoggedUserAccessor
    {
        LoggedClaims Claims { get; }
        LoggedUserDbInfo DbInfo { get; }
        int Id { get; }
        int? IdOrNull { get; }
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
        bool IsDataEntryOperator { get; }
        bool IsInRole(string role);
    }
}
