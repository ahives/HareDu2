namespace HareDu.Internal.Model
{
    using Core.Extensions;
    using HareDu.Model;

    class InternalOperationStatsImpl :
        ChannelOperationStats
    {
        public InternalOperationStatsImpl(ChannelOperationStatsImpl obj)
        {
            TotalMessagesPublished = obj.TotalMessagesPublished;
            MessagesPublishedDetails = obj.MessagesPublishedDetails.IsNotNull() ? new InternalRateImpl(obj.MessagesPublishedDetails) : default;
            TotalMessagesConfirmed = obj.TotalMessagesConfirmed;
            MessagesConfirmedDetails = obj.MessagesConfirmedDetails.IsNotNull() ? new InternalRateImpl(obj.MessagesConfirmedDetails) : default;
            TotalMessagesNotRouted = obj.TotalMessagesNotRouted;
            MessagesNotRoutedDetails = obj.MessagesNotRoutedDetails.IsNotNull() ? new InternalRateImpl(obj.MessagesNotRoutedDetails) : default;
            TotalMessageGets = obj.TotalMessageGets;
            MessageGetDetails = obj.MessageGetDetails.IsNotNull() ? new InternalRateImpl(obj.MessageGetDetails) : default;
            TotalMessageGetsWithoutAck = obj.TotalMessageGetsWithoutAck;
            MessageGetsWithoutAckDetails = obj.MessageGetsWithoutAckDetails.IsNotNull() ? new InternalRateImpl(obj.MessageGetsWithoutAckDetails) : default;
            TotalMessagesDelivered = obj.TotalMessagesDelivered;
            MessageDeliveryDetails = obj.MessageDeliveryDetails.IsNotNull() ? new InternalRateImpl(obj.MessageDeliveryDetails) : default;
            TotalMessageDeliveredWithoutAck = obj.TotalMessageDeliveredWithoutAck;
            MessagesDeliveredWithoutAckDetails = obj.MessagesDeliveredWithoutAckDetails.IsNotNull() ? new InternalRateImpl(obj.MessagesDeliveredWithoutAckDetails) : default;
            TotalMessageDeliveryGets = obj.TotalMessageDeliveryGets;
            MessageDeliveryGetDetails = obj.MessageDeliveryGetDetails.IsNotNull() ? new InternalRateImpl(obj.MessageDeliveryGetDetails) : default;
            TotalMessagesRedelivered = obj.TotalMessagesRedelivered;
            MessagesRedeliveredDetails = obj.MessagesRedeliveredDetails.IsNotNull() ? new InternalRateImpl(obj.MessagesRedeliveredDetails) : default;
            TotalMessagesAcknowledged = obj.TotalMessagesAcknowledged;
            MessagesAcknowledgedDetails = obj.MessagesAcknowledgedDetails.IsNotNull() ? new InternalRateImpl(obj.MessagesAcknowledgedDetails) : default;
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