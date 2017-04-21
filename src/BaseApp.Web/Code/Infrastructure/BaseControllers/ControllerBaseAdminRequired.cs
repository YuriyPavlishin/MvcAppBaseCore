using BaseApp.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Code.Infrastructure.BaseControllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public abstract class ControllerBaseAdminRequired : ControllerBaseNoAuthorize
    {

    }
}
