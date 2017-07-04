namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface IOReopenCountDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}