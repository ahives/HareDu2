namespace HareDu.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class ExchangeMissingException :
        Exception
    {
        public ExchangeMissingException()
        {
        }

        protected ExchangeMissingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ExchangeMissingException(string message)
            : base(message)
        {
        }

        public ExchangeMissingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}