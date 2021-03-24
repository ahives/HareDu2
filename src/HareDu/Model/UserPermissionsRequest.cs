namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public class UserPermissionsRequest
    {
        public UserPermissionsRequest(string configure, string write, string read)
        {
            Configure = configure;
            Write = write;
            Read = read;
        }

        public UserPermissionsRequest()
        {
        }

        [JsonPropertyName("configure")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Configure { get; set; }
        
        [JsonPropertyName("write")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Write { get; set; }
        
        [JsonPropertyName("read")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Read { get; set; }
    }
}