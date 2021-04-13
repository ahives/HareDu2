namespace HareDu.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Extensions;
    using Serialization;

    public class BaseBrokerObject
    {
        readonly HttpClient _client;
        readonly IDictionary<string, Error> _errors;

        protected BaseBrokerObject(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _errors = new Dictionary<string, Error>
            {
                {nameof(MissingMethodException), new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.")},
                {nameof(HttpRequestException), new ErrorImpl("Request failed due to network connectivity, DNS failure, server certificate validation, or timeout.")},
                {nameof(JsonException), new ErrorImpl("The JSON is invalid or T is not compatible with the JSON.")},
                {nameof(Exception), new ErrorImpl("Something went bad in BaseBrokerObject.GetAll method.")},
                {nameof(TaskCanceledException), new ErrorImpl("Request failed due to timeout.")}
            };
        }

        protected async Task<ResultList<T>> GetAll<T>(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var response = await _client.GetAsync(url, cancellationToken).ConfigureAwait(false);
                
                if (!response.IsSuccessStatusCode)
                    return new FaultedResultList<T>(new List<Error> { GetError(response.StatusCode) }, new DebugInfoImpl(url));
                
                var data = await response.ToObject<List<T>>(Deserializer.Options);
                
                return new SuccessfulResultList<T>(data, new DebugInfoImpl(url));
            }
            catch (MissingMethodException)
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

                var response = await _client.GetAsync(url, cancellationToken).ConfigureAwait(false);
                
                if (!response.IsSuccessStatusCode)
                    return new FaultedResult<T>(new List<Error> { GetError(response.StatusCode) }, new DebugInfoImpl(url));

                var data = await response.ToObject<T>(Deserializer.Options);
                
                return new SuccessfulResult<T>(data, new DebugInfoImpl(url));
            }
            catch (MissingMethodException)
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

                var response = await _client.DeleteAsync(url, cancellationToken).ConfigureAwait(false);
                
                if (!response.IsSuccessStatusCode)
                    return new FaultedResult(new List<Error> { GetError(response.StatusCode) }, new DebugInfoImpl(url));

                return new SuccessfulResult(new DebugInfoImpl(url));
            }
            catch (MissingMethodException)
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

                string request = value.ToJsonString(Deserializer.Options);
                byte[] requestBytes = Encoding.UTF8.GetBytes(request);
                var content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _client.PutAsync(url, content, cancellationToken).ConfigureAwait(false);
                
                if (!response.IsSuccessStatusCode)
                    return new FaultedResult(new List<Error> { GetError(response.StatusCode) }, new DebugInfoImpl(url, request));

                return new SuccessfulResult(new DebugInfoImpl(url, request));
            }
            catch (MissingMethodException)
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

                var response = await _client.PutAsync(url, content, cancellationToken).ConfigureAwait(false);
                
                if (!response.IsSuccessStatusCode)
                    return new FaultedResult(new List<Error> { GetError(response.StatusCode) }, new DebugInfoImpl(url, request));

                return new SuccessfulResult(new DebugInfoImpl(url, request));
            }
            catch (MissingMethodException)
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

                string request = value.ToJsonString(Deserializer.Options);
                byte[] requestBytes = Encoding.UTF8.GetBytes(request);
                var content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);
                
                if (!response.IsSuccessStatusCode)
                    return new FaultedResult<T>(new List<Error> { GetError(response.StatusCode) }, new DebugInfoImpl(url, request));

                var data = await response.ToObject<T>(Deserializer.Options).ConfigureAwait(false);

                return new SuccessfulResult<T>(data, new DebugInfoImpl(url, request));
            }
            catch (MissingMethodException)
            {
                return new FaultedResult<T>(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
        }

        protected async Task<ResultList<T>> PostList<T, TValue>(string url, TValue value, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                string request = value.ToJsonString(Deserializer.Options);
                byte[] requestBytes = Encoding.UTF8.GetBytes(request);
                var content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);
                
                if (!response.IsSuccessStatusCode)
                    return new FaultedResultList<T>(new List<Error> { GetError(response.StatusCode) }, new DebugInfoImpl(url, request));

                var data = await response.ToObject<List<T>>(Deserializer.Options);

                return new SuccessfulResultList<T>(data, new DebugInfoImpl(url, request));
            }
            catch (MissingMethodException)
            {
                return new FaultedResultList<T>(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
        }

        protected async Task<Result> PostEmpty(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var response = await _client.PostAsync(url, null, cancellationToken).ConfigureAwait(false);
                
                if (!response.IsSuccessStatusCode)
                    return new FaultedResult(new List<Error> { GetError(response.StatusCode) }, new DebugInfoImpl(url));

                return new SuccessfulResult(new DebugInfoImpl(url));
            }
            catch (MissingMethodException)
            {
                return new FaultedResult(new List<Error>{ new ErrorImpl("Could not properly handle '.' and/or '/' characters in URL.") });
            }
        }

        protected async Task<ResultList<T>> GetAllRequest<T>(string url, CancellationToken cancellationToken = default)
        {
            string rawResponse = null;

            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var response = await _client.GetAsync(url, cancellationToken).ConfigureAwait(false);

                rawResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new FaultedResultList<T>(new DebugInfoImpl(url, rawResponse, new List<Error> {GetError(response.StatusCode)}));

                var data = rawResponse.ToObject<List<T>>(Deserializer.Options);

                return new SuccessfulResultList<T>(data.GetDataOrEmpty(), new DebugInfoImpl(url, rawResponse, new List<Error>()));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResultList<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(MissingMethodException)]}));
            }
            catch (HttpRequestException e)
            {
                return new FaultedResultList<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(HttpRequestException)]}));
            }
            catch (JsonException e)
            {
                return new FaultedResultList<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(JsonException)]}));
            }
            catch (TaskCanceledException e)
            {
                return new FaultedResultList<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(TaskCanceledException)]}));
            }
            catch (Exception e)
            {
                return new FaultedResultList<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(Exception)]}));
            }
        }

        protected async Task<Result<T>> GetRequest<T>(string url, CancellationToken cancellationToken = default)
        {
            string rawResponse = null;

            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var response = await _client.GetAsync(url, cancellationToken).ConfigureAwait(false);

                rawResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new FaultedResult<T>(new DebugInfoImpl(url, rawResponse, new List<Error> {GetError(response.StatusCode)}));

                var data = rawResponse.ToObject<T>(Deserializer.Options);
                
                return new SuccessfulResult<T>(data, new DebugInfoImpl(url, null, rawResponse));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(MissingMethodException)]}));
            }
            catch (HttpRequestException e)
            {
                return new FaultedResult<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(HttpRequestException)]}));
            }
            catch (JsonException e)
            {
                return new FaultedResult<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(JsonException)]}));
            }
            catch (TaskCanceledException e)
            {
                return new FaultedResult<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(TaskCanceledException)]}));
            }
            catch (Exception e)
            {
                return new FaultedResult<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(Exception)]}));
            }
        }

        protected async Task<Result> DeleteRequest(string url, CancellationToken cancellationToken = default)
        {
            string rawResponse = null;

            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var response = await _client.DeleteAsync(url, cancellationToken).ConfigureAwait(false);

                rawResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                    return new FaultedResult(new DebugInfoImpl(url, rawResponse, new List<Error> {GetError(response.StatusCode)}));

                return new SuccessfulResult(new DebugInfoImpl(url, null, rawResponse));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(MissingMethodException)]}));
            }
            catch (HttpRequestException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(HttpRequestException)]}));
            }
            catch (JsonException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(JsonException)]}));
            }
            catch (TaskCanceledException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(TaskCanceledException)]}));
            }
            catch (Exception e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(Exception)]}));
            }
        }

        protected async Task<Result> PutRequest<TRequest>(string url, TRequest request, CancellationToken cancellationToken = default)
        {
            string rawResponse = null;

            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                string requestContent = request.ToJsonString(Deserializer.Options);
                byte[] requestBytes = Encoding.UTF8.GetBytes(requestContent);
                var content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _client.PutAsync(url, content, cancellationToken).ConfigureAwait(false);

                rawResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                    return new FaultedResult(new DebugInfoImpl(url, rawResponse, new List<Error> {GetError(response.StatusCode)}));

                return new SuccessfulResult(new DebugInfoImpl(url, requestContent, rawResponse));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(MissingMethodException)]}));
            }
            catch (HttpRequestException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(HttpRequestException)]}));
            }
            catch (JsonException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(JsonException)]}));
            }
            catch (TaskCanceledException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(TaskCanceledException)]}));
            }
            catch (Exception e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(Exception)]}));
            }
        }

        protected async Task<Result> PutRequest(string url, string request, CancellationToken cancellationToken = default)
        {
            string rawResponse = null;

            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                byte[] requestBytes = Encoding.UTF8.GetBytes(request);
                var content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _client.PutAsync(url, content, cancellationToken).ConfigureAwait(false);

                rawResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                    return new FaultedResult(new DebugInfoImpl(url, rawResponse, new List<Error> {GetError(response.StatusCode)}));

                return new SuccessfulResult(new DebugInfoImpl(url, request, rawResponse));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(MissingMethodException)]}));
            }
            catch (HttpRequestException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(HttpRequestException)]}));
            }
            catch (JsonException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(JsonException)]}));
            }
            catch (TaskCanceledException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(TaskCanceledException)]}));
            }
            catch (Exception e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(Exception)]}));
            }
        }

        protected async Task<Result<T>> PostRequest<T, TRequest>(string url, TRequest request, CancellationToken cancellationToken = default)
        {
            string rawResponse = null;

            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                string requestContent = request.ToJsonString(Deserializer.Options);
                byte[] requestBytes = Encoding.UTF8.GetBytes(requestContent);
                var content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);

                rawResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new FaultedResult<T>(new DebugInfoImpl(url, rawResponse, new List<Error> {GetError(response.StatusCode)}));

                var data = rawResponse.ToObject<T>(Deserializer.Options);

                return new SuccessfulResult<T>(data.GetDataOrDefault(), new DebugInfoImpl(url, requestContent, rawResponse));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(MissingMethodException)]}));
            }
            catch (HttpRequestException e)
            {
                return new FaultedResult<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(HttpRequestException)]}));
            }
            catch (JsonException e)
            {
                return new FaultedResult<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(JsonException)]}));
            }
            catch (TaskCanceledException e)
            {
                return new FaultedResult<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(TaskCanceledException)]}));
            }
            catch (Exception e)
            {
                return new FaultedResult<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(Exception)]}));
            }
        }

        protected async Task<Result> PostRequest<TRequest>(string url, TRequest request, CancellationToken cancellationToken = default)
        {
            string rawResponse = null;

            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                string requestContent = request.ToJsonString(Deserializer.Options);
                byte[] requestBytes = Encoding.UTF8.GetBytes(requestContent);
                var content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);

                rawResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new FaultedResult(new DebugInfoImpl(url, rawResponse, new List<Error> {GetError(response.StatusCode)}));

                return new SuccessfulResult(new DebugInfoImpl(url, requestContent, rawResponse));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(MissingMethodException)]}));
            }
            catch (HttpRequestException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(HttpRequestException)]}));
            }
            catch (JsonException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(JsonException)]}));
            }
            catch (TaskCanceledException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(TaskCanceledException)]}));
            }
            catch (Exception e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(Exception)]}));
            }
        }

        protected async Task<ResultList<T>> PostListRequest<T, TRequest>(string url, TRequest request, CancellationToken cancellationToken = default)
        {
            string rawResponse = null;

            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                string requestContent = request.ToJsonString(Deserializer.Options);
                byte[] requestBytes = Encoding.UTF8.GetBytes(requestContent);
                var content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);

                rawResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                    return new FaultedResultList<T>(new DebugInfoImpl(url, rawResponse, new List<Error> {GetError(response.StatusCode)}));

                var data = rawResponse.ToObject<List<T>>(Deserializer.Options);

                return new SuccessfulResultList<T>(data.GetDataOrEmpty(), new DebugInfoImpl(url, requestContent, rawResponse));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResultList<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(MissingMethodException)]}));
            }
            catch (HttpRequestException e)
            {
                return new FaultedResultList<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(HttpRequestException)]}));
            }
            catch (JsonException e)
            {
                return new FaultedResultList<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(JsonException)]}));
            }
            catch (TaskCanceledException e)
            {
                return new FaultedResultList<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(TaskCanceledException)]}));
            }
            catch (Exception e)
            {
                return new FaultedResultList<T>(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(Exception)]}));
            }
        }

        protected async Task<Result> PostEmptyRequest(string url, CancellationToken cancellationToken = default)
        {
            string rawResponse = null;

            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                var response = await _client.PostAsync(url, null, cancellationToken).ConfigureAwait(false);

                rawResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                    return new FaultedResult(new DebugInfoImpl(url, rawResponse, new List<Error> {GetError(response.StatusCode)}));

                return new SuccessfulResult(new DebugInfoImpl(url, rawResponse, new List<Error>()));
            }
            catch (MissingMethodException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(MissingMethodException)]}));
            }
            catch (HttpRequestException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(HttpRequestException)]}));
            }
            catch (JsonException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(JsonException)]}));
            }
            catch (TaskCanceledException e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(TaskCanceledException)]}));
            }
            catch (Exception e)
            {
                return new FaultedResult(new DebugInfoImpl(url, rawResponse, e.Message, e.StackTrace, new List<Error> {_errors[nameof(Exception)]}));
            }
        }

        void HandleDotsAndSlashes()
        {
            var method = typeof(UriParser).GetMethod("GetSyntax", BindingFlags.Static | BindingFlags.NonPublic);
            if (method.IsNull())
                throw new MissingMethodException("UriParser", "GetSyntax");

            var uriParser = method.Invoke(null, new object[] {"http"});

            var setUpdatableFlagsMethod = uriParser
                .GetType()
                .GetMethod("SetUpdatableFlags", BindingFlags.Instance | BindingFlags.NonPublic);
            
            if (setUpdatableFlagsMethod.IsNull())
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
                
                case HttpStatusCode.NotAcceptable:
                    return new ErrorImpl("RabbitMQ server rejected the request because the method is not acceptable.");
    
                case HttpStatusCode.MethodNotAllowed:
                    return new ErrorImpl("RabbitMQ server rejected the request because the method is not allowed.");
                
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
            
            public DebugInfoImpl(string url)
            {
                URL = url;
            }
            
            public DebugInfoImpl(string url, string request, List<Error> errors)
            {
                URL = url;
                Request = request;
                Errors = errors;
            }

            public DebugInfoImpl(string url, List<Error> errors)
            {
                URL = url;
                Errors = errors;
            }

            public DebugInfoImpl(string url, string request, string exception, string stackTrace, List<Error> errors)
            {
                URL = url;
                Request = request;
                Exception = exception;
                StackTrace = stackTrace;
                Errors = errors;
            }

            public DebugInfoImpl(string url, string request, string response)
            {
                URL = url;
                Response = response;
                Request = request;
                Errors = new List<Error>();
            }

            public DebugInfoImpl(string url, string request, string response, List<Error> errors)
            {
                URL = url;
                Response = response;
                Request = request;
                Errors = errors;
            }

            public string URL { get; }
            public string Request { get; }
            public string Exception { get; }
            public string StackTrace { get; }
            public string Response { get; }
            public IReadOnlyList<Error> Errors { get; }
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