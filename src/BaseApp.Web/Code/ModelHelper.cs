using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using BaseApp.Common.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BaseApp.Web.Code
{
    public static class ModelHelper
    {
        public static string GetDisplayName<TModel>(Expression<Func<TModel, object>> expression)
        {
            var property = PropertyUtil.GetProperty(expression);
            string propertyDisplayName = null;

            var attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();
            if (attr == null)
            {
                var attrDisplayName = (DisplayNameAttribute)property.GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault();
                if (attrDisplayName != null)
                {
                    propertyDisplayName = attrDisplayName.DisplayName;
                }
            }
            else
            {
                propertyDisplayName = attr.Name;
            }
            return propertyDisplayName ?? property.Name;
        }

        public static string GetPropertyPath<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression)
        {
            var expressionProvider = htmlHelper.ViewContext.HttpContext.RequestServices
                .GetService(typeof(ModelExpressionProvider)) as ModelExpressionProvider;

            return expressionProvider.GetExpressionText(expression);
        }
    }
}
