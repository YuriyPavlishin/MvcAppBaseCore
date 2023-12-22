using System;
using BaseApp.Common.Injection.Config;
using BaseApp.Web.Models;

namespace BaseApp.Web.Code.Infrastructure.LogOn
{
    [Injectable(InjectableTypes.LifetimeScope)]
    public interface ILogonManager
    {
        void SignInViaCookies(LoggedClaims loggedClaims, bool isPersistent);
        void SignOutAsCookies();
        string GenerateToken(LoggedClaims loggedClaims, DateTime? expires);
        LoggedClaims LoggedClaims { get; }
        LoggedUserDbInfo LoggedUserDbInfo { get; }

        void RefreshCurrentLoggedUserInfo(bool refreshClaims = true);
    }
}
