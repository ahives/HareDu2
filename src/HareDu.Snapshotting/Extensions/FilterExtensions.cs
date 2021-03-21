namespace HareDu.Snapshotting.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using HareDu.Model;
    using Model;

    public static class FilterExtensions
    {
        public static IReadOnlyList<ChannelSnapshot> FilterByConnection(this IReadOnlyList<ChannelInfo> channels, string connection)
        {
            if (channels == null || !channels.Any())
                return new List<ChannelSnapshot>();

            return channels
                .Where(x => x.ConnectionDetails?.Name == connection)
                .Select(x => new ChannelSnapshotImpl(x, connection))
                .Cast<ChannelSnapshot>()
                .ToList();
        }

        public static IEnumerable<ConnectionInfo> FilterByNode(this IReadOnlyList<ConnectionInfo> connections, string node)
        {
            if (connections == null || !connections.Any())
                return Enumerable.Empty<ConnectionInfo>();

            return connections.Where(x => x.Node == node);
        }
        
        
        class ChannelSnapshotImpl :
            ChannelSnapshot
        {
            public ChannelSnapshotImpl(ChannelInfo channel, string connection)
            {
                Identifier = channel.Name;
                ConnectionIdentifier = connection;
                Consumers = channel.TotalConsumers;
                Node = channel.Node;
                PrefetchCount = channel.PrefetchCount;
                UncommittedAcknowledgements = channel.UncommittedAcknowledgements;
                UncommittedMessages = channel.UncommittedMessages;
                UnconfirmedMessages = channel.UnconfirmedMessages;
                UnacknowledgedMessages = channel.UnacknowledgedMessages;
                QueueOperations = new QueueOperationMetricsImpl(channel);
            }

            public uint PrefetchCount { get; }
            public ulong UncommittedAcknowledgements { get; }
            public ulong UncommittedMessages { get; }
            public ulong UnconfirmedMessages { get; }
            public ulong UnacknowledgedMessages { get; }
            public ulong Consumers { get; }
            public string Identifier { get; }
            public string ConnectionIdentifier { get; }
            public string Node { get; }
            public QueueOperationMetrics QueueOperations { get; }

            
            class QueueOperationMetricsImpl :
                QueueOperationMetrics
            {
                public QueueOperationMetricsImpl(ChannelInfo channel)
                {
                    Incoming = new QueueOperationImpl(
                        channel.OperationStats?.TotalMessagesPublished ?? 0,
                        channel.OperationStats?.MessagesPublishedDetails?.Value ?? 0.0M);
                    Gets = new QueueOperationImpl(
                        channel.OperationStats?.TotalMessageGets ?? 0,
                        channel.OperationStats?.MessageGetDetails?.Value ?? 0.0M);
                    GetsWithoutAck = new QueueOperationImpl(
                        channel.OperationStats?.TotalMessageGetsWithoutAck ?? 0,
                        channel.OperationStats?.MessageGetsWithoutAckDetails?.Value ?? 0.0M);
                    Delivered = new QueueOperationImpl(
                        channel.OperationStats?.TotalMessagesDelivered ?? 0,
                        channel.OperationStats?.MessageDeliveryDetails?.Value ?? 0.0M);
                    DeliveredWithoutAck = new QueueOperationImpl(
                        channel.OperationStats?.TotalMessageDeliveredWithoutAck ?? 0,
                        channel.OperationStats?.MessagesDeliveredWithoutAckDetails?.Value ?? 0.0M);
                    DeliveredGets = new QueueOperationImpl(
                        channel.OperationStats?.TotalMessageDeliveryGets ?? 0,
                        channel.OperationStats?.MessageDeliveryGetDetails?.Value ?? 0.0M);
                    Redelivered = new QueueOperationImpl(
                        channel.OperationStats?.TotalMessagesRedelivered ?? 0,
                        channel.OperationStats?.MessagesRedeliveredDetails?.Value ?? 0.0M);
                    Acknowledged = new QueueOperationImpl(
                        channel.OperationStats?.TotalMessagesAcknowledged ?? 0,
                        channel.OperationStats?.MessagesAcknowledgedDetails?.Value ?? 0.0M);
                    NotRouted = new QueueOperationImpl(
                        channel.OperationStats?.TotalMessagesNotRouted ?? 0,
                        channel.OperationStats?.MessagesNotRoutedDetails?.Value ?? 0.0M);
                }

                public QueueOperation Incoming { get; }
                public QueueOperation Gets { get; }
                public QueueOperation GetsWithoutAck { get; }
                public QueueOperation Delivered { get; }
                public QueueOperation DeliveredWithoutAck { get; }
                public QueueOperation DeliveredGets { get; }
                public QueueOperation Redelivered { get; }
                public QueueOperation Acknowledged { get; }
                public QueueOperation NotRouted { get; }

                
                class QueueOperationImpl :
                    QueueOperation
                {
                    public QueueOperationImpl(ulong total, decimal rate)
                    {
                        Total = total;
                        Rate = rate;
                    }

                    public ulong Total { get; }
                    public decimal Rate { get; }
                }
            }
        }
    }
}