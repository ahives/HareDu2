namespace HareDu.Model
{
    using System.Collections.Generic;

    public interface PolicyInfo
    {
        string VirtualHost { get; }
        
        string Name { get; }
        
        string Pattern { get; }
        
        string AppliedTo { get; }

        IDictionary<string, string> Definition { get; }

        int Priority { get; }
    }
}