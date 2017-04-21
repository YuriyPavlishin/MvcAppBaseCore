using System;
using BaseApp.Data.DataContext;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure.LogOn;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApp.Web.Code.Infrastructure
{
    public class AppDependencyResolver
    {
        private static AppDependencyResolver _resolver;

        public static AppDependencyResolver Current
        {
            get
            {
                if (_resolver == null)
                    throw new Exception("AppDependencyResolver not initialized. You should initialize it in Startup class");
                return _resolver;
            }
        }

        public static void Init(IServiceProvider services)
        {
            _resolver = new AppDependencyResolver(services);
        }

        private readonly IServiceProvider _serviceProvider;

        private AppDependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public UnitOfWork CreateUoWinCurrentThread()
        {
            var scopeResolver = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            return UnitOfWork.CreateInScope(scopeResolver.ServiceProvider.GetRequiredService<DBData>(), scopeResolver);
        }

        public ILoggedUserAccessor GetLoggedUser()
        {
            return _serviceProvider.GetRequiredService<ILoggedUserAccessor>();
        }
    }

}
