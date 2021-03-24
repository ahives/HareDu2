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

    class UserPermissionsImpl :
        BaseBrokerObject,
        UserPermissions
    {
        public UserPermissionsImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<UserPermissionsInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/permissions";

            ResultList<UserPermissionsInfoImpl> result = await GetAll<UserPermissionsInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<UserPermissionsInfo> MapResult(ResultList<UserPermissionsInfoImpl> result) => new ResultListCopy(result);

            return MapResult(result);
        }

        public async Task<Result> Create(Action<UserPermissionsCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new UserPermissionsCreateActionImpl();
            action(impl);

            impl.Validate();

            UserPermissionsDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string url = $"api/permissions/{impl.VirtualHost.Value.ToSanitizedName()}/{impl.Username.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString(Deserializer.Options)));

            return await Put(url, definition, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(Action<UserPermissionsDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new UserPermissionsDeleteActionImpl();
            action(impl);

            impl.Validate();

            string url = $"api/permissions/{impl.VirtualHost.Value.ToSanitizedName()}/{impl.Username.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url));

            return await Delete(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Create(string username, string vhost,
            Action<UserPermissionsConfigurator> configurator, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new UserPermissionsConfiguratorImpl();
            configurator?.Invoke(impl);

            UserPermissionsRequest request = new UserPermissionsRequest(impl.ConfigurePattern, impl.WritePattern, impl.ReadPattern);

            Debug.Assert(request != null);

            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(username))
                errors.Add(new ErrorImpl("The username and/or password is missing."));
            
            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            
            string url = $"api/permissions/{vhost.ToSanitizedName()}/{username}";
            
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

            string url = $"api/permissions/{vhost.ToSanitizedName()}/{username}";
            
            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, errors));

            return await DeleteRequest(url, cancellationToken).ConfigureAwait(false);
        }

        
        class ResultListCopy :
            ResultList<UserPermissionsInfo>
        {
            public ResultListCopy(ResultList<UserPermissionsInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;

                var data = new List<UserPermissionsInfo>();
                foreach (UserPermissionsInfoImpl item in result.Data)
                    data.Add(new InternalUserPermissionsInfo(item));

                Data = data;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<UserPermissionsInfo> Data { get; }
            public bool HasData { get; }
        }


        class UserPermissionsConfiguratorImpl :
            UserPermissionsConfigurator
        {
            public string ConfigurePattern { get; private set; }
            public string ReadPattern { get; private set; }
            public string WritePattern { get; private set; }

            public void UsingConfigurePattern(string pattern) => ConfigurePattern = pattern;

            public void UsingWritePattern(string pattern) => WritePattern = pattern;

            public void UsingReadPattern(string pattern) => ReadPattern = pattern;
        }

        
        class UserPermissionsDeleteActionImpl :
            UserPermissionsDeleteAction
        {
            string _vhost;
            string _user;
            readonly List<Error> _errors;

            public Lazy<string> Username { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<List<Error>> Errors { get; }

            public UserPermissionsDeleteActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Username = new Lazy<string>(() => _user, LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Targeting(Action<UserPermissionsTarget> target)
            {
                var impl = new UserPermissionsTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
            }

            public void User(string name) => _user = name;

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            
                if (string.IsNullOrWhiteSpace(_user))
                    _errors.Add(new ErrorImpl("The username and/or password is missing."));
            }

            
            class UserPermissionsTargetImpl :
                UserPermissionsTarget
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string name) => VirtualHostName = name;
            }
        }

        
        class UserPermissionsCreateActionImpl :
            UserPermissionsCreateAction
        {
            string _configurePattern;
            string _writePattern;
            string _readPattern;
            string _vhost;
            string _user;
            bool _targetingCalled;
            bool _userCalled;
            readonly List<Error> _errors;

            public Lazy<UserPermissionsDefinition> Definition { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<string> Username { get; }
            public Lazy<List<Error>> Errors { get; }

            public UserPermissionsCreateActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<UserPermissionsDefinition>(
                    () => new UserPermissionsDefinition(_configurePattern, _writePattern, _readPattern), LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                Username = new Lazy<string>(() => _user, LazyThreadSafetyMode.PublicationOnly);
            }

            public void User(string username)
            {
                _userCalled = true;
                
                _user = username;
            
                if (string.IsNullOrWhiteSpace(_user))
                    _errors.Add(new ErrorImpl("The username and/or password is missing."));
            }

            public void Configure(Action<UserPermissionsConfiguration> configure)
            {
                var impl = new UserPermissionsConfigurationImpl();
                configure(impl);

                _configurePattern = impl.ConfigurePattern;
                _writePattern = impl.WritePattern;
                _readPattern = impl.ReadPattern;
            }

            public void Targeting(Action<UserPermissionsTarget> target)
            {
                _targetingCalled = true;
                
                var impl = new UserPermissionsTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;

                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            public void Validate()
            {
                if (!_targetingCalled)
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
                
                if (!_userCalled)
                    _errors.Add(new ErrorImpl("The username and/or password is missing."));
            }

            
            class UserPermissionsTargetImpl :
                UserPermissionsTarget
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string name) => VirtualHostName = name;
            }

            
            class UserPermissionsConfigurationImpl :
                UserPermissionsConfiguration
            {
                public string ConfigurePattern { get; private set; }
                public string ReadPattern { get; private set; }
                public string WritePattern { get; private set; }
                
                public void UsingConfigurePattern(string pattern) => ConfigurePattern = pattern;

                public void UsingWritePattern(string pattern) => WritePattern = pattern;

                public void UsingReadPattern(string pattern) => ReadPattern = pattern;
            }
        }
    }
}