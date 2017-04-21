using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;
using BaseApp.Data.Transaction;

namespace BaseApp.Data.Files
{
    public interface IAttachmentService
    {
        Attachment CreateAttachment(UnitOfWork unitOfWork, int userId, string fileName, byte[] content, ITransactionWrapper tran);
        void DeleteAttachment(UnitOfWork unitOfWork, Attachment attachment, ITransactionWrapper tran);
    }
}
