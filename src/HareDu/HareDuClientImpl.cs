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
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Exceptions;
    using Internal.Resources;

    internal class HareDuClientImpl :
        Logging,
        HareDuClient
    {
        readonly ClientSettings _settings;
        
        public HttpClient Client { get; private set; }

        public HareDuClientImpl(ClientSettings settings)
            : base(settings.Logger)
        {
            _settings = settings;
        }

        public TResource Factory<TResource>()
            where TResource : Resource
        {
            var uri = new Uri($"{_settings.Host}/");
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(_settings.Credentials.Username, _settings.Credentials.Password)
            };
            
            var client = new HttpClient(handler){BaseAddress = uri};
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (_settings.Timeout != TimeSpan.Zero)
                client.Timeout = _settings.Timeout;

            Client = client;

            var type = typeof(TResource);
            var impl = GetType().Assembly.GetTypes().FirstOrDefault(x => type.IsAssignableFrom(x) && !x.IsInterface);

            if (impl == null)
                throw new HareDuResourceInitException($"Failed to find implementation class for interface {typeof(TResource)}");
            
            return (TResource)Activator.CreateInstance(impl, client, _settings.Logger);
        }

        public void CancelPendingRequest()
        {
            LogInfo("Cancelling all pending requests.");

            Client.CancelPendingRequests();
        }
    }
}