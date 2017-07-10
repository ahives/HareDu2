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

    internal class ExchangeResourceImpl :
        ResourceBase,
        ExchangeResource
    {
        public ExchangeResourceImpl(HttpClient client, ILog logger)
            : base(client, logger)
        {
        }

        public async Task<Result<Exchange>> Get(string exchangeName, string vhostName, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(exchangeName))
                throw new ExchangeMissingException("The name of the exchange is missing.");

            if (string.IsNullOrWhiteSpace(vhostName))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            string sanitizedVHostName = vhostName.SanitizeVirtualHostName();

            string url = $"api/exchanges/{sanitizedVHostName}/{exchangeName}";

            LogInfo($"Sent request to return all information corresponding to virtual host '{sanitizedVHostName}' on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<Exchange> result = await response.GetResponse<Exchange>();

            return result;
        }

        public async Task<Result<IEnumerable<Exchange>>> GetAll(string vhostName, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(vhostName))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            string sanitizedVHostName = vhostName.SanitizeVirtualHostName();

            string url = $"api/exchanges/{sanitizedVHostName}";

            LogInfo($"Sent request to return all information corresponding to virtual host '{sanitizedVHostName}' on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<Exchange>> result = await response.GetResponse<IEnumerable<Exchange>>();

            return result;
        }

        public async Task<Result> Create(string exchangeName, string vhostName, Action<ExchangeBehavior> settings,
            CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(exchangeName))
                throw new ExchangeMissingException("The name of the exchange is missing.");

            if (string.IsNullOrWhiteSpace(vhostName))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");
            
            var impl = new ExchangeBehaviorImpl();
            settings(impl);

            ExchangeSettings excahngeSettings = impl.Settings.Value;
            
            if (string.IsNullOrWhiteSpace(excahngeSettings.RoutingType))
                throw new ExchangeRoutingTypeMissingException("The routing type of the exchange is missing.");

            string sanitizedVHostName = vhostName.SanitizeVirtualHostName();

            string url = $"api/exchanges/{sanitizedVHostName}/{exchangeName}";

            LogInfo($"Sent request to RabbitMQ server to create exchange '{exchangeName} in virtual host '{sanitizedVHostName}'.");

            HttpResponseMessage response = await HttpPut(url, impl.Settings.Value, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string exchangeName, string vhostName, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(exchangeName))
                throw new ExchangeMissingException("The name of the exchange is missing.");

            if (string.IsNullOrWhiteSpace(vhostName))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            string sanitizedVHostName = vhostName.SanitizeVirtualHostName();

            string url = $"api/exchanges/{sanitizedVHostName}/{exchangeName}";

            LogInfo($"Sent request to RabbitMQ server to delete exchange '{exchangeName}' from virtual host '{vhostName}'.");

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string exchangeName, string vhostName, Action<DeleteCondition> condition, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(exchangeName))
                throw new ExchangeMissingException("The name of the exchange is missing.");

            if (string.IsNullOrWhiteSpace(vhostName))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            var impl = new DeleteConditionImpl();
            condition(impl);
            
            string sanitizedVHostName = vhostName.SanitizeVirtualHostName();

            string url = $"api/exchanges/{sanitizedVHostName}/{exchangeName}";

            if (impl.DeleteIfUnused)
                url = $"api/exchanges/{sanitizedVHostName}/{exchangeName}?if-unused=true";

            LogInfo($"Sent request to RabbitMQ server to delete exchange '{exchangeName}' from virtual host '{vhostName}'.");

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        
        class DeleteConditionImpl :
            DeleteCondition
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