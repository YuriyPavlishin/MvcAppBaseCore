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
    public class LogonManager(
        IHttpContextAccessor contextAccessor,
        IMemoryCache memoryCache,
        IUnitOfWork unitOfWork,
        IOptions<TokenAuthOptions> tokenAuthOptions)
        : ILogonManager
    {
        private readonly TokenAuthOptions _tokenAuthOptions = tokenAuthOptions.Value;

        public LoggedClaims LoggedClaims => IsAuthenticated 
            ? new LoggedClaims(contextAccessor.HttpContext.User.Claims.ToList())
            : null;

        public void SignInViaCookies(LoggedClaims loggedClaims, bool isPersistent)
        {
            var identity = new ClaimsIdentity(loggedClaims.GetAsClaims(), CookieAuthenticationDefaults.AuthenticationScheme);
            
            contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity)
                , new AuthenticationProperties() { IsPersistent = isPersistent })
                .Wait();
        }

        public void SignOutAsCookies()
        {
            contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme)
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
            
            memoryCache.Remove(LoggedUserDbInfo.GetUserDbInfoCacheKey(LoggedClaims.Login));
            if (refreshClaims)
            {
                var authType = contextAccessor.HttpContext.User.Identity.AuthenticationType;
                switch (authType)
                {
                    case CookieAuthenticationDefaults.AuthenticationScheme:
                        var newClaims = new LoggedClaims(unitOfWork.Users.GetAccountByIdOrNull(LoggedClaims.UserId));
                        SignInViaCookies(newClaims, true /*TODO: detect if current cookies persistent or not*/);
                        break;
                    default:
                        throw new Exception($"RefreshCurrentLoggedUserInfo does not support {authType} authentication");
                }
            }
        }
        
        private bool IsAuthenticated => contextAccessor.HttpContext.User?.Identity?.IsAuthenticated ?? false;
    }
}
