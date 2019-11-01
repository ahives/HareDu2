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
namespace HareDu.Tests.Fakes
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Configuration;
    using Moq;
    using Moq.Protected;
    using NUnit.Framework;

    public class FakeBrokerConnectionClient :
        IBrokerConnectionClient
    {
        readonly string _payload;

        public FakeBrokerConnectionClient(string file)
        {
            _payload = File.ReadAllText($"{TestContext.CurrentContext.TestDirectory}/{file}");
        }

        public FakeBrokerConnectionClient()
        {
            _payload = string.Empty;
        }

        public HttpClient Create(HareDuClientSettings settings)
        {
            var mock = new Mock<HttpMessageHandler>();
            
            mock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(_payload)
                    })
                .Verifiable();
            
            var uri = new Uri($"{settings.BrokerUrl}/");
            var client = new HttpClient(mock.Object){BaseAddress = uri};
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}