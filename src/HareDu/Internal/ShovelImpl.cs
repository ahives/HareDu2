namespace HareDu.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Extensions;
    using Model;

    public class ShovelImpl :
        BaseBrokerObject,
        Shovel
    {
        public ShovelImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<ShovelInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/shovels";
            
            return await GetAll<ShovelInfo>(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Create(string shovel, string uri, string vhost,
            Action<ShovelConfigurator> configurator, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var errors = new List<Error>();
            
            if (configurator.IsNull())
                errors.Add(new ErrorImpl("The shovel configurator is missing."));

            if (errors.Any())
                return new FaultedResult(errors);
                
            if (string.IsNullOrWhiteSpace(uri))
                errors.Add(new ErrorImpl("The connection URI is missing."));

            var impl = new ShovelConfiguratorImpl(uri);
            configurator?.Invoke(impl);

            impl.Validate();
            
            ShovelRequest request = impl.Request.Value;

            Debug.Assert(request != null);

            errors.AddRange(impl.Errors.Value);
            
            if (string.IsNullOrWhiteSpace(shovel))
                errors.Add(new ErrorImpl("The name of the shovel is missing."));
            
            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            string url = $"api/parameters/shovel/{vhost.ToSanitizedName()}/{shovel}";

            if (errors.Any())
                return new FaultedResult(errors, new DebugInfoImpl(url, request.ToJsonString(), errors));

            return await Put(url, request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(string shovel, string vhost, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(shovel))
                errors.Add(new ErrorImpl("The name of the shovel is missing."));
            
            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            string url = $"api/parameters/shovel/{vhost.ToSanitizedName()}/{shovel}";

            if (errors.Any())
                return new FaultedResult(errors, new DebugInfoImpl(url, errors));

            return await Delete(url, cancellationToken).ConfigureAwait(false);
        }


        class ShovelConfiguratorImpl :
            ShovelConfigurator
        {
            ShovelProtocolType _destinationProtocol;
            ShovelProtocolType _sourceProtocol;
            AckMode? _acknowledgeMode;
            string _destinationQueue;
            string _sourceQueue;
            string _sourceExchangeName;
            string _sourceExchangeRoutingKey;
            string _destinationExchangeName;
            string _destinationExchangeRoutingKey;
            int _reconnectDelay;
            ulong _sourcePrefetchCount;
            bool _destinationAddForwardHeaders;
            bool _destinationAddTimestampHeader;
            bool _sourceCalled;
            bool _destinationCalled;
            object _deleteShovelAfter;

            readonly List<Error> _errors;

            public Lazy<ShovelRequest> Request { get; }
            public Lazy<List<Error>> Errors { get; }

            public ShovelConfiguratorImpl(string uri)
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Request = new Lazy<ShovelRequest>(() =>
                        new ShovelRequestImpl(_acknowledgeMode,
                            _reconnectDelay,
                            _sourceProtocol,
                            uri,
                            _sourceQueue,
                            _sourceExchangeName,
                            _sourceExchangeRoutingKey,
                            _sourcePrefetchCount,
                            _deleteShovelAfter,
                            _destinationProtocol,
                            _destinationExchangeName,
                            _destinationExchangeRoutingKey,
                            _destinationQueue,
                            _destinationAddForwardHeaders,
                            _destinationAddTimestampHeader), LazyThreadSafetyMode.PublicationOnly);
            }

            public void ReconnectDelay(int delayInSeconds) => _reconnectDelay = delayInSeconds < 1 ? 1 : delayInSeconds;

            public void AcknowledgementMode(AckMode mode) => _acknowledgeMode = mode;

            public void Source(string queue, Action<ShovelSourceConfigurator> configurator)
            {
                _sourceCalled = true;
                
                var impl = new ShovelSourceConfiguratorImpl();
                configurator?.Invoke(impl);

                _sourceProtocol = impl.ShovelProtocol;
                _sourceQueue = queue;
                _sourceExchangeName = impl.ExchangeName;
                _sourceExchangeRoutingKey = impl.ExchangeRoutingKey;
                _sourcePrefetchCount = impl.PrefetchCount;
                _deleteShovelAfter = impl.DeleteAfterShovel;
                
                if (string.IsNullOrWhiteSpace(queue) && string.IsNullOrWhiteSpace(impl.ExchangeName))
                    _errors.Add(new ErrorImpl("Both source queue and exchange missing."));
                
                if (!string.IsNullOrWhiteSpace(queue) && !string.IsNullOrWhiteSpace(impl.ExchangeName))
                    _errors.Add(new ErrorImpl("Both source queue and exchange cannot be present."));
            }

            public void Destination(string queue, Action<ShovelDestinationConfigurator> configurator)
            {
                _destinationCalled = true;
                
                var impl = new ShovelDestinationConfiguratorImpl();
                configurator?.Invoke(impl);

                _destinationProtocol = impl.ShovelProtocol;
                _destinationQueue = queue;
                _destinationExchangeName = impl.ExchangeName;
                _destinationExchangeRoutingKey = impl.ExchangeRoutingKey;
                _destinationAddForwardHeaders = impl.AddHeaders;
                _destinationAddTimestampHeader = impl.AddTimestampHeader;
                
                if (string.IsNullOrWhiteSpace(queue) && string.IsNullOrWhiteSpace(impl.ExchangeName))
                    _errors.Add(new ErrorImpl("Both source queue and exchange missing."));
                
                if (!string.IsNullOrWhiteSpace(queue) && !string.IsNullOrWhiteSpace(impl.ExchangeName))
                    _errors.Add(new ErrorImpl("Both destination queue and exchange cannot be present."));
            }

            public void Validate()
            {
                if (!_sourceCalled)
                {
                    _errors.Add(new ErrorImpl("The name of the source protocol is missing."));
                    _errors.Add(new ErrorImpl("Both source queue and exchange cannot be present."));
                }

                if (!_destinationCalled)
                {
                    _errors.Add(new ErrorImpl("The name of the destination protocol is missing."));
                    _errors.Add(new ErrorImpl("Both destination queue and exchange cannot be present."));
                }
            }

            
            class ShovelRequestImpl :
                ShovelRequest
            {
                public ShovelRequestParams Value { get; }

                public ShovelRequestImpl(
                    AckMode? acknowledgeMode,
                    int reconnectDelay,
                    ShovelProtocolType sourceProtocol,
                    string uri,
                    string sourceQueue,
                    string sourceExchangeName,
                    string sourceExchangeRoutingKey,
                    ulong sourcePrefetchCount,
                    object deleteShovelAfter,
                    ShovelProtocolType destinationProtocol,
                    string destinationExchangeName,
                    string destinationExchangeRoutingKey,
                    string destinationQueue,
                    bool destinationAddForwardHeaders,
                    bool destinationAddTimestampHeader)
                {
                    Value = new ShovelRequestParamsImpl(
                        acknowledgeMode,
                        reconnectDelay,
                        sourceProtocol,
                        uri,
                        sourceQueue,
                        sourceExchangeName,
                        sourceExchangeRoutingKey,
                        sourcePrefetchCount,
                        deleteShovelAfter,
                        destinationProtocol,
                        destinationExchangeName,
                        destinationExchangeRoutingKey,
                        destinationQueue,
                        destinationAddForwardHeaders,
                        destinationAddTimestampHeader);
                }

                
                class ShovelRequestParamsImpl :
                    ShovelRequestParams
                {
                    public ShovelRequestParamsImpl(
                        AckMode? acknowledgeMode,
                        int reconnectDelay,
                        ShovelProtocolType sourceProtocol,
                        string uri,
                        string sourceQueue,
                        string sourceExchangeName,
                        string sourceExchangeRoutingKey,
                        ulong sourcePrefetchCount,
                        object deleteShovelAfter,
                        ShovelProtocolType destinationProtocol,
                        string destinationExchangeName,
                        string destinationExchangeRoutingKey,
                        string destinationQueue,
                        bool destinationAddForwardHeaders,
                        bool destinationAddTimestampHeader)
                    {
                        AcknowledgeMode = acknowledgeMode.IsNotNull() ? acknowledgeMode.ConvertTo() : AckMode.OnConfirm.ConvertTo();
                        ReconnectDelay = reconnectDelay;
                        SourceProtocol = sourceProtocol.ConvertTo();
                        SourceUri = uri;
                        SourceQueue = sourceQueue;
                        SourceExchange = sourceExchangeName;
                        SourceExchangeRoutingKey = sourceExchangeRoutingKey;
                        SourcePrefetchCount = sourcePrefetchCount;
                        SourceDeleteAfter = deleteShovelAfter;
                        DestinationProtocol = destinationProtocol.ConvertTo();
                        DestinationExchange = destinationExchangeName;
                        DestinationExchangeKey = destinationExchangeRoutingKey;
                        DestinationUri = uri;
                        DestinationQueue = destinationQueue;
                        DestinationAddForwardHeaders = destinationAddForwardHeaders;
                        DestinationAddTimestampHeader = destinationAddTimestampHeader;
                    }

                    public string SourceProtocol { get; }
                    public string SourceUri { get; }
                    public string SourceQueue { get; }
                    public string DestinationProtocol { get; }
                    public string DestinationUri { get; }
                    public string DestinationQueue { get; }
                    public int ReconnectDelay { get; }
                    public string AcknowledgeMode { get; }
                    public object SourceDeleteAfter { get; }
                    public ulong SourcePrefetchCount { get; }
                    public string SourceExchange { get; }
                    public string SourceExchangeRoutingKey { get; }
                    public string DestinationExchange { get; }
                    public string DestinationExchangeKey { get; }
                    public string DestinationPublishProperties { get; }
                    public bool DestinationAddForwardHeaders { get; }
                    public bool DestinationAddTimestampHeader { get; }
                }
            }


            class ShovelSourceConfiguratorImpl :
                ShovelSourceConfigurator
            {
                public ShovelProtocolType ShovelProtocol { get; private set; }
                public string ExchangeName { get; private set; }
                public string ExchangeRoutingKey { get; private set; }
                public ulong PrefetchCount { get; private set; }
                public object DeleteAfterShovel { get; private set; }

                public ShovelSourceConfiguratorImpl()
                {
                    ShovelProtocol = ShovelProtocolType.Amqp091;
                    PrefetchCount = 1000;
                }

                public void Protocol(ShovelProtocolType protocol) => ShovelProtocol = protocol;
                
                public void DeleteAfter(DeleteShovelMode mode) => DeleteAfterShovel = mode.ConvertTo();

                public void DeleteAfter(uint messages) => DeleteAfterShovel = messages;

                public void MaxCopiedMessages(ulong messages) => PrefetchCount = messages < 1000 ? 1000 : messages;

                public void Exchange(string exchange, string routingKey = null)
                {
                    ExchangeName = exchange;

                    if (routingKey == null)
                        return;
                    
                    ExchangeRoutingKey = routingKey;
                }
            }

                
            class ShovelDestinationConfiguratorImpl :
                ShovelDestinationConfigurator
            {
                public ShovelProtocolType ShovelProtocol { get; private set; }
                public string ExchangeName { get; private set; }
                public string ExchangeRoutingKey { get; private set; }
                public bool AddHeaders { get; private set; }
                public bool AddTimestampHeader { get; private set; }

                public ShovelDestinationConfiguratorImpl()
                {
                    ShovelProtocol = ShovelProtocolType.Amqp091;
                }

                public void Protocol(ShovelProtocolType protocol) => ShovelProtocol = protocol;

                public void Exchange(string exchange, string routingKey = null)
                {
                    ExchangeName = exchange;

                    if (routingKey == null)
                        return;
                    
                    ExchangeRoutingKey = routingKey;
                }

                public void AddForwardHeaders() => AddHeaders = true;

                public void AddTimestampHeaderToMessage() => AddTimestampHeader = true;
            }
        }
    }
}