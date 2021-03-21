namespace HareDu.Serialization.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class CustomDateTimeConverter :
        JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string stringValue = reader.GetString();
                if (DateTimeOffset.TryParse(stringValue, out var value))
                    return value;
            }
            else if (reader.TokenType == JsonTokenType.Null)
                return default;

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}