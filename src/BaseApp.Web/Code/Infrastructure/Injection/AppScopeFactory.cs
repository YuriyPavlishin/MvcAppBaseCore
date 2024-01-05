using System;
using Autofac;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure.LogOn;

namespace BaseApp.Web.Code.Infrastructure.Injection;

public class AppScopeFactory(ILifetimeScope rootScope) : IAppScopeFactory
{
    public IAppScope CreateScope(LoggedClaims claims, IUnitOfWork unitOfWork = null)
    {
        ArgumentNullException.ThrowIfNull(claims);

        var result = new AppLifetimeScope(rootScope.BeginLifetimeScope(a =>
        {
            if (unitOfWork != null)
                a.RegisterInstance(unitOfWork).As<IUnitOfWork>().ExternallyOwned();
        }));
        result.GetService<ILoggedUserAccessor>().SetLoggedUser(claims);
        
        return result;
    }
}