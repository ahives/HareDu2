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
namespace HareDu.Diagnostics.Tests.Fakes
{
    using System.Collections.Generic;
    using System.Linq;
    using Snapshotting.Model;

    public class FakeConnectionSnapshot1 :
        ConnectionSnapshot
    {
        public FakeConnectionSnapshot1(int numOfChannels, ulong channelLimit)
        {
            Identifier = "Connection1";
            OpenChannelsLimit = channelLimit;
            Channels = GetChannels(numOfChannels).ToList();
        }

        public string Identifier { get; }
        public NetworkTrafficSnapshot NetworkTraffic { get; }
        public ulong OpenChannelsLimit { get; }
        public string NodeIdentifier { get; }
        public string VirtualHost { get; }
        public ConnectionState State { get; }
        public IReadOnlyList<ChannelSnapshot> Channels { get; }

        IEnumerable<ChannelSnapshot> GetChannels(int numOfChannels)
        {
            for (int i = 0; i < numOfChannels; i++)
            {
                yield return new FakeChannelSnapshot($"Channel{i}", 4, 2, 5, 8, 6, 1);
            }
        }


        class FakeChannelSnapshot :
            ChannelSnapshot
        {
            public FakeChannelSnapshot(string name, uint prefetchCount, ulong uncommittedAcknowledgements,
                ulong uncommittedMessages, ulong unconfirmedMessages, ulong unacknowledgedMessages, ulong consumers)
            {
                PrefetchCount = prefetchCount;
                UncommittedAcknowledgements = uncommittedAcknowledgements;
                UncommittedMessages = uncommittedMessages;
                UnconfirmedMessages = unconfirmedMessages;
                UnacknowledgedMessages = unacknowledgedMessages;
                Consumers = consumers;
                Identifier = name;
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
        }
    }
}