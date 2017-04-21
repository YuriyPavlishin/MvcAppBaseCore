using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using BaseApp.Web.Code.Infrastructure.Menu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace BaseApp.Web.Code.Infrastructure.Menu
{
    public class MenuUrlFactory
    {
        public MenuMvcArgs MenuMvcArgs { get; }

        public MenuUrlFactory(MenuMvcArgs menuMvcArgs)
        {
            MenuMvcArgs = menuMvcArgs;
        }

        public MenuUrlInfo Create<T>(System.Linq.Expressions.Expression<Action<T>> action, string area, object routeValues)
            where T : Controller
        {
            Type controllerType = typeof(T);

            string actionName = GetActionName(action);
            string controllerName = GetControllerName(controllerType);
            string url = MakeUrl(actionName, controllerName, routeValues, area);

            //return new MenuUrlInfo(url, controllerType, actionName, controllerName, area, routeValues);
            return new MenuUrlInfo(url, IsCurrent(actionName, controllerName, area, routeValues), HasPermission(controllerType));
        }

        public MenuUrlInfo Create<T>(System.Linq.Expressions.Expression<Action<T>> action, object routeValues = null)
            where T : Controller
        {
            return Create(action, "", routeValues);
        }

        public MenuUrlInfo CreateAdmin<T>(System.Linq.Expressions.Expression<Action<T>> action, object routeValues = null)
            where T : Controller
        {
            return Create(action, "Admin", routeValues);
        }

        private bool HasPermission(Type controllerType)
        {
            return true;//TODO: determinate action permission

            //var controller = (Controller)DependencyResolver.Current.GetService(ControllerType);
            //return urlHelper.HasActionPermission(controller, Action);
        }

        private bool IsCurrent(string action, string controller, string area = null, object routeValues = null)
        {
            var routeData = MenuMvcArgs.RouteData;
            var queryString = MenuMvcArgs.QueryString;
            
            var currentAction = (string)routeData.Values["action"];
            var currentController = (string)routeData.Values["controller"];
            var currentArea = GetCurrentArea(routeData);

            var isCurrent = string.Equals(currentAction, action, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(currentController, controller, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(currentArea, area, StringComparison.OrdinalIgnoreCase);
            
            if (isCurrent && routeValues != null)
            {
                var r = new RouteValueDictionary(routeValues);
                isCurrent = r.All(m =>
                {
                    object value = null;
                    if (routeData.Values.ContainsKey(m.Key))
                    {
                        value = routeData.Values[m.Key];
                    }
                    else if (queryString.ContainsKey(m.Key))
                    {
                        value = queryString[m.Key];
                    }

                    if (value != null)
                    {
                        var converter = TypeDescriptor.GetConverter(m.Value.GetType());
                        if (converter.CanConvertFrom(value.GetType()))
                        {
                            object convertedValue = converter.ConvertFrom(value);
                            return convertedValue != null && convertedValue.Equals(m.Value);
                        }
                    }
                    return false;
                });
            }

            return isCurrent;
        }

        #region static helper methods

        private static string GetCurrentArea(RouteData routeData)
        {
            object currArea;
            if (routeData.DataTokens.TryGetValue("area", out currArea))
                return (string)currArea;
            return "";
        }

        private string MakeUrl(string action, string controller, object routeValues,
            string area)
        {
            return MenuMvcArgs.UrlHelper.Action(action, controller, MergeRouteValuesWithArea(routeValues, area));
        }

        private static RouteValueDictionary MergeRouteValuesWithArea(object routeValues, string area)
        {
            if (routeValues == null)
            {
                routeValues = new { };
            }

            var r = new RouteValueDictionary(routeValues);
            if (!r.Keys.Contains("area"))
            {
                r.Add("area", area);
            }
            return r;
        }

        private static string GetActionName<T>(Expression<Action<T>> expr) where T : Controller
        {
            var memberExpr = expr.Body as MethodCallExpression;
            if (memberExpr == null)
                throw new ArgumentException("expr should represent access to a method");
            if (!typeof(IActionResult).IsAssignableFrom(memberExpr.Method.ReturnType))
                throw new ArgumentException("expr should represent access to a method that returns ActionResult");

            return memberExpr.Method.Name;
        }

        private static string GetControllerName(Type controllerType)
        {
            string controllerTypeName = controllerType.Name;
            string controller = "";
            string suffix = "Controller";
            if (controllerTypeName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            {
                controller = controllerTypeName.Remove(controllerTypeName.Length - suffix.Length, suffix.Length);
            }
            return controller;
        }

        #endregion
    }
}
