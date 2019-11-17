using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace BaseApp.Web.Code.Infrastructure.CustomRazor.Models
{
    public class CustomRazorViewRoute : IRouter
    {
        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return new VirtualPathData(this, "");
        }

        public Task RouteAsync(RouteContext context)
        {
            return Task.CompletedTask;
        }
    }
}
