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
namespace HareDu.Snapshotting
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Model;
    using Model;

    public static class SnapshotFilterExtensions
    {
//        public static List<ChannelMetrics> FilterByConnection(this IEnumerable<ChannelInfo> channels, string connection)
//        {
//            if (channels == null || !channels.Any())
//                return new List<ChannelMetrics>();
//
//            return channels
//                .Where(x => x.ConnectionDetails?.Name == connection)
//                .Select(x => new ChannelMetricsImpl(x))
//                .Cast<ChannelMetrics>()
//                .ToList();
//        }

        public static IReadOnlyList<ChannelSnapshot> FilterByConnection(this IReadOnlyList<ChannelInfo> channels, string connection)
        {
            if (channels == null || !channels.Any())
                return new List<ChannelSnapshot>();

            return channels
                .Where(x => x.ConnectionDetails?.Name == connection)
                .Select(x => new ChannelSnapshotImpl(x))
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
            public ChannelSnapshotImpl(ChannelInfo channel)
            {
                Name = channel.Name;
                Consumers = channel.TotalConsumers;
                Node = channel.Node;
                PrefetchCount = channel.PrefetchCount;
                UncommittedAcknowledgements = channel.UncommittedAcknowledgements;
                UncommittedMessages = channel.UncommittedMessages;
                UnconfirmedMessages = channel.UnconfirmedMessages;
                UnacknowledgedMessages = channel.UnacknowledgedMessages;
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
    }
}