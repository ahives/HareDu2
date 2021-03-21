namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface ShovelRequest
    {
        [JsonPropertyName("value")]
        ShovelRequestParams Value { get; }
    }
}