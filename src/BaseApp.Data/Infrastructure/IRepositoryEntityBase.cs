using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BaseApp.Data.Infrastructure
{
    public interface IRepositoryEntityBase<T> where T : class, new()
    {
        T GetOrNull(int id);
        T Get(int id);

        T CreateEmpty();

        void MarkForInsert(T entity);
        void MarkForUpdate(T entity);
        void MarkForDelete(T entity);
        void MarkForDelete(IEnumerable<T> entities);

        TResult GetCustomOrDefault<TResult>(int id, Expression<Func<T, TResult>> selector);
        TResult GetCustom<TResult>(int id, Expression<Func<T, TResult>> selector);

    }
}
