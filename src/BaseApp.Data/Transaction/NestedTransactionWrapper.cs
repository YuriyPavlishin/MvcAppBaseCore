using BaseApp.Data.Transaction.Actions;

namespace BaseApp.Data.Transaction
{
    public class NestedTransactionWrapper : ITransactionWrapper
    {
        private bool IsCommited;
        private TransactionWrapperBase InnerTransaction;

        public NestedTransactionWrapper(TransactionWrapperBase innerTransaction)
        {
            InnerTransaction = innerTransaction;
        }

        public void RegisterAfterCommitAction(ActionBase action)
        {
            InnerTransaction.RegisterAfterCommitAction(action);
        }

        public void RegisterAfterRollbackAction(ActionBase action)
        {
            InnerTransaction.RegisterAfterRollbackAction(action);
        }

        public void Commit()
        {
            IsCommited = true;
        }

        public void Dispose()
        {
            if (!IsCommited)
            {
                InnerTransaction.RollbackOnDispose();
            }
        }
    }
}
