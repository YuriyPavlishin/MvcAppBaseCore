using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Code.Infrastructure.Api;

[Route("api/admin/[controller]/[action]")]
[ApiExplorerSettings(GroupName = ApiConstants.AdminGroupName)]
public class ControllerBaseAdminApi : ControllerBaseApi
{
}