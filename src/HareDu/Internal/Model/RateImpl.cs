namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class RateImpl
    {
        [JsonPropertyName("rate")]
        public decimal Value { get; set; }
    }
}