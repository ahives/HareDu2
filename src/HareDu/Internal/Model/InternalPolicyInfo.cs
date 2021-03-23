namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using HareDu.Model;

    class InternalPolicyInfo :
        PolicyInfo
    {
        public InternalPolicyInfo(PolicyInfoImpl obj)
        {
            VirtualHost = obj.VirtualHost;
            Name = obj.Name;
            Pattern = obj.Pattern;
            AppliedTo = obj.AppliedTo;
            Definition = obj.Definition;
            Priority = obj.Priority;
        }

        public string VirtualHost { get; }
        public string Name { get; }
        public string Pattern { get; }
        public string AppliedTo { get; }
        public IDictionary<string, string> Definition { get; }
        public int Priority { get; }
    }
}