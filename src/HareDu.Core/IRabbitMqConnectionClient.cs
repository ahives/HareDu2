namespace HareDu.Core
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Configuration;

    public interface IRabbitMqConnectionClient
    {
        HttpClient Create(HareDuClientSettings settings);
    }

    public class RabbitMqConnectionClient : IRabbitMqConnectionClient
    {
        static IRabbitMqConnectionClient _instance = null;
        static readonly object _gate = new object();

        public RabbitMqConnectionClient()
        {
        }

        public static IRabbitMqConnectionClient Instance
        {
            get
            {
                lock (_gate)
                {
                    return _instance ?? (_instance = new RabbitMqConnectionClient());
                }
            }
        }
        
        public HttpClient Create(HareDuClientSettings settings)
        {
            var uri = new Uri($"{settings.RabbitMqServerUrl}/");
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