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
namespace HareDu.Tests
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using Core.Configuration;
    using HareDu.Registration;
    using Moq;
    using Moq.Protected;
    using NUnit.Framework;

    public class HareDuTesting
    {
        protected ContainerBuilder GetContainerBuilder(string path)
        {
            var builder = new ContainerBuilder();

            builder.Register(x =>
                {
                    string data = File.ReadAllText($"{TestContext.CurrentContext.TestDirectory}/{path}");
                    
                    return new BrokerObjectFactory(GetClient(data));
                })
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.RegisterType<BrokerConfigProvider>()
                .As<IBrokerConfigProvider>()
                .SingleInstance();

            builder.RegisterType<YamlConfigProvider>()
                .As<IFileConfigProvider>()
                .SingleInstance();

            return builder;
        }

        protected ContainerBuilder GetContainerBuilder()
        {
            var builder = new ContainerBuilder();

            builder.Register(x => new BrokerObjectFactory(GetClient(string.Empty)))
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.RegisterType<BrokerConfigProvider>()
                .As<IBrokerConfigProvider>()
                .SingleInstance();

            builder.RegisterType<YamlConfigProvider>()
                .As<IFileConfigProvider>()
                .SingleInstance();

            return builder;
        }
        
        protected HttpClient GetClient(string data)
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
                        Content = new StringContent(data)
                    })
                .Verifiable();
            
            var uri = new Uri("http://localhost:15672/");
            var client = new HttpClient(mock.Object){BaseAddress = uri};
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}