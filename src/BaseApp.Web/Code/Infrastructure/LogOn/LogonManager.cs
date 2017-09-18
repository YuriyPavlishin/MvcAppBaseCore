using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Extensions;
using BaseApp.Web.Code.Infrastructure.TokenAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BaseApp.Web.Code.Infrastructure.LogOn
{
    public class LogonManager: ILogonManager
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenAuthOptions _tokenAuthOptions;

        public LogonManager(IHttpContextAccessor contextAccessor, IMemoryCache memoryCache, IUnitOfWork unitOfWork, IOptions<TokenAuthOptions> tokenAuthOptions)
        {
            _contextAccessor = contextAccessor;
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;
            _tokenAuthOptions = tokenAuthOptions.Value;
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

        public string GenerateToken(LoggedClaims loggedClaims, DateTime? expires)
        {
            var handler = new JwtSecurityTokenHandler();

            var identity = new ClaimsIdentity(loggedClaims.GetAsClaims(), JwtBearerDefaults.AuthenticationScheme);
            var descriptor = new SecurityTokenDescriptor
                {
                    Issuer = _tokenAuthOptions.Issuer,
                    Audience = _tokenAuthOptions.Audience,
                    SigningCredentials = _tokenAuthOptions.SigningCredentials,
                    Subject = identity,
                    Expires = expires
                };

            var securityToken = handler.CreateToken(descriptor);

            return handler.WriteToken(securityToken);
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
                        var newClaims = new LoggedClaims(_unitOfWork.Users.GetAccountById(LoggedClaims.UserId));
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
