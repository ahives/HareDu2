namespace HareDu.Diagnostics
{
    using System;

    public interface ProbeContext
    {
        ProbeResult Result { get; }
        
        DateTimeOffset Timestamp { get; }
    }
}