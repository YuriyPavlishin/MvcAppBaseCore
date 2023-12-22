using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseApp.Common.Injection.Config;
using BaseApp.Data.DataRepository;
using BaseApp.Data.DataRepository.Users;
using BaseApp.Data.Transaction;

namespace BaseApp.Data.Infrastructure
{
    [Injectable(InjectableTypes.LifetimeScope)]
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IAttachmentRepository Attachments { get; }
        ICountryRepository Countries { get; }
        ISchedulerRepository Schedulers { get; }

        ITransactionWrapper BeginTransaction();

        void SaveChanges();
    }
}
