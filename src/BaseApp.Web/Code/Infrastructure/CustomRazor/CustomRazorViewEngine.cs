using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BaseApp.Web.Code.Infrastructure.CustomRazor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace BaseApp.Web.Code.Infrastructure.CustomRazor
{
    public class CustomRazorViewEngine
    {
        private ActionContext _actionContext;

        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public CustomRazorViewEngine(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderViewToStringAsync<TModel>(string viewPath, TModel model)
        {
            if (_actionContext == null)
            {
                _actionContext = GetActionContext();
            }
            
            var view = FindView(_actionContext, viewPath);

            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(
                    _actionContext,
                    view,
                    new ViewDataDictionary<TModel>(metadataProvider: new EmptyModelMetadataProvider(), modelState: new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(_actionContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await view.RenderAsync(viewContext);

                return sw.ToString();
            }
        }

        private IView FindView(ActionContext actionContext, string viewPath)
        {
            var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewPath, isMainPage: true);
            if (getViewResult.Success) return getViewResult.View;

            var findViewResult = _viewEngine.FindView(actionContext, viewPath, isMainPage: true);
            if (findViewResult.Success) return findViewResult.View;

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewPath}'. The following locations were searched:" }.Concat(searchedLocations)
            );

            throw new InvalidOperationException(errorMessage);
        }

        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext {RequestServices = _serviceProvider};
            return new ActionContext(httpContext, new RouteData { Routers = { new CustomRazorViewRoute() }}, new ActionDescriptor());
        }
    }
}