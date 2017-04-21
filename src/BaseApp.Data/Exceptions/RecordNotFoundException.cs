using System;

namespace BaseApp.Data.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        public Type RecordType { get; private set; }
        public int RecordId { get; private set; }
        public bool ExistsAsDeleted { get; private set; }

        public RecordNotFoundException(Type recordType, int recordId, bool existsAsDeleted = false)
            : base(GetFormattedMessage(recordType, recordId))
        {
            RecordType = recordType;
            RecordId = recordId;
            ExistsAsDeleted = existsAsDeleted;
        }

        public override string ToString()
        {
            return GetFormattedMessage(RecordType, RecordId);
        }

        private static string GetFormattedMessage(Type recordType, int recordId)
        {
            return $"{recordType.Name} not found. ID - {recordId}";
        }
    }
}
