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
namespace HareDu.Core.Configuration
{
    using System;

    public class DefaultBrokerObjectConfig :
        BrokerConfig
    {
        public DefaultBrokerObjectConfig()
        {
            BrokerUrl = "http://localhost:15672";
            Credentials = new BrokerCredentialsImpl("guest", "guest");
        }

        public string BrokerUrl { get; }
        public TimeSpan Timeout { get; }
        public BrokerCredentials Credentials { get; }

        
        class BrokerCredentialsImpl :
            BrokerCredentials
        {
            public BrokerCredentialsImpl(string username, string password)
            {
                Username = username;
                Password = password;
            }

            public string Username { get; }
            public string Password { get; }
        }
    }
}