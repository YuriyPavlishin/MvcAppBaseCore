using BaseApp.Web.Code.Infrastructure.Menu.Models;

namespace BaseApp.Web.Code.Infrastructure.Menu
{
    public interface IMenuBuilder
    {
        MenuTree Build(bool hideItemsWithoutPermission = true, bool hideEmptyItems = true);
    }
}
