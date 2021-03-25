namespace HareDu
{
    using System;
    using System.Runtime.Serialization;

    public class HareDuBrokerObjectInitException :
        Exception
    {
        public HareDuBrokerObjectInitException()
        {
        }

        protected HareDuBrokerObjectInitException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public HareDuBrokerObjectInitException(string message)
            : base(message)
        {
        }

        public HareDuBrokerObjectInitException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}