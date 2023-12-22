using BaseApp.Common.Injection.Config;

namespace BaseApp.Common.Files
{
    [Injectable(InjectableTypes.SingleInstance)]
    public interface IFileFactoryService
    {
        IFileService Attachments { get; }
    }
}
