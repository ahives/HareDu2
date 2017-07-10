namespace HareDu.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class ExchangeRoutingTypeMissingException :
        Exception
    {
        public ExchangeRoutingTypeMissingException()
        {
        }

        protected ExchangeRoutingTypeMissingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ExchangeRoutingTypeMissingException(string message)
            : base(message)
        {
        }

        public ExchangeRoutingTypeMissingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}