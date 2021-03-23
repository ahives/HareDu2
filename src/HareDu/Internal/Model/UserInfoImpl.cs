namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class UserInfoImpl
    {
        [JsonPropertyName("name")]
        public string Username { get; set; }
        
        [JsonPropertyName("password_hash")]
        public string PasswordHash { get; set; }
        
        [JsonPropertyName("hashing_algorithm")]
        public string HashingAlgorithm { get; set; }
        
        [JsonPropertyName("tags")]
        public string Tags { get; set; }
    }
}