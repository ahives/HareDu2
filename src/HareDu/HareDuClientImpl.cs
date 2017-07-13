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