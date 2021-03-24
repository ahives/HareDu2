namespace HareDu.Internal.Model
{
    using Core.Extensions;
    using HareDu.Model;

    class InternalQueueStats :
        QueueStats
    {
        public InternalQueueStats(QueueStatsImpl obj)
        {
            TotalMessagesReadyForDelivery = obj.TotalMessagesReadyForDelivery;
            TotalUnacknowledgedDeliveredMessages = obj.TotalUnacknowledgedDeliveredMessages;
            TotalMessages = obj.TotalMessages;
            MessagesReadyForDeliveryDetails = obj.MessagesReadyForDeliveryDetails.IsNotNull() ? new InternalRate(obj.MessagesReadyForDeliveryDetails) : default;
            UnacknowledgedDeliveredMessagesDetails = obj.UnacknowledgedDeliveredMessagesDetails.IsNotNull() ? new InternalRate(obj.UnacknowledgedDeliveredMessagesDetails) : default;
            MessageDetails = obj.MessageDetails.IsNotNull() ? new InternalRate(obj.MessageDetails) : default;
        }

        public ulong TotalMessagesReadyForDelivery { get; }
        public Rate MessagesReadyForDeliveryDetails { get; }
        public ulong TotalUnacknowledgedDeliveredMessages { get; }
        public Rate UnacknowledgedDeliveredMessagesDetails { get; }
        public ulong TotalMessages { get; }
        public Rate MessageDetails { get; }
    }
}