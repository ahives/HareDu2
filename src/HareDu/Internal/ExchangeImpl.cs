namespace HareDu.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Extensions;
    using HareDu.Model;
    using Model;
    using Serialization;

    class ExchangeImpl :
        BaseBrokerObject,
        Exchange
    {
        public ExchangeImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<ExchangeInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/exchanges";

            var result = await GetAll<ExchangeInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<ExchangeInfo> MapResult(ResultList<ExchangeInfoImpl> result) => new ResultListCopy(result);

            return MapResult(result);
        }

        public async Task<Result> Create(Action<ExchangeCreateAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled();

            var impl = new ExchangeCreateActionImpl();
            action(impl);
            
            impl.Validate();
            
            ExchangeDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string url = $"api/exchanges/{impl.VirtualHost.Value.ToSanitizedName()}/{impl.ExchangeName.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString(Deserializer.Options)));

            return await Put(url, definition, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(Action<ExchangeDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new ExchangeDeleteActionImpl();
            action(impl);
            
            impl.Validate();

            string vhost = impl.VirtualHost.Value.ToSanitizedName();
            
            string url = $"api/exchanges/{vhost}/{impl.ExchangeName.Value}";
            if (!string.IsNullOrWhiteSpace(impl.Query.Value))
                url = $"api/exchanges/{vhost}/{impl.ExchangeName.Value}?{impl.Query.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult<ExchangeInfo>(impl.Errors.Value, new DebugInfoImpl(url));

            return await Delete(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Create(string exchange, string vhost, Action<ExchangeConfigurator> configurator = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new ExchangeConfiguratorImpl();
            configurator?.Invoke(impl);
            
            ExchangeRequest request = impl.Request.Value;

            Debug.Assert(request != null);

            var errors = new List<Error>();
            
            errors.AddRange(impl.Errors.Value);
            
            if (string.IsNullOrWhiteSpace(exchange))
                errors.Add(new ErrorImpl("The name of the exchange is missing."));

            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            string url = $"api/exchanges/{vhost.ToSanitizedName()}/{exchange}";
            
            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, request.ToJsonString(Deserializer.Options), errors));

            return await PutRequest(url, request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(string exchange, string vhost, Action<ExchangeDeletionConfigurator> configurator = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new ExchangeDeletionConfigurationImpl();
            configurator?.Invoke(impl);

            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(exchange))
                errors.Add(new ErrorImpl("The name of the exchange is missing."));

            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            string virtualHost = vhost.ToSanitizedName();

            string url = string.IsNullOrWhiteSpace(impl.Query.Value)
                ? $"api/exchanges/{virtualHost}/{exchange}"
                : $"api/exchanges/{virtualHost}/{exchange}?{impl.Query.Value}";

            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, errors));

            return await DeleteRequest(url, cancellationToken).ConfigureAwait(false);
        }

        
        class ResultListCopy :
            ResultList<ExchangeInfo>
        {
            public ResultListCopy(ResultList<ExchangeInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;

                var data = new List<ExchangeInfo>();
                foreach (ExchangeInfoImpl item in result.Data)
                    data.Add(new InternalExchangeInfo(item));

                Data = data;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<ExchangeInfo> Data { get; }
            public bool HasData { get; }
        }

        
        class ExchangeDeletionConfigurationImpl :
            ExchangeDeletionConfigurator
        {
            string _query;

            public Lazy<string> Query { get; }

            public ExchangeDeletionConfigurationImpl()
            {
                Query = new Lazy<string>(() => _query, LazyThreadSafetyMode.PublicationOnly);
            }

            public void WhenUnused()
            {
                _query = "if-unused=true";
            }
        }


        class ExchangeConfiguratorImpl :
            ExchangeConfigurator
        {
            ExchangeRoutingType _routingType;
            bool _durable;
            bool _autoDelete;
            bool _internal;
            IDictionary<string, ArgumentValue<object>> _arguments;
            
            readonly List<Error> _errors;

            public Lazy<ExchangeRequest> Request { get; }
            public Lazy<List<Error>> Errors { get; }

            public ExchangeConfiguratorImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Request = new Lazy<ExchangeRequest>(
                    () => new  ExchangeRequest(_routingType, _durable, _autoDelete, _internal, _arguments.GetArgumentsOrNull()), LazyThreadSafetyMode.PublicationOnly);
            }
            
            public void HasRoutingType(ExchangeRoutingType routingType) => _routingType = routingType;

            public void IsDurable() => _durable = true;

            public void IsForInternalUse() => _internal = true;

            public void HasArguments(Action<ExchangeArgumentConfigurator> arguments)
            {
                var impl = new ExchangeArgumentConfiguratorImpl();
                arguments?.Invoke(impl);

                _arguments = impl.Arguments;

                if (_arguments.IsNotNull())
                    _errors.AddRange(_arguments.Select(x => x.Value?.Error).Where(error => error.IsNotNull()).ToList());
            }

            public void AutoDeleteWhenNotInUse() => _autoDelete = true;


            class ExchangeArgumentConfiguratorImpl :
                ExchangeArgumentConfigurator
            {
                public IDictionary<string, ArgumentValue<object>> Arguments { get; }

                public ExchangeArgumentConfiguratorImpl()
                {
                    Arguments = new Dictionary<string, ArgumentValue<object>>();
                }

                public void Add<T>(string arg, T value) => SetArg(arg, value);

                void SetArg(string arg, object value) =>
                    Arguments.Add(arg.Trim(),
                        Arguments.ContainsKey(arg)
                            ? new ArgumentValue<object>(value, $"Argument '{arg}' has already been set")
                            : new ArgumentValue<object>(value));
            }
        }

        
        class ExchangeDeleteActionImpl :
            ExchangeDeleteAction
        {
            string _vhost;
            string _exchange;
            string _query;
            readonly List<Error> _errors;

            public Lazy<string> Query { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<string> ExchangeName { get; }
            public Lazy<List<Error>> Errors { get; }

            public ExchangeDeleteActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Query = new Lazy<string>(() => _query, LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                ExchangeName = new Lazy<string>(() => _exchange, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Exchange(string name) => _exchange = name;

            public void When(Action<ExchangeDeleteCondition> condition)
            {
                var impl = new ExchangeDeleteConditionImpl();
                condition(impl);

                string query = string.Empty;
                if (impl.DeleteIfUnused)
                    query = "if-unused=true";

                _query = query;
            }

            public void Targeting(Action<ExchangeTarget> target)
            {
                var impl = new ExchangeTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
            }

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
             
                if (string.IsNullOrWhiteSpace(_exchange))
                    _errors.Add(new ErrorImpl("The name of the exchange is missing."));
            }

            
            class ExchangeTargetImpl :
                ExchangeTarget
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string name) => VirtualHostName = name;
            }


            class ExchangeDeleteConditionImpl :
                ExchangeDeleteCondition
            {
                public bool DeleteIfUnused { get; private set; }

                public void Unused() => DeleteIfUnused = true;
            }
        }


        class ExchangeCreateActionImpl :
            ExchangeCreateAction
        {
            ExchangeRoutingType? _routingType;
            bool _durable;
            bool _autoDelete;
            bool _internal;
            IDictionary<string, ArgumentValue<object>> _arguments;
            string _vhost;
            string _exchange;
            readonly List<Error> _errors;

            public Lazy<ExchangeDefinition> Definition { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<string> ExchangeName { get; }
            public Lazy<List<Error>> Errors { get; }

            public ExchangeCreateActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<ExchangeDefinition>(
                    () => new ExchangeDefinition(_routingType, _durable, _autoDelete, _internal, _arguments.ToDictionary(x => x.Key, x => x.Value.Value)), LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                ExchangeName = new Lazy<string>(() => _exchange, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Exchange(string name) => _exchange = name;

            public void Configure(Action<ExchangeConfiguration> configuration)
            {
                var impl = new ExchangeConfigurationImpl();
                configuration(impl);

                _durable = impl.Durable;
                _routingType = impl.RoutingType;
                _autoDelete = impl.AutoDelete;
                _internal = impl.InternalUse;
                _arguments = impl.Arguments;
            }

            public void Targeting(Action<ExchangeTarget> target)
            {
                var impl = new ExchangeTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
            }

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));

                if (!_routingType.HasValue)
                    _errors.Add(new ErrorImpl("The routing type of the exchange is missing."));

                if (!_arguments.IsNull())
                    _errors.AddRange(_arguments.Select(x => x.Value?.Error).Where(error => !error.IsNull()).ToList());

                if (string.IsNullOrWhiteSpace(_exchange))
                    _errors.Add(new ErrorImpl("The name of the exchange is missing."));
            }

            
            class ExchangeTargetImpl :
                ExchangeTarget
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string name) => VirtualHostName = name;
            }


            class ExchangeConfigurationImpl :
                ExchangeConfiguration
            {
                public ExchangeRoutingType? RoutingType { get; private set; }
                public IDictionary<string, ArgumentValue<object>> Arguments { get; private set; }
                public bool Durable { get; private set; }
                public bool InternalUse { get; private set; }
                public bool AutoDelete { get; private set; }

                public void HasRoutingType(ExchangeRoutingType routingType) => RoutingType = routingType;

                public void IsDurable() => Durable = true;

                public void IsForInternalUse() => InternalUse = true;

                public void HasArguments(Action<ExchangeDefinitionArguments> arguments)
                {
                    var impl = new ExchangeDefinitionArgumentsImpl();
                    arguments(impl);

                    Arguments = impl.Arguments;
                }

                public void AutoDeleteWhenNotInUse() => AutoDelete = true;
            }


            class ExchangeDefinitionArgumentsImpl :
                ExchangeDefinitionArguments
            {
                public IDictionary<string, ArgumentValue<object>> Arguments { get; }

                public ExchangeDefinitionArgumentsImpl()
                {
                    Arguments = new Dictionary<string, ArgumentValue<object>>();
                }

                public void Set<T>(string arg, T value)
                {
                    SetArg(arg, value);
                }

                void SetArg(string arg, object value)
                {
                    Arguments.Add(arg.Trim(),
                        Arguments.ContainsKey(arg)
                            ? new ArgumentValue<object>(value, $"Argument '{arg}' has already been set")
                            : new ArgumentValue<object>(value));
                }
            }


            // class ExchangeDefinitionImpl :
            //     ExchangeDefinition
            // {
            //     public ExchangeDefinitionImpl(string routingType,
            //         bool durable,
            //         bool autoDelete,
            //         bool @internal,
            //         IDictionary<string, ArgumentValue<object>> arguments)
            //     {
            //         RoutingType = routingType;
            //         Durable = durable;
            //         AutoDelete = autoDelete;
            //         Internal = @internal;
            //
            //         if (arguments.IsNull())
            //             return;
            //         
            //         Arguments = arguments.ToDictionary(x => x.Key, x => x.Value.Value);
            //     }
            //
            //     public string RoutingType { get; }
            //     public bool Durable { get; }
            //     public bool AutoDelete { get; }
            //     public bool Internal { get; }
            //     public IDictionary<string, object> Arguments { get; }
            // }
        }
    }

    class InternalExchangeInfo :
        ExchangeInfo
    {
        public InternalExchangeInfo(ExchangeInfoImpl obj)
        {
            Name = obj.Name;
            VirtualHost = obj.VirtualHost;
            RoutingType = obj.RoutingType;
            Durable = obj.Durable;
            AutoDelete = obj.AutoDelete;
            Internal = obj.Internal;
            Arguments = obj.Arguments;
        }

        public string Name { get; }
        public string VirtualHost { get; }
        public ExchangeRoutingType RoutingType { get; }
        public bool Durable { get; }
        public bool AutoDelete { get; }
        public bool Internal { get; }
        public IDictionary<string, object> Arguments { get; }
    }

    class ExchangeInfoImpl
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("vhost")]
        public string VirtualHost { get; set; }
        
        [JsonPropertyName("type")]
        public ExchangeRoutingType RoutingType { get; set; }
        
        [JsonPropertyName("durable")]
        public bool Durable { get; set; }
        
        [JsonPropertyName("auto_delete")]
        public bool AutoDelete { get; set; }
        
        [JsonPropertyName("internal")]
        public bool Internal { get; set; }
        
        [JsonPropertyName("arguments")]
        public IDictionary<string, object> Arguments { get; set; }
    }
}