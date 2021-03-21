namespace HareDu.Serialization.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Model;

    public class ShovelInfoConverter :
        JsonConverter<ShovelInfo>
    {
        public override ShovelInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return default;
            
            var value = JsonDocument.ParseValue(ref reader);
            var text = value.RootElement.GetRawText();

            var copyReader = reader;
            
            var obj = JsonSerializer.Deserialize<ShovelInfo>(ref copyReader, options);

            return obj;
        }

        public override void Write(Utf8JsonWriter writer, ShovelInfo value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}