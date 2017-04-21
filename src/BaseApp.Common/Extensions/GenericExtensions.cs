using System;
using System.Linq;
using System.Linq.Expressions;
using BaseApp.Common.Utils;

namespace BaseApp.Common.Extensions
{
    public static class GenericExtensions
    {
        public static string GetPropertyName<TObject>(this TObject type,
                                                       Expression<Func<TObject, object>> propertyRefExpr)
        {
            return PropertyUtil.GetName(propertyRefExpr);
        }

        public static bool In<T>(this T item, params T[] items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            return items.Contains(item);
        }
    }
}
