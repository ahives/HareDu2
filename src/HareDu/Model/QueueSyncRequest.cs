namespace HareDu.Model
{
    using System.Text.Json.Serialization;
    using Serialization.Converters;

    public interface QueueSyncRequest
    {
        [JsonPropertyName("action")]
        [JsonConverter(typeof(QueueSyncActionEnumConverter))]
        QueueSyncAction Action { get; }
    }
}