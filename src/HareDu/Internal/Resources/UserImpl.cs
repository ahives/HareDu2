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
    using Model;

    internal class UserImpl :
        ResourceBase,
        User
    {
        public UserImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<Result<IReadOnlyList<UserInfo>>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = $"api/users";
            
            Result<IReadOnlyList<UserInfo>> result = await GetAll<UserInfo>(url, cancellationToken);

            return result;
        }

        public async Task<Result> Create(Action<UserCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new UserCreateActionImpl();
            action(impl);

            DefinedUser definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string user = impl.User.Value;
            
            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(user))
                errors.Add(new ErrorImpl("The username is missing."));
            
            if (string.IsNullOrWhiteSpace(definition.Password) && string.IsNullOrWhiteSpace(definition.PasswordHash))
                errors.Add(new ErrorImpl("The password/hash is missing."));
            
            if (errors.Any())
                return new FaultedResult(errors);
                    
            string url = $"api/users/{user}";

            Result result = await Put(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result> Delete(Action<UserDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new UserDeleteActionImpl();
            action(impl);

            if (string.IsNullOrWhiteSpace(impl.Username))
                return new FaultedResult(new List<Error> {new ErrorImpl("The username is missing.")});

            string url = $"api/users/{impl.Username}";

            Result result = await Delete(url, cancellationToken);

            return result;
        }

        
        class UserDeleteActionImpl :
            UserDeleteAction
        {
            public string Username { get; private set; }
            
            public void User(string name) => Username = name;
        }


        class UserCreateActionImpl :
            UserCreateAction
        {
            static string _password;
            static string _passwordHash;
            static string _tags;
            static string _user;
            
            public Lazy<DefinedUser> Definition { get; }
            public Lazy<string> User { get; }

            public UserCreateActionImpl()
            {
                Definition = new Lazy<DefinedUser>(
                    () => new DefinedUserImpl(_password, _passwordHash, _tags), LazyThreadSafetyMode.PublicationOnly);
                User = new Lazy<string>(() => _user, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Username(string username) => _user = username;

            public void Password(string password) => _password = password;

            public void WithPasswordHash(string passwordHash) => _passwordHash = passwordHash;

            public void WithTags(Action<UserAccessOptions> tags)
            {
                var impl = new UserAccessOptionsImpl();
                tags(impl);

                _tags = impl.ToString();
            }

            
            class UserAccessOptionsImpl :
                UserAccessOptions
            {
                List<string> Tags { get; } = new List<string>();
                
                public void None() => Tags.Add(UserAccessTag.None);

                public void Administrator() => Tags.Add(UserAccessTag.Administrator);

                public void Monitoring() => Tags.Add(UserAccessTag.Monitoring);

                public void Management() => Tags.Add(UserAccessTag.Management);

                public override string ToString()
                {
                    if (Tags.Contains(UserAccessTag.None) || !Tags.Any())
                        return UserAccessTag.None;

                    return string.Join(",", Tags);
                }
            }


            class DefinedUserImpl :
                DefinedUser
            {
                public DefinedUserImpl(string password, string passwordHash, string tags)
                {
                    PasswordHash = passwordHash;
                    Password = password;
                    Tags = tags;
                }

                public string PasswordHash { get; }
                public string Password { get; }
                public string Tags { get; }
            }
        }
    }
}