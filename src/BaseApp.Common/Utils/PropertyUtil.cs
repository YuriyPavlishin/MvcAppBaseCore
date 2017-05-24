using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BaseApp.Common.Utils
{
    public static class PropertyUtil
    {
        public static string GetName<T>(Expression<Func<T, object>> propertyRefExpr)
        {
            return GetPropertyCore<T>(propertyRefExpr.Body).Name;
        }

        public static string GetName<T>(Expression<T> propertyRefExpr)
        {
            return GetPropertyCore<T>(propertyRefExpr.Body).Name;
        }

        public static PropertyInfo GetProperty<T>(Expression<Func<T, object>> propertyExpression)
        {
            return GetPropertyCore<T>(propertyExpression.Body);
        }

        private static PropertyInfo GetPropertyCore<T>(Expression expressionBody)
        {
            if (expressionBody == null)
                throw new ArgumentNullException(nameof(expressionBody), "expressionBody is null.");

            var memberExpression = expressionBody as MemberExpression;
            if (memberExpression == null)
            {
                var unaryExpr = expressionBody as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                    memberExpression = unaryExpr.Operand as MemberExpression;
            }

            if (memberExpression == null)
                throw new Exception("Invalid expression - not a MemberExpression");

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
                throw new Exception("Invalid expression - not a property");

            return property;
        }

        public static bool IsAssignableFromOrContainsType<T>(this Type sourceType) where T: class 
        {
            if ((!sourceType.IsClass && !sourceType.IsInterface) || sourceType == typeof(string) || sourceType == typeof(char[]))
                return false;
            if (typeof(T).IsAssignableFrom(sourceType))
                return true;
            if (typeof(IEnumerable<T>).IsAssignableFrom(sourceType))
                return true;
            
            if (typeof(IEnumerable<>).IsAssignableFrom(sourceType))
            {
                return IsAssignableFromOrContainsType<T>(sourceType.GetGenericArguments()[0]);
            }

            var properties = sourceType.GetProperties();
            foreach (var property in properties.Where(x => x.PropertyType.IsClass || x.PropertyType.IsInterface))
            {
                if (IsAssignableFromOrContainsType<T>(property.PropertyType))
                    return true;
            }

            return false;
        }
    }
}
