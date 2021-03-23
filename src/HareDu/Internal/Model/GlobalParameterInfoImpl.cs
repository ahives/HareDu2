namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class GlobalParameterInfoImpl
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public object Value { get; set; }
    }
}