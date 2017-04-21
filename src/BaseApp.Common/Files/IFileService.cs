using BaseApp.Common.Files.Models;

namespace BaseApp.Common.Files
{
    public interface IFileService
    {
        FileSaveResult SaveFileWithUniqueName(string fileName, byte[] content);
        byte[] GetFileContent(string genFileName);
        void DeleteFile(string genFileName);
        string RecoverOriginalFileName(string genFileName);
        string GetFilePath(string genFileName);
    }
}
