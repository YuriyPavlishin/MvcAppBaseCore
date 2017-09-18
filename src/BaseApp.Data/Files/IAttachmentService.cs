using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;
using BaseApp.Data.Transaction;

namespace BaseApp.Data.Files
{
    public interface IAttachmentService
    {
        Attachment CreateAttachment(IUnitOfWork unitOfWork, int userId, string fileName, byte[] content, ITransactionWrapper tran);
        void DeleteAttachment(IUnitOfWork unitOfWork, Attachment attachment, ITransactionWrapper tran);
    }
}
