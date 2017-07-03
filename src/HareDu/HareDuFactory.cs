namespace HareDu
{
    using System;
    using System.Threading;
    using Common.Logging;
    using Exceptions;

    public static class HareDuFactory
    {
        public static HareDuClient New(Action<HareDuClientBehavior> behavior)
        {
            try
            {
                var init = new HareDuClientBehaviorImpl();
                behavior(init);
                var client = new HareDuClientImpl(init.Settings.Value);

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
            public Lazy<ClientSettings> Settings { get; }

            public HareDuClientBehaviorImpl()
            {
                Settings = new Lazy<ClientSettings>(InitClientSettings, LazyThreadSafetyMode.PublicationOnly);
            }

            static ClientSettings InitClientSettings() => new ClientSettingsImpl(_host, _logger, _timeout);

            public void ConnectTo(string host)
            {
                _host = host;
            }

            public void EnableLogging(Action<LoggerSettings> logger)
            {
                if (_logger != null)
                    return;
                
                var loggingCharacteristicsImpl = new LoggerSettingsImpl();
                logger(loggingCharacteristicsImpl);
                    
                _logger = LogManager.GetLogger(loggingCharacteristicsImpl.Target);
            }

            public void TimeoutAfter(TimeSpan timeout)
            {
                _timeout = timeout;
            }

            
            class ClientSettingsImpl :
                ClientSettings
            {
                public ClientSettingsImpl(string host, ILog logger, TimeSpan timeout)
                {
                    Host = host;
                    Logger = logger;
                    Timeout = timeout;
                }

                public string Host { get; }
                public ILog Logger { get; }
                public TimeSpan Timeout { get; }
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