namespace HareDu.Model
{
    using System.Collections.Generic;

    public interface VirtualHostLimitsInfo
    {
        string VirtualHostName { get; }
        
        IDictionary<string, ulong> Limits { get; }
    }
}