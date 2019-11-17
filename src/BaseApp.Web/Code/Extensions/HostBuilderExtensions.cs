using BaseApp.Data.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BaseApp.Web.Code.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHost Migrate(this IHost host)
        {
            using (var scope = host.Services.GetService<IServiceScopeFactory>().CreateScope())
            using (var dbContext = scope.ServiceProvider.GetRequiredService<DBData>())
            {
                dbContext.Database.Migrate();
            }
            return host;
        }        
    }
}
