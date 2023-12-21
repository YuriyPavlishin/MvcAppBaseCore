using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.Infrastructure
{
    public abstract class RepositoryBase(DataContextProvider context)
    {
        protected DataContextProvider Context { get; } = context;

        protected virtual T CreateEmpty<T>() where T : class, new()
        {
            return Context.Set<T>().Add(new T()).Entity;
        }

        /*TODO or remove
        protected void MarkForSave<T>(T entity) where T : class
        {
            if (Context.IsNewInstance(entity))
            {
                MarkForInsert(entity);
            }
            else
            {
                MarkForUpdate(entity);
            }
        }*/

        protected void MarkForInsert<T>(T entity) where T : class
        {
            Context.Entry(entity).State = EntityState.Added;
        }

        protected void MarkForUpdate<T>(T entity) where T : class
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        protected void MarkForDelete<T>(T entity) where T : class
        {
            Context.Set<T>().Remove(entity);
        }

        protected void MarkForDelete<T>(IEnumerable<T> entities) where T : class
        {
            Context.Set<T>().RemoveRange(entities);
        }

        protected T GetRepository<T>() where T : RepositoryBase
        {
            return Context.GetRepository<T>();
        }
    }
}
