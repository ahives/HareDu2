// Copyright 2013-2020 Albert L. Hives
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
namespace HareDu.Shovel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Internal;

    public class ShovelFactory :
        BaseBrokerObject,
        IShovelFactory
    {
        public ShovelFactory(HttpClient client)
            : base(client)
        {
        }

        public async Task<Result> Shovel(Action<ShovelCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new ShovelCreateActionImpl();
            action(impl);

            string request = impl.Request.Value;

            string url = $"/api/parameters/shovel/{impl.VirtualHostName.Value}/{impl.ShovelName.Value}";

            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, request));
            
//            Result result = await Put(url, request, cancellationToken);
            
            Result result = new SuccessfulResult(new DebugInfoImpl(url, request));

            return result;
        }

        
        class ShovelDefinitionImpl<T> :
            ShovelDefinition<T>
        {
            public ShovelDefinitionImpl(T value)
            {
                Definition = value;
            }

            public T Definition { get; }
        }

        
        class ShovelCreateActionImpl :
            ShovelCreateAction
        {
            string _name;
            string _vhost;
            int _reconnectDelay;
            string _acknowledgementMode;
            ShovelProtocol _sourceProtocol;
            ShovelProtocol _destinationProtocol;
            string _sourceQueue;
            string _destinationQueue;
            string _sourceExchange;
            string _destinationExchange;
            bool _destinationAddTimestampMessageHeader;
            string _sourceDeleteShovelAfter;
            string _sourceExchangeRoutingKey;
            string _destinationExchangeRoutingKey;
            bool _destinationAddForwardMessageHeaders;
            int _sourceQueuePrefetchCount;
            string _sourceUri;
            string _destinationUri;
            readonly List<Error> _errors;

            public Lazy<string> ShovelName { get; }
            public Lazy<string> VirtualHostName { get; }
            public Lazy<List<Error>> Errors { get; }
            public Lazy<string> Request { get; }

            public ShovelCreateActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Request = new Lazy<string>(GetRequest, LazyThreadSafetyMode.PublicationOnly);
                ShovelName = new Lazy<string>(() => _name, LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Configure(Action<ShovelConfiguration> configuration)
            {
                var impl = new ShovelConfigurationImpl();
                configuration(impl);

                _name = impl.ShovelName.Value;
                _vhost = impl.VirtualHostName.Value;
                _reconnectDelay = impl.ReconnectDelayInSeconds.Value;
                _acknowledgementMode = impl.AckMode.Value;
                
                if (string.IsNullOrWhiteSpace(_name))
                    _errors.Add(new ErrorImpl("The name of the shovel is missing."));
                
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The virtual host is missing."));
            }

            public void Source(Action<ShovelSource> configuration)
            {
                var impl = new ShovelSourceImpl();
                configuration(impl);

                _sourceProtocol = impl.ShovelProtocol.Value;
                _sourceQueue = impl.QueueName.Value;
                _sourceExchange = impl.ExchangeName.Value;
                _sourceDeleteShovelAfter = impl.DeleteShovelAfter.Value;
                _sourceExchangeRoutingKey = impl.ExchangeRoutingKey.Value;
                _sourceQueuePrefetchCount = impl.QueuePrefetchCount.Value;
                _sourceUri = impl.UriString.Value;

                if (string.IsNullOrWhiteSpace(_sourceQueue) && string.IsNullOrWhiteSpace(_sourceExchange))
                {
                    _errors.Add(new ErrorImpl("The shovel source (i.e. queue or exchange) is missing."));

                    if (string.IsNullOrWhiteSpace(_sourceExchangeRoutingKey))
                        _errors.Add(new ErrorImpl("The exchange routing key is missing."));
                }
                
                if (_sourceQueuePrefetchCount < 1)
                    _errors.Add(new ErrorImpl("The queue prefetch count should be greater or equal to 1."));

                if (string.IsNullOrWhiteSpace(_sourceUri))
                    _errors.Add(new ErrorImpl("The source shovel URI is missing."));
            }

            public void Destination(Action<ShovelDestination> configuration)
            {
                var impl = new ShovelDestinationImpl();
                configuration(impl);

                _destinationProtocol = impl.ShovelProtocol.Value;
                _destinationQueue = impl.QueueName.Value;
                _destinationExchange = impl.ExchangeName.Value;
                _destinationAddTimestampMessageHeader = impl.AddTimestampMessageHeader.Value;
                _destinationExchangeRoutingKey = impl.ExchangeRoutingKey.Value;
                _destinationAddForwardMessageHeaders = impl.AddForwardMessageHeaders.Value;
                _destinationUri = impl.UriString.Value;

                if (string.IsNullOrWhiteSpace(_destinationQueue) && string.IsNullOrWhiteSpace(_destinationExchange))
                {
                    _errors.Add(new ErrorImpl("The shovel destination (i.e. queue or exchange) is missing."));

                    if (string.IsNullOrWhiteSpace(_destinationExchangeRoutingKey))
                        _errors.Add(new ErrorImpl("The exchange routing key is missing."));
                }

                if (string.IsNullOrWhiteSpace(_destinationUri))
                    _errors.Add(new ErrorImpl("The destination shovel URI is missing."));
            }

            string GetRequest()
            {
                if (_sourceProtocol == ShovelProtocol.AMQP_10)
                {
                    Amqp10Definition @params = new Amqp10DefinitionImpl(
                        _sourceProtocol,
                        _destinationProtocol,
                        _acknowledgementMode,
                        _reconnectDelay,
                        _sourceQueuePrefetchCount,
                        _sourceDeleteShovelAfter,
                        _sourceUri,
                        _destinationUri,
                        _destinationAddForwardMessageHeaders,
                        _destinationAddTimestampMessageHeader);

                    ShovelDefinition<Amqp10Definition> definition =
                        new ShovelDefinitionImpl<Amqp10Definition>(@params);

                    return definition.ToJsonString();
                }
                else
                {
                    Amqp091Definition @params = new Amqp091DefinitionImpl(
                        _sourceProtocol,
                        _destinationProtocol,
                        _acknowledgementMode,
                        _reconnectDelay,
                        _sourceQueuePrefetchCount,
                        _sourceQueue,
                        _destinationQueue,
                        _sourceExchange,
                        _destinationExchange,
                        _sourceExchangeRoutingKey,
                        _destinationExchangeRoutingKey,
                        _sourceDeleteShovelAfter,
                        _sourceUri,
                        _destinationUri,
                        _destinationAddForwardMessageHeaders,
                        _destinationAddTimestampMessageHeader);

                    ShovelDefinition<Amqp091Definition> definition =
                        new ShovelDefinitionImpl<Amqp091Definition>(@params);

                    return definition.ToJsonString();
                }
            }

            
            class Amqp10DefinitionImpl :
                Amqp10Definition
            {
                public Amqp10DefinitionImpl(
                    ShovelProtocol sourceProtocol,
                    ShovelProtocol destinationProtocol,
                    string acknowledgementMode,
                    int reconnectDelay,
                    int sourcePrefetchCount,
                    string deleteShovelAfter,
                    string sourceUri,
                    string destinationUri,
                    bool destinationAddForwardHeaders,
                    bool destinationAddTimestampHeader)
                {
                    SourceProtocol = sourceProtocol.ToProtocolString();
                    DestinationProtocol = destinationProtocol.ToProtocolString();
                    AcknowledgementMode = acknowledgementMode;
                    ReconnectDelay = reconnectDelay;
                    SourcePrefetchCount = sourcePrefetchCount;
                    DeleteShovelAfter = deleteShovelAfter;
                    SourceUri = sourceUri;
                    DestinationUri = destinationUri;
                    DestinationAddForwardHeaders = destinationAddForwardHeaders;
                    DestinationAddTimestampHeader = destinationAddTimestampHeader;
                }

                public string SourceProtocol { get; }
                public string DestinationProtocol { get; }
                public string AcknowledgementMode { get; }
                public int ReconnectDelay { get; }
                public int SourcePrefetchCount { get; }
                public string SourceUri { get; }
                public string DestinationUri { get; }
                public string DeleteShovelAfter { get; }
                public bool DestinationAddForwardHeaders { get; }
                public bool DestinationAddTimestampHeader { get; }
            }

            
            class Amqp091DefinitionImpl :
                Amqp091Definition
            {
                public Amqp091DefinitionImpl(
                    ShovelProtocol sourceProtocol,
                    ShovelProtocol destinationProtocol,
                    string acknowledgementMode,
                    int reconnectDelay,
                    int sourcePrefetchCount,
                    string sourceQueue,
                    string destinationQueue,
                    string sourceExchange,
                    string destinationExchange,
                    string sourceExchangeRoutingKey,
                    string destinationExchangeRoutingKey,
                    string deleteShovelAfter,
                    string sourceUri,
                    string destinationUri,
                    bool destinationAddForwardHeaders,
                    bool destinationAddTimestampHeader)
                {
                    SourceProtocol = sourceProtocol.ToProtocolString();
                    DestinationProtocol = destinationProtocol.ToProtocolString();
                    AcknowledgementMode = acknowledgementMode;
                    ReconnectDelay = reconnectDelay;
                    SourcePrefetchCount = sourcePrefetchCount;
                    SourceQueue = sourceQueue;
                    DestinationQueue = destinationQueue;
                    SourceExchange = sourceExchange;
                    DestinationExchange = destinationExchange;
                    SourceExchangeRoutingKey = sourceExchangeRoutingKey;
                    DestinationExchangeRoutingKey = destinationExchangeRoutingKey;
                    DeleteShovelAfter = deleteShovelAfter;
                    SourceUri = sourceUri;
                    DestinationUri = destinationUri;
                    DestinationAddForwardHeaders = destinationAddForwardHeaders;
                    DestinationAddTimestampHeader = destinationAddTimestampHeader;
                }

                public string SourceProtocol { get; }
                public string DestinationProtocol { get; }
                public string AcknowledgementMode { get; }
                public int ReconnectDelay { get; }
                public int SourcePrefetchCount { get; }
                public string SourceQueue { get; }
                public string DestinationQueue { get; }
                public string SourceExchange { get; }
                public string DestinationExchange { get; }
                public string SourceExchangeRoutingKey { get; }
                public string DestinationExchangeRoutingKey { get; }
                public string DeleteShovelAfter { get; }
                public string SourceUri { get; }
                public string DestinationUri { get; }
                public bool DestinationAddForwardHeaders { get; }
                public bool DestinationAddTimestampHeader { get; }
            }


            class ShovelConfigurationImpl :
                ShovelConfiguration
            {
                string _name;
                string _vhost;
                int _reconnectDelay;
                string _acknowledgementMode;

                public Lazy<string> ShovelName { get; }

                public Lazy<string> VirtualHostName { get; }

                public Lazy<int> ReconnectDelayInSeconds { get; }

                public Lazy<string> AckMode { get; }

                public ShovelConfigurationImpl()
                {
                    ShovelName = new Lazy<string>(() => _name, LazyThreadSafetyMode.PublicationOnly);
                    VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                    ReconnectDelayInSeconds = new Lazy<int>(() => _reconnectDelay, LazyThreadSafetyMode.PublicationOnly);
                    AckMode = new Lazy<string>(() => _acknowledgementMode, LazyThreadSafetyMode.PublicationOnly);
                }

                public void Name(string name) => _name = name;

                public void VirtualHost(string vhost) => _vhost = vhost;

                public void ReconnectDelay(int seconds) => _reconnectDelay = seconds;

                public void AcknowledgementMode(string mode) => _acknowledgementMode = mode;
            }

            
            class ShovelDestinationImpl :
                ShovelDestination
            {
                string _uri;
                string _queue;
                string _exchange;
                string _exchangeRoutingKey;
                bool _addForwardHeaders;
                bool _addTimestampHeader;
                ShovelProtocol _protocol;

                public Lazy<ShovelProtocol> ShovelProtocol { get; }
                public Lazy<bool> AddTimestampMessageHeader { get; }
                public Lazy<bool> AddForwardMessageHeaders { get; }
                public Lazy<string> ExchangeRoutingKey { get; }
                public Lazy<string> ExchangeName { get; }
                public Lazy<string> QueueName { get; }
                public Lazy<string> UriString { get; }

                public ShovelDestinationImpl()
                {
                    QueueName = new Lazy<string>(() => _queue, LazyThreadSafetyMode.PublicationOnly);
                    ExchangeName = new Lazy<string>(() => _exchange, LazyThreadSafetyMode.PublicationOnly);
                    ExchangeRoutingKey = new Lazy<string>(() => _exchangeRoutingKey, LazyThreadSafetyMode.PublicationOnly);
                    AddForwardMessageHeaders = new Lazy<bool>(() => _addForwardHeaders, LazyThreadSafetyMode.PublicationOnly);
                    AddTimestampMessageHeader = new Lazy<bool>(() => _addTimestampHeader, LazyThreadSafetyMode.PublicationOnly);
                    UriString = new Lazy<string>(() => _uri, LazyThreadSafetyMode.PublicationOnly);
                    ShovelProtocol = new Lazy<ShovelProtocol>(() => _protocol, LazyThreadSafetyMode.PublicationOnly);
                }

                public void AddForwardHeaders() => _addForwardHeaders = true;

                public void AddTimestampHeader() => _addTimestampHeader = true;

                public void Uri(Action<ShovelUriBuilder> builder)
                {
                    var impl = new ShovelUriBuilderImpl();
                    builder(impl);

                    _uri = impl.Uri.Value;
                }

                public void Queue(string name) => _queue = name;

                public void Exchange(string name, string routingKey)
                {
                    _exchange = name;
                    _exchangeRoutingKey = routingKey;
                }

                public void SetPublishProperties(IDictionary<string, string> properties)
                {
                    throw new NotImplementedException();
                }

                public void Protocol(ShovelProtocol protocol) => _protocol = protocol;
            }

            
            class ShovelSourceImpl :
                ShovelSource
            {
                string _queue;
                string _exchangeRoutingKey;
                string _exchange;
                int _prefetchCount;
                string _deleteAfter;
                string _uri;
                ShovelProtocol _protocol;

                public Lazy<ShovelProtocol> ShovelProtocol { get; }
                public Lazy<string> DeleteShovelAfter { get; }
                public Lazy<int> QueuePrefetchCount { get; }
                public Lazy<string> ExchangeRoutingKey { get; }
                public Lazy<string> ExchangeName { get; }
                public Lazy<string> QueueName { get; }
                public Lazy<string> UriString { get; }

                public ShovelSourceImpl()
                {
                    QueueName = new Lazy<string>(() => _queue, LazyThreadSafetyMode.PublicationOnly);
                    ExchangeName = new Lazy<string>(() => _exchange, LazyThreadSafetyMode.PublicationOnly);
                    ExchangeRoutingKey =
                        new Lazy<string>(() => _exchangeRoutingKey, LazyThreadSafetyMode.PublicationOnly);
                    QueuePrefetchCount = new Lazy<int>(() => _prefetchCount, LazyThreadSafetyMode.PublicationOnly);
                    DeleteShovelAfter = new Lazy<string>(() => _deleteAfter, LazyThreadSafetyMode.PublicationOnly);
                    UriString = new Lazy<string>(() => _uri, LazyThreadSafetyMode.PublicationOnly);
                    ShovelProtocol = new Lazy<ShovelProtocol>(() => _protocol, LazyThreadSafetyMode.PublicationOnly);
                }

                public void Uri(Action<ShovelUriBuilder> builder)
                {
                    var impl = new ShovelUriBuilderImpl();
                    builder(impl);

                    _uri = impl.Uri.Value;
                }

                public void Queue(string name) => _queue = name;

                public void Exchange(string name, string routingKey)
                {
                    _exchange = name;
                    _exchangeRoutingKey = routingKey;
                }

                public void PrefetchCount(int prefetchCount) => _prefetchCount = prefetchCount;

                public void DeleteAfter(string value) => _deleteAfter = value;

                public void Protocol(ShovelProtocol protocol) => _protocol = protocol;
            }


            class ShovelUriBuilderImpl :
                ShovelUriBuilder
            {
                string _uri;

                public Lazy<string> Uri { get; }

                public ShovelUriBuilderImpl()
                {
                    Uri = new Lazy<string>(() => _uri, LazyThreadSafetyMode.PublicationOnly);
                }

                public void Build(Action<ShovelUri> builder)
                {
                    var impl = new ShovelUriImpl();
                    builder(impl);

                    var uri = new StringBuilder("amqp://");

                    if (!string.IsNullOrWhiteSpace(impl.Username.Value))
                    {
                        uri.Append(impl.Username.Value);

                        uri.Append(!string.IsNullOrWhiteSpace(impl.Password.Value)
                            ? $":{impl.Password.Value}@"
                            : "@");
                    }
                    else
                    {
                        if (impl.Username.Value == "\"\"" && impl.Password.Value == "\"\"")
                            uri.Append(":@");
                    }

                    if (!string.IsNullOrWhiteSpace(impl.HostName.Value))
                        uri.Append(impl.HostName.Value);

                    if (impl.Port.Value >= 1)
                        uri.Append($":{impl.Port.Value}");

                    if (!string.IsNullOrWhiteSpace(impl.VirtualHost.Value))
                    {
                        uri.Append(impl.VirtualHost.Value);

                        switch (impl.VirtualHost.Value)
                        {
                            case "\"\"":
                                uri.Append("/");
                                break;
                            case "/":
                                uri.Append("%2f");
                                break;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(impl.CertificateAuthority.Value))
                        uri.Append($"&cacertfile={impl.CertificateAuthority.Value}");

                    if (!string.IsNullOrWhiteSpace(impl.CertificateFile.Value))
                        uri.Append($"&certfile={impl.CertificateFile.Value}");

                    if (!string.IsNullOrWhiteSpace(impl.KeyFile.Value))
                        uri.Append($"&keyfile={impl.KeyFile.Value}");

                    if (!string.IsNullOrWhiteSpace(impl.VerifyCertificate.Value))
                    {
                        uri.Append($"&verify={impl.VerifyCertificate.Value}");

                        if (!string.IsNullOrWhiteSpace(impl.ServerNameIndication.Value))
                            uri.Append($"&server_name_indication={impl.ServerNameIndication.Value}");
                    }

                    if (impl.ChannelMax.Value >= 1)
                        uri.Append($"&channel_max={impl.ChannelMax.Value}");

                    if (impl.ConnectionTimeout.Value >= 1)
                        uri.Append($"&connection_timeout={impl.ConnectionTimeout.Value}");

                    if (impl.HeartbeatTimeout.Value >= 1)
                        uri.Append($"&heartbeat={impl.HeartbeatTimeout.Value}");

                    foreach (var mechanism in impl.SaslAuthMechanisms.Value)
                    {
                        if (!string.IsNullOrWhiteSpace(mechanism))
                            uri.Append($"&auth_mechanism={mechanism}");
                    }

                    _uri = uri.ToString();
                }


                class ShovelUriImpl :
                    ShovelUri
                {
                    string _vhost;
                    string _host;
                    int _port;
                    string _username;
                    string _password;
                    string _certificateAuthority;
                    int _heartbeatTimeout;
                    int _channelMax;
                    int _connectionTimeout;
                    string _serverNameIndication;
                    int _idleTimeout;
                    string _certificateFile;
                    string _keyFile;
                    string _verifyCertificate;
                    readonly List<string> _saslAuthMechanism;

                    public Lazy<string> VirtualHost { get; }
                    public Lazy<List<string>> SaslAuthMechanisms { get; }
                    public Lazy<int> IdleTimeout { get; }
                    public Lazy<string> ServerNameIndication { get; }
                    public Lazy<int> ConnectionTimeout { get; }
                    public Lazy<int> ChannelMax { get; }
                    public Lazy<int> HeartbeatTimeout { get; }
                    public Lazy<string> CertificateAuthority { get; }
                    public Lazy<string> Password { get; }
                    public Lazy<string> Username { get; }
                    public Lazy<int> Port { get; }
                    public Lazy<string> HostName { get; }
                    public Lazy<string> CertificateFile { get; }
                    public Lazy<string> KeyFile { get; }
                    public Lazy<string> VerifyCertificate { get; }

                    public ShovelUriImpl()
                    {
                        _saslAuthMechanism = new List<string>();

                        VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                        HostName = new Lazy<string>(() => _host, LazyThreadSafetyMode.PublicationOnly);
                        Port = new Lazy<int>(() => _port, LazyThreadSafetyMode.PublicationOnly);
                        Username = new Lazy<string>(() => _username, LazyThreadSafetyMode.PublicationOnly);
                        Password = new Lazy<string>(() => _password, LazyThreadSafetyMode.PublicationOnly);
                        CertificateAuthority = new Lazy<string>(() => _certificateAuthority,
                            LazyThreadSafetyMode.PublicationOnly);
                        HeartbeatTimeout = new Lazy<int>(() => _heartbeatTimeout, LazyThreadSafetyMode.PublicationOnly);
                        ChannelMax = new Lazy<int>(() => _channelMax, LazyThreadSafetyMode.PublicationOnly);
                        ConnectionTimeout =
                            new Lazy<int>(() => _connectionTimeout, LazyThreadSafetyMode.PublicationOnly);
                        ServerNameIndication = new Lazy<string>(() => _serverNameIndication,
                            LazyThreadSafetyMode.PublicationOnly);
                        IdleTimeout = new Lazy<int>(() => _idleTimeout, LazyThreadSafetyMode.PublicationOnly);
                        SaslAuthMechanisms = new Lazy<List<string>>(() => _saslAuthMechanism,
                            LazyThreadSafetyMode.PublicationOnly);
                        CertificateFile =
                            new Lazy<string>(() => _certificateFile, LazyThreadSafetyMode.PublicationOnly);
                        KeyFile = new Lazy<string>(() => _keyFile, LazyThreadSafetyMode.PublicationOnly);
                        VerifyCertificate =
                            new Lazy<string>(() => _verifyCertificate, LazyThreadSafetyMode.PublicationOnly);
                    }

                    public void SetUsername(string username) => _username = username;

                    public void SetPassword(string password) => _password = password;

                    public void SetHost(string host) => _host = host;

                    public void SetPort(int port) => _port = port;

                    public void SetCertificateAuthority(string certificateAuthority) =>
                        _certificateAuthority = certificateAuthority;

                    public void SetCertificateFile(string certificateFile) => _certificateFile = certificateFile;

                    public void SetKeyFile(string keyFile) => _keyFile = keyFile;

                    public void SetSaslAuthenticationMechanism(string saslAuthMechanism) =>
                        _saslAuthMechanism.Add(saslAuthMechanism);

                    public void SetVirtualHost(string vhost) => _vhost = vhost;

                    public void SetIdleTimeout(int timeout) => _idleTimeout = timeout;

                    public void SetVerifyCertificate(string value) => _verifyCertificate = value;

                    public void SetServerNameIndication(string server) => _serverNameIndication = server;

                    public void SetConnectionTimeout(int timeout) => _connectionTimeout = timeout;

                    public void SetChannelMax(int channelMax) => _channelMax = channelMax;

                    public void SetHeartbeat(int timeout) => _heartbeatTimeout = timeout;
                }
            }
        }
    }
}