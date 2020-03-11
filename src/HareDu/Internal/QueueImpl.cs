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
    using Model;

    class QueueImpl :
        BaseBrokerObject,
        Queue
    {
        public QueueImpl(HttpClient client)
            : base(client)
        {
        }

        public Task<ResultList<QueueInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = $"api/queues";
            
            return GetAll<QueueInfo>(url, cancellationToken);
        }

        public Task<Result> Create(Action<QueueCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueueCreateActionImpl();
            action(impl);
            
            impl.Validate();
            
            QueueDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string url = $"api/queues/{impl.VirtualHost.Value.ToSanitizedName()}/{impl.QueueName.Value}";
            
            if (impl.Errors.Value.Any())
                return Task.FromResult<Result>(new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString())));

            return Put(url, definition, cancellationToken);
        }

        public Task<Result> Delete(Action<QueueDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueueDeleteActionImpl();
            action(impl);

            impl.Validate();
            
            string url = $"api/queues/{impl.VirtualHost.Value.ToSanitizedName()}/{impl.QueueName.Value}";
            if (!string.IsNullOrWhiteSpace(impl.Query.Value))
                url = $"{url}?{impl.Query.Value}";
            
            if (impl.Errors.Value.Any())
                return Task.FromResult<Result>(new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, null)));

            return Delete(url, cancellationToken);
        }

        public Task<Result> Empty(Action<QueueEmptyAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueueEmptyActionImpl();
            action(impl);

            impl.Validate();

            string url = $"api/queues/{impl.VirtualHost.Value.ToSanitizedName()}/{impl.QueueName.Value}/contents";
            
            if (impl.Errors.Value.Any())
                return Task.FromResult<Result>(new FaultedResult<QueueInfo>(impl.Errors.Value, new DebugInfoImpl(url, null)));

            return Delete(url, cancellationToken);
        }

        public Task<ResultList<PeekedMessageInfo>> Peek(Action<QueuePeekAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueuePeekActionImpl();
            action(impl);

            impl.Validate();

            QueuePeekDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string url = $"api/queues/{impl.VirtualHost.Value.ToSanitizedName()}/{impl.QueueName.Value}/get";
            
            if (impl.Errors.Value.Any())
                return Task.FromResult<ResultList<PeekedMessageInfo>>(new FaultedResultList<PeekedMessageInfo>(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString())));

            return PostList<PeekedMessageInfo, QueuePeekDefinition>(url, definition, cancellationToken);
        }

        
        class QueuePeekActionImpl :
            QueuePeekAction
        {
            string _vhost;
            string _queue;
            uint _take;
            string _encoding;
            ulong _truncateIfAbove;
            string _requeueMode;
            readonly List<Error> _errors;

            public Lazy<QueuePeekDefinition> Definition { get; }
            public Lazy<string> QueueName { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<List<Error>> Errors { get; }

            public QueuePeekActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<QueuePeekDefinition>(
                    () => new QueuePeekDefinitionImpl(_take, _requeueMode, _encoding, _truncateIfAbove), LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                QueueName = new Lazy<string>(() => _queue, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Queue(string name) => _queue = name;

            public void Configure(Action<QueuePeekConfiguration> configuration)
            {
                var impl = new QueuePeekConfigurationImpl();
                configuration(impl);

                _take = impl.TakeAmount;
                _requeueMode = impl.RequeueMode;
                _encoding = impl.MessageEncoding;
                _truncateIfAbove = impl.TruncateMessageThresholdInBytes;
            }

            public void Targeting(Action<QueuePeekTarget> target)
            {
                var impl = new QueuePeekTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
            }

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_queue))
                    _errors.Add(new ErrorImpl("The name of the queue is missing."));
            
                if (_take < 1)
                    _errors.Add(new ErrorImpl("Must be set a value greater than 1."));

                if (string.IsNullOrWhiteSpace(_encoding))
                    _errors.Add(new ErrorImpl("Encoding must be set to auto or base64."));
            
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            
            class QueuePeekDefinitionImpl :
                QueuePeekDefinition
            {
                public QueuePeekDefinitionImpl(uint take, string requeueMode, string encoding, ulong truncateMessageThreshold)
                {
                    Take = take;
                    Encoding = encoding;
                    RequeueMode = requeueMode;
                    TruncateMessageThreshold = truncateMessageThreshold;
                }

                public uint Take { get; }
                public string Encoding { get; }
                public ulong TruncateMessageThreshold { get; }
                public string RequeueMode { get; }
            }

            
            class QueuePeekConfigurationImpl :
                QueuePeekConfiguration
            {
                public uint TakeAmount { get; private set; }
                public string RequeueMode { get; private set; }
                public string MessageEncoding { get; private set; }
                public ulong TruncateMessageThresholdInBytes { get; private set; }

                public void Take(uint count) => TakeAmount = count;

                public void Encoding(MessageEncoding encoding)
                {
                    switch (encoding)
                    {
                        case HareDu.MessageEncoding.Auto:
                            MessageEncoding = "auto";
                            break;
                            
                        case HareDu.MessageEncoding.Base64:
                            MessageEncoding = "base64";
                            break;
                            
                        default:
                            throw new ArgumentOutOfRangeException(nameof(encoding), encoding, null);
                    }
                }

                public void TruncateIfAbove(uint bytes) => TruncateMessageThresholdInBytes = bytes;
                
                public void AckMode(RequeueMode mode)
                {
                    switch (mode)
                    {
                        case HareDu.RequeueMode.DoNotAckRequeue:
                            RequeueMode = "ack_requeue_false";
                            break;
                
                        case HareDu.RequeueMode.RejectRequeue:
                            RequeueMode = "reject_requeue_true";
                            break;
                
                        case HareDu.RequeueMode.DoNotRejectRequeue:
                            RequeueMode = "reject_requeue_false";
                            break;

                        default:
                            RequeueMode = "ack_requeue_true";
                            break;
                    }
                }
            }

            
            class QueuePeekTargetImpl :
                QueuePeekTarget
            {
                public string VirtualHostName { get; private set; }
                
                public void VirtualHost(string name) => VirtualHostName = name;
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
                    () => new QueueDefinitionImpl(_durable, _autoDelete, _node, _arguments), LazyThreadSafetyMode.PublicationOnly);
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


            class QueueDefinitionImpl :
                QueueDefinition
            {
                public QueueDefinitionImpl(bool durable,
                    bool autoDelete,
                    string node,
                    IDictionary<string, ArgumentValue<object>> arguments)
                {
                    Durable = durable;
                    AutoDelete = autoDelete;
                    Node = node;

                    if (arguments.IsNull())
                        return;
                    
                    Arguments = arguments.ToDictionary(x => x.Key, x => x.Value.Value);
                }

                public string Node { get; }
                public bool Durable { get; }
                public bool AutoDelete { get; }
                public IDictionary<string, object> Arguments { get; }
            }
        }
    }
}