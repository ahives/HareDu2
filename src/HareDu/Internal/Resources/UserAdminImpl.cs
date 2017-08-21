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
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Exceptions;
    using Model;

    internal class UserAdminImpl :
        ResourceBase,
        UserAdmin
    {
        public UserAdminImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<IEnumerable<UserInfo>>> GetAll(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/users";

            LogInfo($"Sent request to return all user information on current RabbitMQ server");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<UserInfo>> result = await response.GetResponse<IEnumerable<UserInfo>>();

            return result;
        }

        public async Task<Result> Create(Action<UserDefinition> definition, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new UserDefinitionImpl();
            definition(impl);

            UserChacteristics settings = impl.Characteristics.Value;
            
            string url = $"api/users/{impl.User}";

            LogInfo($"Sent request to RabbitMQ server to create user '{impl.User}'");

            HttpResponseMessage response = await HttpPut(url, settings, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string username, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(username))
                throw new UserCredentialsMissingException("The username is missing.");

            string url = $"api/users/{username}";

            LogInfo($"Sent request to RabbitMQ server to create user '{username}'");

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        
        class UserDefinitionImpl :
            UserDefinition
        {
            static string _password;
            static string _passwordHash;
            static string _tags;
            
            public Lazy<UserChacteristics> Characteristics { get; }
            public string User { get; private set; }

            public UserDefinitionImpl()
                => Characteristics = new Lazy<UserChacteristics>(Init, LazyThreadSafetyMode.PublicationOnly);

            UserChacteristics Init() => new UserChacteristicsImpl(_password, _passwordHash, _tags);

            public void Username(string username)
            {
                if (string.IsNullOrWhiteSpace(username))
                    throw new UserCredentialsMissingException("The username and/or password is missing.");
            
                User = username;
            }

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
                
                public void None() => Tags.Add(UserPermissionTag.None);

                public void Administrator() => Tags.Add(UserPermissionTag.Administrator);

                public void Monitoring() => Tags.Add(UserPermissionTag.Monitoring);

                public void Management() => Tags.Add(UserPermissionTag.Management);

                public override string ToString()
                {
                    if (Tags.Contains(UserPermissionTag.None) || !Tags.Any())
                        return UserPermissionTag.None;

                    return string.Join(",", Tags);
                }
            }


            class UserChacteristicsImpl :
                UserChacteristics
            {
                public UserChacteristicsImpl(string password, string passwordHash, string tags)
                {
                    if (string.IsNullOrWhiteSpace(password) && string.IsNullOrWhiteSpace(passwordHash))
                        throw new UserCredentialsMissingException("The username and/or password is missing.");
                    
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