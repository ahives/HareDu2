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

    class QueueImpl :
        BaseBrokerObject,
        Queue
    {
        public QueueImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<QueueInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/queues";

            ResultList<QueueInfoImpl> result = await GetAll<QueueInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<QueueInfo> MapResult(ResultList<QueueInfoImpl> result) => new ResultListCopy(result);

            return MapResult(result);
        }

        public async Task<Result> Create(Action<QueueCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueueCreateActionImpl();
            action(impl);
            
            impl.Validate();
            
            QueueDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string url = $"api/queues/{impl.VirtualHost.Value.ToSanitizedName()}/{impl.QueueName.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString(Deserializer.Options)));

            return await Put(url, definition, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(Action<QueueDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueueDeleteActionImpl();
            action(impl);

            impl.Validate();
            
            string url = $"api/queues/{impl.VirtualHost.Value.ToSanitizedName()}/{impl.QueueName.Value}";
            if (!string.IsNullOrWhiteSpace(impl.Query.Value))
                url = $"{url}?{impl.Query.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url));

            return await Delete(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Empty(Action<QueueEmptyAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueueEmptyActionImpl();
            action(impl);

            impl.Validate();

            string url = $"api/queues/{impl.VirtualHost.Value.ToSanitizedName()}/{impl.QueueName.Value}/contents";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult<QueueInfo>(impl.Errors.Value, new DebugInfoImpl(url));

            return await Delete(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Create(string queue, string vhost, string node, Action<QueueConfigurator> configurator = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueueConfiguratorImpl(node);
            configurator?.Invoke(impl);
            
            impl.Validate();
            
            QueueRequest request = impl.Definition.Value;

            Debug.Assert(request != null);

            var errors = new List<Error>();
            
            errors.AddRange(impl.Errors.Value);
            
            if (string.IsNullOrWhiteSpace(queue))
                errors.Add(new ErrorImpl("The name of the queue is missing."));

            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            string url = $"api/queues/{vhost.ToSanitizedName()}/{queue}";
            
            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, request.ToJsonString(Deserializer.Options), errors));

            return await PutRequest(url, request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(string queue, string vhost, Action<QueueDeletionConfigurator> configurator = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueueDeletionConfiguratorImpl();
            configurator?.Invoke(impl);

            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(queue))
                errors.Add(new ErrorImpl("The name of the queue is missing."));

            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            string url = string.IsNullOrWhiteSpace(impl.Query.Value)
                ? $"api/queues/{vhost.ToSanitizedName()}/{queue}"
                : $"api/queues/{vhost.ToSanitizedName()}/{queue}?{impl.Query.Value}";
            
            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, errors));

            return await DeleteRequest(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Empty(string queue, string vhost, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            if (string.IsNullOrWhiteSpace(queue))
                errors.Add(new ErrorImpl("The name of the queue is missing."));

            string url = $"api/queues/{vhost.ToSanitizedName()}/{queue}/contents";

            if (errors.Any())
                return new FaultedResult<QueueInfo>(new DebugInfoImpl(url, errors));

            return await DeleteRequest(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Sync(string queue, string vhost, QueueSyncAction syncAction, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            QueueSyncRequest request = new QueueSyncRequest(syncAction);

            Debug.Assert(request != null);
            
            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            if (string.IsNullOrWhiteSpace(queue))
                errors.Add(new ErrorImpl("The name of the queue is missing."));

            string url = $"api/queues/{vhost.ToSanitizedName()}/{queue}/actions";

            if (errors.Any())
                return new FaultedResult<QueueInfo>(new DebugInfoImpl(url, errors));

            return await PostRequest(url, request, cancellationToken).ConfigureAwait(false);
        }

        
        class ResultListCopy :
            ResultList<QueueInfo>
        {
            public ResultListCopy(ResultList<QueueInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;

                var data = new List<QueueInfo>();
                foreach (QueueInfoImpl item in result.Data)
                    data.Add(new InternalQueueInfo(item));

                Data = data;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<QueueInfo> Data { get; }
            public bool HasData { get; }
        }


        class QueueDeletionConfiguratorImpl :
            QueueDeletionConfigurator
        {
            string _query;

            public Lazy<string> Query { get; }

            public QueueDeletionConfiguratorImpl()
            {
                Query = new Lazy<string>(() => _query, LazyThreadSafetyMode.PublicationOnly);
            }

            public void WhenHasNoConsumers()
            {
                _query = string.IsNullOrWhiteSpace(_query)
                    ? "if-unused=true"
                    : _query.Contains("if-unused=true") ? _query : $"{_query}&if-unused=true";
            }

            public void WhenEmpty()
            {
                _query = string.IsNullOrWhiteSpace(_query)
                    ? "if-empty=true"
                    : _query.Contains("if-empty=true") ? _query : $"{_query}&if-empty=true";
            }
        }


        class QueueConfiguratorImpl :
            QueueConfigurator
        {
            bool _durable;
            bool _autoDelete;
            IDictionary<string, ArgumentValue<object>> _arguments;
            
            readonly List<Error> _errors;

            public Lazy<QueueRequest> Definition { get; }
            public Lazy<List<Error>> Errors { get; }

            public QueueConfiguratorImpl(string node)
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<QueueRequest>(
                    () => new QueueRequest(node, _durable, _autoDelete, _arguments.GetArgumentsOrNull()), LazyThreadSafetyMode.PublicationOnly);
            }

            public void IsDurable() => _durable = true;

            public void HasArguments(Action<QueueArgumentConfigurator> configurator)
            {
                var impl = new QueueArgumentConfiguratorImpl();
                configurator?.Invoke(impl);

                _arguments = impl.Arguments;
            }

            public void AutoDeleteWhenNotInUse() => _autoDelete = true;

            public void Validate()
            {
                if (_arguments.IsNotNull())
                    _errors.AddRange(_arguments.Select(x => x.Value?.Error).Where(error => error.IsNotNull()).ToList());
            }

            
            class QueueArgumentConfiguratorImpl :
                QueueArgumentConfigurator
            {
                public IDictionary<string, ArgumentValue<object>> Arguments { get; }

                public QueueArgumentConfiguratorImpl()
                {
                    Arguments = new Dictionary<string, ArgumentValue<object>>();
                }

                public void Set<T>(string arg, T value) => SetArg(arg, value);

                public void SetQueueExpiration(ulong milliseconds) =>
                    SetArg("x-expires", milliseconds, milliseconds < 1 ? "x-expires cannot have a value less than 1" : null);

                public void SetPerQueuedMessageExpiration(ulong milliseconds) => SetArg("x-message-ttl", milliseconds);

                public void SetDeadLetterExchange(string exchange) => SetArg("x-dead-letter-exchange", exchange);

                public void SetDeadLetterExchangeRoutingKey(string routingKey) => SetArg("x-dead-letter-routing-key", routingKey);

                public void SetAlternateExchange(string exchange) => SetArg("alternate-exchange", exchange);

                void SetArg(string arg, object value, string errorMsg = null)
                {
                    string normalizedArg = arg.Trim();
                    
                    if (Arguments.ContainsKey(normalizedArg))
                        Arguments[normalizedArg] = new ArgumentValue<object>(value, errorMsg);
                    else
                        Arguments.Add(normalizedArg, new ArgumentValue<object>(value, errorMsg));
                }
            }
        }


        class QueueEmptyActionImpl :
            QueueEmptyAction
        {
            string _vhost;
            string _queue;
            readonly List<Error> _errors;

            public Lazy<string> QueueName { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<List<Error>> Errors { get; }

            public QueueEmptyActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                QueueName = new Lazy<string>(() => _queue, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Queue(string name) => _queue = name;

            public void Targeting(Action<QueueTarget> target)
            {
                var impl = new QueueTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
            }

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));

                if (string.IsNullOrWhiteSpace(_queue))
                    _errors.Add(new ErrorImpl("The name of the queue is missing."));
            }

            
            class QueueTargetImpl :
                QueueTarget
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string name) => VirtualHostName = name;
            }
        }


        class QueueDeleteActionImpl :
            QueueDeleteAction
        {
            string _vhost;
            string _queue;
            string _query;
            readonly List<Error> _errors;

            public Lazy<string> Query { get; }
            public Lazy<string> QueueName { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<List<Error>> Errors { get; }

            public QueueDeleteActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                QueueName = new Lazy<string>(() => _queue, LazyThreadSafetyMode.PublicationOnly);
                Query = new Lazy<string>(() => _query, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Queue(string name) => _queue = name;

            public void Targeting(Action<QueueTarget> target)
            {
                var impl = new QueueTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
            }

            public void When(Action<QueueDeleteCondition> condition)
            {
                var impl = new QueueDeleteConditionImpl();
                condition(impl);
                
                string query = string.Empty;

                if (impl.DeleteIfUnused)
                    query = "if-unused=true";

                if (impl.DeleteIfEmpty)
                    query = !string.IsNullOrWhiteSpace(query) ? $"{query}&if-empty=true" : "if-empty=true";

                _query = query;
            }

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));

                if (string.IsNullOrWhiteSpace(_queue))
                    _errors.Add(new ErrorImpl("The name of the queue is missing."));
            }

            
            class QueueTargetImpl :
                QueueTarget
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string name) => VirtualHostName = name;
            }


            class QueueDeleteConditionImpl :
                QueueDeleteCondition
            {
                public bool DeleteIfUnused { get; private set; }
                public bool DeleteIfEmpty { get; private set; }

                public void HasNoConsumers() => DeleteIfUnused = true;

                public void IsEmpty() => DeleteIfEmpty = true;
            }
        }


        class QueueCreateActionImpl :
            QueueCreateAction
        {
            bool _durable;
            bool _autoDelete;
            string _node;
            IDictionary<string, ArgumentValue<object>> _arguments;
            string _vhost;
            string _queue;
            readonly List<Error> _errors;

            public Lazy<QueueDefinition> Definition { get; }
            public Lazy<string> QueueName { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<List<Error>> Errors { get; }

            public QueueCreateActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<QueueDefinition>(
                    () => new QueueDefinition(_node, _durable, _autoDelete, _arguments.ToDictionary(x => x.Key, x => x.Value.Value)), LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                QueueName = new Lazy<string>(() => _queue, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Queue(string name) => _queue = name;

            public void Configure(Action<QueueConfiguration> configuration)
            {
                var impl = new QueueConfigurationImpl();
                configuration(impl);

                _durable = impl.Durable;
                _autoDelete = impl.AutoDelete;
                _arguments = impl.Arguments;
            }

            public void Targeting(Action<QueueCreateTarget> target)
            {
                var impl = new QueueCreateTargetImpl();
                target(impl);

                _node = impl.NodeName;
                _vhost = impl.VirtualHostName;
            }

            public void Validate()
            {
                if (!_arguments.IsNull())
                    _errors.AddRange(_arguments.Select(x => x.Value?.Error).Where(error => !error.IsNull()).ToList());

                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));

                if (string.IsNullOrWhiteSpace(_queue))
                    _errors.Add(new ErrorImpl("The name of the queue is missing."));
            }

            
            class QueueCreateTargetImpl :
                QueueCreateTarget
            {
                public string VirtualHostName { get; private set; }
                public string NodeName { get; private set; }
                
                public void Node(string node) => NodeName = node;
            
                public void VirtualHost(string name) => VirtualHostName = name;
            }


            class QueueConfigurationImpl :
                QueueConfiguration
            {
                public bool Durable { get; private set; }
                public IDictionary<string, ArgumentValue<object>> Arguments { get; private set; }
                public bool AutoDelete { get; private set; }

                public void IsDurable() => Durable = true;

                public void HasArguments(Action<QueueCreateArguments> arguments)
                {
                    var impl = new QueueCreateArgumentsImpl();
                    arguments(impl);

                    Arguments = impl.Arguments;
                }

                public void AutoDeleteWhenNotInUse() => AutoDelete = true;
            }

            
            class QueueCreateArgumentsImpl :
                QueueCreateArguments
            {
                public IDictionary<string, ArgumentValue<object>> Arguments { get; }

                public QueueCreateArgumentsImpl()
                {
                    Arguments = new Dictionary<string, ArgumentValue<object>>();
                }

                public void Set<T>(string arg, T value)
                {
                    SetArg(arg, value);
                }

                public void SetQueueExpiration(ulong milliseconds)
                {
                    SetArg("x-expires", milliseconds, milliseconds < 1 ? "x-expires cannot have a value less than 1" : null);
                }

                public void SetPerQueuedMessageExpiration(ulong milliseconds)
                {
                    SetArg("x-message-ttl", milliseconds);
                }

                public void SetDeadLetterExchange(string exchange)
                {
                    SetArg("x-dead-letter-exchange", exchange);
                }

                public void SetDeadLetterExchangeRoutingKey(string routingKey)
                {
                    SetArg("x-dead-letter-routing-key", routingKey);
                }

                public void SetAlternateExchange(string exchange)
                {
                    SetArg("alternate-exchange", exchange);
                }

                void SetArg(string arg, object value, string errorMsg = null)
                {
                    string normalizedArg = arg.Trim();
                    if (Arguments.ContainsKey(normalizedArg))
                        Arguments[normalizedArg] = new ArgumentValue<object>(value, errorMsg);
                    else
                        Arguments.Add(normalizedArg, new ArgumentValue<object>(value, errorMsg));
                }
            }
        }
    }
}