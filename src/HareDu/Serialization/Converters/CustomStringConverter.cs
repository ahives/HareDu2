namespace HareDu.Serialization.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class CustomStringConverter :
        JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                var stringValue = reader.GetInt32();

                return stringValue.ToString();
            }

            if (reader.TokenType == JsonTokenType.String)
                return reader.GetString();

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}