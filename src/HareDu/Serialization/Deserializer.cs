namespace HareDu.Serialization
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Converters;

    public static class Deserializer
    {
        public static JsonSerializerOptions Options =>
            new()
            {
                WriteIndented = true,
                Converters =
                {
                    new CustomDecimalConverter(),
                    new CustomDateTimeConverter(),
                    new CustomLongConverter(),
                    new CustomStringConverter(),
                    new AckModeEnumConverter(),
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };
    }
}