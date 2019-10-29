// Copyright 2013-2019 Albert L. Hives
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
namespace HareDu.Core
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensions;

    public class RmqBrokerClient
    {
        readonly HttpClient _client;

        protected RmqBrokerClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        protected async Task<ResultList<T>> GetAll<T>(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var responseMessage = await _client.GetAsync(url, cancellationToken);
                
                if (!responseMessage.IsSuccessStatusCode)
                    return new FaultedResultList<T>(new List<Error> { GetError(responseMessage.StatusCode) }, new DebugInfoImpl(url, null));
                
                var data = await responseMessage.DeserializeResponse<List<T>>();
                
                return new SuccessfulResultList<T>(data, new DebugInfoImpl(url, null));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResultList<T>(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
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
                    return new FaultedResult<T>(new List<Error> { GetError(responseMessage.StatusCode) }, new DebugInfoImpl(url, null));

                var data = await responseMessage.DeserializeResponse<T>();
                
                return new SuccessfulResult<T>(data, new DebugInfoImpl(url, null));
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
                
                if (!responseMessage.IsSuccessStatusCode)
                    return new FaultedResult(new List<Error> { GetError(responseMessage.StatusCode) }, new DebugInfoImpl(url, null));

                return new SuccessfulResult(new DebugInfoImpl(url, null));
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

                string request = value.ToJsonString();
                byte[] requestBytes = Encoding.UTF8.GetBytes(request);
                var content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseMessage = await _client.PutAsync(url, content, cancellationToken);
                
                if (!responseMessage.IsSuccessStatusCode)
                    return new FaultedResult(new List<Error> { GetError(responseMessage.StatusCode) }, new DebugInfoImpl(url, request));

                return new SuccessfulResult(new DebugInfoImpl(url, request));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
        }

        protected async Task<Result> Put(string url, string request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                byte[] requestBytes = Encoding.UTF8.GetBytes(request);
                var content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseMessage = await _client.PutAsync(url, content, cancellationToken);
                
                if (!responseMessage.IsSuccessStatusCode)
                    return new FaultedResult(new List<Error> { GetError(responseMessage.StatusCode) }, new DebugInfoImpl(url, request));

                return new SuccessfulResult(new DebugInfoImpl(url, request));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
        }

        protected async Task<Result<T>> Post<T, TValue>(string url, TValue value, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                string request = value.ToJsonString();
                byte[] requestBytes = Encoding.UTF8.GetBytes(request);
                var content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseMessage = await _client.PostAsync(url, content, cancellationToken);
                
                if (!responseMessage.IsSuccessStatusCode)
                    return new FaultedResult<T>(new List<Error> { GetError(responseMessage.StatusCode) }, new DebugInfoImpl(url, request));

                var data = await responseMessage.DeserializeResponse<T>();

                return new SuccessfulResult<T>(data, new DebugInfoImpl(url, request));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult<T>(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
        }

        protected async Task<Result> PostEmpty(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var responseMessage = await _client.PostAsync(url, null, cancellationToken);
                
                if (!responseMessage.IsSuccessStatusCode)
                    return new FaultedResult(new List<Error> { GetError(responseMessage.StatusCode) }, new DebugInfoImpl(url, null));

                return new SuccessfulResult(new DebugInfoImpl(url, null));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
        }

        void HandleDotsAndSlashes()
        {
            var getSyntaxMethod = typeof(UriParser).GetMethod("GetSyntax", BindingFlags.Static | BindingFlags.NonPublic);
            if (getSyntaxMethod == null)
                throw new MissingMethodException("UriParser", "GetSyntax");

            var uriParser = getSyntaxMethod.Invoke(null, new object[] {"http"});

            var setUpdatableFlagsMethod = uriParser
                .GetType()
                .GetMethod("SetUpdatableFlags", BindingFlags.Instance | BindingFlags.NonPublic);
            
            if (setUpdatableFlagsMethod == null)
                throw new MissingMethodException("UriParser", "SetUpdatableFlags");

            setUpdatableFlagsMethod.Invoke(uriParser, new object[] {0});
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

        
        protected class DebugInfoImpl :
            DebugInfo
        {
            public DebugInfoImpl(string url, string request)
            {
                URL = url;
                Request = request;
            }

            public string URL { get; }
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