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