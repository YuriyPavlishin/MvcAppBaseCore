using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;

namespace BaseApp.Data.DataRepository.Impl
{
    public class AttachmentRepository : RepositoryEntityBase<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(DataContextProvider context): base(context)
        {
        }
    }
}
