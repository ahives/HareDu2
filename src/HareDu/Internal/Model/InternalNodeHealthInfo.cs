namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalNodeHealthInfo :
        NodeHealthInfo
    {
        public InternalNodeHealthInfo(NodeHealthInfoImpl obj)
        {
            Status = obj.Status;
            Reason = obj.Reason;
        }

        public NodeStatus Status { get; }
        public long Reason { get; }
    }
}