namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    public class GlobalParameterRequest
    {
        public GlobalParameterRequest()
        {
        }

        public GlobalParameterRequest(string name, object argument)
        {
            Name = name;
            Value = argument;
        }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }
            
        [JsonPropertyName("value")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public object Value { get; set; }
    }
}