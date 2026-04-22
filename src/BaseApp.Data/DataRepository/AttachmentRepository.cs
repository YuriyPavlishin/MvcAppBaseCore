using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;

namespace BaseApp.Data.DataRepository;

public interface IAttachmentRepository : IRepositoryEntityBase<Attachment>
{
}

public class AttachmentRepository(DataContextProvider context) : RepositoryEntityBase<Attachment>(context), IAttachmentRepository;
