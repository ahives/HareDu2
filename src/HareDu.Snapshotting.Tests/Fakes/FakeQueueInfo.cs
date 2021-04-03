namespace HareDu.Snapshotting.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using HareDu.Model;

    public class FakeQueueInfo :
        QueueInfo
    {
        public FakeQueueInfo()
        {
            TotalMessages = 7823668;
            UnacknowledgedMessages = 7273020;
            ReadyMessages = 9293093;
            TotalReductions = 992039;
            Name = "Queue 1";
            MessageBytesPagedOut = 239939803;
            TotalMessagesPagedOut = 90290398;
            MessageBytesInRAM = 992390933;
            TotalBytesOfMessagesDeliveredButUnacknowledged = 82830892;
            TotalBytesOfMessagesReadyForDelivery = 892839823;
            TotalBytesOfAllMessages = 82938938723;
            UnacknowledgedMessagesInRAM = 82938982323;
            MessagesReadyForDeliveryInRAM = 8892388929;
            MessagesInRAM = 9883892938;
            Consumers = 773709938;
            ConsumerUtilization = 0.50M;
            Memory = 92990390;
            MessageStats = new QueueMessageStatsImpl();
        }

        public Rate MessageDetails { get; }
        public ulong TotalMessages { get; }
        public Rate UnacknowledgedMessageDetails { get; }
        public ulong UnacknowledgedMessages { get; }
        public Rate ReadyMessageDetails { get; }
        public ulong ReadyMessages { get; }
        public Rate ReductionDetails { get; }
        public long TotalReductions { get; }
        public IDictionary<string, object> Arguments { get; }
        public bool Exclusive { get; }
        public bool AutoDelete { get; }
        public bool Durable { get; }
        public string VirtualHost { get; }
        public string Name { get; }
        public string Node { get; }
        public ulong MessageBytesPagedOut { get; }
        public ulong TotalMessagesPagedOut { get; }
        public BackingQueueStatus BackingQueueStatus { get; }
        public DateTimeOffset HeadMessageTimestamp { get; }
        public ulong MessageBytesPersisted { get; }
        public ulong MessageBytesInRAM { get; }
        public ulong TotalBytesOfMessagesDeliveredButUnacknowledged { get; }
        public ulong TotalBytesOfMessagesReadyForDelivery { get; }
        public ulong TotalBytesOfAllMessages { get; }
        public ulong MessagesPersisted { get; }
        public ulong UnacknowledgedMessagesInRAM { get; }
        public ulong MessagesReadyForDeliveryInRAM { get; }
        public ulong MessagesInRAM { get; }
        public GarbageCollectionDetails GC { get; }
        public QueueState State { get; }
        public IList<string> RecoverableSlaves { get; }
        public ulong Consumers { get; }
        public string ExclusiveConsumerTag { get; }
        public string Policy { get; }
        public decimal ConsumerUtilization { get; }
        public DateTimeOffset IdleSince { get; }
        public ulong Memory { get; }
        public QueueMessageStats MessageStats { get; }

        
        class QueueMessageStatsImpl :
            QueueMessageStats
        {
            public QueueMessageStatsImpl()
            {
                TotalMessagesPublished = 763928923;
                TotalMessageGets = 82938820903;
                TotalMessageGetsWithoutAck = 23997979383;
                TotalMessagesDelivered = 238847970;
                TotalMessageDeliveredWithoutAck = 48898693232;
                TotalMessageDeliveryGets = 50092830929;
                TotalMessagesRedelivered = 488983002934;
                TotalMessagesAcknowledged = 92303949398;
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
}