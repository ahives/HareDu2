// Copyright 2013-2020 Albert L. Hives
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
namespace HareDu.Testing.Fakes
{
    using System;
    using System.Collections.Generic;
    using Model;

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
            MessageBytesInRam = 992390933;
            TotalBytesOfMessagesDeliveredButUnacknowledged = 82830892;
            TotalBytesOfMessagesReadyForDelivery = 892839823;
            TotalBytesOfAllMessages = 82938938723;
            UnacknowledgedMessagesInRam = 82938982323;
            MessagesReadyForDeliveryInRam = 8892388929;
            MessagesInRam = 9883892938;
            Consumers = 773709938;
            ConsumerUtilization = 0.50M;
            Memory = 92990390;
            MessageStats = new QueueMessageStatsImpl();
        }

        public Rate MessageRate { get; }
        public ulong TotalMessages { get; }
        public Rate UnackedMessageRate { get; }
        public ulong UnacknowledgedMessages { get; }
        public Rate ReadyMessageRate { get; }
        public ulong ReadyMessages { get; }
        public Rate ReductionRate { get; }
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
        public ulong MessageBytesInRam { get; }
        public ulong TotalBytesOfMessagesDeliveredButUnacknowledged { get; }
        public ulong TotalBytesOfMessagesReadyForDelivery { get; }
        public ulong TotalBytesOfAllMessages { get; }
        public ulong MessagesPersisted { get; }
        public ulong UnacknowledgedMessagesInRam { get; }
        public ulong MessagesReadyForDeliveryInRam { get; }
        public ulong MessagesInRam { get; }
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