namespace HareDu.Core
{
    using System.Collections.Generic;

    public interface DebugInfo
    {
        string URL { get; }
        
        string Request { get; }
        
        string Exception { get; }
        
        string StackTrace { get; }
        
        string Response { get; }

        IReadOnlyList<Error> Errors { get; }
    }
}