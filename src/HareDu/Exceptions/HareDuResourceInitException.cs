namespace HareDu.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class HareDuResourceInitException :
        Exception
    {
        public HareDuResourceInitException()
        {
        }

        protected HareDuResourceInitException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public HareDuResourceInitException(string message)
            : base(message)
        {
        }

        public HareDuResourceInitException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}