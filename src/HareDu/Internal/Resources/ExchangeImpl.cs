// Copyright 2013-2018 Albert L. Hives
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

        public async Task<Result<IEnumerable<ExchangeInfo>>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/exchanges";
            var result = await Get<IEnumerable<ExchangeInfo>>(url, cancellationToken);

            return result;
        }

        public async Task<Result<ExchangeInfo>> Create(Action<ExchangeCreateAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new ExchangeCreateActionImpl();
            action(impl);
            
            if (impl.Errors.Value.Any())
                return Result.None<ExchangeInfo>(errors: impl.Errors.Value);

            DefinedExchange definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string exchange = impl.ExchangeName.Value;
            string vhost = impl.VirtualHost.Value;
            
            if (string.IsNullOrWhiteSpace(exchange))
                return Result.None<ExchangeInfo>(errors: new List<Error>{ new ErrorImpl("The name of the exchange is missing.") });

            if (string.IsNullOrWhiteSpace(vhost))
                return Result.None<ExchangeInfo>(errors: new List<Error>{ new ErrorImpl("The name of the virtual host is missing.") });

            if (string.IsNullOrWhiteSpace(definition?.RoutingType))
                return Result.None<ExchangeInfo>(errors: new List<Error>{ new ErrorImpl("The routing type of the exchange is missing.") });
            
            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/exchanges/{sanitizedVHost}/{exchange}";

            var result = await Put<DefinedExchange, ExchangeInfo>(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result<ExchangeInfo>> Delete(Action<ExchangeDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new ExchangeDeleteActionImpl();
            action(impl);

            string exchange = impl.ExchangeName.Value;
            string vhost = impl.VirtualHost.Value;
            
            if (string.IsNullOrWhiteSpace(exchange))
                return Result.None<ExchangeInfo>(errors: new List<Error>{ new ErrorImpl("The name of the exchange is missing.") });

            if (string.IsNullOrWhiteSpace(vhost))
                return Result.None<ExchangeInfo>(errors: new List<Error>{ new ErrorImpl("The name of the virtual host is missing.") });
            
            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/exchanges/{sanitizedVHost}/{exchange}";

            string query = impl.Query.Value;
            
            if (!string.IsNullOrWhiteSpace(query))
                url = $"api/exchanges/{sanitizedVHost}/{exchange}?{query}";

            var result = await Delete<ExchangeInfo>(url, cancellationToken);

            return result;
        }

        
        class ExchangeDeleteActionImpl :
            ExchangeDeleteAction
        {
            static string _vhost;
            static string _exchange;
            static string _query;

            public Lazy<string> Query { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<string> ExchangeName { get; }

            public ExchangeDeleteActionImpl()
            {
                Query = new Lazy<string>(() => _query, LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                ExchangeName = new Lazy<string>(() => _exchange, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Exchange(string name) => _exchange = name;

            public void WithConditions(Action<ExchangeDeleteCondition> condition)
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

                public void IfUnused() => DeleteIfUnused = true;
            }
        }


        class ExchangeCreateActionImpl :
            ExchangeCreateAction
        {
            static string _routingType;
            static bool _durable;
            static bool _autoDelete;
            static bool _internal;
            static IDictionary<string, ArgumentValue<object>> _arguments;
            static string _vhost;
            static string _exchange;

            public Lazy<DefinedExchange> Definition { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<string> ExchangeName { get; }
            public Lazy<List<Error>> Errors { get; }

            public ExchangeCreateActionImpl()
            {
                Errors = new Lazy<List<Error>>(() => _arguments.Select(x => x.Value.Error).ToList(), LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<DefinedExchange>(
                    () => new DefinedExchangeImpl(_routingType, _durable, _autoDelete, _internal, _arguments), LazyThreadSafetyMode.PublicationOnly);
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

            
            class ExchangeTargetImpl :
                ExchangeTarget
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string name) => VirtualHostName = name;
            }


            class ExchangeConfigurationImpl :
                ExchangeConfiguration
            {
                public string RoutingType { get; private set; }
                public IDictionary<string, ArgumentValue<object>> Arguments { get; private set; }
                public bool Durable { get; private set; }
                public bool InternalUse { get; private set; }
                public bool AutoDelete { get; private set; }

                public void HasRoutingType(ExchangeRoutingType routingType)
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
                public IDictionary<string, ArgumentValue<object>> Arguments { get; } = new Dictionary<string, ArgumentValue<object>>();

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


            class DefinedExchangeImpl :
                DefinedExchange
            {
                public DefinedExchangeImpl(string routingType, bool durable, bool autoDelete, bool @internal, IDictionary<string, ArgumentValue<object>> arguments)
                {
                    RoutingType = routingType;
                    Durable = durable;
                    AutoDelete = autoDelete;
                    Internal = @internal;
                    Arguments = arguments.ToDictionary(x => x.Key, x => x.Value.Value);
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