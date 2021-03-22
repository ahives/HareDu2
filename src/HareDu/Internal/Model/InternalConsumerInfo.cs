namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using Core.Extensions;
    using HareDu.Model;

    class InternalConsumerInfo :
        ConsumerInfo
    {
        public InternalConsumerInfo(ConsumerInfoImpl obj)
        {
            PreFetchCount = obj.PreFetchCount;
            Arguments = obj.Arguments;
            AcknowledgementRequired = obj.AcknowledgementRequired;
            Exclusive = obj.Exclusive;
            ConsumerTag = obj.ConsumerTag;
            ChannelDetails = obj.ChannelDetails.IsNotNull() ? new InternalChannelDetails(obj.ChannelDetails) : default;
            QueueConsumerDetails = obj.QueueConsumerDetails.IsNotNull() ? new InternalQueueConsumerDetails(obj.QueueConsumerDetails) : default;
        }

        public ulong PreFetchCount { get; }
        public IDictionary<string, object> Arguments { get; }
        public bool AcknowledgementRequired { get; }
        public bool Exclusive { get; }
        public string ConsumerTag { get; }
        public ChannelDetails ChannelDetails { get; }
        public QueueConsumerDetails QueueConsumerDetails { get; }
    }
}