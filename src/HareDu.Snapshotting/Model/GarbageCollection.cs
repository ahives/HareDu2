namespace HareDu.Snapshotting.Model
{
    public interface GarbageCollection
    {
        CollectedGarbage ChannelsClosed { get; }
        
        CollectedGarbage ConnectionsClosed { get; }

        CollectedGarbage QueuesDeleted { get; }

        CollectedGarbage ReclaimedBytes { get; }
    }
}