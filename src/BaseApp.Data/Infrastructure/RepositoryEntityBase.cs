﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BaseApp.Data.Exceptions;
using BaseApp.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.Infrastructure
{
    public abstract class RepositoryEntityBase<T> : RepositoryBase
        where T : class, new()
    {
        protected DbSet<T> EntitySet => Context.Set<T>();

        protected RepositoryEntityBase(DataContextProvider context)
            : base(context)
        {
        }

        public T GetOrNull(int id)
        {
            return FindWithIgnoreQueryFilters(id);
        }

        public T Get(int id)
        {
            var record = GetOrNull(id);
            if (record == null)
                throw new RecordNotFoundException(typeof(T), id);
            return record;
        }

        public virtual List<T> GetAll()
        {
            return EntitySet.GetDefaultOrder().ToList();
        }

        public virtual T CreateEmpty()
        {
            return base.CreateEmpty<T>();
        }

        public virtual void MarkForInsert(T entity)
        {
            base.MarkForInsert(entity);
        }

        public virtual void MarkForUpdate(T entity)
        {
            base.MarkForUpdate(entity);
        }

        public virtual void MarkForDelete(T entity)
        {
            base.MarkForDelete(entity);
        }

        public virtual void MarkForDelete(IEnumerable<T> entities)
        {
            base.MarkForDelete(entities);
        }

        public TResult GetCustomOrDefault<TResult>(int id, Expression<Func<T, TResult>> selector)
        {
            return GetCustomInner(id, selector, true);
        }

        public TResult GetCustom<TResult>(int id, Expression<Func<T, TResult>> selector)
        {
            return GetCustomInner(id, selector);
        }

        protected TResult GetCustomInner<TResult>(int id, Expression<Func<T, TResult>> selector, bool returnDefault = false, IQueryable<T> useAsBaseQuery = null)
        {
            var exp = GetByIdExpression(id);

            var baseQuery = useAsBaseQuery ?? EntitySet;
            var query = baseQuery.Where(exp).Select(selector);

            var resultList = query.ToArray();
            if (returnDefault && resultList.Length == 0)
                return default(TResult);

            if (resultList.Length == 0)
                throw new RecordNotFoundException(typeof(T), id);

            return resultList[0];
        }

        private T FindWithIgnoreQueryFilters<TValue>(TValue id)
        {
            var expr = GetByIdExpression(id);

            var cache = Context.Set<T>().Local.SingleOrDefault(expr.Compile());
            return
                cache
                ?? Context.Set<T>().IgnoreQueryFilters().SingleOrDefault(expr);
        }

        private Expression<Func<T, bool>> GetByIdExpression<TValue>(TValue id)
        {
            var keys = Context.GetEntityKeys<T>().ToArray();
            if (keys.Length != 1)
                throw new Exception("GetCustomInner works only with Entity which has one primary key column.");

            var param = Expression.Parameter(typeof(T), "p");
            var exp = Expression.Lambda<Func<T, bool>>(
                Expression.Equal(
                    Expression.Property(param, keys[0]),
                    ExpressionClosureFactory.GetField(id)
                ),
                param
            );
            return exp;
        }
    }
}
