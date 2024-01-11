using System.Threading.Tasks;
using BaseApp.Common.Injection.Config;

namespace BaseApp.Web.Code.Infrastructure.CustomRazor
{
    [Injectable(InjectableTypes.SingleInstance)]
    public interface ICustomRazorViewService
    {
        Task<string> RenderAsync<T>(string viewPath, T model);
    }
}
