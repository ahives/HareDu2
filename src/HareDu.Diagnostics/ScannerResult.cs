namespace HareDu.Diagnostics
{
    using System;
    using System.Collections.Generic;

    public interface ScannerResult
    {
        Guid Id { get; }
        
        string ScannerId { get; }
        
        IReadOnlyList<ProbeResult> Results { get; }
        
        DateTimeOffset Timestamp { get; }
    }
}