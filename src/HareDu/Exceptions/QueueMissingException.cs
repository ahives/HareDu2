namespace HareDu.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class QueueMissingException :
        Exception
    {
        public QueueMissingException()
        {
        }

        protected QueueMissingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public QueueMissingException(string message)
            : base(message)
        {
        }

        public QueueMissingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}