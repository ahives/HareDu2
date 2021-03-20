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

    public class BaseBrokerObject
    {
        readonly HttpClient _client;

        protected BaseBrokerObject(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
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
                
                var data = await response.ToObject<List<T>>();
                
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

                var data = await response.ToObject<T>();
                
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

                string request = value.ToJsonString();
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

                string request = value.ToJsonString();
                byte[] requestBytes = Encoding.UTF8.GetBytes(request);
                var content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);
                
                if (!response.IsSuccessStatusCode)
                    return new FaultedResult<T>(new List<Error> { GetError(response.StatusCode) }, new DebugInfoImpl(url, request));

                var data = await response.ToObject<T>().ConfigureAwait(false);

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

                string request = value.ToJsonString();
                byte[] requestBytes = Encoding.UTF8.GetBytes(request);
                var content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);
                
                if (!response.IsSuccessStatusCode)
                    return new FaultedResultList<T>(new List<Error> { GetError(response.StatusCode) }, new DebugInfoImpl(url, request));

                var data = await response.ToObject<List<T>>();

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