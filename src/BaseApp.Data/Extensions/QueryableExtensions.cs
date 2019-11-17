using System;
using System.Linq;
using System.Linq.Dynamic.Core;
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

            page.TotalItemCount = qSource.Count();

            if (!string.IsNullOrWhiteSpace(page.Sort))
            {
                qSource = qSource.OrderByForPaging(page.Sort);
            }
            qSource = qSource.Skip((page.Page - 1) * page.PageSizeReal).Take(page.PageSizeReal);

            return qSource;
        }

        public static IQueryable<T> OrderByForPaging<T>(this IQueryable<T> source, string sortExpression)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "source is null.");

            if (string.IsNullOrEmpty(sortExpression))
                throw new ArgumentException("sortExpression is null or empty.", nameof(sortExpression));
            
            return source.OrderBy(sortExpression.Replace("__", "."));
        }

        public static IQueryable<T> GetNotDeleted<T>(this IQueryable<T> q) where T : IDeletable, new() 
        {
            return q.Where(x=>x.DeletedDate == null);
        }

        public static IQueryable<T> GetDefaultOrder<T>(this IQueryable<T> q)
        {
            Type orderedType = typeof(T);


            const string ordinalColumn = "Ordinal";
            bool existsOrdinal = orderedType.GetProperty(ordinalColumn) != null;

            const string nameColumn = "Name";
            bool existsName = orderedType.GetProperty(nameColumn) != null;

            if (existsName)
                return q.OrderByForPaging($"{(existsOrdinal ? ordinalColumn + ", " : "")}{nameColumn}");


            const string titleColumn = "Title";
            bool existsTitle = orderedType.GetProperty(titleColumn) != null;

            if (existsTitle)
                return q.OrderByForPaging($"{(existsOrdinal ? ordinalColumn + ", " : "")}{titleColumn}");


            if (existsOrdinal)
                return q.OrderByForPaging(ordinalColumn);

            return q;
        }
    }
}
