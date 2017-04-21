using System;
using BaseApp.Web.Models;

namespace BaseApp.Web.Code.Infrastructure.LogOn
{
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
