namespace HareDu.Snapshotting.Model
{
    using System.Collections.Generic;
    using HareDu.Model;

    public interface ConnectionSnapshot :
        Snapshot
    {
        string Identifier { get; }
        
        NetworkTrafficSnapshot NetworkTraffic { get; }
        
        ulong OpenChannelsLimit { get; }
        
        string NodeIdentifier { get; }
        
        string VirtualHost { get; }
        
        BrokerConnectionState State { get; }

        IReadOnlyList<ChannelSnapshot> Channels { get; }
    }
}