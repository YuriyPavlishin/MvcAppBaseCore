using BaseApp.Data.DataContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApp.Web.Code.Extensions
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHost Migrate(this IWebHost webhost)
        {
            // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
            using (var scope = webhost.Services.GetService<IServiceScopeFactory>().CreateScope())
            using (var dbContext = scope.ServiceProvider.GetRequiredService<DBData>())
            {
                dbContext.Database.Migrate();
            }
            return webhost;
        }        
    }
}
