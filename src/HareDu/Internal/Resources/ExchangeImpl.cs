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

    internal class ExchangeImpl :
        ResourceBase,
        Exchange
    {
        public ExchangeImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<IEnumerable<ExchangeInfo>>> GetAll(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/exchanges";

            LogInfo($"Sent request to return all information on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<ExchangeInfo>> result = await response.GetResponse<IEnumerable<ExchangeInfo>>();

            return result;
        }

        public async Task<Result> Create(Action<ExchangeDefinition> definition, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new ExchangeDefinitionImpl();
            definition(impl);

            ExchangeSettings settings = impl.Settings.Value;
            
            if (string.IsNullOrWhiteSpace(settings?.RoutingType))
                throw new ExchangeRoutingTypeMissingException("The routing type of the exchange is missing.");

            string sanitizedVHost = impl.VirtualHost.SanitizeVirtualHostName();

            string url = $"api/exchanges/{sanitizedVHost}/{impl.ExchangeName}";

            LogInfo($"Sent request to RabbitMQ server to create exchange '{impl.ExchangeName} in virtual host '{sanitizedVHost}'.");

            HttpResponseMessage response = await HttpPut(url, settings, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string exchange, string vhost, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(exchange))
                throw new ExchangeMissingException("The name of the exchange is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/exchanges/{sanitizedVHost}/{exchange}";

            LogInfo($"Sent request to RabbitMQ server to delete exchange '{exchange}' from virtual host '{sanitizedVHost}'.");

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string exchange, string vhost, Action<DeleteExchangeCondition> condition, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(exchange))
                throw new ExchangeMissingException("The name of the exchange is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            var impl = new DeleteExchangeConditionImpl();
            condition(impl);
            
            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/exchanges/{sanitizedVHost}/{exchange}";

            if (impl.DeleteIfUnused)
                url = $"api/exchanges/{sanitizedVHost}/{exchange}?if-unused=true";

            LogInfo($"Sent request to RabbitMQ server to delete exchange '{exchange}' from virtual host '{sanitizedVHost}'.");

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        
        class DeleteExchangeConditionImpl :
            DeleteExchangeCondition
        {
            public bool DeleteIfUnused { get; private set; }
            
            public void IfUnused() => DeleteIfUnused = true;
        }


        class ExchangeDefinitionImpl :
            ExchangeDefinition
        {
            static string _routingType;
            static bool _durable;
            static bool _autoDelete;
            static bool _internal;
            static IDictionary<string, object> _arguments;
            
            public Lazy<ExchangeSettings> Settings { get; }
            public string VirtualHost { get; private set; }
            public string ExchangeName { get; private set; }

            public ExchangeDefinitionImpl() => Settings = new Lazy<ExchangeSettings>(Init, LazyThreadSafetyMode.PublicationOnly);

            static ExchangeSettings Init() => new ExchangeSettingsImpl(_routingType, _durable, _autoDelete, _internal, _arguments);

            public void Configure(Action<ExchangeConfiguration> definition)
            {
                var impl = new ExchangeConfigurationImpl();
                definition(impl);

                _durable = impl.Durable;
                _routingType = impl.RoutingType;
                _autoDelete = impl.AutoDelete;
                _internal = impl.InternalUse;
                _arguments = impl.Arguments;

                if (string.IsNullOrWhiteSpace(impl.ExchangeName))
                    throw new ExchangeMissingException("The name of the exchange is missing.");

                ExchangeName = impl.ExchangeName;
            }

            public void OnVirtualHost(string vhost)
            {
                if (string.IsNullOrWhiteSpace(vhost))
                    throw new VirtualHostMissingException("The name of the virtual host is missing.");

                VirtualHost = vhost;
            }


            class ExchangeConfigurationImpl :
                ExchangeConfiguration
            {
                public string ExchangeName { get; private set; }
                public string RoutingType { get; private set; }
                public IDictionary<string, object> Arguments { get; private set; }
                public bool Durable { get; private set; }
                public bool InternalUse { get; private set; }
                public bool AutoDelete { get; private set; }

                public void Name(string name) => ExchangeName = name;

                public void UsingRoutingType(ExchangeRoutingType routingType)
                {
                    switch (routingType)
                    {
                        case ExchangeRoutingType.Fanout:
                            RoutingType = "fanout";
                            break;
                            
                        case ExchangeRoutingType.Direct:
                            RoutingType = "direct";
                            break;
                            
                        case ExchangeRoutingType.Topic:
                            RoutingType = "topic";
                            break;
                            
                        case ExchangeRoutingType.Headers:
                            RoutingType = "headers";
                            break;
                            
                        case ExchangeRoutingType.Federated:
                            RoutingType = "federated";
                            break;
                            
                        case ExchangeRoutingType.Match:
                            RoutingType = "match";
                            break;
                            
                        default:
                            throw new ArgumentOutOfRangeException(nameof(routingType), routingType, null);
                    }
                }

                public void IsDurable() => Durable = true;

                public void IsForInternalUse() => InternalUse = true;

                public void WithArguments(Action<ExchangeDefinitionArguments> arguments)
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
                public IDictionary<string, object> Arguments { get; } = new Dictionary<string, object>();

                public void Set<T>(string arg, T value)
                {
                    Validate(arg);
                    
                    Arguments.Add(arg, value);
                }

                void Validate(string arg)
                {
                    if (Arguments.ContainsKey(arg))
                        throw new ExchangeDefinitionException($"Argument '{arg}' has already been set");
                }
            }


            class ExchangeSettingsImpl :
                ExchangeSettings
            {
                public ExchangeSettingsImpl(string routingType, bool durable, bool autoDelete, bool @internal, IDictionary<string, object> arguments)
                {
                    RoutingType = routingType;
                    Durable = durable;
                    AutoDelete = autoDelete;
                    Internal = @internal;
                    Arguments = arguments;
                }

                public string RoutingType { get; }
                public bool Durable { get; }
                public bool AutoDelete { get; }
                public bool Internal { get; }
                public IDictionary<string, object> Arguments { get; }
            }
        }
    }
}