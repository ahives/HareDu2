namespace HareDu.Model
{
    using System.Collections.Generic;

    public interface ConsumerInfo
    {
        ulong PreFetchCount { get; }
        
        IDictionary<string, object> Arguments { get; }
        
        bool AcknowledgementRequired { get; }
        
        bool Exclusive { get; }
        
        string ConsumerTag { get; }
        
        ChannelDetails ChannelDetails { get; }
        
        QueueConsumerDetails QueueConsumerDetails { get; }
    }
}