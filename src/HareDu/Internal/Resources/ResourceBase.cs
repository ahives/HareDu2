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
    using Configuration;
    using Extensions;
    using Newtonsoft.Json;
    using Serialization;

    internal class ResourceBase
    {
        readonly HttpClient _client;

        protected ResourceBase(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        protected async Task<Result<T>> Get<T>(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var responseMessage = await _client.GetAsync(url, cancellationToken);
                var response = await DeserializeResponse<T>(responseMessage);
                var request = await GetRequest(responseMessage);
                var error = GetError(responseMessage.StatusCode);
                
                return new ResultImpl<T>(response, error.IsNull() ? new List<Error>() : new List<Error>{error}, new DebugInfoImpl(request));
            }
            catch (MissingMethodException e)
            {
                return new ResultImpl<T>(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
        }

        protected async Task<Result<T>> Delete<T>(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var responseMessage = await _client.DeleteAsync(url, cancellationToken);
                var response = await DeserializeResponse<T>(responseMessage);
                var request = await GetRequest(responseMessage);
                var error = GetError(responseMessage.StatusCode);

                return Result.None<T>(new DebugInfoImpl(request), new List<Error>{error});
            }
            catch (MissingMethodException e)
            {
                return new ResultImpl<T>(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
        }

        protected async Task<Result<TResult>> Put<TValue, TResult>(string url, TValue value, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var responseMessage = await _client.PutAsJsonAsync(url, value, cancellationToken);
                var response = await DeserializeResponse<TResult>(responseMessage);
                var request = await GetRequest(responseMessage);
                var error = GetError(responseMessage.StatusCode);

                return Result.None<TResult>(new DebugInfoImpl(request), error.IsNull() ? new List<Error>() : new List<Error>{error});
            }
            catch (MissingMethodException e)
            {
                return new ResultImpl<TResult>(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
        }

        protected async Task<Result<TResult>> Post<TValue, TResult>(string url, TValue value, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var responseMessage = await _client.PostAsJsonAsync(url, value, cancellationToken);
                var response = await DeserializeResponse<TResult>(responseMessage);
                var request = await GetRequest(responseMessage);
                var error = GetError(responseMessage.StatusCode);

                return new ResultImpl<TResult>(response, error.IsNull() ? new List<Error>() : new List<Error>{error}, new DebugInfoImpl(request));
            }
            catch (MissingMethodException e)
            {
                return new ResultImpl<TResult>(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
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
                case HttpStatusCode.BadGateway:
                    return new ErrorImpl("");
                case HttpStatusCode.BadRequest:
                    return new ErrorImpl("");
                case HttpStatusCode.Forbidden:
                    return new ErrorImpl("");
                case HttpStatusCode.InternalServerError:
                    return new ErrorImpl("");
                case HttpStatusCode.RequestTimeout:
                    return new ErrorImpl("");
                case HttpStatusCode.ServiceUnavailable:
                    return new ErrorImpl("");
                case HttpStatusCode.Unauthorized:
                    return new ErrorImpl("");
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

        
        class ResultImpl<T> :
            Result<T>
        {
            public ResultImpl(T data, IEnumerable<Error> errors, DebugInfo debugInfo)
            {
                Data = data;
                Timestamp = DateTimeOffset.UtcNow;
                DebugInfo = debugInfo;
                Errors = errors;
                HasResult = !Data.IsNull();
            }
        
            public ResultImpl(IEnumerable<Error> errors)
            {
                Data = default;
                Timestamp = DateTimeOffset.UtcNow;
                DebugInfo = default;
                Errors = errors;
                HasResult = !Data.IsNull();
            }

            public T Data { get; }
            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IEnumerable<Error> Errors { get; }
            public bool HasResult { get; }
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