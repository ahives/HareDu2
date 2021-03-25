namespace HareDu.Model
{
    using System.Text.Json.Serialization;

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
        public QueueSyncAction Action { get; set; }
    }
}