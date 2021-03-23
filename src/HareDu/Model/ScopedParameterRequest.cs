namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public class ScopedParameterRequest<T>
    {
        public ScopedParameterRequest(string virtualHost, string component, string parameterName, T parameterValue)
        {
            VirtualHost = virtualHost;
            Component = component;
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        public ScopedParameterRequest()
        {
        }

        [JsonPropertyName("vhost")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string VirtualHost { get; set; }

        [JsonPropertyName("component")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Component { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ParameterName { get; set; }

        [JsonPropertyName("value")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T ParameterValue { get; set; }
    }
}