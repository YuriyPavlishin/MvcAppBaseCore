using BaseApp.Common.Injection.Config;

namespace BaseApp.Web.Code.Infrastructure.Menu
{
    [Injectable(InjectableTypes.LifetimeScope)]
    public interface IMenuBuilderFactory
    {
        IMenuBuilder Create<T>() where T : MenuBuilderBase;
    }
}
