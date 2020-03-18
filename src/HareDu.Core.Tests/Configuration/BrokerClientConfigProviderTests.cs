// Copyright 2013-2020 Albert L. Hives
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
namespace HareDu.Core.Tests.Configuration
{
    using System;
    using Core.Configuration;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class BrokerClientConfigProviderTests
    {
        [Test]
        public void Verify_can_programmatically_initialize_client_api()
        {
            // var configProvider = new ConfigurationProvider();
            // var provider = new BrokerConfigProvider(configProvider);
            var provider = new BrokerConfigProvider();
            var settings = provider.Configure(x =>
            {
                x.ConnectTo("http://localhost:15670");
                x.UsingCredentials("guest1", "guest1");
                x.TimeoutAfter(new TimeSpan(0, 0, 30));
            });
            
            settings.Url.ShouldBe("http://localhost:15670");
            settings.Credentials.ShouldNotBeNull();
            settings.Credentials.Username.ShouldBe("guest1");
            settings.Credentials.Password.ShouldBe("guest1");
        }
        
        [Test]
        public void Verify_can_initialize_client_api_via_config_file()
        {
            // var configProvider = new ConfigurationProvider();
            // var provider = new BrokerConfigProvider(configProvider);
            var provider = new BrokerConfigProvider();
            var settings = provider.Configure(x =>
            {
            });
            
            settings.Url.ShouldBe("http://localhost:15672");
            settings.Credentials.ShouldNotBeNull();
            settings.Credentials.Username.ShouldBe("guest");
            settings.Credentials.Password.ShouldBe("guest");
        }
    }
}