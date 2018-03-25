// Copyright 2013-2018 Albert L. Hives
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
namespace HareDu.Internal.Resources
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Serialization;

    internal class ResourceBase
    {
        readonly HttpClient _client;

        protected ResourceBase(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        protected async Task<Result<IReadOnlyList<T>>> GetAll<T>(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var responseMessage = await _client.GetAsync(url, cancellationToken);
                if (!responseMessage.IsSuccessStatusCode)
                    return new FaultedResult<IReadOnlyList<T>>(new List<Error> { GetError(responseMessage.StatusCode) });
                
                var response = await DeserializeResponse<IReadOnlyList<T>>(responseMessage);
                var request = await GetRequest(responseMessage);
                
                return new SuccessfulListResult<T>(response, new DebugInfoImpl(request));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult<IReadOnlyList<T>>(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
        }

        protected async Task<Result<T>> Get<T>(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var responseMessage = await _client.GetAsync(url, cancellationToken);
                if (!responseMessage.IsSuccessStatusCode)
                    return new FaultedResult<T>(new List<Error> { GetError(responseMessage.StatusCode) });

                var response = await DeserializeResponse<T>(responseMessage);
                var request = await GetRequest(responseMessage);
                
                return new SuccessfulResult<T>(response, new DebugInfoImpl(request));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult<T>(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
        }

        protected async Task<Result> Delete(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var responseMessage = await _client.DeleteAsync(url, cancellationToken);
                string request = await GetRequest(responseMessage);
                
                if (!responseMessage.IsSuccessStatusCode)
                    return new FaultedResult(new DebugInfoImpl(url), new List<Error> { GetError(responseMessage.StatusCode) });

                return new SuccessfulResult(new DebugInfoImpl(request));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
        }

        protected async Task<Result> Put<TValue>(string url, TValue value, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var responseMessage = await _client.PutAsJsonAsync(url, value, cancellationToken);
                if (!responseMessage.IsSuccessStatusCode)
                    return new FaultedResult(new List<Error> { GetError(responseMessage.StatusCode) });

                var request = await GetRequest(responseMessage);

                return new SuccessfulResult(new DebugInfoImpl(request));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
        }

        protected async Task<Result<TResult>> Post<TValue, TResult>(string url, TValue value, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var responseMessage = await _client.PostAsJsonAsync(url, value, cancellationToken);
                if (!responseMessage.IsSuccessStatusCode)
                    return new FaultedResult<TResult>(new List<Error> { GetError(responseMessage.StatusCode) });

                var response = await DeserializeResponse<TResult>(responseMessage);
                var request = await GetRequest(responseMessage);

                return new SuccessfulResult<TResult>(response, new DebugInfoImpl(request));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult<TResult>(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
        }

        protected string SanitizeVirtualHostName(string value)
        {
            return value == @"/" ? value.Replace("/", "%2f") : value;
        }

        void HandleDotsAndSlashes()
        {
            var getSyntaxMethod = typeof(UriParser).GetMethod("GetSyntax", BindingFlags.Static | BindingFlags.NonPublic);
            if (getSyntaxMethod == null)
                throw new MissingMethodException("UriParser", "GetSyntax");

            var uriParser = getSyntaxMethod.Invoke(null, new object[] {"http"});

            var setUpdatableFlagsMethod = uriParser.GetType().GetMethod("SetUpdatableFlags", BindingFlags.Instance | BindingFlags.NonPublic);
            if (setUpdatableFlagsMethod == null)
                throw new MissingMethodException("UriParser", "SetUpdatableFlags");

            setUpdatableFlagsMethod.Invoke(uriParser, new object[] {0});
        }
        
        async Task<T> DeserializeResponse<T>(HttpResponseMessage responseMessage)
        {
            string response = await responseMessage.Content.ReadAsStringAsync();
            T deserializedResponse = SerializerCache.Deserializer.Deserialize<T>(new JsonTextReader(new StringReader(response)));

            return deserializedResponse;
        }

        async Task<string> GetRequest(HttpResponseMessage responseMessage)
        {
            return responseMessage.RequestMessage.Content != null
                ? await responseMessage.RequestMessage.Content.ReadAsStringAsync()
                : string.Empty;
        }

        Error GetError(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:
                    return new ErrorImpl("RabbitMQ server did not recognize the request due to malformed syntax.");
                case HttpStatusCode.Forbidden:
                    return new ErrorImpl("RabbitMQ server rejected the request.");
                case HttpStatusCode.InternalServerError:
                    return new ErrorImpl("Internal error happened on RabbitMQ server.");
                case HttpStatusCode.RequestTimeout:
                    return new ErrorImpl("No response from the RabbitMQ server within the specified window of time.");
                case HttpStatusCode.ServiceUnavailable:
                    return new ErrorImpl("RabbitMQ server temporarily not able to handle request");
                case HttpStatusCode.Unauthorized:
                    return new ErrorImpl("Unauthorized access to RabbitMQ server resource.");
                default:
                    return null;
            }
        }

        
        class DebugInfoImpl :
            DebugInfo
        {
            public DebugInfoImpl(string request)
            {
                Request = request;
            }

            public string Request { get; }
        }


        protected class ErrorImpl :
            Error
        {
            public ErrorImpl(string reason)
            {
                Reason = reason;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public string Reason { get; }
            public DateTimeOffset Timestamp { get; }
        }
    }
}