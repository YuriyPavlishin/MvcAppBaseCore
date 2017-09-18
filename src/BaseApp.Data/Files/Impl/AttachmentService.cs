using System;
using BaseApp.Common.Files;
using BaseApp.Common.Utils;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;
using BaseApp.Data.Transaction;
using BaseApp.Data.Transaction.Actions;

namespace BaseApp.Data.Files.Impl
{
    public class AttachmentService: IAttachmentService
    {
        private readonly IFileFactoryService FileFactory;
        protected virtual IFileService FileService => FileFactory.Attachments;

        public AttachmentService(IFileFactoryService fileFactory)
        {
            FileFactory = fileFactory;
        }

        public Attachment CreateAttachment(IUnitOfWork unitOfWork, int userId, string fileName, byte[] content, ITransactionWrapper tran)
        {
            var saveFileResult = FileService.SaveFileWithUniqueName(fileName, content);
            var genAction = new GenericCatchErrorAction(() => FileService.DeleteFile(saveFileResult.GenFileName)
                , $"Error while remove file in save method, path {saveFileResult.GenFileName}");
            tran.RegisterAfterRollbackAction(genAction);

            var attachment = unitOfWork.Attachments.CreateEmpty();
            attachment.CreatedByUserId = userId;
            attachment.CreatedDate = DateTime.Now;
            attachment.ContentType = MimeTypeResolver.Resolve(fileName);
            attachment.FileSize = content.Length;
            attachment.FileName = fileName;
            attachment.GenFileName = saveFileResult.GenFileName;
            
            return attachment;
        }

        public void DeleteAttachment(IUnitOfWork unitOfWork, Attachment attachment, ITransactionWrapper tran)
        {
            if (!string.IsNullOrWhiteSpace(attachment.GenFileName))
            {
                var genAction = new GenericCatchErrorAction(() => FileService.DeleteFile(attachment.GenFileName)
                    , $"Error while remove file in delete method, path: {attachment.GenFileName}");
                tran.RegisterAfterCommitAction(genAction);
            }
            unitOfWork.Attachments.MarkForDelete(attachment);
        }
    }
}
