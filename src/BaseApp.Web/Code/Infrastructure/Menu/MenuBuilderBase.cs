using System.Collections.Generic;
using System.Linq;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure.LogOn;
using BaseApp.Web.Code.Infrastructure.Menu.Models;

namespace BaseApp.Web.Code.Infrastructure.Menu
{
    public abstract class MenuBuilderBase: IMenuBuilder
    {
        protected MenuUrlFactory UrlFactory { get; }
        protected UnitOfWork UnitOfWork { get; }
        protected ILoggedUserAccessor LoggedUser { get; }

        protected MenuBuilderBase(MenuBuilderArgs args)
        {
            UrlFactory = new MenuUrlFactory(args.MenuMvcArgs);
            UnitOfWork = args.UnitOfWork;
            LoggedUser = args.LoggedUser;
        }

        protected abstract List<MenuItem> GetMenuItems();

        public MenuTree Build(bool hideItemsWithoutPermission = true, bool hideEmptyItems = true)
        {
            var menuItems = GetMenuItems();
          
            if (hideItemsWithoutPermission)
            {
                RemoveItemsWithoutPermission(menuItems, hideEmptyItems);
            }

            return new MenuTree
            {
                Items = menuItems
            };
        }

        private void RemoveItemsWithoutPermission(List<MenuItem> items, bool hideEmptyItems)
        {
            foreach (var menuItem in items.ToList())
            {
                if (!menuItem.HasPermission())
                {
                    items.Remove(menuItem);
                }
                else if (menuItem.HasItems)
                {
                    RemoveItemsWithoutPermission(menuItem.Items, hideEmptyItems);
                    if (hideEmptyItems && !menuItem.HasItems && menuItem.MenuUrlInfo == null)
                    {
                        items.Remove(menuItem);
                    }
                }
            }
        }
    }
}