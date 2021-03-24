namespace HareDu.Internal
{
    using System;
    using System.Collections.Generic;
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

    class VirtualHostLimitsImpl :
        BaseBrokerObject,
        VirtualHostLimits
    {
        public VirtualHostLimitsImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<VirtualHostLimitsInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/vhost-limits";

            ResultList<VirtualHostLimitsInfoImpl> result = await GetAll<VirtualHostLimitsInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<VirtualHostLimitsInfo> MapResult(ResultList<VirtualHostLimitsInfoImpl> result) => new ResultListCopy(result);

            return MapResult(result);
        }

        public async Task<Result> Define(Action<VirtualHostConfigureLimitsAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new VirtualHostConfigureLimitsActionImpl();
            action(impl);

            impl.Validate();

            VirtualHostLimitsDefinition definition = impl.Definition.Value;

            string url = $"api/vhost-limits/vhost/{impl.VirtualHostName.Value.ToSanitizedName()}";

            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString(Deserializer.Options)));

            return await Put(url, definition, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(Action<VirtualHostDeleteLimitsAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new VirtualHostDeleteLimitsActionImpl();
            action(impl);

            impl.Validate();

            string url = $"api/vhost-limits/vhost/{impl.VirtualHostName.Value.ToSanitizedName()}";

            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url));

            return await Delete(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Define(string vhost, Action<VirtualHostLimitsConfigurator> configurator = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new VirtualHostLimitsConfiguratorImpl();
            configurator?.Invoke(impl);

            impl.Validate();
            
            VirtualHostLimitsRequest request = impl.Request.Value;

            var errors = new List<Error>();
            
            errors.AddRange(impl.Errors.Value);
            
            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            string url = $"api/vhost-limits/vhost/{vhost.ToSanitizedName()}";

            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, request.ToJsonString(Deserializer.Options), errors));

            return await PutRequest(url, request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(string vhost, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            string url = $"api/vhost-limits/vhost/{vhost.ToSanitizedName()}";

            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, errors));

            return await DeleteRequest(url, cancellationToken).ConfigureAwait(false);
        }

        
        class ResultListCopy :
            ResultList<VirtualHostLimitsInfo>
        {
            public ResultListCopy(ResultList<VirtualHostLimitsInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;

                var data = new List<VirtualHostLimitsInfo>();
                foreach (VirtualHostLimitsInfoImpl item in result.Data)
                    data.Add(new InternalVirtualHostLimitsInfo(item));

                Data = data;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<VirtualHostLimitsInfo> Data { get; }
            public bool HasData { get; }
        }


        class VirtualHostLimitsConfiguratorImpl :
            VirtualHostLimitsConfigurator
        {
            ulong _maxQueueLimits;
            ulong _maxConnectionLimits;
            bool _setMaxConnectionLimitCalled;
            bool _setMaxQueueLimitCalled;

            readonly List<Error> _errors;

            public Lazy<VirtualHostLimitsRequest> Request { get; }
            public Lazy<List<Error>> Errors { get; }

            public VirtualHostLimitsConfiguratorImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Request = new Lazy<VirtualHostLimitsRequest>(
                    () => new VirtualHostLimitsRequest(_maxConnectionLimits, _maxQueueLimits), LazyThreadSafetyMode.PublicationOnly);
            }

            public void SetMaxConnectionLimit(ulong value)
            {
                _setMaxConnectionLimitCalled = true;
                
                _maxConnectionLimits = value;
                
                if (_maxConnectionLimits < 1)
                    _errors.Add(new ErrorImpl("Max connection limit value is missing."));
            }

            public void SetMaxQueueLimit(ulong value)
            {
                _setMaxQueueLimitCalled = true;
                
                _maxQueueLimits = value;
                
                if (_maxQueueLimits < 1)
                    _errors.Add(new ErrorImpl("Max queue limit value is missing."));
            }

            public void Validate()
            {
                if (!_setMaxConnectionLimitCalled && !_setMaxQueueLimitCalled)
                    _errors.Add(new ErrorImpl("There are no limits to define."));
            }
        }


        class VirtualHostConfigureLimitsActionImpl :
            VirtualHostConfigureLimitsAction
        {
            string _vhost;
            ulong _maxConnectionLimits;
            ulong _maxQueueLimits;
            readonly List<Error> _errors;

            public Lazy<string> VirtualHostName { get; }
            public Lazy<List<Error>> Errors { get; }
            public Lazy<VirtualHostLimitsDefinition> Definition { get; }

            public VirtualHostConfigureLimitsActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<VirtualHostLimitsDefinition>(
                    () => new VirtualHostLimitsDefinition(_maxConnectionLimits, _maxQueueLimits), LazyThreadSafetyMode.PublicationOnly);
            }

            public void VirtualHost(string name) => _vhost = name;

            public void Configure(Action<VirtualHostLimitsConfiguration> configuration)
            {
                var impl = new VirtualHostLimitsConfigurationImpl();
                configuration(impl);

                _maxConnectionLimits = impl.MaxConnectionLimit.Value;
                _maxQueueLimits = impl.MaxQueueLimit.Value;
            }

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
                
                if (_maxConnectionLimits < 1)
                    _errors.Add(new ErrorImpl("Max connection limit value is missing."));
                
                if (_maxQueueLimits < 1)
                    _errors.Add(new ErrorImpl("Max queue limit value is missing."));
            }

            
            class VirtualHostLimitsConfigurationImpl :
                VirtualHostLimitsConfiguration
            {
                ulong _maxQueueLimits;
                ulong _maxConnectionLimits;
                
                public Lazy<ulong> MaxConnectionLimit { get; }
                public Lazy<ulong> MaxQueueLimit { get; }
                
                public VirtualHostLimitsConfigurationImpl()
                {
                    MaxConnectionLimit = new Lazy<ulong>(() => _maxConnectionLimits, LazyThreadSafetyMode.PublicationOnly);
                    MaxQueueLimit = new Lazy<ulong>(() => _maxQueueLimits, LazyThreadSafetyMode.PublicationOnly);
                }

                public void SetMaxConnectionLimit(ulong value) => _maxConnectionLimits = value;

                public void SetMaxQueueLimit(ulong value) => _maxQueueLimits = value;
            }
        }


        class VirtualHostDeleteLimitsActionImpl :
            VirtualHostDeleteLimitsAction
        {
            string _vhost;
            readonly List<Error> _errors;

            public Lazy<string> VirtualHostName { get; }
            public Lazy<List<Error>> Errors { get; }

            public VirtualHostDeleteLimitsActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void For(string vhost) => _vhost = vhost;

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }
        }
    }
}