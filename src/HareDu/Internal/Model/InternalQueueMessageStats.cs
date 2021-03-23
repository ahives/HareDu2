namespace HareDu.Internal.Model
{
    using Core.Extensions;
    using HareDu.Model;

    class InternalQueueMessageStats :
        QueueMessageStats
    {
        public InternalQueueMessageStats(QueueMessageStatsImpl obj)
        {
            TotalMessagesPublished = obj.TotalMessagesPublished;
            MessagesPublishedDetails = obj.MessagesPublishedDetails.IsNotNull() ? new InternalRate(obj.MessagesPublishedDetails) : default;
            TotalMessageGets = obj.TotalMessageGets;
            MessageGetDetails = obj.MessageGetDetails.IsNotNull() ? new InternalRate(obj.MessageGetDetails) : default;
            TotalMessageGetsWithoutAck = obj.TotalMessageGetsWithoutAck;
            MessageGetsWithoutAckDetails = obj.MessageGetsWithoutAckDetails.IsNotNull() ? new InternalRate(obj.MessageGetsWithoutAckDetails) : default;
            TotalMessagesDelivered = obj.TotalMessagesDelivered;
            MessageDeliveryDetails = obj.MessageDeliveryDetails.IsNotNull() ? new InternalRate(obj.MessageDeliveryDetails) : default;
            TotalMessageDeliveredWithoutAck = obj.TotalMessageDeliveredWithoutAck;
            MessagesDeliveredWithoutAckDetails = obj.MessagesDeliveredWithoutAckDetails.IsNotNull() ? new InternalRate(obj.MessagesDeliveredWithoutAckDetails) : default;
            TotalMessageDeliveryGets = obj.TotalMessageDeliveryGets;
            MessageDeliveryGetDetails = obj.MessageDeliveryGetDetails.IsNotNull() ? new InternalRate(obj.MessageDeliveryGetDetails) : default;
            TotalMessagesRedelivered = obj.TotalMessagesRedelivered;
            MessagesRedeliveredDetails = obj.MessagesRedeliveredDetails.IsNotNull() ? new InternalRate(obj.MessagesRedeliveredDetails) : default;
            TotalMessagesAcknowledged = obj.TotalMessagesAcknowledged;
            MessagesAcknowledgedDetails = obj.MessagesAcknowledgedDetails.IsNotNull() ? new InternalRate(obj.MessagesAcknowledgedDetails) : default;
        }

        public ulong TotalMessagesPublished { get; }
        public Rate MessagesPublishedDetails { get; }
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