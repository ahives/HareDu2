namespace HareDu.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class VirtualHostMissingException :
        Exception
    {
        public VirtualHostMissingException()
        {
        }

        protected VirtualHostMissingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public VirtualHostMissingException(string message)
            : base(message)
        {
        }

        public VirtualHostMissingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}