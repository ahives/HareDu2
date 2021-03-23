namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class UserPermissionsInfoImpl
    {
        [JsonPropertyName("user")]
        public string User { get; set; }
        
        [JsonPropertyName("vhost")]
        public string VirtualHost { get; set; }
        
        [JsonPropertyName("configure")]
        public string Configure { get; set; }
        
        [JsonPropertyName("write")]
        public string Write { get; set; }
        
        [JsonPropertyName("read")]
        public string Read { get; set; }
    }
}