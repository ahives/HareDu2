namespace HareDu.Serialization.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Internal.Model;

    class InconsistentObjectDataConverter :
        JsonConverter<SocketOptionsImpl>
    {
        public override SocketOptionsImpl? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var value = JsonDocument.ParseValue(ref reader);

            if (reader.TokenType == JsonTokenType.EndArray)
                return default;

            var text = value.RootElement.GetRawText();

            return JsonSerializer.Deserialize<SocketOptionsImpl>(text, options);
        }

        public override void Write(Utf8JsonWriter writer, SocketOptionsImpl value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}