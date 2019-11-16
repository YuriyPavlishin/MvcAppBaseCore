using System.Threading.Tasks;

namespace BaseApp.Web.Code.Infrastructure.CustomRazor
{
    public interface ICustomRazorViewService
    {
        string Render<T>(string viewPath, T model);
        Task<string> RenderAsync<T>(string viewPath, T model);
    }
}
