using System.Threading.Tasks;
using BaseApp.Common.Injection.Config;

namespace BaseApp.Web.Code.Infrastructure.CustomRazor
{
    [Injectable(InjectableTypes.SingleInstance)]
    public interface ICustomRazorViewService
    {
        string Render<T>(string viewPath, T model);
        Task<string> RenderAsync<T>(string viewPath, T model);
    }
}
