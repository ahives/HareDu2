namespace HareDu.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class HostUrlMissingException :
        Exception
    {
        public HostUrlMissingException()
        {
        }

        protected HostUrlMissingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public HostUrlMissingException(string message)
            : base(message)
        {
        }

        public HostUrlMissingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}