namespace HareDu.Model
{
    public interface ChurnRates
    {
        ulong TotalChannelsClosed { get; }
        
        Rate ClosedChannelDetails { get; }

        ulong TotalChannelsCreated { get; }
        
        Rate CreatedChannelDetails { get; }

        ulong TotalConnectionsClosed { get; }
        
        Rate ClosedConnectionDetails { get; }

        ulong TotalConnectionsCreated { get; }
        
        Rate CreatedConnectionDetails { get; }

        ulong TotalQueuesCreated { get; }
        
        Rate CreatedQueueDetails { get; }

        ulong TotalQueuesDeclared { get; }
        
        Rate DeclaredQueueDetails { get; }

        ulong TotalQueuesDeleted { get; }
        
        Rate DeletedQueueDetails { get; }
    }
}