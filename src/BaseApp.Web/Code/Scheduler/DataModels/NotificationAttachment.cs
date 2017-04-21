using System.IO;

namespace BaseApp.Web.Code.Scheduler.DataModels
{
    public abstract class NotificationAttachment
    {
        public string FileName { get; set; }
        public abstract byte[] GetFileBytes();

        public static NotificationAttachment Create(string fileName, byte[] bytes)
        {
            return new ByteNotificationAttachment() {FileBytes = bytes, FileName = fileName};
        }

        public static NotificationAttachment Create(string fileName, string filePath)
        {
            return new FileNotificationAttachment() { FilePath = filePath, FileName = fileName };
        }
    }

    public class ByteNotificationAttachment : NotificationAttachment
    {
        public byte[] FileBytes { get; set; }

        public override byte[] GetFileBytes()
        {
            return FileBytes;
        }
    }

    public class FileNotificationAttachment : NotificationAttachment
    {
        public string FilePath { get; set; }

        public override byte[] GetFileBytes()
        {
            return File.ReadAllBytes(FilePath);
        }
    }
}