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
namespace HareDu.Model
{
    using System.Collections.Generic;
    using Core.Extensions;
    using Core.Model;

    class ClusterStatusImpl :
        ClusterStatus
    {
        public ClusterStatusImpl(ClusterInfo cluster, IReadOnlyList<NodeInfo> clusterNodes, IReadOnlyList<ConnectionInfo> connections, IReadOnlyList<ChannelInfo> channels)
        {
            ClusterName = cluster.ClusterName;
            ErlangVerion = cluster.ErlangVerion;
            RabbitMqVersion = cluster.RabbitMqVersion;
            Queue = new QueueImpl(cluster.MessageStats);
            IO = new IOImpl(cluster.MessageStats);
        }


        class IOImpl :
            IO
        {
            public IOImpl(MessageStats stats)
            {
                Reads = new DiskDetailsImpl(stats.TotalDiskReads, stats.DiskReadDetails.IsNull() ? 0 : stats.DiskReadDetails.Rate);
                Writes = new DiskDetailsImpl(stats.TotalDiskWrites, stats.DiskWriteDetails.IsNull() ? 0 : stats.DiskWriteDetails.Rate);
            }

            public DiskDetails Reads { get; }
            public DiskDetails Writes { get; }

            
            class DiskDetailsImpl :
                DiskDetails
            {
                public DiskDetailsImpl(long total, decimal rate)
                {
                    Total = total;
                    Rate = rate;
                }

                public long Total { get; }
                public decimal Rate { get; }
            }
        }


        class QueueImpl :
            Queue
        {
            public QueueImpl(MessageStats stats)
            {
                Published = new MessageDetailsImpl(stats.TotalMessagesPublished, stats.MessagesPublishedDetails.IsNull() ? 0 : stats.MessagesPublishedDetails.Rate);
                Confirmed = new MessageDetailsImpl(stats.TotalMessagesConfirmed, stats.MessagesConfirmedDetails.IsNull() ? 0 : stats.MessagesConfirmedDetails.Rate);
                UnroutableMessagesReturned = new MessageDetailsImpl(stats.TotalUnroutableMessagesReturned, stats.UnroutableMessagesReturnedDetails.IsNull() ? 0 : stats.UnroutableMessagesReturnedDetails.Rate);
                Gets = new MessageDetailsImpl(stats.TotalMessageGets, stats.MessageGetDetails.IsNull() ? 0 : stats.MessageGetDetails.Rate);
                GetsWithoutAcknowledgement = new MessageDetailsImpl(stats.TotalMessageGetsWithoutAck, stats.MessageGetsWithoutAckDetails.IsNull() ? 0 : stats.MessageGetsWithoutAckDetails.Rate);
                Delivered = new MessageDetailsImpl(stats.TotalMessagesDelivered, stats.MessageDeliveryDetails.IsNull() ? 0 : stats.MessageDeliveryDetails.Rate);
                DeliveredWithoutAcknowledgement = new MessageDetailsImpl(stats.TotalMessageDeliveredWithoutAck, stats.MessagesDeliveredWithoutAckDetails.IsNull() ? 0 : stats.MessagesDeliveredWithoutAckDetails.Rate);
                Redelivered = new MessageDetailsImpl(stats.TotalMessagesRedelivered, stats.MessagesRedeliveredDetails.IsNull() ? 0 : stats.MessagesRedeliveredDetails.Rate);
                Acknowledged = new MessageDetailsImpl(stats.TotalMessagesAcknowledged, stats.MessagesAcknowledgedDetails.IsNull() ? 0 : stats.MessagesAcknowledgedDetails.Rate);
                DeliveryGets = new MessageDetailsImpl(stats.TotalMessageDeliveryGets, stats.MessageDeliveryGetDetails.IsNull() ? 0 : stats.MessageDeliveryGetDetails.Rate);
            }

            public MessageDetails Published { get; }
            public MessageDetails Confirmed { get; }
            public MessageDetails UnroutableMessagesReturned { get; }
            public MessageDetails Gets { get; }
            public MessageDetails GetsWithoutAcknowledgement { get; }
            public MessageDetails Delivered { get; }
            public MessageDetails DeliveredWithoutAcknowledgement { get; }
            public MessageDetails Redelivered { get; }
            public MessageDetails Acknowledged { get; }
            public MessageDetails DeliveryGets { get; }

            
            class MessageDetailsImpl :
                MessageDetails
            {
                public MessageDetailsImpl(long total, decimal rate)
                {
                    Total = total;
                    Rate = rate;
                }

                public long Total { get; }
                public decimal Rate { get; }
            }
        }

        public string RabbitMqVersion { get; }
        public string ClusterName { get; }
        public string ErlangVerion { get; }
        public Queue Queue { get; }
        public IO IO { get; }
    }
}