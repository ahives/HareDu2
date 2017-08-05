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
namespace HareDu.Internal.Resources
{
    using System;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Polly;
    using Polly.Retry;

    internal class ResourceBase :
        Logging
    {
        readonly HttpClient _client;
        readonly HareDuClientSettings _settings;
        readonly RetryPolicy<Task<HttpResponseMessage>> _policy;

        protected ResourceBase(HttpClient client, HareDuClientSettings settings)
            : base(settings.LoggerName, settings.Logger, settings.EnableLogger)
        {
            _client = client;
            _settings = settings;
            _policy = Policy<Task<HttpResponseMessage>>
                .Handle<HttpRequestException>()
                .Retry(_settings.RetryLimit, (e, i) => LogError(e.Exception));
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

        protected virtual async Task<HttpResponseMessage> HttpGet(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                return _settings.EnableTransientRetry
                    ? await _policy.Execute(() => _client.GetAsync(url, cancellationToken))
                    : await _client.GetAsync(url, cancellationToken);
            }
            catch (Exception e)
            {
                LogError(e);
                throw;
            }
        }

        protected virtual async Task<HttpResponseMessage> HttpDelete(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                return _settings.EnableTransientRetry
                    ? await _policy.Execute(() => _client.DeleteAsync(url, cancellationToken))
                    : await _client.DeleteAsync(url, cancellationToken);
            }
            catch (Exception e)
            {
                LogError(e);
                throw;
            }
        }

        protected virtual async Task<HttpResponseMessage> HttpPut<T>(string url, T value, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                return _settings.EnableTransientRetry
                    ? await _policy.Execute(() => _client.PutAsJsonAsync(url, value, cancellationToken))
                    : await _client.PutAsJsonAsync(url, value, cancellationToken);
            }
            catch (Exception e)
            {
                LogError(e);
                throw;
            }
        }

        protected virtual async Task<HttpResponseMessage> HttpPost<T>(string url, T value, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                return _settings.EnableTransientRetry
                    ? await _policy.Execute(() => _client.PostAsJsonAsync(url, value, cancellationToken))
                    : await _client.PostAsJsonAsync(url, value, cancellationToken);
            }
            catch (Exception e)
            {
                LogError(e);
                throw;
            }
        }
    }
}