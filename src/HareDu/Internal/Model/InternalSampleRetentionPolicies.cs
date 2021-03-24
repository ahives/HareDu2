namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using HareDu.Model;

    class InternalSampleRetentionPolicies :
        SampleRetentionPolicies
    {
        public InternalSampleRetentionPolicies(SampleRetentionPoliciesImpl obj)
        {
            Global = obj.Global;
            Basic = obj.Basic;
            Detailed = obj.Detailed;
        }

        public IList<ulong> Global { get; }
        public IList<ulong> Basic { get; }
        public IList<ulong> Detailed { get; }
    }
}