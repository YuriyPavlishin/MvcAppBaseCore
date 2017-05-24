using System;
using BaseApp.Common.Utils;
using BaseApp.Web.Code.Infrastructure.Api;
using BaseApp.Web.Code.Infrastructure.LogOn;
using BaseApp.Web.Models.Api.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Controllers.Api
{
    public class TokenController: ControllerBaseApi
    {
        private readonly ILogonManager _logonManager;

        public TokenController(ILogonManager logonManager)
        {
            _logonManager = logonManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public ApiResult<TokenRetrieveModel> Retrieve(TokenRetrieveArgs args)
        {
            var account = UnitOfWork.Users.GetAccountByLoginOrNull(args.UserName);
            if (account == null || !PasswordHash.ValidatePassword(args.Password, account.Password))
                return ApiResult.Success(new TokenRetrieveModel {IsAuthenticated = false} );

            DateTime? expires = DateTime.UtcNow.AddHours(25);
            var token = _logonManager.GenerateToken(new LoggedClaims(account), expires);

            return ApiResult.Success(new TokenRetrieveModel {IsAuthenticated = true, Token = token, TokenExpiresAt = expires});
        }
    }
}
