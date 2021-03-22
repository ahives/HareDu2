namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalChannelDetails :
        ChannelDetails
    {
        public InternalChannelDetails(ChannelDetailsImpl obj)
        {
            PeerHost = obj.PeerHost;
            PeerPort = obj.PeerPort;
            Number = obj.Number;
            Name = obj.Name;
            Node = obj.Node;
            ConnectionName = obj.ConnectionName;
            User = obj.User;
        }

        public string PeerHost { get; }
        public long PeerPort { get; }
        public long Number { get; }
        public string Name { get; }
        public string Node { get; }
        public string ConnectionName { get; }
        public string User { get; }
    }
}