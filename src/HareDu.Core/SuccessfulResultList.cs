namespace HareDu.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    public class SuccessfulResultList<T> :
        ResultList<T>
    {
        public SuccessfulResultList(IReadOnlyList<T> data, DebugInfo debugInfo)
        {
            Data = data.IsNull() ? new List<T>() : data;
            DebugInfo = debugInfo;
            Errors = new List<Error>();
            Timestamp = DateTimeOffset.UtcNow;
            HasData = !Data.IsNull() && Data.Any();
        }

        public DateTimeOffset Timestamp { get; }
        public DebugInfo DebugInfo { get; }
        public IReadOnlyList<Error> Errors { get; }
        public bool HasFaulted { get; }
        public IReadOnlyList<T> Data { get; }
        public bool HasData { get; }
    }
}