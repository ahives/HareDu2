namespace HareDu.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public interface Result
    {
        DateTimeOffset Timestamp { get; }
        
        DebugInfo DebugInfo { get; }
        
        IReadOnlyList<Error> Errors { get; }
        
        bool HasFaulted { get; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface Result<out T> :
        Result
    {
        T Data { get; }
        
        bool HasData { get; }
    }
}