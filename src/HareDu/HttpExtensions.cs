namespace HareDu
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Internal.Serialization;
    using Model;
    using Newtonsoft.Json;

    internal static class HttpExtensions
    {
        internal static async Task<Result<TData>> GetResponse<TData>(this HttpResponseMessage responseMessage)
        {
            string rawResponse = await responseMessage.Content.ReadAsStringAsync();
            TData response = SerializerCache.Deserializer.Deserialize<TData>(new JsonTextReader(new StringReader(rawResponse)));
            
            Result<TData> result = new ResultImpl<TData>(responseMessage, response);

            return result;
        }

        internal static Result GetResponse(this HttpResponseMessage responseMessage)
        {
            Result result = new ResultImpl(responseMessage);

            return result;
        }

        
        class ResultImpl<TData> :
            Result<TData>
        {
            public ResultImpl(HttpResponseMessage responseMessage, TData response)
            {
                Data = response;
                Reason = responseMessage.ReasonPhrase;
                StatusCode = responseMessage.StatusCode;
            }

            public TData Data { get; }
            public string Reason { get; }
            public HttpStatusCode StatusCode { get; }
        }

        
        class ResultImpl :
            Result
        {
            public ResultImpl(HttpResponseMessage responseMessage)
            {
                Reason = responseMessage.ReasonPhrase;
                StatusCode = responseMessage.StatusCode;
            }

            public string Reason { get; }
            public HttpStatusCode StatusCode { get; }
        }
    }
}