namespace HareDu.Core
{
    using System;
    using System.Collections.Generic;
    using Extensions;

    public class SuccessfulResult :
        Result
    {
        public SuccessfulResult(DebugInfo debugInfo)
        {
            DebugInfo = debugInfo;
            Errors = new List<Error>();
            Timestamp = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset Timestamp { get; }
        public DebugInfo DebugInfo { get; }
        public IReadOnlyList<Error> Errors { get; }
        public bool HasFaulted => false;
    }
    
    public class SuccessfulResult<T> :
        Result<T>
    {
        public SuccessfulResult(T data, DebugInfo debugInfo)
        {
            Data = data.IsNull() ? default : data;
            DebugInfo = debugInfo;
            Errors = new List<Error>();
            Timestamp = DateTimeOffset.UtcNow;
            HasData = !Data.IsNull();
        }

        public T Data { get; }
        public bool HasData { get; }
        public DateTimeOffset Timestamp { get; }
        public DebugInfo DebugInfo { get; }
        public IReadOnlyList<Error> Errors { get; }
        public bool HasFaulted => false;
    }
}