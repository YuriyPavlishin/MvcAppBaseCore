using BaseApp.Common.Injection.Config;

namespace BaseApp.Web.Code.Infrastructure.LogOn
{
    [Injectable(InjectableTypes.LifetimeScope)]
    public interface ILoggedUserAccessor
    {
        void SetLoggedUser(LoggedClaims claims);
        LoggedClaims Claims { get; }
        LoggedUserDbInfo DbInfo { get; }
        int Id { get; }
        int? IdOrNull { get; }
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
        bool IsInRole(string role);
    }
}
