namespace HareDu.Model
{
    using System.Text.Json.Serialization;
    using Serialization.Converters;

    public class QueueSyncRequest
    {
        public QueueSyncRequest(QueueSyncAction action)
        {
            Action = action;
        }

        public QueueSyncRequest()
        {
        }

        [JsonPropertyName("action")]
        [JsonConverter(typeof(QueueSyncActionEnumConverter))]
        public QueueSyncAction Action { get; set; }
    }
}