namespace HareDu
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    public class SuccessfulListResult<T> :
        Result<IReadOnlyList<T>>
    {
        public SuccessfulListResult(IReadOnlyList<T> data, IReadOnlyList<Error> errors, DebugInfo debugInfo)
        {
            Data = data;
            DebugInfo = debugInfo;
            Errors = errors;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public IReadOnlyList<T> Data { get; }
        public DateTimeOffset Timestamp { get; }
        public DebugInfo DebugInfo { get; }
        public IReadOnlyList<Error> Errors { get; }
        public bool HasResult => !Data.IsNull() && Data.Any();
    }
}