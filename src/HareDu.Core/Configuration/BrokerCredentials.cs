namespace HareDu.Core.Configuration
{
    using Newtonsoft.Json;

    public interface BrokerCredentials
    {
        [JsonProperty("username")]
        string Username { get; }
        
        [JsonProperty("password")]
        string Password { get; }
    }
}