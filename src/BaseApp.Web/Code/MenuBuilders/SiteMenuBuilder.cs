using System.Collections.Generic;
using BaseApp.Web.Code.Infrastructure.Menu;
using BaseApp.Web.Code.Infrastructure.Menu.Models;
using BaseApp.Web.Controllers;

namespace BaseApp.Web.Code.MenuBuilders
{
    public class SiteMenuBuilder : MenuBuilderBase
    {
        public SiteMenuBuilder(MenuBuilderArgs args) : base(args)
        {
        }

        protected override List<MenuItem> GetMenuItems()
        {
            var menuItems = new List<MenuItem>();
            menuItems.Add(GetHomeMenuItem());
            menuItems.Add(GetGoogleMenuItem());
            menuItems.Add(GetProfileMenuItem());
            menuItems.AddRange(GetExamples());

            return menuItems;
        }
      
        private MenuItem GetHomeMenuItem()
        {
            return new MenuItem("Home")
            {
                MenuUrlInfo = UrlFactory.Create<HomeController>(m => m.Index())
            };
        }

        private MenuItem GetProfileMenuItem()
        {
            return new MenuItem("Profile")
            {
                MenuUrlInfo = UrlFactory.Create<AccountController>(m => m.UserProfile())
            };
        }

        private MenuItem GetGoogleMenuItem()
        {
            return new MenuItem("Search")
            {
                MenuUrlInfo = new MenuUrlInfoRaw("http://google.com"),
                OpenInNewWindow = true
            };
        }

        private List<MenuItem> GetExamples()
        {
            var result = new List<MenuItem>();

            result.Add(new MenuItem("Countries")
            {
                MenuUrlInfo = UrlFactory.Create<ExampleController>(m => m.Countries())
            });

            return result;
        }
    }
}