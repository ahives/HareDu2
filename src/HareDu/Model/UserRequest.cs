namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public class UserRequest
    {
        public UserRequest(string passwordHash, string password, string tags)
        {
            PasswordHash = passwordHash;
            Password = password;
            Tags = tags;
        }

        public UserRequest()
        {
        }

        [JsonPropertyName("password_hash")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string PasswordHash { get; set; }
        
        [JsonPropertyName("password")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Password { get; set; }
        
        [JsonPropertyName("tags")]
        public string Tags { get; set; }
    }
}