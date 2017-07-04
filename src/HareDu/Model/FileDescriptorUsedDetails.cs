namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface FileDescriptorUsedDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}