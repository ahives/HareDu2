// Copyright 2013-2019 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Snapshotting.Tests.Fakes
{
    using System.Collections.Generic;
    using HareDu.Model;

    public class FakeClusterInfoImpl :
        ClusterInfo
    {
        public FakeClusterInfoImpl()
        {
            RabbitMqVersion = "3.7.18";
            ErlangVersion = "22.1";
            MessageStats = new MessageStatsImpl();
            ClusterName = "fake_cluster";
            MessageStats = new MessageStatsImpl();
        }

        public string ManagementVersion { get; }
        public string RatesMode { get; }
        public IList<ExchangeType> ExchangeTypes { get; }
        public string RabbitMqVersion { get; }
        public string ClusterName { get; }
        public string ErlangVersion { get; }
        public string ErlangFullVersion { get; }
        public MessageStats MessageStats { get; }
        public ChurnRates ChurnRates { get; }
        public QueueStats QueueStats { get; }
        public ClusterObjectTotals ObjectTotals { get; }
        public long StatsDatabaseEventQueue { get; }
        public string Node { get; }
        public IList<Listener> Listeners { get; }
        public IList<NodeContext> Contexts { get; }


        class MessageStatsImpl :
            MessageStats
        {
            public MessageStatsImpl()
            {
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
                MessagesPublishedDetails = new MessagesPublishedDetailsImpl(7);
                UnroutableMessagesDetails = new UnroutableMessagesDetailsImpl(48);
                MessageGetDetails = new MessageGetDetailsImpl(324);
                MessageGetsWithoutAckDetails = new MessageGetsWithoutAckDetailsImpl(84);
                MessageDeliveryDetails = new MessageDeliveryDetailsImpl(84);
                MessagesDeliveredWithoutAckDetails = new MessagesDeliveredWithoutAckDetailsImpl(56);
                MessagesRedeliveredDetails = new MessagesRedeliveredDetailsImpl(89);
                MessagesAcknowledgedDetails = new MessagesAcknowledgedDetailsImpl(723);
                MessageDeliveryGetDetails = new MessageDeliveryGetDetailsImpl(738);
                MessagesConfirmedDetails = new MessagesConfirmedDetailsImpl(7293);
            }

            public ulong TotalMessagesPublished { get; }
            public MessagesPublishedDetails MessagesPublishedDetails { get; }
            public long TotalMessagesConfirmed { get; }
            public MessagesConfirmedDetails MessagesConfirmedDetails { get; }
            public ulong TotalUnroutableMessages { get; }
            public UnroutableMessagesDetails UnroutableMessagesDetails { get; }
            public ulong TotalDiskReads { get; }
            public DiskReadDetails DiskReadDetails { get; }
            public ulong TotalDiskWrites { get; }
            public DiskWriteDetails DiskWriteDetails { get; }
            public ulong TotalMessageGets { get; }
            public MessageGetDetails MessageGetDetails { get; }
            public ulong TotalMessageGetsWithoutAck { get; }
            public MessageGetsWithoutAckDetails MessageGetsWithoutAckDetails { get; }
            public ulong TotalMessagesDelivered { get; }
            public MessageDeliveryDetails MessageDeliveryDetails { get; }
            public ulong TotalMessageDeliveredWithoutAck { get; }
            public MessagesDeliveredWithoutAckDetails MessagesDeliveredWithoutAckDetails { get; }
            public ulong TotalMessagesRedelivered { get; }
            public MessagesRedeliveredDetails MessagesRedeliveredDetails { get; }
            public ulong TotalMessagesAcknowledged { get; }
            public MessagesAcknowledgedDetails MessagesAcknowledgedDetails { get; }
            public ulong TotalMessageDeliveryGets { get; }
            public MessageDeliveryGetDetails MessageDeliveryGetDetails { get; }

                
            class MessageGetDetailsImpl :
                MessageGetDetails
            {
                public MessageGetDetailsImpl(decimal rate)
                {
                    Rate = rate;
                }

                public decimal Rate { get; }
            }

                
            class MessagesConfirmedDetailsImpl :
                MessagesConfirmedDetails
            {
                public MessagesConfirmedDetailsImpl(decimal rate)
                {
                    Rate = rate;
                }

                public decimal Rate { get; }
            }

                
            class UnroutableMessagesDetailsImpl :
                UnroutableMessagesDetails
            {
                public UnroutableMessagesDetailsImpl(decimal rate)
                {
                    Rate = rate;
                }

                public decimal Rate { get; }
            }

                
            class MessagesPublishedDetailsImpl :
                MessagesPublishedDetails
            {
                public MessagesPublishedDetailsImpl(decimal rate)
                {
                    Rate = rate;
                }

                public decimal Rate { get; }
            }
                
                
            class MessageDeliveryGetDetailsImpl :
                MessageDeliveryGetDetails
            {
                public MessageDeliveryGetDetailsImpl(decimal rate)
                {
                    Rate = rate;
                }

                public decimal Rate { get; }
            }
                
                
            class MessagesAcknowledgedDetailsImpl :
                MessagesAcknowledgedDetails
            {
                public MessagesAcknowledgedDetailsImpl(decimal rate)
                {
                    Rate = rate;
                }

                public decimal Rate { get; }
            }
                
                
            class MessagesRedeliveredDetailsImpl :
                MessagesRedeliveredDetails
            {
                public MessagesRedeliveredDetailsImpl(decimal rate)
                {
                    Rate = rate;
                }

                public decimal Rate { get; }
            }
                
                
            class MessagesDeliveredWithoutAckDetailsImpl :
                MessagesDeliveredWithoutAckDetails
            {
                public MessagesDeliveredWithoutAckDetailsImpl(decimal rate)
                {
                    Rate = rate;
                }

                public decimal Rate { get; }
            }
                
                
            class MessageDeliveryDetailsImpl :
                MessageDeliveryDetails
            {
                public MessageDeliveryDetailsImpl(decimal rate)
                {
                    Rate = rate;
                }

                public decimal Rate { get; }
            }
                
                
            class MessageGetsWithoutAckDetailsImpl :
                MessageGetsWithoutAckDetails
            {
                public MessageGetsWithoutAckDetailsImpl(decimal rate)
                {
                    Rate = rate;
                }

                public decimal Rate { get; }
            }
        }
    }
}