using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BaseApp.Data.DataContext.Interfaces;
using BaseApp.Data.Exceptions;
using BaseApp.Data.Extensions;

namespace BaseApp.Data.Infrastructure
{
    public class RepositoryEntityDeletableBase<T>(DataContextProvider context) : RepositoryEntityBase<T>(context)
        where T : class, IDeletable, new()
    {
        protected IQueryable<T> EntitySetNotDeleted => EntitySet.GetNotDeleted();

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

        public new TResult GetCustomOrDefault<TResult>(int id, Expression<Func<T, TResult>> selector)
        {
            return GetCustomInner(id, selector, true, EntitySetNotDeleted);
        }

        public new TResult GetCustom<TResult>(int id, Expression<Func<T, TResult>> selector)
        {
            return GetCustomInner(id, selector, false, EntitySetNotDeleted);
        }

        public TResult GetCustomWithDeletedOrDefault<TResult>(int id, Expression<Func<T, TResult>> selector)
        {
            return GetCustomInner(id, selector, true);
        }

        public TResult GetCustomWithDeleted<TResult>(int id, Expression<Func<T, TResult>> selector)
        {
            return GetCustomInner(id, selector);
        }
    }
}
