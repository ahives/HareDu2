namespace HareDu.Model
{
    public interface ChannelOperationStats
    {
        ulong TotalMessagesPublished { get; }

        Rate MessagesPublishedDetails { get; }
        
        ulong TotalMessagesConfirmed { get; }

        Rate MessagesConfirmedDetails { get; }
        
        ulong TotalMessagesNotRouted { get; }

        Rate MessagesNotRoutedDetails { get; }
        
        ulong TotalMessageGets { get; }

        Rate MessageGetDetails { get; }
        
        ulong TotalMessageGetsWithoutAck { get; }

        Rate MessageGetsWithoutAckDetails { get; }
        
        ulong TotalMessagesDelivered { get; }

        Rate MessageDeliveryDetails { get; }
        
        ulong TotalMessageDeliveredWithoutAck { get; }

        Rate MessagesDeliveredWithoutAckDetails { get; }
        
        ulong TotalMessageDeliveryGets { get; }

        Rate MessageDeliveryGetDetails { get; }
        
        ulong TotalMessagesRedelivered { get; }

        Rate MessagesRedeliveredDetails { get; }
        
        ulong TotalMessagesAcknowledged { get; }

        Rate MessagesAcknowledgedDetails { get; }
    }
}