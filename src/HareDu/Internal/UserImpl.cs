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
    using Serialization;

    class UserImpl :
        BaseBrokerObject,
        User
    {
        public UserImpl(HttpClient client)
            : base(client)
        {
        }

        public Task<ResultList<UserInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/users";
            
            return GetAll<UserInfo>(url, cancellationToken);
        }

        public Task<ResultList<UserInfo>> GetAllWithoutPermissions(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/users/without-permissions";
            
            return GetAll<UserInfo>(url, cancellationToken);
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
                    () => new UserDefinitionImpl(_password, _passwordHash, _tags), LazyThreadSafetyMode.PublicationOnly);
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


            class UserDefinitionImpl :
                UserDefinition
            {
                public UserDefinitionImpl(string password, string passwordHash, string tags)
                {
                    PasswordHash = passwordHash;

                    string Normalize(string value) => string.IsNullOrWhiteSpace(value) ? null : value;

                    Password = Normalize(password);
                    Tags = Normalize(tags);
                    
                    if (!string.IsNullOrWhiteSpace(Password))
                        PasswordHash = null;
                }

                public string PasswordHash { get; }
                public string Password { get; }
                public string Tags { get; }
            }
        }
    }
}