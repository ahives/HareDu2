namespace HareDu.Internal.Model
{
    using Core.Extensions;
    using HareDu.Model;

    class InternalOperationStatsImpl :
        ChannelOperationStats
    {
        public InternalOperationStatsImpl(ChannelOperationStatsImpl item)
        {
            TotalMessagesPublished = item.TotalMessagesPublished;
            MessagesPublishedDetails = item.MessagesPublishedDetails.IsNotNull() ? new InternalRateImpl(item.MessagesPublishedDetails) : default;
            TotalMessagesConfirmed = item.TotalMessagesConfirmed;
            MessagesConfirmedDetails = item.MessagesConfirmedDetails.IsNotNull() ? new InternalRateImpl(item.MessagesConfirmedDetails) : default;
            TotalMessagesNotRouted = item.TotalMessagesNotRouted;
            MessagesNotRoutedDetails = item.MessagesNotRoutedDetails.IsNotNull() ? new InternalRateImpl(item.MessagesNotRoutedDetails) : default;
            TotalMessageGets = item.TotalMessageGets;
            MessageGetDetails = item.MessageGetDetails.IsNotNull() ? new InternalRateImpl(item.MessageGetDetails) : default;
            TotalMessageGetsWithoutAck = item.TotalMessageGetsWithoutAck;
            MessageGetsWithoutAckDetails = item.MessageGetsWithoutAckDetails.IsNotNull() ? new InternalRateImpl(item.MessageGetsWithoutAckDetails) : default;
            TotalMessagesDelivered = item.TotalMessagesDelivered;
            MessageDeliveryDetails = item.MessageDeliveryDetails.IsNotNull() ? new InternalRateImpl(item.MessageDeliveryDetails) : default;
            TotalMessageDeliveredWithoutAck = item.TotalMessageDeliveredWithoutAck;
            MessagesDeliveredWithoutAckDetails = item.MessagesDeliveredWithoutAckDetails.IsNotNull() ? new InternalRateImpl(item.MessagesDeliveredWithoutAckDetails) : default;
            TotalMessageDeliveryGets = item.TotalMessageDeliveryGets;
            MessageDeliveryGetDetails = item.MessageDeliveryGetDetails.IsNotNull() ? new InternalRateImpl(item.MessageDeliveryGetDetails) : default;
            TotalMessagesRedelivered = item.TotalMessagesRedelivered;
            MessagesRedeliveredDetails = item.MessagesRedeliveredDetails.IsNotNull() ? new InternalRateImpl(item.MessagesRedeliveredDetails) : default;
            TotalMessagesAcknowledged = item.TotalMessagesAcknowledged;
            MessagesAcknowledgedDetails = item.MessagesAcknowledgedDetails.IsNotNull() ? new InternalRateImpl(item.MessagesAcknowledgedDetails) : default;
        }

        public ulong TotalMessagesPublished { get; }
        public Rate MessagesPublishedDetails { get; }
        public ulong TotalMessagesConfirmed { get; }
        public Rate MessagesConfirmedDetails { get; }
        public ulong TotalMessagesNotRouted { get; }
        public Rate MessagesNotRoutedDetails { get; }
        public ulong TotalMessageGets { get; }
        public Rate MessageGetDetails { get; }
        public ulong TotalMessageGetsWithoutAck { get; }
        public Rate MessageGetsWithoutAckDetails { get; }
        public ulong TotalMessagesDelivered { get; }
        public Rate MessageDeliveryDetails { get; }
        public ulong TotalMessageDeliveredWithoutAck { get; }
        public Rate MessagesDeliveredWithoutAckDetails { get; }
        public ulong TotalMessageDeliveryGets { get; }
        public Rate MessageDeliveryGetDetails { get; }
        public ulong TotalMessagesRedelivered { get; }
        public Rate MessagesRedeliveredDetails { get; }
        public ulong TotalMessagesAcknowledged { get; }
        public Rate MessagesAcknowledgedDetails { get; }
    }
}