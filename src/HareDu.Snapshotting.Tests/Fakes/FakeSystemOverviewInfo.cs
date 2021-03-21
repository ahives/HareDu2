namespace HareDu.Snapshotting.Tests.Fakes
{
    using System.Collections.Generic;
    using HareDu.Model;

    public class FakeSystemOverviewInfo :
        SystemOverviewInfo
    {
        public FakeSystemOverviewInfo()
        {
            RabbitMqVersion = "3.7.18";
            ErlangVersion = "22.1";
            MessageStats = new MessageStatsImpl();
            ClusterName = "fake_cluster";
            MessageStats = new MessageStatsImpl();
            QueueStats = new QueueStatsImpl();
        }

        public string ManagementVersion { get; }
        public string RatesMode { get; }
        public SampleRetentionPolicies SampleRetentionPolicies { get; }
        public IList<ExchangeType> ExchangeTypes { get; }
        public string ProductVersion { get; }
        public string ProductName { get; }
        public string RabbitMqVersion { get; }
        public string ClusterName { get; }
        public string ErlangVersion { get; }
        public string ErlangFullVersion { get; }
        public bool DisableStats { get; }
        public bool EnableQueueTotals { get; }
        public MessageStats MessageStats { get; }
        public ChurnRates ChurnRates { get; }
        public QueueStats QueueStats { get; }
        public ClusterObjectTotals ObjectTotals { get; }
        public ulong StatsDatabaseEventQueue { get; }
        public string Node { get; }
        public IList<Listener> Listeners { get; }
        public IList<NodeContext> Contexts { get; }

        
        class QueueStatsImpl :
            QueueStats
        {
            public QueueStatsImpl()
            {
                TotalMessagesReadyForDelivery = 82937489379;
                MessagesReadyForDeliveryDetails = new RateImpl(34.4M);
                TotalUnacknowledgedDeliveredMessages = 892387397238;
                UnacknowledgedDeliveredMessagesDetails = new RateImpl(73.3M);
                TotalMessages = 9230748297;
                MessageDetails = new RateImpl(80.3M);
            }

            public ulong TotalMessagesReadyForDelivery { get; }
            public Rate MessagesReadyForDeliveryDetails { get; }
            public ulong TotalUnacknowledgedDeliveredMessages { get; }
            public Rate UnacknowledgedDeliveredMessagesDetails { get; }
            public ulong TotalMessages { get; }
            public Rate MessageDetails { get; }

            
            class RateImpl :
                Rate
            {
                public RateImpl(decimal rate)
                {
                    Value = rate;
                }

                public decimal Value { get; }
            }
        }


        class MessageStatsImpl :
            MessageStats
        {
            public MessageStatsImpl()
            {
                TotalMessagesAcknowledged = 7287736;
                TotalMessageDeliveryGets = 78263767;
                TotalMessagesPublished = 1234;
                TotalMessagesConfirmed = 83;
                TotalUnroutableMessages = 737;
                TotalDiskReads = 83;
                TotalMessageGets = 723;
                TotalMessageGetsWithoutAck = 373;
                TotalMessagesDelivered = 7234;
                TotalMessagesRedelivered = 7237;
                TotalMessageDeliveredWithoutAck = 8723;
                TotalMessagesRedelivered = 838;
                MessagesPublishedDetails = new RateImpl(7);
                UnroutableMessagesDetails = new RateImpl(48);
                MessageGetDetails = new RateImpl(324);
                MessageGetsWithoutAckDetails = new RateImpl(84);
                MessageDeliveryDetails = new RateImpl(84);
                MessagesDeliveredWithoutAckDetails = new RateImpl(56);
                MessagesRedeliveredDetails = new RateImpl(89);
                MessagesAcknowledgedDetails = new RateImpl(723);
                MessageDeliveryGetDetails = new RateImpl(738);
                MessagesConfirmedDetails = new RateImpl(7293);
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


            class RateImpl :
                Rate
            {
                public RateImpl(decimal rate)
                {
                    Value = rate;
                }

                public decimal Value { get; }
            }
        }
    }
}