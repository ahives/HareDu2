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
    using HareDu.Model;
    using Model;
    using Serialization;

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

            ResultList<ShovelInfoImpl> result = await GetAll<ShovelInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<ShovelInfo> MapResult(ResultList<ShovelInfoImpl> result) => new ResultListCopy(result);

            return MapResult(result);
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
            
            ShovelRequest request = impl.BuildRequest();

            Debug.Assert(request != null);

            errors.AddRange(impl.Errors.Value);
            
            if (string.IsNullOrWhiteSpace(shovel))
                errors.Add(new ErrorImpl("The name of the shovel is missing."));
            
            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            string url = $"api/parameters/shovel/{vhost.ToSanitizedName()}/{shovel}";

            if (errors.Any())
                return new FaultedResult(errors, new DebugInfoImpl(url, request.ToJsonString(Deserializer.Options), errors));

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

        
        class ResultListCopy :
            ResultList<ShovelInfo>
        {
            public ResultListCopy(ResultList<ShovelInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;

                var data = new List<ShovelInfo>();
                foreach (var item in result.Data)
                    data.Add(item);

                Data = data;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<ShovelInfo> Data { get; }
            public bool HasData { get; }
        }


        class ShovelConfiguratorImpl :
            ShovelConfigurator
        {
            readonly string _uri;
            ShovelProtocolType _destinationProtocol;
            ShovelProtocolType _sourceProtocol;
            AckMode _acknowledgeMode;
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

            public Lazy<List<Error>> Errors { get; }

            public ShovelConfiguratorImpl(string uri)
            {
                _uri = uri;
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
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

            public ShovelRequest BuildRequest()
            {
                var requestParams = new ShovelRequestParams(_acknowledgeMode,
                    _reconnectDelay,
                    _sourceProtocol,
                    _uri,
                    _sourceQueue,
                    _sourceExchangeName,
                    _sourceExchangeRoutingKey,
                    _sourcePrefetchCount,
                    _deleteShovelAfter,
                    _destinationProtocol,
                    _uri,
                    _destinationExchangeName,
                    _destinationExchangeRoutingKey,
                    _destinationQueue,
                    _destinationAddForwardHeaders,
                    _destinationAddTimestampHeader);
                var request = new ShovelRequest(requestParams);

                return request;
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