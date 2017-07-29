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
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Logging;
    using Exceptions;
    using HareDu.Model;

    internal class ResourceBase :
        Logging
    {
        readonly HttpClient _client;
        readonly ILog _logger;

        protected ResourceBase(HttpClient client, ILog logger)
            : base(logger)
        {
            _client = client;
            _logger = logger;
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

        protected virtual Task<HttpResponseMessage> HttpGet(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                return _client.GetAsync(url, cancellationToken);
            }
            catch (Exception e)
            {
                LogError(e);
                throw;
            }
        }

        protected virtual Task<HttpResponseMessage> HttpDelete(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                return _client.DeleteAsync(url, cancellationToken);
            }
            catch (Exception e)
            {
                LogError(e);
                throw;
            }
        }

        protected virtual Task<HttpResponseMessage> HttpPut<T>(string url, T value, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                return _client.PutAsJsonAsync(url, value, cancellationToken);
            }
            catch (Exception e)
            {
                LogError(e);
                throw;
            }
        }

        protected virtual Task<HttpResponseMessage> HttpPost<T>(string url, T value, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (url.Contains("/%2f"))
                    HandleDotsAndSlashes();

                return _client.PostAsJsonAsync(url, value, cancellationToken);
            }
            catch (Exception e)
            {
                LogError(e);
                throw;
            }
        }
    }
}