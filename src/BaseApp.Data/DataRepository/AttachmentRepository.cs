using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;

namespace BaseApp.Data.DataRepository
{
    public class AttachmentRepository : RepositoryEntityBase<Attachment>
    {
        public AttachmentRepository(DataContextProvider context): base(context)
        {
        }
    }
}
