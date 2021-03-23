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
    using Extensions;
    using HareDu.Model;
    using Model;
    using Serialization;

    class TopicPermissionsImpl :
        BaseBrokerObject,
        TopicPermissions
    {
        public TopicPermissionsImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<TopicPermissionsInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/topic-permissions";

            ResultList<TopicPermissionsInfoImpl> result = await GetAll<TopicPermissionsInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<TopicPermissionsInfo> MapResult(ResultList<TopicPermissionsInfoImpl> result) => new ResultListCopy(result);

            return MapResult(result);
        }

        public async Task<Result> Create(Action<TopicPermissionsCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new TopicPermissionsCreateActionImpl();
            action(impl);

            impl.Validate();

            TopicPermissionsDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string url = $"api/topic-permissions/{impl.VirtualHostName.Value.ToSanitizedName()}/{impl.Username.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString(Deserializer.Options)));

            return await Put(url, definition, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(Action<TopicPermissionsDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new TopicPermissionsDeleteActionImpl();
            action(impl);
            
            impl.Validate();

            string url = $"api/topic-permissions/{impl.VirtualHostName.Value.ToSanitizedName()}/{impl.Username.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url));

            return await Delete(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Create(string username, string exchange, string vhost, Action<TopicPermissionsConfigurator> configurator,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new TopicPermissionsConfiguratorImpl();
            configurator?.Invoke(impl);

            impl.Validate();

            var errors = new List<Error>();
            
            errors.AddRange(impl.Errors.Value);

            if (string.IsNullOrWhiteSpace(exchange))
                errors.Add(new ErrorImpl("Then name of the exchange is missing."));
            
            if (string.IsNullOrWhiteSpace(username))
                errors.Add(new ErrorImpl("The username and/or password is missing."));
            
            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            TopicPermissionsRequest request = new TopicPermissionsRequest(exchange, impl.WritePattern.Value, impl.ReadPattern.Value);

            Debug.Assert(request != null);

            string url = $"api/topic-permissions/{vhost.ToSanitizedName()}/{username}";
            
            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, request.ToJsonString(Deserializer.Options), errors));

            return await PutRequest(url, request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(string username, string vhost, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(username))
                errors.Add(new ErrorImpl("The username and/or password is missing."));
            
            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            string url = $"api/topic-permissions/{vhost.ToSanitizedName()}/{username}";
            
            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, errors));

            return await DeleteRequest(url, cancellationToken).ConfigureAwait(false);
        }


        class TopicPermissionsConfiguratorImpl :
            TopicPermissionsConfigurator
        {
            string _writePattern;
            string _readPattern;
            bool _usingWritePatternCalled;
            bool _usingReadPatternCalled;
            
            readonly List<Error> _errors;

            public Lazy<string> WritePattern { get; }
            public Lazy<string> ReadPattern { get; }
            public Lazy<List<Error>> Errors { get; }

            public TopicPermissionsConfiguratorImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                WritePattern = new Lazy<string>(() => _writePattern, LazyThreadSafetyMode.PublicationOnly);
                ReadPattern = new Lazy<string>(() => _readPattern, LazyThreadSafetyMode.PublicationOnly);
            }

            public void UsingWritePattern(string pattern)
            {
                _usingWritePatternCalled = true;
                
                _writePattern = pattern;
                
                if (string.IsNullOrWhiteSpace(_writePattern))
                    _errors.Add(new ErrorImpl("The write pattern is missing."));
            }

            public void UsingReadPattern(string pattern)
            {
                _usingReadPatternCalled = true;
                
                _readPattern = pattern;
                
                if (string.IsNullOrWhiteSpace(_readPattern))
                    _errors.Add(new ErrorImpl("The read pattern is missing."));
            }

            public void Validate()
            {
                if (!_usingWritePatternCalled)
                    _errors.Add(new ErrorImpl("The write pattern is missing."));
                
                if (!_usingReadPatternCalled)
                    _errors.Add(new ErrorImpl("The read pattern is missing."));
            }
        }

        
        class ResultListCopy :
            ResultList<TopicPermissionsInfo>
        {
            public ResultListCopy(ResultList<TopicPermissionsInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;

                var data = new List<TopicPermissionsInfo>();
                foreach (TopicPermissionsInfoImpl item in result.Data)
                    data.Add(new InternalTopicPermissionsInfo(item));

                Data = data;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<TopicPermissionsInfo> Data { get; }
            public bool HasData { get; }
        }

        
        class TopicPermissionsDeleteActionImpl :
            TopicPermissionsDeleteAction
        {
            string _vhost;
            string _user;
            bool _userCalled;
            bool _virtualHostCalled;
            readonly List<Error> _errors;

            public Lazy<string> Username { get; }
            public Lazy<string> VirtualHostName { get; }
            public Lazy<List<Error>> Errors { get; }

            public TopicPermissionsDeleteActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Username = new Lazy<string>(() => _user, LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void User(string username)
            {
                _userCalled = true;
                
                _user = username;
            
                if (string.IsNullOrWhiteSpace(_user))
                    _errors.Add(new ErrorImpl("The user is missing."));
            }

            public void VirtualHost(string name)
            {
                _virtualHostCalled = true;
                
                _vhost = name;
            
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            public void Validate()
            {
                if (!_userCalled)
                    _errors.Add(new ErrorImpl("The username and/or password is missing."));
                
                if (!_virtualHostCalled)
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }
        }


        class TopicPermissionsCreateActionImpl :
            TopicPermissionsCreateAction
        {
            string _exchange;
            string _writePattern;
            string _readPattern;
            string _vhost;
            string _user;
            bool _userCalled;
            bool _configureCalled;
            bool _virtualHostCalled;
            readonly List<Error> _errors;

            public Lazy<TopicPermissionsDefinition> Definition { get; }
            public Lazy<string> VirtualHostName { get; }
            public Lazy<string> Username { get; }
            public Lazy<List<Error>> Errors { get; }

            public TopicPermissionsCreateActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<TopicPermissionsDefinition>(
                    () => new TopicPermissionsDefinition(_exchange, _writePattern, _readPattern), LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                Username = new Lazy<string>(() => _user, LazyThreadSafetyMode.PublicationOnly);
            }

            public void User(string username)
            {
                _userCalled = true;
                
                _user = username;
            
                if (string.IsNullOrWhiteSpace(_user))
                    _errors.Add(new ErrorImpl("The username and/or password is missing."));
            }

            public void Configure(Action<TopicPermissionsConfiguration> configure)
            {
                _configureCalled = true;
                
                var impl = new TopicPermissionsConfigurationImpl();
                configure(impl);

                _exchange = impl.ExchangeName;
                _writePattern = impl.WritePattern;
                _readPattern = impl.ReadPattern;
                
                if (string.IsNullOrWhiteSpace(_exchange))
                    _errors.Add(new ErrorImpl("Then name of the exchange is missing."));
                
                if (string.IsNullOrWhiteSpace(_writePattern))
                    _errors.Add(new ErrorImpl("The write pattern is missing."));
                
                if (string.IsNullOrWhiteSpace(_readPattern))
                    _errors.Add(new ErrorImpl("The read pattern is missing."));
            }

            public void VirtualHost(string name)
            {
                _virtualHostCalled = true;
                
                _vhost = name;

                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            public void Validate()
            {
                if (!_userCalled)
                    _errors.Add(new ErrorImpl("The username and/or password is missing."));
                
                if (!_virtualHostCalled)
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));

                if (!_configureCalled)
                {
                    _errors.Add(new ErrorImpl("Then name of the exchange is missing."));
                    _errors.Add(new ErrorImpl("The write pattern is missing."));
                    _errors.Add(new ErrorImpl("The read pattern is missing."));
                }
            }

            
            class TopicPermissionsConfigurationImpl :
                TopicPermissionsConfiguration
            {
                public string ExchangeName { get; private set; }
                public string WritePattern { get; private set; }
                public string ReadPattern { get; private set; }

                public void OnExchange(string name) => ExchangeName = name;

                public void UsingWritePattern(string pattern) => WritePattern = pattern;

                public void UsingReadPattern(string pattern) => ReadPattern = pattern;
            }
        }
    }
}