using System;
using System.Linq.Expressions;
using BaseApp.Data.DataContext.Interfaces;
using BaseApp.Data.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.Infrastructure
{
    public class RepositoryEntityDeletableBase<T> : RepositoryEntityBase<T>
        where T : class, IDeletable, new()
    {
        public RepositoryEntityDeletableBase(DataContextProvider context)
            : base(context)
        {
        }

        public new T GetOrNull(int id)
        {
            var result = base.GetOrNull(id);
            if (result == null)
                return null;

            return result.DeletedDate != null ? null : result;
        }

        public new T Get(int id)
        {
            var result = base.Get(id);

            if (result.DeletedDate != null)
                throw new RecordNotFoundException(typeof(T), id, true);

            return result;
        }

        public T GetWithDeletedOrNull(int id)
        {
            return base.GetOrNull(id);
        }

        public T GetWithDeleted(int id)
        {
            return base.Get(id);
        }

        public TResult GetCustomWithDeletedOrDefault<TResult>(int id, Expression<Func<T, TResult>> selector)
        {
            return GetCustomInner(id, selector, true, EntitySet.IgnoreQueryFilters());
        }

        public TResult GetCustomWithDeleted<TResult>(int id, Expression<Func<T, TResult>> selector)
        {
            return GetCustomInner(id, selector, false, EntitySet.IgnoreQueryFilters());
        }
    }
}
