namespace BaseApp.Common.Files
{
    public interface IFileFactoryService
    {
        IFileService Attachments { get; }
    }
}
