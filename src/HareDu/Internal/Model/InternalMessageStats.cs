namespace HareDu.Internal.Model
{
    using Core.Extensions;
    using HareDu.Model;

    class InternalMessageStats :
        MessageStats
    {
        public InternalMessageStats(MessageStatsImpl obj)
        {
            TotalMessagesPublished = obj.TotalMessagesPublished;
            TotalMessagesConfirmed = obj.TotalMessagesConfirmed;
            TotalUnroutableMessages = obj.TotalUnroutableMessages;
            TotalDiskReads = obj.TotalDiskReads;
            TotalDiskWrites = obj.TotalDiskWrites;
            TotalMessageGets = obj.TotalMessageGets;
            TotalMessageGetsWithoutAck = obj.TotalMessageGetsWithoutAck;
            TotalMessagesDelivered = obj.TotalMessagesDelivered;
            TotalMessageDeliveredWithoutAck = obj.TotalMessageDeliveredWithoutAck;
            TotalMessagesRedelivered = obj.TotalMessagesRedelivered;
            TotalMessagesAcknowledged = obj.TotalMessagesAcknowledged;
            TotalMessageDeliveryGets = obj.TotalMessageDeliveryGets;
            MessagesPublishedDetails = obj.MessagesPublishedDetails.IsNotNull() ? new InternalRate(obj.MessagesPublishedDetails) : default;
            MessagesConfirmedDetails = obj.MessagesConfirmedDetails.IsNotNull() ? new InternalRate(obj.MessagesConfirmedDetails) : default;
            UnroutableMessagesDetails = obj.UnroutableMessagesDetails.IsNotNull() ? new InternalRate(obj.UnroutableMessagesDetails) : default;
            DiskReadDetails = obj.DiskReadDetails.IsNotNull() ? new InternalRate(obj.DiskReadDetails) : default;
            DiskWriteDetails = obj.DiskWriteDetails.IsNotNull() ? new InternalRate(obj.DiskWriteDetails) : default;
            MessageGetDetails = obj.MessageGetDetails.IsNotNull() ? new InternalRate(obj.MessageGetDetails) : default;
            MessageGetsWithoutAckDetails = obj.MessageGetsWithoutAckDetails.IsNotNull() ? new InternalRate(obj.MessageGetsWithoutAckDetails) : default;
            MessageDeliveryDetails = obj.MessageDeliveryDetails.IsNotNull() ? new InternalRate(obj.MessageDeliveryDetails) : default;
            MessagesDeliveredWithoutAckDetails = obj.MessagesDeliveredWithoutAckDetails.IsNotNull() ? new InternalRate(obj.MessagesDeliveredWithoutAckDetails) : default;
            MessagesRedeliveredDetails = obj.MessagesRedeliveredDetails.IsNotNull() ? new InternalRate(obj.MessagesRedeliveredDetails) : default;
            MessagesAcknowledgedDetails = obj.MessagesAcknowledgedDetails.IsNotNull() ? new InternalRate(obj.MessagesAcknowledgedDetails) : default;
            MessageDeliveryGetDetails = obj.MessageDeliveryGetDetails.IsNotNull() ? new InternalRate(obj.MessageDeliveryGetDetails) : default;
        }

        public ulong TotalMessagesPublished { get; }
        public Rate MessagesPublishedDetails { get; }
        public ulong TotalMessagesConfirmed { get; }
        public Rate MessagesConfirmedDetails { get; }
        public ulong TotalUnroutableMessages { get; }
        public Rate UnroutableMessagesDetails { get; }
        public ulong TotalDiskReads { get; }
        public Rate DiskReadDetails { get; }
        public ulong TotalDiskWrites { get; }
        public Rate DiskWriteDetails { get; }
        public ulong TotalMessageGets { get; }
        public Rate MessageGetDetails { get; }
        public ulong TotalMessageGetsWithoutAck { get; }
        public Rate MessageGetsWithoutAckDetails { get; }
        public ulong TotalMessagesDelivered { get; }
        public Rate MessageDeliveryDetails { get; }
        public ulong TotalMessageDeliveredWithoutAck { get; }
        public Rate MessagesDeliveredWithoutAckDetails { get; }
        public ulong TotalMessagesRedelivered { get; }
        public Rate MessagesRedeliveredDetails { get; }
        public ulong TotalMessagesAcknowledged { get; }
        public Rate MessagesAcknowledgedDetails { get; }
        public ulong TotalMessageDeliveryGets { get; }
        public Rate MessageDeliveryGetDetails { get; }
    }
}