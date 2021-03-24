namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalNodeContext :
        NodeContext
    {
        public InternalNodeContext(NodeContextImpl obj)
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