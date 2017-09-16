// Copyright 2013-2017 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Internal
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Serialization;

    internal static class HttpExtensions
    {
        internal static async Task<Result<TData>> GetResponse<TData>(this HttpResponseMessage responseMessage)
        {
            string rawResponse = await responseMessage.Content.ReadAsStringAsync();
            TData response = SerializerCache.Deserializer.Deserialize<TData>(new JsonTextReader(new StringReader(rawResponse)));

            string rawRequest = string.Empty;
            if (responseMessage.RequestMessage.Content != null)
                rawRequest = await responseMessage.RequestMessage.Content.ReadAsStringAsync();
            
            Result<TData> result = new ResultImpl<TData>(responseMessage, response, rawRequest);

            return result;
        }

        internal static async Task<Result> GetResponse(this HttpResponseMessage responseMessage)
        {
            string rawRequest = await responseMessage.RequestMessage.Content.ReadAsStringAsync();
            
            Result result = new ResultImpl(responseMessage, rawRequest);

            return result;
        }


        class ResultImpl<TData> :
            Result<TData>
        {
            public ResultImpl(HttpResponseMessage responseMessage, TData response, string request)
            {
                DebugInfo = $"Debug: {request}";
                Data = response;
                Reason = responseMessage.ReasonPhrase;
                StatusCode = responseMessage.StatusCode;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public string DebugInfo { get; }
            public TData Data { get; }
            public string Reason { get; }
            public HttpStatusCode StatusCode { get; }
            public DateTimeOffset Timestamp { get; }
        }

        
        class ResultImpl :
            Result
        {
            public ResultImpl(HttpResponseMessage responseMessage, string request)
            {
                DebugInfo = $"Debug: {request}";
                Reason = responseMessage.ReasonPhrase;
                StatusCode = responseMessage.StatusCode;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public string DebugInfo { get; }
            public string Reason { get; }
            public HttpStatusCode StatusCode { get; }
            public DateTimeOffset Timestamp { get; }
        }
    }
}