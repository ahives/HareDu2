namespace HareDu.Serialization.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class QueueSyncActionEnumConverter :
        JsonConverter<QueueSyncAction>
    {
        public override QueueSyncAction Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            string? text = reader.GetString();
            
            switch (text)
            {
                case "sync":
                    return QueueSyncAction.Sync;
                
                case "cancel_sync":
                    return QueueSyncAction.CancelSync;
                
                default:
                    throw new JsonException();
            }
        }

        public override void Write(Utf8JsonWriter writer, QueueSyncAction value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case QueueSyncAction.Sync:
                    writer.WriteStringValue("sync");
                    break;
                
                case QueueSyncAction.CancelSync:
                    writer.WriteStringValue("cancel_sync");
                    break;
                
                default:
                    throw new JsonException();
            }
        }
    }
}