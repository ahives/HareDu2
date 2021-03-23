namespace HareDu.Internal.Model
{
    using System;
    using System.Collections.Generic;
    using Core.Extensions;
    using HareDu.Model;

    class InternalQueueInfo :
        QueueInfo
    {
        public InternalQueueInfo(QueueInfoImpl obj)
        {
            MessageDetails = obj.MessageDetails.IsNotNull() ? new InternalRate(obj.MessageDetails) : default;
            TotalMessages = obj.TotalMessages;
            UnacknowledgedMessageDetails = obj.UnacknowledgedMessageDetails.IsNotNull() ? new InternalRate(obj.UnacknowledgedMessageDetails) : default;
            UnacknowledgedMessages = obj.UnacknowledgedMessages;
            ReadyMessageDetails = obj.ReadyMessageDetails.IsNotNull() ? new InternalRate(obj.ReadyMessageDetails) : default;
            ReadyMessages = obj.ReadyMessages;
            ReductionDetails = obj.ReductionDetails.IsNotNull() ? new InternalRate(obj.ReductionDetails) : default;
            TotalReductions = obj.TotalReductions;
            Arguments = obj.Arguments;
            Exclusive = obj.Exclusive;
            AutoDelete = obj.AutoDelete;
            Durable = obj.Durable;
            VirtualHost = obj.VirtualHost;
            Name = obj.Name;
            Node = obj.Node;
            MessageBytesPagedOut = obj.MessageBytesPagedOut;
            TotalMessagesPagedOut = obj.TotalMessagesPagedOut;
            BackingQueueStatus = obj.BackingQueueStatus.IsNotNull() ? new InternalBackingQueueStatus(obj.BackingQueueStatus) : default;
            HeadMessageTimestamp = obj.HeadMessageTimestamp;
            MessageBytesPersisted = obj.MessageBytesPersisted;
            MessageBytesInRAM = obj.MessageBytesInRAM;
            TotalBytesOfMessagesDeliveredButUnacknowledged = obj.TotalBytesOfMessagesDeliveredButUnacknowledged;
            TotalBytesOfMessagesReadyForDelivery = obj.TotalBytesOfMessagesReadyForDelivery;
            TotalBytesOfAllMessages = obj.TotalBytesOfAllMessages;
            MessagesPersisted = obj.MessagesPersisted;
            UnacknowledgedMessagesInRAM = obj.UnacknowledgedMessagesInRAM;
            MessagesReadyForDeliveryInRAM = obj.MessagesReadyForDeliveryInRAM;
            MessagesInRAM = obj.MessagesInRAM;
            GC = obj.GC.IsNotNull() ? new InternalGarbageCollectionDetails(obj.GC) : default;
            State = obj.State;
            RecoverableSlaves = obj.RecoverableSlaves;
            Consumers = obj.Consumers;
            ExclusiveConsumerTag = obj.ExclusiveConsumerTag;
            Policy = obj.Policy;
            ConsumerUtilization = obj.ConsumerUtilization;
            IdleSince = obj.IdleSince;
            Memory = obj.Memory;
            MessageStats = obj.MessageStats.IsNotNull() ? new InternalMessageStats(obj.MessageStats) : default;
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
        public string State { get; }
        public IList<string> RecoverableSlaves { get; }
        public ulong Consumers { get; }
        public string ExclusiveConsumerTag { get; }
        public string Policy { get; }
        public decimal ConsumerUtilization { get; }
        public DateTimeOffset IdleSince { get; }
        public ulong Memory { get; }
        public QueueMessageStats MessageStats { get; }
    }
}