using Autofac;

namespace BaseApp.Web.Code.Infrastructure.Injection;

public class AppLifetimeScope(ILifetimeScope scope) : IAppScope
{
    public T GetService<T>()
    {
        return scope.Resolve<T>();
    }

    public void Dispose()
    {
        scope.Dispose();
    }
}