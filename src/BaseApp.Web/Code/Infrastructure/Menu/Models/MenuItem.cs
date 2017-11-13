using System.Collections.Generic;
using System.Linq;
using BaseApp.Common;

namespace BaseApp.Web.Code.Infrastructure.Menu.Models
{
    public class MenuItem
    {
        public string Label { get; set; }
        public IMenuUrlInfo MenuUrlInfo { get; set; }
        public Enums.MenuItemTypes? MenuItemType { get; set; }
        public List<MenuItem> Items { get; set; }
        public string CssClass { get; set; }
        public bool OpenInNewWindow { get; set; }

        public MenuItem(string label)
        {
            Label = label;
            Items = new List<MenuItem>();
        }

        public bool HasItems => Items != null && Items.Any();

        public bool IsCurrent(Enums.MenuItemTypes? currentMenuItem)
        {
            if (currentMenuItem != null)
                return MenuItemType == currentMenuItem;

            return MenuUrlInfo != null && MenuUrlInfo.IsCurrent;
        }

        public bool HasPermission()
        {
            return MenuUrlInfo?.HasPermission ?? true;
        }

        public bool HasCurrentChild(Enums.MenuItemTypes? currentMenuItem)
        {
            return HasCurrentChild(Items, currentMenuItem);
        }

        private bool HasCurrentChild(List<MenuItem> items, Enums.MenuItemTypes? currentMenuItem)
        {
            return items.Any(menuItem => menuItem.IsCurrent(currentMenuItem) || HasCurrentChild(menuItem.Items, currentMenuItem));
        }
    }
}