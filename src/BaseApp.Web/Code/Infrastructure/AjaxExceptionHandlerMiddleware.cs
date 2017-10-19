using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using BaseApp.Common.Logs;
using BaseApp.Web.Code.Extensions;

namespace BaseApp.Web.Code.Infrastructure
{
    public class AjaxExceptionHandlerMiddleware
    {
        private readonly RequestDelegate Next;

        public AjaxExceptionHandlerMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.IsAjaxRequest())
            {
                await Next(context);
                return;
            }

            try
            {
                await Next(context);
            }
            catch (Exception ex)
            {
                LogHolder.MainLog.Error(ex, "Ajax error occured");

                // We can't do anything if the response has already started, just abort.
                if (context.Response.HasStarted)
                {
                    LogHolder.MainLog.Error("Ajax Response.HasStarted");
                    throw;
                }

                try
                {
                    context.Response.Clear();
                    context.Response.ContentType = "text/plain";
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync($"{ex.Message}").ConfigureAwait(false);
                    
                    // TODO: Optional re-throw? We'll re-throw the original exception by default if the error handler throws.
                    return;
                }
                catch (Exception ex2)
                {
                    // Suppress secondary exceptions, re-throw the original.
                    LogHolder.MainLog.Error(ex2, "Exception occured while ajax error processing");
                }
                
                throw; // Re-throw the original if we couldn't handle it
            }
        }       
    }
}
