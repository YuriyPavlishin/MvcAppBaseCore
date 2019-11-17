using System;
using BaseApp.Data.DataContext;
using BaseApp.Data.DataRepository;
using BaseApp.Data.DataRepository.Impl;
using BaseApp.Data.DataRepository.Users;
using BaseApp.Data.DataRepository.Users.Impl;
using BaseApp.Data.Exceptions;
using BaseApp.Data.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApp.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBData Context;
        private readonly DataContextProvider ContextProvider;
        private DbContextTransactionWrapper CurrentTransaction { get; set; }
        
        public UnitOfWork(DBData context)
        {
            Context = context;
            ContextProvider = new DataContextProvider(Context);
        }

        private IServiceScope CreatedInScope { get; set; }
        public static IUnitOfWork CreateInScope(DBData context, IServiceScope createdInScope)
        {
            var uow = new UnitOfWork(context)
                {
                    CreatedInScope = createdInScope
                };
            return uow;
        }

        #region repositories

        public IUserRepository Users => GetRepository<UserRepository>();
        public IAttachmentRepository Attachments => GetRepository<AttachmentRepository>();
        public ICountryRepository Countries => GetRepository<CountryRepository>();
        public ISchedulerRepository Schedulers => GetRepository<SchedulerRepository>();

        #endregion

        public void SaveChanges()
        {
            try
            {
                Context.SaveChanges();
            }
            //TODO or remove
            //catch (DbEntityValidationException ex)
            //{
            //    //used to extend Message property with validation errors info
            //    throw new DbEntityValidationExceptionWrapper(ex);
            //}
            catch (DbUpdateException ex)
            {
                //used to analyze exception reason
                throw new DbUpdateExceptionWrapper(ex);
            }
        }

        /// <summary>
        /// start new transaction or return NestedTransactionWrapper for already started transaction
        /// </summary>
        public ITransactionWrapper BeginTransaction()
        {
            if (CurrentTransaction != null && !CurrentTransaction.IsDisposed)
            {
                return new NestedTransactionWrapper(CurrentTransaction);
            }
            else
            {
                CurrentTransaction = new DbContextTransactionWrapper(Context);
                return CurrentTransaction;
            }
        }

        private T GetRepository<T>() where T : RepositoryBase
        {
            return ContextProvider.GetRepository<T>();
        }

        public void Dispose()
        {
            CreatedInScope?.Dispose();
            Context.Dispose();
        }
    }
}
