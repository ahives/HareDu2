namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalContext :
        NodeContext
    {
        public InternalContext(NodeContextImpl obj)
        {
            Description = obj.Description;
            Path = obj.Path;
            Port = obj.Port;
        }

        public string Description { get; }
        public string Path { get; }
        public string Port { get; }
    }
}