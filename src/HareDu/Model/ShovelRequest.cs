namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface ShovelRequest
    {
        [JsonProperty("value")]
        ShovelRequestParams Value { get; }
    }
}