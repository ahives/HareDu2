namespace HareDu.Snapshotting.Model
{
    public interface ChannelSnapshot :
        Snapshot
    {
        uint PrefetchCount { get; }

        ulong UncommittedAcknowledgements { get; }

        ulong UncommittedMessages { get; }

        ulong UnconfirmedMessages { get; }

        ulong UnacknowledgedMessages { get; }

        ulong Consumers { get; }

        string Identifier { get; }
        
        string ConnectionIdentifier { get; }
        
        string Node { get; }
        
        QueueOperationMetrics QueueOperations { get; }
    }
}