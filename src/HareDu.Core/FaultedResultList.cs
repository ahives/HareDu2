namespace HareDu.Core
{
    using System;
    using System.Collections.Generic;

    public class FaultedResultList<T> :
        ResultList<T>
    {
        public FaultedResultList(List<Error> errors)
        {
            Errors = errors;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public FaultedResultList(List<Error> errors, DebugInfo debugInfo)
        {
            DebugInfo = debugInfo;
            Errors = errors;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public FaultedResultList(DebugInfo debugInfo)
        {
            DebugInfo = debugInfo;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset Timestamp { get; }
        public DebugInfo DebugInfo { get; }
        public IReadOnlyList<Error> Errors { get; }
        public bool HasFaulted => true;
        public IReadOnlyList<T> Data { get; }
        public bool HasData => false;
    }
}