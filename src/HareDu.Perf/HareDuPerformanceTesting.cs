namespace HareDu.Perf
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Moq.Protected;

    public class HareDuPerformanceTesting
    {
        protected ServiceCollection GetContainerBuilder(string file)
        {
            var services = new ServiceCollection();

            services.AddSingleton<IBrokerObjectFactory>(x =>
            {
                string data = File.ReadAllText($"{Environment.CurrentDirectory}/{file}");

                return new BrokerObjectFactory(GetClient(data));
            });
            
            return services;
        }

        protected ServiceCollection GetContainerBuilder()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IBrokerObjectFactory>(x => new BrokerObjectFactory(GetClient(string.Empty)));

            return services;
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