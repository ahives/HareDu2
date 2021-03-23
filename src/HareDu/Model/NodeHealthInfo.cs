namespace HareDu.Model
{
    public interface NodeHealthInfo
    {
        NodeStatus Status { get; }

        long Reason { get; }
    }
}