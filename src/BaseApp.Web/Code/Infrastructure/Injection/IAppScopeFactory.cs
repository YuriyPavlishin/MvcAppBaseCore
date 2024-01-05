using BaseApp.Common.Injection.Config;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure.LogOn;

namespace BaseApp.Web.Code.Infrastructure.Injection;

[Injectable(InjectableTypes.SingleInstance)]
public interface IAppScopeFactory
{
    IAppScope CreateScope(LoggedClaims claims, IUnitOfWork unitOfWork = null);
}