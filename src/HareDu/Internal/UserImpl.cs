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

    class UserImpl :
        BaseBrokerObject,
        User
    {
        public UserImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<UserInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/users";

            ResultList<UserInfoImpl> result = await GetAll<UserInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<UserInfo> MapResult(ResultList<UserInfoImpl> result) => new ResultListCopy(result);

            return MapResult(result);
        }

        public async Task<ResultList<UserInfo>> GetAllWithoutPermissions(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/users/without-permissions";

            ResultList<UserInfoImpl> result = await GetAll<UserInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<UserInfo> MapResult(ResultList<UserInfoImpl> result) => new ResultListCopy(result);

            return MapResult(result);
        }

        public Task<Result> Create(Action<UserCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new UserCreateActionImpl();
            action(impl);

            impl.Validate();

            UserDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);
                    
            string url = $"api/users/{impl.User.Value}";
            
            if (impl.Errors.Value.Any())
                return Task.FromResult<Result>(new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString(Deserializer.Options))));

            return Put(url, definition, cancellationToken);
        }

        public Task<Result> Delete(Action<UserDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new UserDeleteActionImpl();
            action(impl);

            impl.Validate();

            string url = $"api/users/{impl.Username}";

            if (impl.Errors.Value.Any())
                return Task.FromResult<Result>(new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url)));

            return Delete(url, cancellationToken);
        }

        public async Task<Result> Create(string username, string password, string passwordHash = null,
            Action<UserConfigurator> configurator = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new UserConfiguratorImpl();
            configurator?.Invoke(impl);

            string Normalize(string value) => string.IsNullOrWhiteSpace(value) ? null : value;

            UserRequest request =
                new UserRequest(!string.IsNullOrWhiteSpace(Normalize(password)) ? null : passwordHash,
                    Normalize(password), impl.Tags.Value);

            Debug.Assert(request != null);
                    
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(username))
                errors.Add(new ErrorImpl("The username is missing."));

            if (string.IsNullOrWhiteSpace(password))
            {
                if (string.IsNullOrWhiteSpace(passwordHash))
                    errors.Add(new ErrorImpl("The password/hash is missing."));
            }
            
            string url = $"api/users/{username}";

            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, request.ToJsonString(Deserializer.Options), errors));

            return await PutRequest(url, request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(string username, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();
                
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(username))
                errors.Add(new ErrorImpl("The username is missing."));

            string url = $"api/users/{username}";

            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, errors));

            return await DeleteRequest(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(IList<string> usernames, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/users/bulk-delete";

            if (usernames.IsEmpty())
                return new FaultedResult(new DebugInfoImpl(url, new List<Error>{new ErrorImpl("Valid usernames is missing.")}));
                
            var errors = new List<Error>();

            for (int i = 0; i < usernames.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(usernames[i]))
                    errors.Add(new ErrorImpl($"The username at index {i} is missing."));
            }

            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, errors));

            BulkUserDeleteRequest request = new BulkUserDeleteRequest(usernames);

            Debug.Assert(request != null);

            return await PostRequest(url, request, cancellationToken).ConfigureAwait(false);
        }

        
        class ResultListCopy :
            ResultList<UserInfo>
        {
            public ResultListCopy(ResultList<UserInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;

                var data = new List<UserInfo>();
                foreach (UserInfoImpl item in result.Data)
                    data.Add(new InternalUserInfo(item));

                Data = data;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<UserInfo> Data { get; }
            public bool HasData { get; }
        }


        class UserConfiguratorImpl :
            UserConfigurator
        {
            string _tags;

            public Lazy<string> Tags { get; }

            public UserConfiguratorImpl()
            {
                Tags = new Lazy<string>(() => _tags, LazyThreadSafetyMode.PublicationOnly);
            }

            public void WithTags(Action<UserAccessOptions> tags)
            {
                var impl = new UserAccessOptionsImpl();
                tags?.Invoke(impl);

                _tags = impl.ToString();
            }

            
            class UserAccessOptionsImpl :
                UserAccessOptions
            {
                List<string> Tags { get; }
                
                public UserAccessOptionsImpl()
                {
                    Tags = new List<string>();
                }

                public void None() => Tags.Add(UserAccessTag.None);

                public void Administrator() => Tags.Add(UserAccessTag.Administrator);

                public void Monitoring() => Tags.Add(UserAccessTag.Monitoring);

                public void Management() => Tags.Add(UserAccessTag.Management);
                
                public void PolicyMaker() => Tags.Add(UserAccessTag.PolicyMaker);

                public void Impersonator() => Tags.Add(UserAccessTag.Impersonator);

                public override string ToString()
                {
                    if (Tags.Contains(UserAccessTag.None) || !Tags.Any())
                        return UserAccessTag.None;

                    return string.Join(",", Tags);
                }
            }
        }

        
        class UserDeleteActionImpl :
            UserDeleteAction
        {
            string _user;
            readonly List<Error> _errors;

            public Lazy<string> Username { get; }
            public Lazy<List<Error>> Errors { get; }

            public UserDeleteActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Username = new Lazy<string>(() => _user, LazyThreadSafetyMode.PublicationOnly);
            }

            public void User(string name) => _user = name;

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_user))
                    _errors.Add(new ErrorImpl("The username is missing."));
            }
        }


        class UserCreateActionImpl :
            UserCreateAction
        {
            string _password;
            string _passwordHash;
            string _tags;
            string _user;
            readonly List<Error> _errors;

            public Lazy<UserDefinition> Definition { get; }
            public Lazy<string> User { get; }
            public Lazy<List<Error>> Errors { get; }

            public UserCreateActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<UserDefinition>(
                    () => new UserDefinition(_passwordHash, _password, _tags), LazyThreadSafetyMode.PublicationOnly);
                User = new Lazy<string>(() => _user, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Username(string username) => _user = username;

            public void Password(string password) => _password = password;

            public void PasswordHash(string passwordHash) => _passwordHash = passwordHash;

            public void WithTags(Action<UserAccessOptions> tags)
            {
                var impl = new UserAccessOptionsImpl();
                tags(impl);

                _tags = impl.ToString();
            }

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_user))
                    _errors.Add(new ErrorImpl("The username is missing."));

                if (string.IsNullOrWhiteSpace(_password))
                {
                    if (string.IsNullOrWhiteSpace(_passwordHash))
                        _errors.Add(new ErrorImpl("The password/hash is missing."));
                }
            }

            
            class UserAccessOptionsImpl :
                UserAccessOptions
            {
                List<string> Tags { get; }
                
                public UserAccessOptionsImpl()
                {
                    Tags = new List<string>();
                }

                public void None() => Tags.Add(UserAccessTag.None);

                public void Administrator() => Tags.Add(UserAccessTag.Administrator);

                public void Monitoring() => Tags.Add(UserAccessTag.Monitoring);

                public void Management() => Tags.Add(UserAccessTag.Management);
                
                public void PolicyMaker() => Tags.Add(UserAccessTag.PolicyMaker);

                public void Impersonator() => Tags.Add(UserAccessTag.Impersonator);

                public override string ToString()
                {
                    if (Tags.Contains(UserAccessTag.None) || !Tags.Any())
                        return UserAccessTag.None;

                    return string.Join(",", Tags);
                }
            }
        }
    }
}