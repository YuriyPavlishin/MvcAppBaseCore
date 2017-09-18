using System;
using BaseApp.Data.DataContext;

namespace BaseApp.Data.Infrastructure
{
    public interface IUnitOfWorkFactory : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }
    }

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public UnitOfWorkFactory(DBData context)
        {
            UnitOfWork = new UnitOfWork(context);
        }
        
        public IUnitOfWork UnitOfWork { get; }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
