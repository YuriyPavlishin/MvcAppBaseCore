using System;
using System.Linq;
using System.Reflection;
using Autofac;
using BaseApp.Common.Extensions;
using BaseApp.Common.Injection.Config;

namespace BaseApp.Web.Code.Infrastructure.Injection;

public static class InjectableRegistrationScanner
{
    public static void RegisterServices(ContainerBuilder builder, params Assembly[] additionalAssemblies)
    {
        var allTypes = new[] { Assembly.GetCallingAssembly() }.Concat(additionalAssemblies).SelectMany(x => x.GetTypes()).ToList();
        var registerTypes = allTypes
            .Where(t => t.GetCustomAttribute<InjectableAttribute>() != null)
            .Select(t => new { Injectable = t.GetCustomAttribute<InjectableAttribute>(), Type = t } ).ToList();
        foreach (var registerType in registerTypes)
        {
            var implementations = allTypes.Where(x => registerType.Type.IsAssignableFrom(x) && x != registerType.Type).ToList();
            if (implementations.Count > 1)
                throw new Exception($"{registerType.Type} has more than one implementation: {", ".UseForJoinNonEmptyObjects(implementations)}");
            if (implementations.Count == 0)
                throw new Exception($"{registerType.Type} implementation not found");
            
            var forRegistration = builder.RegisterType(implementations[0]).As(registerType.Type);
            switch (registerType.Injectable.InjectableType)
            {
                case InjectableTypes.LifetimeScope:
                    forRegistration.InstancePerLifetimeScope();
                    break;
                case InjectableTypes.Dependency:
                    forRegistration.InstancePerDependency();
                    break;
                case InjectableTypes.SingleInstance:
                    forRegistration.SingleInstance();
                    break;
                default:
                    throw new NotSupportedException(registerType.Injectable.InjectableType.ToString());
            }
        }
    }
}