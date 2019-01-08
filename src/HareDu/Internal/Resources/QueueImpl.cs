// Copyright 2013-2019 Albert L. Hives
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
namespace HareDu.Internal.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensions;
    using Model;

    internal class QueueImpl :
        ResourceBase,
        Queue
    {
        public QueueImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<Result<IReadOnlyList<QueueInfo>>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = $"api/queues";
            
            Result<IReadOnlyList<QueueInfo>> result = await GetAll<QueueInfo>(url, cancellationToken);

            return result;
        }

        public async Task<Result> Create(Action<QueueCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueueCreateActionImpl();
            action(impl);
            
            QueueDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string url = $"api/queues/{SanitizeVirtualHostName(impl.VirtualHost.Value)}/{impl.QueueName.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString()));

            Result result = await Put(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result> Delete(Action<QueueDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueueDeleteActionImpl();
            action(impl);

            string url = $"api/queues/{SanitizeVirtualHostName(impl.VirtualHost.Value)}/{impl.QueueName.Value}";
            if (!string.IsNullOrWhiteSpace(impl.Query.Value))
                url = $"{url}?{impl.Query.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, null));

            Result result = await Delete(url, cancellationToken);

            return result;
        }

        public async Task<Result> Empty(Action<QueueEmptyAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueueEmptyActionImpl();
            action(impl);

            string url = $"api/queues/{SanitizeVirtualHostName(impl.VirtualHost.Value)}/{impl.QueueName.Value}/contents";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, null));

            Result result = await Delete(url, cancellationToken);

            return result;
        }

        public async Task<Result<QueueInfo>> Peek(Action<QueuePeekAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueuePeekActionImpl();
            action(impl);

            QueuePeekDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string url = $"api/queues/{SanitizeVirtualHostName(impl.VirtualHost.Value)}/{impl.QueueName.Value}/get";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult<QueueInfo>(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString()));

            Result<QueueInfo> result = await Post<QueuePeekDefinition, QueueInfo>(url, definition, cancellationToken);

            return result;
        }

        
        class QueuePeekActionImpl :
            QueuePeekAction
        {
            string _vhost;
            string _queue;
            int _take;
            bool _putBackWhenFinished;
            string _encoding;
            long _truncateIfAbove;
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
                    () => new QueuePeekDefinitionImpl(_take, _putBackWhenFinished, _encoding, _truncateIfAbove), LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                QueueName = new Lazy<string>(() => _queue, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Queue(string name)
            {
                _queue = name;

                if (string.IsNullOrWhiteSpace(_queue))
                    _errors.Add(new ErrorImpl("The name of the queue is missing."));
            }
            
            public void Configure(Action<QueuePeekConfiguration> configuration)
            {
                var impl = new QueuePeekConfigurationImpl();
                configuration(impl);

                _take = impl.TakeAmount;
                _putBackWhenFinished = impl.PutBack;
                _encoding = impl.MessageEncoding;
                _truncateIfAbove = impl.TruncateMessageThresholdInBytes;
            
                if (_take < 1)
                    _errors.Add(new ErrorImpl("Must be set a value greater than 1."));

                if (string.IsNullOrWhiteSpace(_encoding))
                    _errors.Add(new ErrorImpl("Encoding must be set to auto or base64."));
            }

            public void Target(Action<QueuePeekTarget> target)
            {
                var impl = new QueuePeekTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
            
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            
            class QueuePeekDefinitionImpl :
                QueuePeekDefinition
            {
                public QueuePeekDefinitionImpl(int take, bool putBackWhenFinished, string encoding, long truncateMessageThreshold)
                {
                    Take = take;
                    PutBackWhenFinished = putBackWhenFinished;
                    Encoding = encoding;
                    TruncateMessageThreshold = truncateMessageThreshold;
                }

                public int Take { get; }
                public bool PutBackWhenFinished { get; }
                public string Encoding { get; }
                public long TruncateMessageThreshold { get; }
            }

            
            class QueuePeekConfigurationImpl :
                QueuePeekConfiguration
            {
                public int TakeAmount { get; private set; }
                public bool PutBack { get; private set; }
                public string MessageEncoding { get; private set; }
                public long TruncateMessageThresholdInBytes { get; private set; }

                public void Take(int count) => TakeAmount = count;

                public void PutBackWhenFinished() => PutBack = true;

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

                public void TruncateIfAbove(int bytes) => TruncateMessageThresholdInBytes = bytes;
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

            public void Queue(string name)
            {
                _queue = name;

                if (string.IsNullOrWhiteSpace(_queue))
                    _errors.Add(new ErrorImpl("The name of the queue is missing."));
            }
            
            public void Target(Action<QueueTarget> target)
            {
                var impl = new QueueTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
            
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
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
            static string _vhost;
            static string _queue;
            static string _query;
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

            public void Queue(string name)
            {
                _queue = name;

                if (string.IsNullOrWhiteSpace(_queue))
                    _errors.Add(new ErrorImpl("The name of the queue is missing."));
            }
            
            public void Target(Action<QueueTarget> target)
            {
                var impl = new QueueTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
            
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
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

            public void Queue(string name)
            {
                _queue = name;

                if (string.IsNullOrWhiteSpace(_queue))
                    _errors.Add(new ErrorImpl("The name of the queue is missing."));
            }

            public void Configure(Action<QueueConfiguration> configuration)
            {
                var impl = new QueueConfigurationImpl();
                configuration(impl);

                _durable = impl.Durable;
                _autoDelete = impl.AutoDelete;
                _arguments = impl.Arguments;

                if (!_arguments.IsNull())
                    _errors.AddRange(_arguments.Select(x => x.Value?.Error).Where(error => !error.IsNull()).ToList());
            }

            public void Target(Action<QueueCreateTarget> target)
            {
                var impl = new QueueCreateTargetImpl();
                target(impl);

                _node = impl.NodeName;
                _vhost = impl.VirtualHostName;

                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
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
                    SetArg(arg, "x-expires", value);
                    SetArg(arg, "x-message-ttl", value);
                    SetArg(arg, "x-dead-letter-exchange", value);
                    SetArg(arg, "x-dead-letter-routing-key", value);
                    SetArg(arg, "alternate-exchange", value);
                }

                public void SetQueueExpiration(long milliseconds)
                {
                    SetArg("x-expires", milliseconds);
                }

                public void SetPerQueuedMessageExpiration(long milliseconds)
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

                void SetArg(string arg, string targetArg, object value)
                {
                    Arguments.Add(arg.Trim(),
                        arg == targetArg && Arguments.ContainsKey(targetArg)
                            ? new ArgumentValue<object>(value, $"Argument '{arg}' has already been set")
                            : new ArgumentValue<object>(value));
                }

                void SetArg(string arg, object value)
                {
                    Arguments.Add(arg.Trim(),
                        Arguments.ContainsKey(arg)
                            ? new ArgumentValue<object>(value, $"Argument '{arg}' has already been set")
                            : new ArgumentValue<object>(value));
                }
            }


            class QueueDefinitionImpl :
                QueueDefinition
            {
                public QueueDefinitionImpl(bool durable, bool autoDelete, string node, IDictionary<string, ArgumentValue<object>> arguments)
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