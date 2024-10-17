using System;
using System.Linq;
using System.Linq.Expressions;
using BaseApp.Data.DataContext.Interfaces;
using BaseApp.Data.Models;

namespace BaseApp.Data.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> PagingSorting<T>(this IQueryable<T> source, PagingSortingInfo page)
        {
            IQueryable<T> qSource = source;
            if (source == null)
                throw new ArgumentNullException(nameof(source), "source is null.");
            if (page == null)
                return qSource;

            if (!page.SkipTotalItemCountCalculation)
            {
                page.TotalItemCount = qSource.Count();
            }

            if (!string.IsNullOrWhiteSpace(page.SortMember))
            {
                qSource = qSource.OrderByDynamic(page.SortMember, page.SortDescending);
            }
            qSource = qSource.Skip((page.Page - 1) * page.PageSizeReal).Take(page.PageSizeReal);

            return qSource;
        }

        public static IQueryable<T> GetNotDeleted<T>(this IQueryable<T> q) where T : IDeletable, new()
        {
            return q.Where(x => x.DeletedDate == null);
        }
        
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string sortMember, bool sortDescending, bool thenBy = false)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var selector = Expression.PropertyOrField(parameter, sortMember);
            var method = thenBy
                ? sortDescending ? "ThenByDescending" :"ThenBy"
                : sortDescending ? "OrderByDescending" :"OrderBy";
            var resultExpression = Expression.Call(typeof(Queryable), method, [source.ElementType, selector.Type], source.Expression, Expression.Quote(Expression.Lambda(selector, parameter)));
            return source.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
