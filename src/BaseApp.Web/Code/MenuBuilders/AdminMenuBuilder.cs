using System.Collections.Generic;
using BaseApp.Web.Code.Infrastructure.Menu;
using BaseApp.Web.Code.Infrastructure.Menu.Models;

namespace BaseApp.Web.Code.MenuBuilders
{
    public class AdminMenuBuilder : MenuBuilderBase
    {
        public AdminMenuBuilder(MenuBuilderArgs args) : base(args)
        {
        }

        protected override List<MenuItem> GetMenuItems()
        {
            var items = new List<MenuItem>();
            //items.Add(GetUsersMenuItem());

            return items;
        }

        //private MenuItem GetUsersMenuItem()
        //{
        //    return new MenuItem("Users")
        //    {
        //        UrlInfo = UrlFactory.CreateAdmin<UserController>(UrlHelper, m => m.Index())
        //    };
        //}
    }
}