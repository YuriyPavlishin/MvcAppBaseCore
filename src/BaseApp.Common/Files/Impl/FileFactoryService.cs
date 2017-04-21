using BaseApp.Common.Files.Models;
using Microsoft.Extensions.Options;

namespace BaseApp.Common.Files.Impl
{
    public class FileFactoryService: IFileFactoryService
    {
        public IFileService Attachments { get; }

        public FileFactoryService(IOptions<FileFactoryOptions> options)
        {
            Attachments = new FileService(options.Value.AttachmentsFolder);
        }
    }
}
