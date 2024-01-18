using Microsoft.AspNetCore.Http;

namespace BaseApp.Web.Code.Infrastructure.ClientRequests;

public class ClientRequestAccessor(IHttpContextAccessor httpContextAccessor) : IClientRequestAccessor
{
    public string GetIpAddress()
    {
        return new IpAddressResolver(httpContextAccessor.HttpContext).GetUserHostIp();
    }
}