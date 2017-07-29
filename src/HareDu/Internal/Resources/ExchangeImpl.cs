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
    using Common.Logging;
    using Exceptions;
    using Model;

    internal class ExchangeImpl :
        ResourceBase,
        Exchange
    {
        public ExchangeImpl(HttpClient client, ILog logger)
            : base(client, logger)
        {
        }

        public async Task<Result<IEnumerable<ExchangeInfo>>> GetAll(string vhost, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/exchanges/{sanitizedVHost}";

            LogInfo($"Sent request to return all information corresponding to virtual host '{sanitizedVHost}' on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<ExchangeInfo>> result = await response.GetResponse<IEnumerable<ExchangeInfo>>();

            return result;
        }

        public async Task<Result> Create(string exchange, string vhost, Action<ExchangeBehavior> behavior,
            CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(exchange))
                throw new ExchangeMissingException("The name of the exchange is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");
            
            var impl = new ExchangeBehaviorImpl();
            behavior(impl);

            ExchangeSettings excahngeSettings = impl.Settings.Value;
            
            if (string.IsNullOrWhiteSpace(excahngeSettings.RoutingType))
                throw new ExchangeRoutingTypeMissingException("The routing type of the exchange is missing.");

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/exchanges/{sanitizedVHost}/{exchange}";

            LogInfo($"Sent request to RabbitMQ server to create exchange '{exchange} in virtual host '{sanitizedVHost}'.");

            HttpResponseMessage response = await HttpPut(url, excahngeSettings, cancellationToken);
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

        public async Task<Result> Delete(string exchange, string vhost, Action<ExchangeDeleteCondition> condition, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(exchange))
                throw new ExchangeMissingException("The name of the exchange is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            var impl = new ExchangeDeleteConditionImpl();
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

        
        class ExchangeDeleteConditionImpl :
            ExchangeDeleteCondition
        {
            public bool DeleteIfUnused { get; private set; }
            
            public void IfUnused()
            {
                DeleteIfUnused = true;
            }
        }


        class ExchangeBehaviorImpl :
            ExchangeBehavior
        {
            static string _routingType;
            static bool _durable;
            static bool _autoDelete;
            static bool _internal;
            static IDictionary<string, object> _arguments;
            
            public Lazy<ExchangeSettings> Settings { get; }

            public ExchangeBehaviorImpl() => Settings = new Lazy<ExchangeSettings>(Init, LazyThreadSafetyMode.PublicationOnly);

            static ExchangeSettings Init() => new ExchangeSettingsImpl(_routingType, _durable, _autoDelete, _internal, _arguments);

            public void UsingRoutingType(string routingType) => _routingType = routingType;
            
            public void UsingRoutingType(Action<ExchangeRoutingType> routingType)
            {
                var impl = new ExchangeRoutingTypeImpl();
                routingType(impl);
                
                UsingRoutingType(impl.RoutingType);
            }

            public void IsDurable() => _durable = true;

            public void IsForInternalUse() => _internal = true;

            public void WithArguments(IDictionary<string, object> args)
            {
                if (args == null)
                    return;

                _arguments = args;
            }

            public void AutoDeleteWhenNotInUse() => _autoDelete = true;

            
            class ExchangeRoutingTypeImpl :
                ExchangeRoutingType
            {
                public string RoutingType { get; private set; }
                
                public void Fanout()
                {
                    RoutingType = "fanout";
                }

                public void Direct()
                {
                    RoutingType = "direct";
                }

                public void Topic()
                {
                    RoutingType = "topic";
                }

                public void Headers()
                {
                    RoutingType = "headers";
                }

                public void Federated()
                {
                    RoutingType = "federated";
                }

                public void Match()
                {
                    RoutingType = "match";
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