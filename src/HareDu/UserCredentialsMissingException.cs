namespace HareDu
{
    using System;
    using System.Runtime.Serialization;

    public class UserCredentialsMissingException :
        Exception
    {
        public UserCredentialsMissingException()
        {
        }

        protected UserCredentialsMissingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public UserCredentialsMissingException(string message)
            : base(message)
        {
        }

        public UserCredentialsMissingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}