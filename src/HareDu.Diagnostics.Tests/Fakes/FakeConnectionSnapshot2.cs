namespace HareDu.Diagnostics.Tests.Fakes
{
    using System.Collections.Generic;
    using Model;
    using Snapshotting.Extensions;
    using Snapshotting.Model;

    public class FakeConnectionSnapshot2 :
        ConnectionSnapshot
    {
        public FakeConnectionSnapshot2(BrokerConnectionState state)
        {
            State = state;
        }

        public string Identifier { get; }
        public NetworkTrafficSnapshot NetworkTraffic { get; }
        public ulong OpenChannelsLimit { get; }
        public string NodeIdentifier { get; }
        public string VirtualHost { get; }
        public BrokerConnectionState State { get; }
        public IReadOnlyList<ChannelSnapshot> Channels { get; }
    }
}