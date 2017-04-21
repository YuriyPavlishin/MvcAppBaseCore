using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BaseApp.Data.Transaction
{
    public class DbContextTransactionWrapper : TransactionWrapperBase
    {
        public IDbContextTransaction InnerTransaction { get; private set; }

        public DbContextTransactionWrapper(DbContext context)
        {
            InnerTransaction = context.Database.BeginTransaction();
        }

        protected override void DoCommit()
        {
            InnerTransaction.Commit();
        }

        protected override void DoRollback()
        {
            InnerTransaction.Rollback();
        }

        protected override void DoDispose()
        {
            if (InnerTransaction != null)
            {
                try
                {
                    InnerTransaction.Dispose();
                }
                finally
                {
                    InnerTransaction = null;
                }
            }
        }
    }
}
