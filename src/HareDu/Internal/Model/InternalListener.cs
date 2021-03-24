namespace HareDu.Internal.Model
{
    using Core.Extensions;
    using HareDu.Model;

    class InternalListener :
        Listener
    {
        public InternalListener(ListenerImpl obj)
        {
            Node = obj.Node;
            Protocol = obj.Protocol;
            IPAddress = obj.IPAddress;
            Port = obj.Port;
            SocketOptions = obj.SocketOptions.IsNotNull() ? new InternalSocketOptions(obj.SocketOptions) : default;
        }

        public string Node { get; }
        public string Protocol { get; }
        public string IPAddress { get; }
        public string Port { get; }
        public SocketOptions SocketOptions { get; }
    }
}