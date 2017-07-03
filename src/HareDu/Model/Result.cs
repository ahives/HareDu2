namespace HareDu.Model
{
    using System.Net;
    using Newtonsoft.Json;

    public interface Result
    {
        [JsonIgnore]
        string Reason { get; }
        
        [JsonIgnore]
        HttpStatusCode StatusCode { get; }
    }

    public interface Result<out TData>
    {
        [JsonIgnore]
        TData Data { get; }

        [JsonIgnore]
        string Reason { get; }

        [JsonIgnore]
        HttpStatusCode StatusCode { get; }
    }
}