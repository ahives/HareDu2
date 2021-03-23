namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class ExchangeTypeImpl
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("enabled")]
        public bool IsEnabled { get; set; }
    }
}