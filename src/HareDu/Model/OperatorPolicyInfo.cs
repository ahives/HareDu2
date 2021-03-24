namespace HareDu.Model
{
    using System.Collections.Generic;

    public interface OperatorPolicyInfo
    {
        string VirtualHost { get; }
        
        string Name { get; }
        
        string Pattern { get; }
        
        string AppliedTo { get; }
        
        IDictionary<string, ulong> Definition { get; }
        
        int Priority { get; }
    }
}