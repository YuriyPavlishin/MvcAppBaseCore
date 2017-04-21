namespace BaseApp.Common.Files.Models
{
    public class FileSaveResult
    {
        public string GenFileName { get; private set; }
        public string FullFilePath { get; private set; }

        public FileSaveResult(string genFileName, string fullFilePath)
        {
            GenFileName = genFileName;
            FullFilePath = fullFilePath;
        }
    }
}
