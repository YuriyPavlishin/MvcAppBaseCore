using System;
using System.Linq;
using BaseApp.Common;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace BaseApp.Web.Code.Infrastructure.LogOn
{
    public class LoggedUserAccessor(IUnitOfWork unitOfWork, IMemoryCache memoryCache) : ILoggedUserAccessor
    {
        private LoggedClaims _claims;
        private bool _isInitialized;
        
        public void SetLoggedUser(LoggedClaims claims)
        {
            _isInitialized = true;
            _claims = claims;
        }
        
        public LoggedClaims Claims
        {
            get
            {
                if (!_isInitialized)
                    throw new Exception("Access before authorization not allowed. Possible reason - access in api constructor which fired before AuthorizeAttribute or access from child scope");
                return _claims;
            }
        }
        
        public LoggedUserDbInfo DbInfo
        {
            get
            {
                var claims = Claims;
                if (claims == null)
                    return null;

                var dbInfo = memoryCache.GetOrAdd(LoggedUserDbInfo.GetUserDbInfoCacheKey(claims.Login), () => GetUserDbInfo(claims.Login, claims.GeneratedDateTicks));
                if (dbInfo.GeneratedDateTicks != claims.GeneratedDateTicks)
                {
                    /*This mean that our cache out of date. This can occur on Web server farm*/
                    dbInfo = GetUserDbInfo(claims.Login, claims.GeneratedDateTicks);
                    memoryCache.Set(LoggedUserDbInfo.GetUserDbInfoCacheKey(claims.Login), dbInfo);
                }
                return dbInfo;
            }
        }

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
        
        private LoggedUserDbInfo GetUserDbInfo(string login, long generatedDateTicks)
        {
            var user = unitOfWork.Users.GetAccountByLoginOrNull(login);
            if (user == null)
                return null;

            return new LoggedUserDbInfo(user.Login, user.FirstName, user.LastName, generatedDateTicks);
        }
    }
}
