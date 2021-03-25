namespace HareDu.Core
{
    using System;
    using System.Runtime.Serialization;

    public class HareDuConfigurationException :
        Exception
    {
        public HareDuConfigurationException()
        {
        }

        protected HareDuConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public HareDuConfigurationException(string message) : base(message)
        {
        }

        public HareDuConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}