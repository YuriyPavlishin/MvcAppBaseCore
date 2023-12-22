using System;

namespace BaseApp.Common.Injection.Config;

public class InjectableAttribute(InjectableTypes injectableType) : Attribute
{
    public InjectableTypes InjectableType { get; } = injectableType;
}

public enum InjectableTypes
{
    LifetimeScope = 1,
    Dependency = 2,
    SingleInstance = 3
}