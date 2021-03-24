namespace HareDu.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Extensions;
    using HareDu.Model;
    using Model;
    using Serialization;

    class VirtualHostImpl :
        BaseBrokerObject,
        VirtualHost
    {
        public VirtualHostImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<VirtualHostInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/vhosts";

            ResultList<VirtualHostInfoImpl> result = await GetAll<VirtualHostInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<VirtualHostInfo> MapResult(ResultList<VirtualHostInfoImpl> result) => new ResultListCopy(result);

            return MapResult(result);
        }

        public async Task<Result> Create(Action<VirtualHostCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new VirtualHostCreateActionImpl();
            action(impl);

            impl.Validate();

            VirtualHostDefinition definition = impl.Definition.Value;

            string url = $"api/vhosts/{impl.VirtualHostName.Value.ToSanitizedName()}";

            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString(Deserializer.Options)));

            return await Put(url, definition, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(Action<VirtualHostDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new VirtualHostDeleteActionImpl();
            action(impl);

            impl.Validate();

            string vHost = impl.VirtualHostName.Value.ToSanitizedName();

            string url = $"api/vhosts/{vHost}";

            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url));

            if (vHost == "2%f")
                return new FaultedResult(new List<Error>{ new ErrorImpl("Cannot delete the default virtual host.") }, new DebugInfoImpl(url));

            return await Delete(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Startup(string vhost, Action<VirtualHostStartupAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new VirtualHostStartupActionImpl();
            action(impl);

            impl.Validate();

            string url = $"/api/vhosts/{vhost.ToSanitizedName()}/start/{impl.Node.Value}";

            var errors = new List<Error>();
            errors.AddRange(impl.Errors.Value);

            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            
            if (errors.Any())
                return new FaultedResult(errors, new DebugInfoImpl(url));

            return await PostEmpty(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Create(string vhost, Action<VirtualHostConfigurator> configurator, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new VirtualHostConfiguratorImpl();
            configurator?.Invoke(impl);

            VirtualHostRequest request = impl.Request.Value;

            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            string url = $"api/vhosts/{vhost.ToSanitizedName()}";

            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, request.ToJsonString(Deserializer.Options), errors));

            return await PutRequest(url, request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(string vhost, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var errors = new List<Error>();

            string virtualHost = vhost.ToSanitizedName();

            if (virtualHost == "2%f")
                errors.Add(new ErrorImpl("Cannot delete the default virtual host."));
            else
            {
                if (string.IsNullOrWhiteSpace(virtualHost))
                    errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            string url = $"api/vhosts/{virtualHost}";

            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, errors));

            return await DeleteRequest(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Startup(string vhost, string node, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            if (string.IsNullOrWhiteSpace(node))
                errors.Add(new ErrorImpl("RabbitMQ node is missing."));

            string url = $"/api/vhosts/{vhost.ToSanitizedName()}/start/{node}";
            
            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, errors));

            return await PostEmptyRequest(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result<ServerHealthInfo>> GetHealth(string vhost, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = $"api/aliveness-test/{vhost.ToSanitizedName()}";;

            Result<ServerHealthInfoImpl> result = await GetRequest<ServerHealthInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            Result<ServerHealthInfo> MapResult(Result<ServerHealthInfoImpl> result) => new ResultCopy(result);

            return MapResult(result);
        }

        
        class ResultListCopy :
            ResultList<VirtualHostInfo>
        {
            public ResultListCopy(ResultList<VirtualHostInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;

                var data = new List<VirtualHostInfo>();
                foreach (VirtualHostInfoImpl item in result.Data)
                    data.Add(new InternalVirtualHostInfo(item));

                Data = data;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<VirtualHostInfo> Data { get; }
            public bool HasData { get; }
        }

        
        class ResultCopy :
            Result<ServerHealthInfo>
        {
            public ResultCopy(Result<ServerHealthInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;
                Data = new InternalServerHealthInfo(result.Data);
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public ServerHealthInfo Data { get; }
            public bool HasData { get; }
        }


        class VirtualHostConfiguratorImpl :
            VirtualHostConfigurator
        {
            bool _tracing;
            string _description;
            string _tags;

            public Lazy<VirtualHostRequest> Request { get; }

            public VirtualHostConfiguratorImpl()
            {
                Request = new Lazy<VirtualHostRequest>(
                    () => new VirtualHostRequest(_tracing, _description, _tags), LazyThreadSafetyMode.PublicationOnly);
            }

            public void WithTracingEnabled() => _tracing = true;

            public void Description(string description) => _description = description;

            public void Tags(Action<VirtualHostTagConfigurator> configurator)
            {
                var impl = new VirtualHostTagConfiguratorImpl();
                configurator?.Invoke(impl);

                StringBuilder builder = new StringBuilder();

                impl.Tags.ForEach(x => builder.AppendFormat("{0},", x));

                _tags = builder.ToString().TrimEnd(',');
            }


            class VirtualHostTagConfiguratorImpl :
                VirtualHostTagConfigurator
            {
                readonly List<string> _tags;

                public List<string> Tags => _tags;

                public VirtualHostTagConfiguratorImpl()
                {
                    _tags = new List<string>();
                }

                public void Add(string tag)
                {
                    if (_tags.Contains(tag))
                        return;

                    _tags.Add(tag);
                }
            }
        }

        
        class VirtualHostStartupActionImpl :
            VirtualHostStartupAction
        {
            string _node;
            readonly List<Error> _errors;

            public Lazy<List<Error>> Errors { get; }
            public Lazy<string> Node { get; }

            public VirtualHostStartupActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Node = new Lazy<string>(() => _node, LazyThreadSafetyMode.PublicationOnly);
            }

            public void On(string node) => _node = node;

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_node))
                    _errors.Add(new ErrorImpl("RabbitMQ node is missing."));
            }
        }


        class VirtualHostDeleteActionImpl :
            VirtualHostDeleteAction
        {
            string _vhost;
            readonly List<Error> _errors;

            public Lazy<string> VirtualHostName { get; }
            public Lazy<List<Error>> Errors { get; }

            public VirtualHostDeleteActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void VirtualHost(string name) => _vhost = name;

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }
        }

        
        class VirtualHostCreateActionImpl :
            VirtualHostCreateAction
        {
            bool _tracing;
            string _vhost;
            string _description;
            string _tags;
            readonly List<Error> _errors;

            public Lazy<VirtualHostDefinition> Definition { get; }
            public Lazy<string> VirtualHostName { get; }
            public Lazy<List<Error>> Errors { get; }
            
            public VirtualHostCreateActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<VirtualHostDefinition>(
                    () => new VirtualHostDefinition(_tracing, _description, _tags), LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void VirtualHost(string name) => _vhost = name;

            public void Configure(Action<VirtualHostConfigurator> configurator)
            {
                var impl = new VirtualHostConfiguratorImpl();
                configurator(impl);

                _tracing = impl.Tracing;
                _description = impl.VirtualHostDescription;
                _tags = impl.VirtualHostTags;
            }

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            
            class VirtualHostConfiguratorImpl :
                VirtualHostConfigurator
            {
                public bool Tracing { get; private set; }
                public string VirtualHostDescription { get; private set; }
                public string VirtualHostTags { get; private set; }

                public void WithTracingEnabled() => Tracing = true;
                public void Description(string description) => VirtualHostDescription = description;
                
                public void Tags(Action<VirtualHostTagConfigurator> configurator)
                {
                    var impl = new VirtualHostTagConfiguratorImpl();
                    configurator(impl);

                    StringBuilder builder = new StringBuilder();
                    
                    impl.Tags.ForEach(x => builder.AppendFormat("{0},", x));

                    VirtualHostTags = builder.ToString().TrimEnd(',');
                }

                
                class VirtualHostTagConfiguratorImpl :
                    VirtualHostTagConfigurator
                {
                    readonly List<string> _tags;

                    public List<string> Tags => _tags;

                    public VirtualHostTagConfiguratorImpl()
                    {
                        _tags = new List<string>();
                    }
                    
                    public void Add(string tag)
                    {
                        if (_tags.Contains(tag))
                            return;
                        
                        _tags.Add(tag);
                    }
                }
            }
        }
    }
}