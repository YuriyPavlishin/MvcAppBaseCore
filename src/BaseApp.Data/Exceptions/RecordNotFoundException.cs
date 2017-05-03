using System;

namespace BaseApp.Data.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        public Type RecordType { get; }
        public int RecordId { get; }
        public bool ExistsAsDeleted { get; }

        public RecordNotFoundException(Type recordType, int recordId, bool existsAsDeleted = false)
            : base(GetFormattedMessage(recordType, recordId))
        {
            RecordType = recordType;
            RecordId = recordId;
            ExistsAsDeleted = existsAsDeleted;
        }

        private static string GetFormattedMessage(Type recordType, int recordId)
        {
            return $"{recordType.Name} not found. ID - {recordId}";
        }
    }
}
