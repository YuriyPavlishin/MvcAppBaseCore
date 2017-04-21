using System;
using System.Linq;
using BaseApp.Common;

namespace BaseApp.Web.Code.Infrastructure.LogOn
{
    public class LoggedUserAccessor: ILoggedUserAccessor
    {
        private readonly ILogonManager _logonManager;

        public LoggedClaims Claims => _logonManager.LoggedClaims;

        public LoggedUserDbInfo DbInfo => _logonManager.LoggedUserDbInfo;

        public int Id
        {
            get
            {
                if (Claims == null)
                    throw new Exception("User not signed in");

                return Claims.UserId;
            }
        }

        public int? IdOrNull => Claims?.UserId;

        public bool IsAuthenticated => Claims != null;

        public bool IsAdmin => IsInRole(Constants.Roles.Admin);

        public bool IsInRole(string role)
        {
            return Claims?.Roles.Contains(role) ?? false;
        }


        public LoggedUserAccessor(ILogonManager logonManager)
        {
            _logonManager = logonManager;
        }
    }
}
