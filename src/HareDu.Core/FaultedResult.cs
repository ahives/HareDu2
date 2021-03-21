namespace HareDu.Core
{
    using System;
    using System.Collections.Generic;

    public class FaultedResult :
        Result
    {
        public FaultedResult(IReadOnlyList<Error> errors)
        {
            Errors = errors;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public FaultedResult(IReadOnlyList<Error> errors, DebugInfo debugInfo)
        {
            DebugInfo = debugInfo;
            Errors = errors;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public FaultedResult(DebugInfo debugInfo)
        {
            DebugInfo = debugInfo;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset Timestamp { get; }
        public DebugInfo DebugInfo { get; }
        public IReadOnlyList<Error> Errors { get; }
        public bool HasFaulted => true;
    }
    
    public class FaultedResult<T> :
        Result<T>
    {
        public FaultedResult(List<Error> errors)
        {
            Errors = errors;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public FaultedResult(IReadOnlyList<Error> errors, DebugInfo debugInfo)
        {
            DebugInfo = debugInfo;
            Errors = errors;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public FaultedResult(DebugInfo debugInfo)
        {
            DebugInfo = debugInfo;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public T Data => default;
        public bool HasData => false;
        public DateTimeOffset Timestamp { get; }
        public DebugInfo DebugInfo { get; }
        public IReadOnlyList<Error> Errors { get; }
        public bool HasFaulted => true;
    }
}