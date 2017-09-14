//using System.Transactions;

namespace BaseApp.Data.Transaction
{
    //public class TransactionScopeWrapper : TransactionWrapperBase
    //{
    //    private TransactionScope InnerTransaction;

    //    public TransactionScopeWrapper()
    //    {
    //        InnerTransaction = new TransactionScope();
    //    }

    //    protected override void DoCommit()
    //    {
    //        InnerTransaction.Complete();
    //    }

    //    protected override void DoRollback()
    //    {
    //        DoDispose();
    //    }

    //    protected override void DoDispose()
    //    {
    //        if (InnerTransaction != null)
    //        {
    //            try
    //            {
    //                InnerTransaction.Dispose();
    //            }
    //            finally 
    //            {
    //                InnerTransaction = null;
    //            }
    //        }
    //    }
    //}
}
