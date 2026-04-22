using BaseApp.Common.Injection.Config;
using Microsoft.AspNetCore.Http;

namespace BaseApp.Web.Code.Infrastructure.ClientRequests;

[Injectable(InjectableTypes.LifetimeScope)]
public interface IClientRequestAccessor
{
    string GetIpAddress();
}

public class ClientRequestAccessor(IHttpContextAccessor httpContextAccessor) : IClientRequestAccessor
{
    public string GetIpAddress()
    {
        return new IpAddressResolver(httpContextAccessor.HttpContext).GetUserHostIp();
    }
}