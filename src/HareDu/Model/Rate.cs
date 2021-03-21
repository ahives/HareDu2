namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface Rate
    {
        [JsonPropertyName("rate")]
        decimal Value { get; }
    }
}