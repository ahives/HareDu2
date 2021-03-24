namespace HareDu.Snapshotting
{
    using System;
    using System.Runtime.Serialization;

    public class HareDuSnapshotException :
        Exception
    {
        public HareDuSnapshotException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public HareDuSnapshotException(string message)
            : base(message)
        {
        }

        public HareDuSnapshotException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}