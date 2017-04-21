using Microsoft.AspNetCore.Authorization;

namespace BaseApp.Web.Code.Infrastructure.BaseControllers
{
    [Authorize]
    public class ControllerBaseAuthorizeRequired: ControllerBaseNoAuthorize
    {
    }
}
