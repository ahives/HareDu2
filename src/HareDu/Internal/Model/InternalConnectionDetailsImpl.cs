namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalConnectionDetailsImpl :
        ConnectionDetails
    {
        public InternalConnectionDetailsImpl(ConnectionDetailsImpl item)
        {
            PeerHost = item.PeerHost;
            PeerPort = item.PeerPort;
            Name = item.Name;
        }

        public string PeerHost { get; }
        public long PeerPort { get; }
        public string Name { get; }
    }
}