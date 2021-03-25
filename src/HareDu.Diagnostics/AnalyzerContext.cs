namespace HareDu.Diagnostics
{
    using System;
    using System.Collections.Generic;

    public interface AnalyzerContext
    {
        Guid Id { get; }
        
        IReadOnlyList<AnalyzerSummary> Summary { get; }
        
        DateTimeOffset Timestamp { get; }
    }
}