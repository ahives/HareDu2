namespace HareDu.Internal.Model
{
    using Core.Extensions;
    using HareDu.Model;

    class InternalChurnRates :
        ChurnRates
    {
        public InternalChurnRates(ChurnRatesImpl obj)
        {
            TotalChannelsClosed = obj.TotalChannelsClosed;
            TotalChannelsCreated = obj.TotalChannelsCreated;
            TotalConnectionsClosed = obj.TotalConnectionsClosed;
            TotalConnectionsCreated = obj.TotalConnectionsCreated;
            TotalQueuesCreated = obj.TotalQueuesCreated;
            TotalQueuesDeclared = obj.TotalQueuesDeclared;
            TotalQueuesDeleted = obj.TotalQueuesDeleted;
            ClosedChannelDetails = obj.ClosedChannelDetails.IsNotNull() ? new InternalRate(obj.ClosedChannelDetails) : default;
            CreatedChannelDetails = obj.CreatedChannelDetails.IsNotNull() ? new InternalRate(obj.CreatedChannelDetails) : default;
            ClosedConnectionDetails = obj.ClosedConnectionDetails.IsNotNull() ? new InternalRate(obj.ClosedConnectionDetails) : default;
            CreatedConnectionDetails = obj.CreatedConnectionDetails.IsNotNull() ? new InternalRate(obj.CreatedConnectionDetails) : default;
            CreatedQueueDetails = obj.CreatedQueueDetails.IsNotNull() ? new InternalRate(obj.CreatedQueueDetails) : default;
            DeclaredQueueDetails = obj.DeclaredQueueDetails.IsNotNull() ? new InternalRate(obj.DeclaredQueueDetails) : default;
            DeletedQueueDetails = obj.DeletedQueueDetails.IsNotNull() ? new InternalRate(obj.DeletedQueueDetails) : default;
        }

        public ulong TotalChannelsClosed { get; }
        public Rate ClosedChannelDetails { get; }
        public ulong TotalChannelsCreated { get; }
        public Rate CreatedChannelDetails { get; }
        public ulong TotalConnectionsClosed { get; }
        public Rate ClosedConnectionDetails { get; }
        public ulong TotalConnectionsCreated { get; }
        public Rate CreatedConnectionDetails { get; }
        public ulong TotalQueuesCreated { get; }
        public Rate CreatedQueueDetails { get; }
        public ulong TotalQueuesDeclared { get; }
        public Rate DeclaredQueueDetails { get; }
        public ulong TotalQueuesDeleted { get; }
        public Rate DeletedQueueDetails { get; }
    }
}