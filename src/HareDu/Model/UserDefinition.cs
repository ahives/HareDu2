namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public class UserDefinition
    {
        public UserDefinition(string passwordHash, string password, string tags)
        {
            string Normalize(string value) => string.IsNullOrWhiteSpace(value) ? null : value;

            Password = Normalize(password);
            Tags = Normalize(tags);
                    
            if (string.IsNullOrWhiteSpace(Password))
                PasswordHash = passwordHash;
        }

        public UserDefinition()
        {
        }

        [JsonPropertyName("password_hash")]
        public string PasswordHash { get; set; }
        
        [JsonPropertyName("password")]
        public string Password { get; set; }
        
        [JsonPropertyName("tags")]
        public string Tags { get; set; }
    }
}