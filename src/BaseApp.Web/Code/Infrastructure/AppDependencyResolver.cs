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

        public IUnitOfWork CreateUoWinCurrentThread()
        {
            var scopeResolver = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            return UnitOfWork.CreateInScope(scopeResolver.ServiceProvider.GetRequiredService<DBData>(), scopeResolver);
        }

        public LoggedUserForValidationModel GetLoggedUser()
        {
            using var scopeResolver = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var loggedUserAccessor = scopeResolver.ServiceProvider.GetRequiredService<ILoggedUserAccessor>();
            return new LoggedUserForValidationModel {Id = loggedUserAccessor.IdOrNull, Login = loggedUserAccessor.Claims?.Login};
        }
    }

}
