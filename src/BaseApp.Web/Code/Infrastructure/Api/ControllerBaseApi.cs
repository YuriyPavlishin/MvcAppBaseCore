using BaseApp.Web.Code.Infrastructure.BaseControllers;
using Microsoft.AspNetCore.Authorization;

namespace BaseApp.Web.Code.Infrastructure.Api
{
    [ApiExceptionFilter]
    [ApiInvalidModelStateFilter]
    [Authorize(ApiConstants.ApiPolicy)]
    public abstract class ControllerBaseApi: ControllerSiteBase
    {
    }
}
