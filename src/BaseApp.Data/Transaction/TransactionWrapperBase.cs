using System;
using System.Collections.Generic;
using BaseApp.Common.Logs;
using BaseApp.Data.Transaction.Actions;

namespace BaseApp.Data.Transaction
{
    public abstract class TransactionWrapperBase : ITransactionWrapper
    {
        private bool IsCommited;
        private bool IsRollbackOnDispose;

        public bool IsDisposed { get; private set; }

        private List<ActionBase> AfterCommitActions = new List<ActionBase>();
        private List<ActionBase> AfterRollbackActions = new List<ActionBase>();

        public void RegisterAfterCommitAction(ActionBase action)
        {
            AfterCommitActions.Add(action);
        }

        public void RegisterAfterRollbackAction(ActionBase action)
        {
            AfterRollbackActions.Add(action);
        }

        public void Commit()
        {
            if (IsDisposed)
                throw new Exception("Transaction already disposed.");
            if (IsCommited)
                throw new Exception("Transaction already commited.");
            if (IsRollbackOnDispose)
                throw new Exception("TransactionAbortedException");
                //throw new TransactionAbortedException();

            DoCommit();
            IsCommited = true;
            AfterCommitActions.ForEach(a => a.Execute());
        }

        public void RollbackOnDispose()
        {
            if (IsDisposed)
                throw new Exception("Transaction already disposed.");
            if (IsCommited)
                throw new Exception("Transaction already commited.");

            IsRollbackOnDispose = true;
        }

        public void Dispose()
        {
            if (!IsCommited)
            {
                try
                {
                    DoRollback();
                }
                catch (Exception ex)
                {
                    LogHolder.MainLog.Error(ex, "Error transaction rollback.");
                }
            }

            try
            {
                DoDispose();
            }
            catch (Exception ex)
            {
                LogHolder.MainLog.Error(ex, "Error transaction dispose.");
            }

            IsDisposed = true;

            try
            {
                if (!IsCommited)
                {
                    AfterRollbackActions.ForEach(a => a.Execute());
                }
            }
            finally 
            {
                AfterCommitActions.Clear();
                AfterRollbackActions.Clear();

                AfterCommitActions = null;
                AfterRollbackActions = null;
            }
        }

        protected abstract void DoCommit();
        protected abstract void DoRollback();
        protected abstract void DoDispose();
    }
}
