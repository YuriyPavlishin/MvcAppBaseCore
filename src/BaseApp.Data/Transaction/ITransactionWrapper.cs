using System;
using BaseApp.Data.Transaction.Actions;

namespace BaseApp.Data.Transaction
{
    public interface ITransactionWrapper : IDisposable
    {
        void RegisterAfterCommitAction(ActionBase action);
        void RegisterAfterRollbackAction(ActionBase action);

        void Commit();
    }
}
