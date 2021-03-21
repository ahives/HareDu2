namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface GlobalParameterInfo
    {
        [JsonPropertyName("name")]
        string Name { get; }

        [JsonPropertyName("value")]
        object Value { get; }
    }
}