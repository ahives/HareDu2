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

    public class FakeConnectivitySnapshot2 :
        ConnectivitySnapshot
    {
        public FakeConnectivitySnapshot2()
        {
            Connections = GetConnections().ToList();
        }

        public ChurnMetrics ChannelsClosed { get; }
        public ChurnMetrics ChannelsCreated { get; }
        public ChurnMetrics ConnectionsClosed { get; }
        public ChurnMetrics ConnectionsCreated { get; }
        public IReadOnlyList<ConnectionSnapshot> Connections { get; }

        IEnumerable<ConnectionSnapshot> GetConnections()
        {
            yield return new FakeConnectionSnapshot("Connection1", 6);
            yield return new FakeConnectionSnapshot("Connection2", 8);
            yield return new FakeConnectionSnapshot("Connection3", 10);
        }

            
        class FakeConnectionSnapshot :
            ConnectionSnapshot
        {
            public FakeConnectionSnapshot(string identifier, long channelLimit)
            {
                Identifier = identifier;
                ChannelLimit = channelLimit;
                Channels = GetChannels().ToList();
            }

            IEnumerable<ChannelSnapshot> GetChannels()
            {
                yield return new FakeChannelSnapshot("Channel1", 4, 2, 5, 8, 2, 1);
                yield return new FakeChannelSnapshot("Channel2", 4, 2, 5, 8, 2, 1);
                yield return new FakeChannelSnapshot("Channel3", 4, 2, 5, 8, 2, 1);
                yield return new FakeChannelSnapshot("Channel4", 4, 2, 5, 8, 2, 1);
                yield return new FakeChannelSnapshot("Channel5", 4, 2, 5, 8, 2, 1);
                yield return new FakeChannelSnapshot("Channel6", 4, 2, 5, 8, 2, 1);
            }

                
            class FakeChannelSnapshot :
                ChannelSnapshot
            {
                public FakeChannelSnapshot(string name, long prefetchCount, long uncommittedAcknowledgements,
                    long uncommittedMessages, long unconfirmedMessages, long unacknowledgedMessages, long consumers)
                {
                    PrefetchCount = prefetchCount;
                    UncommittedAcknowledgements = uncommittedAcknowledgements;
                    UncommittedMessages = uncommittedMessages;
                    UnconfirmedMessages = unconfirmedMessages;
                    UnacknowledgedMessages = unacknowledgedMessages;
                    Consumers = consumers;
                    Name = name;
                }

                public long PrefetchCount { get; }
                public long UncommittedAcknowledgements { get; }
                public long UncommittedMessages { get; }
                public long UnconfirmedMessages { get; }
                public long UnacknowledgedMessages { get; }
                public long Consumers { get; }
                public string Name { get; }
                public string Node { get; }
            }

            public string Identifier { get; }
            public NetworkTrafficSnapshot NetworkTraffic { get; }
            public long ChannelLimit { get; }
            public string Node { get; }
            public string VirtualHost { get; }
            public IReadOnlyList<ChannelSnapshot> Channels { get; }
        }
    }
}