using System.Collections.Generic;
using BaseApp.Common;

namespace BaseApp.Web.Code.Infrastructure.Menu.Models
{
    public class MenuTree
    {
        public List<MenuItem> Items { get; set; }

        public MenuTree()
        {
            Items = new List<MenuItem>();
        }
    }

    public class MenuBranch
    {
        public IEnumerable<MenuItem> menuItems { get; set; }
        public int level { get; set; }
        public Enums.MenuItemTypes? currentMenuItem { get; set; }
    }
}