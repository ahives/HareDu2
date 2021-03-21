namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface ChurnRates
    {
        [JsonPropertyName("channel_closed")]
        ulong TotalChannelsClosed { get; }
        
        [JsonPropertyName("channel_closed_details")]
        Rate ClosedChannelDetails { get; }

        [JsonPropertyName("channel_created")]
        ulong TotalChannelsCreated { get; }
        
        [JsonPropertyName("channel_created_details")]
        Rate CreatedChannelDetails { get; }

        [JsonPropertyName("connection_closed")]
        ulong TotalConnectionsClosed { get; }
        
        [JsonPropertyName("connection_closed_details")]
        Rate ClosedConnectionDetails { get; }

        [JsonPropertyName("connection_created")]
        ulong TotalConnectionsCreated { get; }
        
        [JsonPropertyName("connection_created_details")]
        Rate CreatedConnectionDetails { get; }

        [JsonPropertyName("queue_created")]
        ulong TotalQueuesCreated { get; }
        
        [JsonPropertyName("queue_created_details")]
        Rate CreatedQueueDetails { get; }

        [JsonPropertyName("queue_declared")]
        ulong TotalQueuesDeclared { get; }
        
        [JsonPropertyName("queue_declared_details")]
        Rate DeclaredQueueDetails { get; }

        [JsonPropertyName("queue_deleted")]
        ulong TotalQueuesDeleted { get; }
        
        [JsonPropertyName("queue_deleted_details")]
        Rate DeletedQueueDetails { get; }
    }
}