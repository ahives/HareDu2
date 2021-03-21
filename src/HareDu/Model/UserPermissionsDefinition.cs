namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface UserPermissionsDefinition
    {
        [JsonPropertyName("configure")]
        string Configure { get; }
        
        [JsonPropertyName("write")]
        string Write { get; }
        
        [JsonPropertyName("read")]
        string Read { get; }
    }
}