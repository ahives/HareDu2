namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalQueueConsumerDetails :
        QueueConsumerDetails
    {
        public InternalQueueConsumerDetails(QueueConsumerDetailsImpl obj)
        {
            VirtualHost = obj.VirtualHost;
            Name = obj.Name;
        }

        public string VirtualHost { get; }
        public string Name { get; }
    }
}