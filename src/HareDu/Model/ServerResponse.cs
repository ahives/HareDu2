namespace HareDu.Model
{
    using System.Net;
    using Newtonsoft.Json;

    public interface ServerResponse
    {
        [JsonIgnore]
        string Reason { get; }
        
        [JsonIgnore]
        HttpStatusCode StatusCode { get; }
    }
}