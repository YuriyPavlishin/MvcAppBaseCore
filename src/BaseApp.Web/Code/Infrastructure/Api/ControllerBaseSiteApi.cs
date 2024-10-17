using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Code.Infrastructure.Api;

[Route("api/[controller]/[action]")]
[ApiExplorerSettings(GroupName = ApiConstants.SiteGroupName)]
public class ControllerBaseSiteApi : ControllerBaseApi
{
    
}