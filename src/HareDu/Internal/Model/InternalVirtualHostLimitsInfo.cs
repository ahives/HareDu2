namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using HareDu.Model;

    class InternalVirtualHostLimitsInfo :
        VirtualHostLimitsInfo
    {
        public InternalVirtualHostLimitsInfo(VirtualHostLimitsInfoImpl obj)
        {
            VirtualHostName = obj.VirtualHostName;
            Limits = obj.Limits;
        }

        public string VirtualHostName { get; }
        public IDictionary<string, ulong> Limits { get; }
    }
}