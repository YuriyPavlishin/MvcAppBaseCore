using System;
using System.Linq.Expressions;
using BaseApp.Data.DataContext.Interfaces;

namespace BaseApp.Data.Infrastructure
{
    public interface IRepositoryEntityDeletableBase<T> : IRepositoryEntityBase<T> where T : class, IDeletable, new()
    {
        T GetWithDeletedOrNull(int id);
        T GetWithDeleted(int id);
        TResult GetCustomWithDeletedOrDefault<TResult>(int id, Expression<Func<T, TResult>> selector);
        TResult GetCustomWithDeleted<TResult>(int id, Expression<Func<T, TResult>> selector);
    }
}
