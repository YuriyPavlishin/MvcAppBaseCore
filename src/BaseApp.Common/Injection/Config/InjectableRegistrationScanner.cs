using System;
using System.Linq;
using System.Reflection;
using BaseApp.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApp.Common.Injection.Config;

public static class InjectableRegistrationScanner
{
    public static void RegisterServices(IServiceCollection services, params Assembly[] additionalAssemblies)
    {
        var allTypes = new[] { Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly() }.Concat(additionalAssemblies).SelectMany(x => x.GetTypes()).ToList();
        var registerTypes = allTypes
            .Where(t => t.GetCustomAttribute<InjectableAttribute>() != null)
            .Select(t => new { Injectable = t.GetCustomAttribute<InjectableAttribute>(), Type = t } );
        foreach (var registerType in registerTypes)
        {
            var implementations = allTypes.Where(x => registerType.Type.IsAssignableFrom(x) && x != registerType.Type).ToList();
            if (implementations.Count > 1)
                throw new Exception($"{registerType.Type} has more than one implementation: {", ".UseForJoinNonEmptyObjects(implementations)}");
            if (implementations.Count == 0)
                throw new Exception($"{registerType.Type} implementation not found");
            var implementation = implementations[0];
            
            switch (registerType.Injectable.InjectableType)
            {
                case InjectableTypes.LifetimeScope:
                    services.AddScoped(registerType.Type, implementation);
                    break;
                case InjectableTypes.Dependency:
                    services.AddTransient(registerType.Type, implementation);
                    break;
                case InjectableTypes.SingleInstance:
                    services.AddSingleton(registerType.Type, implementation);
                    break;
                default:
                    throw new NotSupportedException(registerType.Injectable.InjectableType.ToString());
            }
        }
    }
}