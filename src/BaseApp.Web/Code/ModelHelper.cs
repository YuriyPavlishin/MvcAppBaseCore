using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using BaseApp.Common.Utils;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

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

        public static string GetPropertyPath<T>(Expression<Func<T, object>> expression)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            //name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            return name;
        }
    }
}
