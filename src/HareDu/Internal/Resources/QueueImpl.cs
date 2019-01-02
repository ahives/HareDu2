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
            
            DefinedQueue definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string vhost = impl.VirtualHost.Value;
            string queue = impl.QueueName.Value;
            
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            if (string.IsNullOrWhiteSpace(queue))
                errors.Add(new ErrorImpl("The name of the queue is missing."));

            if (!impl.Errors.IsNull())
                errors.AddRange(impl.Errors.Value);
            
            if (errors.Any())
                return new FaultedResult(errors);

            string url = $"api/queues/{SanitizeVirtualHostName(vhost)}/{queue}";

            Result result = await Put(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result> Delete(Action<QueueDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueueDeleteActionImpl();
            action(impl);

            string queue = impl.QueueName.Value;
            string vhost = impl.VirtualHost.Value;
            
            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            if (string.IsNullOrWhiteSpace(queue))
                errors.Add(new ErrorImpl("The name of the queue is missing."));
            
            if (errors.Any())
                return new FaultedResult(errors);

            string url = $"api/queues/{SanitizeVirtualHostName(vhost)}/{queue}";

            string query = impl.Query.Value;
            
            if (!string.IsNullOrWhiteSpace(query))
                url = $"{url}?{query}";

            Result result = await Delete(url, cancellationToken);

            return result;
        }

        public async Task<Result> Empty(Action<QueueEmptyAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueueEmptyActionImpl();
            action(impl);

            string queue = impl.QueueName.Value;
            string vhost = impl.VirtualHost.Value;
            
            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            if (string.IsNullOrWhiteSpace(queue))
                errors.Add(new ErrorImpl("The name of the queue is missing."));
            
            if (errors.Any())
                return new FaultedResult(errors);

            string url = $"api/queues/{SanitizeVirtualHostName(vhost)}/{queue}/contents";

            Result result = await Delete(url, cancellationToken);

            return result;
        }

        public async Task<Result<QueueInfo>> Peek(Action<QueuePeekAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new QueuePeekActionImpl();
            action(impl);

            DefinedQueuePeek definition = impl.Definition.Value;

            Debug.Assert(definition != null);
            
            var errors = new List<Error>();
            
            if (definition.Take < 1)
                errors.Add(new ErrorImpl("Must be set a value greater than 1."));

            if (string.IsNullOrWhiteSpace(definition.Encoding))
                errors.Add(new ErrorImpl("Encoding must be set to auto or base64."));

            string queue = impl.QueueName.Value;
            string vhost = impl.VirtualHost.Value;
            
            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            if (string.IsNullOrWhiteSpace(queue))
                errors.Add(new ErrorImpl("The name of the queue is missing."));
            
            if (errors.Any())
                return new FaultedResult<QueueInfo>(errors);

            string url = $"api/queues/{SanitizeVirtualHostName(vhost)}/{queue}/get";

            Result<QueueInfo> result = await Post<DefinedQueuePeek, QueueInfo>(url, definition, cancellationToken);

            return result;
        }

        
        class QueuePeekActionImpl :
            QueuePeekAction
        {
            static string _vhost;
            static string _queue;
            static int _take;
            static bool _putBackWhenFinished;
            static string _encoding;
            static long _truncateIfAbove;

            public Lazy<DefinedQueuePeek> Definition { get; }
            public Lazy<string> QueueName { get; }
            public Lazy<string> VirtualHost { get; }

            public QueuePeekActionImpl()
            {
                Definition = new Lazy<DefinedQueuePeek>(
                    () => new DefinedQueuePeekImpl(_take, _putBackWhenFinished, _encoding, _truncateIfAbove), LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                QueueName = new Lazy<string>(() => _queue, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Queue(string name) => _queue = name;
            
            public void Configure(Action<QueuePeekConfiguration> configuration)
            {
                var impl = new QueuePeekConfigurationImpl();
                configuration(impl);

                _take = impl.TakeAmount;
                _putBackWhenFinished = impl.PutBack;
                _encoding = impl.MessageEncoding;
                _truncateIfAbove = impl.TruncateMessageThresholdInBytes;
            }

            public void Target(Action<QueuePeekTarget> target)
            {
                var impl = new QueuePeekTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
            }

            
            class DefinedQueuePeekImpl :
                DefinedQueuePeek
            {
                public DefinedQueuePeekImpl(int take, bool putBackWhenFinished, string encoding, long truncateMessageThreshold)
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
            static string _vhost;
            static string _queue;

            public Lazy<string> QueueName { get; }
            public Lazy<string> VirtualHost { get; }

            public QueueEmptyActionImpl()
            {
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                QueueName = new Lazy<string>(() => _queue, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Queue(string name) => _queue = name;
            
            public void Target(Action<QueueTarget> target)
            {
                var impl = new QueueTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
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
            
            public Lazy<string> Query { get; }
            public Lazy<string> QueueName { get; }
            public Lazy<string> VirtualHost { get; }

            public QueueDeleteActionImpl()
            {
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                QueueName = new Lazy<string>(() => _queue, LazyThreadSafetyMode.PublicationOnly);
                Query = new Lazy<string>(() => _query, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Queue(string name) => _queue = name;
            
            public void Target(Action<QueueTarget> target)
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
            static bool _durable;
            static bool _autoDelete;
            static string _node;
            static IDictionary<string, ArgumentValue<object>> _arguments;
            static string _vhost;
            static string _queue;

            public Lazy<DefinedQueue> Definition { get; }
            public Lazy<string> QueueName { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<List<Error>> Errors { get; }

            public QueueCreateActionImpl()
            {
                Errors = new Lazy<List<Error>>(() => GetErrors(_arguments), LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<DefinedQueue>(
                    () => new DefinedQueueImpl(_durable, _autoDelete, _node, _arguments), LazyThreadSafetyMode.PublicationOnly);
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

            public void Target(Action<QueueCreateTarget> target)
            {
                var impl = new QueueCreateTargetImpl();
                target(impl);

                _node = impl.NodeName;
                _vhost = impl.VirtualHostName;
            }

            List<Error> GetErrors(IDictionary<string, ArgumentValue<object>> arguments)
            {
                return arguments.IsNull() ? new List<Error>() : arguments.Select(x => x.Value?.Error).Where(x => !x.IsNull()).ToList();
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


            class DefinedQueueImpl :
                DefinedQueue
            {
                public DefinedQueueImpl(bool durable, bool autoDelete, string node, IDictionary<string, ArgumentValue<object>> arguments)
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