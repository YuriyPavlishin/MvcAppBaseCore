using BaseApp.Web.Code.BLL.Site.Tokens;
using BaseApp.Web.Code.BLL.Site.Tokens.Models;
using BaseApp.Web.Code.Infrastructure.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Controllers.Api.Site
{
    public class TokenController : ControllerBaseSiteApi
    {
        [HttpPost, AllowAnonymous]
        public ApiResult<TokenModel> Retrieve(RetrieveTokenArgs args, [FromServices] ITokenCommandManager commandManager)
        {
            return commandManager.Retrieve(args);
        }
    }
}
