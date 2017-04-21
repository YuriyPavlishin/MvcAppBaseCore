using System;
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
       

       /* public static string GetPath<TObject>(Expression<Func<TObject, object>> propertyRefExpr)
        {
            string basePath = GetPropertyPathCore(propertyRefExpr.Parameters[0]) + ".";
            string path = GetPropertyPathCore(propertyRefExpr.Body)
                            .Remove(0, basePath.Length);

            return path;
        }*/

       /* public static IEnumerable<PropertyInfo> GetProperties<T>(Expression<Func<T, object>> propertyExpression)
        {
            return GetProperties(propertyExpression.Body);
        }*/

       /* private static IEnumerable<PropertyInfo> GetProperties(Expression expression)
        {
            var memberExpression = expression as MemberExpression;
            if (memberExpression == null)
                yield break;

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new Exception("Expression is not a property accessor");
            }
            foreach (var propertyInfo in GetProperties(memberExpression.Expression))
            {
                yield return propertyInfo;
            }
            yield return property;
        }*/

      

        

        /*private static string GetPropertyPathCore(Expression expressionBody)
        {
            if (expressionBody == null)
                throw new ArgumentNullException("expressionBody", "expressionBody is null.");

            string propertyPath;
            switch (expressionBody.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = expressionBody as UnaryExpression;
                    propertyPath = (ue != null ? ue.Operand : null).ToString();
                    break;
                default:
                    propertyPath = expressionBody.ToString();
                    break;
            }

            return propertyPath;

           /* MemberExpression memberExpr = expressionBody as MemberExpression;
            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = expressionBody as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                    memberExpr = unaryExpr.Operand as MemberExpression;
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
                return memberExpr.Member.Name;

            throw new ArgumentException("No property reference expression was found.",
                             "expressionBody");#1#
        }*/
    }
}
