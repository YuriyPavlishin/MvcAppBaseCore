using System;
using System.Collections.Generic;
using System.Linq;
using BaseApp.Data.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BaseApp.Data.Infrastructure
{
    public class DataContextProvider
    {
        private DBData Context { get; set; }
        //public StoredProcedureProvider StoredProcedures { get; private set; }
        private readonly Dictionary<Type, RepositoryBase> Repositories;

        public DataContextProvider(DBData context)
        {
            Context = context;
            //StoredProcedures = new StoredProcedureProvider(context);
            Repositories = new Dictionary<Type, RepositoryBase>();
        }

        public DbSet<T> Set<T>() where T : class
        {
            return Context.Set<T>();
        }

        public EntityEntry Entry(object entity)
        {
            return Context.Entry(entity);
        }

        public EntityEntry<T> Entry<T>(T entity) where T : class
        {
            return Context.Entry(entity);
        }

        /*TODO or remove
        public bool IsNewInstance<T>(T entity) where T : class
        {
            DbSet<T> l_EntitySet = Context.Set<T>();
            var objContext = ((IObjectContextAdapter)Context).ObjectContext;
            ObjectSet<T> objectSet = objContext.CreateObjectSet<T>();
            var key = objContext.CreateEntityKey(string.Format("{0}.{1}", objectSet.EntitySet.EntityContainer.Name, objectSet.EntitySet.Name), entity);

            if (key.EntityKeyValues.Length == 0)
            {
                throw new Exception("No entity key info found - " + typeof(T).Name);
            }

            bool isNew = false;
            if (key.EntityKeyValues.Length == 1)
            {
                string sKey = key.EntityKeyValues.First().Value.ToString();
                int iKey;
                if (int.TryParse(sKey, out iKey) && iKey == 0)
                {
                    isNew = true;
                }
            }

            if (!isNew)
            {
                T find = l_EntitySet.Find(key.EntityKeyValues.Select(c => c.Value).ToArray());
                if (find != null)
                {
                    if (!ReferenceEquals(find, entity))
                    {
                        Context.Entry(find).State = EntityState.Detached;
                    }
                }
                else
                {
                    isNew = true;
                }
            }

            return isNew;
        }*/

        /*TODO: Stored procedures https://github.com/aspnet/EntityFramework/issues/245
         * for now we can use return Context.Set<UserCustom>().FromSql("dbo.p_UserProcTest_s @Id = {0}", id).ToList();
         * but we should add UserCustom to DBData and remove it from migration
         */

        public IEnumerable<string> GetEntityKeys<T>() where T : class
        {
            return Context.Model.FindEntityType(typeof (T)).FindPrimaryKey().Properties
                .Select(x => x.Name);
        }

        internal T GetRepository<T>() where T : RepositoryBase
        {
            Type type = typeof(T);

            RepositoryBase repository;
            if (!Repositories.TryGetValue(type, out repository))
            {
                repository = (RepositoryBase)Activator.CreateInstance(type, this);
                Repositories.Add(type, repository);
            }

            return (T)repository;
        }
    }
}
