namespace HareDu
{
    using System;
    using System.Threading;
    using Common.Logging;
    using Exceptions;

    public static class HareDuFactory
    {
        public static HareDuClient Create(Action<HareDuClientBehavior> behavior)
        {
            try
            {
                var init = new HareDuClientBehaviorImpl();
                behavior(init);

                var settings = init.Settings.Value;

                if (string.IsNullOrWhiteSpace(settings.Host))
                    throw new HostUrlMissingException("Host URL was missing.");
                
                if (string.IsNullOrWhiteSpace(settings.Credentials.Username) || string.IsNullOrWhiteSpace(settings.Credentials.Password))
                    throw new UserCredentialsMissingException("Username and/or password was missing.");

                var client = new HareDuClientImpl(settings);

                return client;
            }
            catch (Exception e)
            {
                throw new HareDuClientInitException("Unable to initialize the HareDu client.", e);
            }
        }


        class HareDuClientBehaviorImpl :
            HareDuClientBehavior
        {
            static string _host;
            static ILog _logger;
            static TimeSpan _timeout;
            static HareDuCredentials _credentials;
            
            public Lazy<ClientSettings> Settings { get; }

            public HareDuClientBehaviorImpl() => Settings = new Lazy<ClientSettings>(InitClientSettings, LazyThreadSafetyMode.PublicationOnly);

            static ClientSettings InitClientSettings() => new ClientSettingsImpl(_host, _logger, _timeout, _credentials);

            public void ConnectTo(string host) => _host = host;

            public void EnableLogging(Action<LoggerSettings> logger)
            {
                if (_logger != null)
                    return;
                
                var loggingCharacteristicsImpl = new LoggerSettingsImpl();
                logger(loggingCharacteristicsImpl);
                    
                _logger = LogManager.GetLogger(loggingCharacteristicsImpl.Target);
            }

            public void TimeoutAfter(TimeSpan timeout) => _timeout = timeout;

            public void UsingCredentials(string username, string password) =>
                _credentials = new HareDuCredentialsImpl(username, password);

            
            class HareDuCredentialsImpl :
                HareDuCredentials
            {
                public HareDuCredentialsImpl(string username, string password)
                {
                    Username = username;
                    Password = password;
                }

                public string Username { get; }
                public string Password { get; }
            }


            class ClientSettingsImpl :
                ClientSettings
            {
                public ClientSettingsImpl(string host, ILog logger, TimeSpan timeout, HareDuCredentials credentials)
                {
                    Host = host;
                    Logger = logger;
                    Timeout = timeout;
                    Credentials = credentials;
                }

                public string Host { get; }
                public ILog Logger { get; }
                public TimeSpan Timeout { get; }
                public HareDuCredentials Credentials { get; }
            }


            class LoggerSettingsImpl :
                LoggerSettings
            {
                public string Target { get; private set; }

                public void Logger(string name)
                {
                    Target = name;
                }
            }
        }
    }
}