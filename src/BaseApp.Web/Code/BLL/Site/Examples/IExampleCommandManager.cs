using System.Threading.Tasks;
using BaseApp.Common.Injection.Config;
using BaseApp.Web.Code.BLL.Site.Examples.Models;

namespace BaseApp.Web.Code.BLL.Site.Examples;

[Injectable(InjectableTypes.LifetimeScope)]
public interface IExampleCommandManager
{
    Task<ExampleUserModel> SaveTestFileDataAsync(ExamplePostFileArgs args);
}