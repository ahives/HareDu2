namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class ServerHealthInfoImpl
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        
        [JsonPropertyName("reason")]
        public string Reason { get; set; }
    }
}