namespace HareDu.Core
{
    using System;

    public interface Error
    {
        string Reason { get; }
        
        DateTimeOffset Timestamp { get; }
    }
}