using BaseApp.Web.Code.Infrastructure.BaseControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Code.Infrastructure.Api
{
    //todo controllerbase and webapi
    [ApiExceptionFilter]
    [ApiInvalidModelStateFilter]
    [Authorize(ApiConstants.ApiPolicy)]
    [Route("api/[controller]/[action]")]
    public abstract class ControllerBaseApi: ControllerSiteBase
    {

    }
}
