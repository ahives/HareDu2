namespace HareDu.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class DeleteVirtualHostException :
        Exception
    {
        public DeleteVirtualHostException()
        {
        }

        protected DeleteVirtualHostException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public DeleteVirtualHostException(string message)
            : base(message)
        {
        }

        public DeleteVirtualHostException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}