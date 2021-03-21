namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface UserPermissionsInfo
    {
        [JsonPropertyName("user")]
        string User { get; }
        
        [JsonPropertyName("vhost")]
        string VirtualHost { get; }
        
        [JsonPropertyName("configure")]
        string Configure { get; }
        
        [JsonPropertyName("write")]
        string Write { get; }
        
        [JsonPropertyName("read")]
        string Read { get; }
    }
}