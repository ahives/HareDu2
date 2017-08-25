// Copyright 2013-2017 Albert L. Hives
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
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Exceptions;
    using Model;

    internal class QueueImpl :
        ResourceBase,
        Queue
    {
        public QueueImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<IEnumerable<QueueInfo>>> GetAll(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/queues";

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<QueueInfo>> result = await response.GetResponse<IEnumerable<QueueInfo>>();

            LogInfo($"Sent request to return all information on current RabbitMQ server.");

            return result;
        }

        public async Task<Result> Create(Action<QueueCreateAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new QueueCreateActionImpl();
            action(impl);

            QueueSettings settings = impl.Settings.Value;

            string vhost = impl.VirtualHost.Value;
            string queue = impl.QueueName.Value;
            
            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            if (string.IsNullOrWhiteSpace(queue))
                throw new QueueMissingException("The name of the queue is missing.");

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/queues/{sanitizedVHost}/{queue}";

            HttpResponseMessage response = await HttpPut(url, settings, cancellationToken);
            Result result = response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server to create queue '{queue}' in virtual host '{sanitizedVHost}'.");

            return result;
        }

        public async Task<Result> Delete(Action<QueueDeleteAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new QueueDeleteActionImpl();
            action(impl);

            string queue = impl.QueueName.Value;
            string vhost = impl.VirtualHost.Value;
            
            if (string.IsNullOrWhiteSpace(queue))
                throw new QueueMissingException("The name of the queue is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");
            
            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/queues/{sanitizedVHost}/{impl.QueueName}";

            string query = impl.Query.Value;
            
            if (string.IsNullOrWhiteSpace(query))
                url = $"{url}?{impl.Query}";

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server to delete queue '{queue}' from virtual host '{sanitizedVHost}'.");

            return result;
        }

        public async Task<Result> Empty(Action<QueueEmptyAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new QueueEmptyActionImpl();
            action(impl);

            string queue = impl.QueueName.Value;
            string vhost = impl.VirtualHost.Value;
            
            if (string.IsNullOrWhiteSpace(queue))
                throw new QueueMissingException("The name of the queue is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");
            
            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/queues/{sanitizedVHost}/{queue}/contents";

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server to empty queue '{queue}' on virtual host '{sanitizedVHost}'.");

            return result;
        }

        public async Task<Result> Peek(Action<QueuePeekAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new QueuePeekActionImpl();
            action(impl);

            string queue = impl.QueueName.Value;
            string vhost = impl.VirtualHost.Value;
            
            if (string.IsNullOrWhiteSpace(queue))
                throw new QueueMissingException("The name of the queue is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");
            
            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/queues/{sanitizedVHost}/{queue}/get";

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server to empty queue '{queue}' on virtual host '{sanitizedVHost}'.");

            return result;
        }

        
        class QueuePeekActionImpl :
            QueuePeekAction
        {
            static string _vhost;
            static string _queue;

            public Lazy<string> QueueName { get; }
            public Lazy<string> VirtualHost { get; }

            public QueuePeekActionImpl()
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

                public void VirtualHost(string vhost) => VirtualHostName = vhost;
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

                public void VirtualHost(string vhost) => VirtualHostName = vhost;
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

            public void WithConditions(Action<QueueDeleteCondition> condition)
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

                public void VirtualHost(string vhost) => VirtualHostName = vhost;
            }


            class QueueDeleteConditionImpl :
                QueueDeleteCondition
            {
                public bool DeleteIfUnused { get; private set; }
                public bool DeleteIfEmpty { get; private set; }

                public void IfUnused() => DeleteIfUnused = true;

                public void IfEmpty() => DeleteIfEmpty = true;
            }
        }


        class QueueCreateActionImpl :
            QueueCreateAction
        {
            static bool _durable;
            static bool _autoDelete;
            static string _node;
            static IDictionary<string, object> _arguments;
            static string _vhost;
            static string _queue;

            public Lazy<QueueSettings> Settings { get; }
            public Lazy<string> QueueName { get; }
            public Lazy<string> VirtualHost { get; }

            public QueueCreateActionImpl()
            {
                Settings = new Lazy<QueueSettings>(
                    () => new QueueSettingsImpl(_durable, _autoDelete, _node, _arguments), LazyThreadSafetyMode.PublicationOnly);
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

            
            class QueueCreateTargetImpl :
                QueueCreateTarget
            {
                public string VirtualHostName { get; private set; }
                public string NodeName { get; private set; }
                
                public void Node(string node) => NodeName = node;
            
                public void VirtualHost(string vhost) => VirtualHostName = vhost;
            }


            class QueueConfigurationImpl :
                QueueConfiguration
            {
                public bool Durable { get; private set; }
                public IDictionary<string, object> Arguments { get; private set; }
                public bool AutoDelete { get; private set; }

                public void IsDurable() => Durable = true;

                public void WithArguments(Action<QueueCreateArguments> arguments)
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
                public IDictionary<string, object> Arguments { get; } = new Dictionary<string, object>();

                public void Set<T>(string arg, T value)
                {
                    Validate(arg.Trim(), "x-expires");
                    Validate(arg.Trim(), "x-message-ttl");
                    Validate(arg.Trim(), "x-dead-letter-exchange");
                    Validate(arg.Trim(), "x-dead-letter-routing-key");
                    Validate(arg.Trim(), "alternate-exchange");
                    
                    Arguments.Add(arg, value);
                }

                public void SetQueueExpiration(long milliseconds)
                {
                    Validate("x-expires");
                    
                    Arguments.Add("x-expires", milliseconds);
                }

                public void SetPerQueuedMessageExpiration(long milliseconds)
                {
                    Validate("x-message-ttl");
                    
                    Arguments.Add("x-message-ttl", milliseconds);
                }

                public void SetDeadLetterExchange(string exchange)
                {
                    Validate("x-dead-letter-exchange");
                    
                    Arguments.Add("x-dead-letter-exchange", exchange);
                }

                public void SetDeadLetterExchangeRoutingKey(string routingKey)
                {
                    Validate("x-dead-letter-routing-key");
                    
                    Arguments.Add("x-dead-letter-routing-key", routingKey);
                }

                public void SetAlternateExchange(string exchange)
                {
                    Validate("alternate-exchange");
                    
                    Arguments.Add("alternate-exchange", exchange);
                }

                void Validate(string arg, string targetArg)
                {
                    if (arg == targetArg && Arguments.ContainsKey(targetArg))
                        throw new PolicyDefinitionException($"Argument '{arg}' has already been set");
                }

                void Validate(string arg)
                {
                    if (Arguments.ContainsKey(arg))
                        throw new QueueArgumentException($"Argument '{arg}' has already been set");
                }
            }


            class QueueSettingsImpl :
                QueueSettings
            {
                public QueueSettingsImpl(bool durable, bool autoDelete, string node, IDictionary<string, object> arguments)
                {
                    Durable = durable;
                    AutoDelete = autoDelete;
                    Node = node;
                    Arguments = arguments;
                }

                public string Node { get; }
                public bool Durable { get; }
                public bool AutoDelete { get; }
                public IDictionary<string, object> Arguments { get; }
            }
        }
    }
}