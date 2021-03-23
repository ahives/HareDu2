namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalConnectionDetails :
        ConnectionDetails
    {
        public InternalConnectionDetails(ConnectionDetailsImpl obj)
        {
            PeerHost = obj.PeerHost;
            PeerPort = obj.PeerPort;
            Name = obj.Name;
        }

        public string PeerHost { get; }
        public long PeerPort { get; }
        public string Name { get; }
    }
}