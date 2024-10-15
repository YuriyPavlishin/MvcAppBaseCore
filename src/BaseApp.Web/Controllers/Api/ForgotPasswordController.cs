using System.Threading.Tasks;
using BaseApp.Web.Code.BLL.ForgotPassword;
using BaseApp.Web.Code.BLL.ForgotPassword.Models;
using BaseApp.Web.Code.Infrastructure.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Controllers.Api;

public class ForgotPasswordController : ControllerBaseApi
{
    [HttpPost, AllowAnonymous]
    public async Task<ApiResult> RequestPassword(RequestForgotPasswordArgs args, [FromServices] IForgotPasswordCommandManager commandManager)
    {
        return await commandManager.RequestAsync(args);
    }
    
    [HttpGet, AllowAnonymous]
    public ApiResult<ForgotPasswordRequestModel> GetRequestPassword(GetRequestForgotPasswordArgs args, [FromServices] IForgotPasswordQueryManager queryManager)
    {
        return queryManager.GetRequest(args);
    }
    
    [HttpPost, AllowAnonymous]
    public async Task<ApiResult<ForgotPasswordRequestModel>> CompleteResetPassword(CompleteForgotPasswordArgs args, [FromServices] IForgotPasswordCommandManager commandManager)
    {
        return await commandManager.CompleteAsync(args);
    }
}