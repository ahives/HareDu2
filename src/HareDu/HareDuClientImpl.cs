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

        public TResource Factory<TResource>(Action<UserCredentials> userCredentials)
            where TResource : Resource
        {
            var userCreds = new UserCredentialsImpl();
            userCredentials(userCreds);

            if (string.IsNullOrWhiteSpace(userCreds?.Username) || string.IsNullOrWhiteSpace(userCreds?.Password))
                throw new UserCredentialsMissingException("Username or password was missing.");
            
            var uri = new Uri($"{_settings.Host}/");
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(userCreds.Username, userCreds.Password)
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

        
        class UserCredentialsImpl :
            UserCredentials
        {
            public string Username { get; private set; }
            public string Password { get; private set; }
            
            public void Credentials(string username, string password)
            {
                Username = username;
                Password = password;
            }
        }
    }
}