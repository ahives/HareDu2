namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface UserDefinition
    {
        [JsonPropertyName("password_hash")]
        string PasswordHash { get; }
        
        [JsonPropertyName("password")]
        string Password { get; }
        
        [JsonPropertyName("tags")]
        string Tags { get; }
    }
}