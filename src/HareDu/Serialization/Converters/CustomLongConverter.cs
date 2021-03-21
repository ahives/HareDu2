namespace HareDu.Serialization.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class CustomLongConverter :
        JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string stringValue = reader.GetString();
                if (long.TryParse(stringValue, out long value))
                    return value;
                
                return stringValue == "infinity" ? long.MaxValue : default;
            }

            if (reader.TokenType == JsonTokenType.Number)
                return reader.GetInt64();
            
            if (reader.TokenType == JsonTokenType.Null)
                return default;

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}