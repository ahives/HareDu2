namespace HareDu.Serialization.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class CustomDecimalConverter :
        JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string stringValue = reader.GetString();
                if (decimal.TryParse(stringValue, out var value))
                    return value;
            }
            else if (reader.TokenType == JsonTokenType.Number)
                return reader.GetDecimal();
            else if (reader.TokenType == JsonTokenType.Null)
                return default;

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}