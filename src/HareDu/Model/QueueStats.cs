namespace HareDu.Model
{
    public interface QueueStats
    {
        ulong TotalMessagesReadyForDelivery { get; }

        Rate MessagesReadyForDeliveryDetails { get; }
        
        ulong TotalUnacknowledgedDeliveredMessages { get; }

        Rate UnacknowledgedDeliveredMessagesDetails { get; }
        
        ulong TotalMessages { get; }

        Rate MessageDetails { get; }
    }
}