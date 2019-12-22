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
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Configuration;

    public class BrokerConnectionClient :
        IBrokerConnectionClient
    {
        public HttpClient Create(BrokerConfig settings)
        {
            var uri = new Uri($"{settings.BrokerUrl}/");
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(settings.Credentials.Username, settings.Credentials.Password)
            };
            
            var client = new HttpClient(handler){BaseAddress = uri};
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (settings.Timeout != TimeSpan.Zero)
                client.Timeout = settings.Timeout;

            return client;
        }
    }
}