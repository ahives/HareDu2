namespace HareDu.Snapshotting
{
    using System;
    using System.Runtime.Serialization;

    public class HareDuSnapshotInitException :
        Exception
    {
        public HareDuSnapshotInitException()
        {
        }

        protected HareDuSnapshotInitException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public HareDuSnapshotInitException(string message)
            : base(message)
        {
        }

        public HareDuSnapshotInitException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}