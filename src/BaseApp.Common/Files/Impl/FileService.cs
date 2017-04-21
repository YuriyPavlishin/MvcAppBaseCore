using System;
using System.IO;
using BaseApp.Common.Files.Models;

namespace BaseApp.Common.Files.Impl
{
    public class FileService: IFileService
    {
        private string DirectoryPath { get; }

        public FileService(string directoryPath)
        {
            DirectoryPath = directoryPath;

            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
        }

        /// <summary>
        /// save file and return generated unique file name
        /// </summary>
        public FileSaveResult SaveFileWithUniqueName(string fileName, byte[] content)
        {
            string genFileName = GenerateUniqueFileName(fileName);
            var fullPath = SaveFile(genFileName, content);

            return new FileSaveResult(genFileName, fullPath);
        }

        private string SaveFile(string fileName, byte[] content)
        {
            string path = GetFilePath(fileName);
            File.WriteAllBytes(path, content);
            return path;
        }

        public byte[] GetFileContent(string genFileName)
        {
            return File.ReadAllBytes(GetFilePath(genFileName));
        }

        public void DeleteFile(string genFileName)
        {
            File.Delete(GetFilePath(genFileName));
        }

        private string GenerateUniqueFileName(string fileName)
        {
            return $"{Path.GetFileNameWithoutExtension(fileName)}_{Guid.NewGuid():N}{Path.GetExtension(fileName)}";
        }

        public string RecoverOriginalFileName(string genFileName)
        {
            if (string.IsNullOrWhiteSpace(genFileName))
                return "";

            int index = genFileName.LastIndexOf("_", StringComparison.InvariantCulture);
            string originalFileName = genFileName.Substring(0, index) + Path.GetExtension(genFileName);

            return originalFileName;
        }

        public string GetFilePath(string genFileName)
        {
            return Path.Combine(DirectoryPath, genFileName);
        }
    }
}
