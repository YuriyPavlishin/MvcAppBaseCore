using System;
using BaseApp.Data.DataRepository;
using BaseApp.Data.DataRepository.Users;
using BaseApp.Data.Transaction;

namespace BaseApp.Data.Infrastructure;

public interface IUnitOfWork: IDisposable
{
    IUserRepository Users { get; }
    IAttachmentRepository Attachments { get; }
    ICountryRepository Countries { get; }
    ISchedulerRepository Schedulers { get; }

    ITransactionWrapper BeginTransaction();

    void SaveChanges();
}
    
public interface IUnitOfWorkPerCall : IUnitOfWork
{
}
