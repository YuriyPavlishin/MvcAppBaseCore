using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Code.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string Home(this IUrlHelper helper)
        {
            return helper.Action("Index", "Home", new { area = "" });
        }

        public static string AdminUsers(this IUrlHelper helper)
        {
            return helper.Action("Index", "User", new { area = "admin" });
        }
    }
}
