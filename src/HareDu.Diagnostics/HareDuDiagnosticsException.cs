namespace HareDu.Diagnostics
{
    using System;
    using System.Runtime.Serialization;

    public class HareDuDiagnosticsException :
        Exception
    {
        public HareDuDiagnosticsException()
        {
        }

        protected HareDuDiagnosticsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public HareDuDiagnosticsException(string message)
            : base(message)
        {
        }

        public HareDuDiagnosticsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}