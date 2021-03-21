namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class BindingRequest
    {
        public BindingRequest(string bindingKey, IDictionary<string,object> arguments)
        {
            BindingKey = bindingKey;
            Arguments = arguments;
        }

        [JsonPropertyName("routing_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string BindingKey { get; }
        
        [JsonPropertyName("arguments")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IDictionary<string, object> Arguments { get; }
    }
}