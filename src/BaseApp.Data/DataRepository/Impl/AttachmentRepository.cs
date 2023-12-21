using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;

namespace BaseApp.Data.DataRepository.Impl
{
    public class AttachmentRepository(DataContextProvider context) : RepositoryEntityBase<Attachment>(context), IAttachmentRepository;
}
