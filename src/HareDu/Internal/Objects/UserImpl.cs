// Copyright 2013-2019 Albert L. Hives
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
namespace HareDu.Internal.Objects
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
    using Model;

    class UserImpl :
        RmqBrokerClient,
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
            
            ResultList<UserInfo> result = await GetAll<UserInfo>(url, cancellationToken);

            return result;
        }

        public async Task<ResultList<UserInfo>> GetAllWithoutPermissions(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/users/without-permissions";
            
            ResultList<UserInfo> result = await GetAll<UserInfo>(url, cancellationToken);

            return result;
        }

        public async Task<Result> Create(Action<UserCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new UserCreateActionImpl();
            action(impl);

            UserDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);
                    
            string url = $"api/users/{impl.User.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString()));

            Result result = await Put(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result> Delete(Action<UserDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new UserDeleteActionImpl();
            action(impl);

            string url = $"api/users/{impl.Username}";

            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, null));

            Result result = await Delete(url, cancellationToken);

            return result;
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

            public void User(string name)
            {
                _user = name;

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

            public void Username(string username)
            {
                _user = username;
            
                if (string.IsNullOrWhiteSpace(_user))
                    _errors.Add(new ErrorImpl("The username is missing."));
            }

            public void Password(string password)
            {
                _password = password;
            
                if (string.IsNullOrWhiteSpace(_password))
                    _errors.Add(new ErrorImpl("The password is missing."));
            }

            public void WithPasswordHash(string password)
            {
                _passwordHash = password.ComputePasswordHash();
            
                if (string.IsNullOrWhiteSpace(_passwordHash))
                    _errors.Add(new ErrorImpl("The password hash is missing."));
            }

            public void WithTags(Action<UserAccessOptions> tags)
            {
                var impl = new UserAccessOptionsImpl();
                tags(impl);

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