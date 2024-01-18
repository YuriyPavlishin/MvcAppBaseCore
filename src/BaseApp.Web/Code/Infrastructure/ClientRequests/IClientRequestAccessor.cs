using BaseApp.Common.Injection.Config;

namespace BaseApp.Web.Code.Infrastructure.ClientRequests;

[Injectable(InjectableTypes.LifetimeScope)]
public interface IClientRequestAccessor
{
    string GetIpAddress();
}