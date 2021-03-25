namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public class ShovelRequest
    {
        public ShovelRequest(ShovelRequestParams value)
        {
            Value = value;
        }

        public ShovelRequest()
        {
        }

        [JsonPropertyName("value")]
        public ShovelRequestParams Value { get; set; }
    }
}