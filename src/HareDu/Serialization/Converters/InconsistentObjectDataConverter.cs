namespace HareDu.Serialization.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Model;

    public class InconsistentObjectDataConverter :
        JsonConverter<SocketOptions>
    {
        public override SocketOptions? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var value = JsonDocument.ParseValue(ref reader);

            if (reader.TokenType == JsonTokenType.EndArray)
                return default;

            var text = value.RootElement.GetRawText();

            return JsonSerializer.Deserialize<SocketOptions>(text, options);
        }

        public override void Write(Utf8JsonWriter writer, SocketOptions value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}