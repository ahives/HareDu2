namespace HareDu.Snapshotting.Model
{
    public interface NetworkTrafficSnapshot :
        Snapshot
    {
        ulong MaxFrameSize { get; }

        Packets Sent { get; }
        
        Packets Received { get; }
    }
}