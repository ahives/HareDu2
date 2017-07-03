namespace HareDu.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class HareDuClientInitException :
        Exception
    {
        public HareDuClientInitException()
        {
        }

        protected HareDuClientInitException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public HareDuClientInitException(string message)
            : base(message)
        {
        }

        public HareDuClientInitException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}