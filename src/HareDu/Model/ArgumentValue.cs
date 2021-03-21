namespace HareDu.Model
{
    using System;
    using Core;

    public class ArgumentValue<TValue>
    {
        public TValue Value { get; }
        public Error Error { get; }
        
        public ArgumentValue(TValue value, string errorMsg = null)
        {
            Value = value;

            if (!string.IsNullOrWhiteSpace(errorMsg))
                Error = new ErrorImpl(errorMsg);
        }

        
        class ErrorImpl :
            Error
        {
            public ErrorImpl(string errorMsg)
            {
                Reason = errorMsg;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public string Reason { get; }
            public DateTimeOffset Timestamp { get; }
        }
    }
}