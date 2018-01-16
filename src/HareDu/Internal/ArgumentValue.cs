namespace HareDu.Internal
{
    using System;

    public class ArgumentValue<TValue>
    {
        public TValue Value { get; }
        public Error Error { get; }
        
        public ArgumentValue(TValue value, string errorMsg)
        {
            Value = value;
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

        public ArgumentValue(TValue value)
        {
            Value = value;
        }
    }
}