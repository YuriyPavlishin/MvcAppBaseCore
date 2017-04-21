using BaseApp.Common.Log;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BaseApp.Web.Code.Infrastructure
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            LogHolder.MainLog.Error(context.Exception);
        }
        
    }
}
