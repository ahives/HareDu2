namespace HareDu.Model
{
    using System.Collections.Generic;

    public interface SampleRetentionPolicies
    {
        IList<ulong> Global { get; }
        
        IList<ulong> Basic { get; }
        
        IList<ulong> Detailed { get; }
    }
}