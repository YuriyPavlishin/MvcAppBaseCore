using System;
using BaseApp.Common.Utils;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.BLL.Site.Tokens.Models;
using BaseApp.Web.Code.Infrastructure.LogOn;

namespace BaseApp.Web.Code.BLL.Site.Tokens.Impl;

public class TokenCommandManager(IUnitOfWork unitOfWork, ILogonManager logonManager) : ITokenCommandManager
{
    public TokenModel Retrieve(RetrieveTokenArgs args)
    {
        var account = unitOfWork.Users.GetAccountByLoginOrNull(args.UserName);
        if (account == null || !PasswordHash.ValidatePassword(args.Password, account.Password))
            return new TokenModel {IsAuthenticated = false};

        var expires = DateTime.UtcNow.AddHours(25);
        var token = logonManager.GenerateToken(new LoggedClaims(account), expires);

        return new TokenModel {IsAuthenticated = true, Token = token, TokenExpiresAt = expires};
    }
}