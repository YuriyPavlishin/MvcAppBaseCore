namespace BaseApp.Web.Code.Infrastructure.Menu
{
    public interface IMenuBuilderFactory
    {
        IMenuBuilder Create<T>() where T : MenuBuilderBase;
    }
}
