namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface UserInfo
    {
        [JsonPropertyName("name")]
        string Username { get; }
        
        [JsonPropertyName("password_hash")]
        string PasswordHash { get; }
        
        [JsonPropertyName("hashing_algorithm")]
        string HashingAlgorithm { get; }
        
        [JsonPropertyName("tags")]
        string Tags { get; }
    }
}