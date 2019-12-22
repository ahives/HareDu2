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
namespace HareDu.Tests
{
    using Configuration;
    using Core.Configuration;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class BrokerClientConfigProviderTests
    {
        [Test]
        public void Test()
        {
            var configProvider = new ConfigurationProvider();
            var provider = new BrokerClientConfigProvider(configProvider);
            var settings = provider.Init(x =>
            {
                x.ConnectTo("http://localhost:15670");
                x.UsingCredentials("guest1", "guest1");
            });
            
            settings.BrokerUrl.ShouldBe("http://localhost:15670");
            settings.Credentials.ShouldNotBeNull();
            settings.Credentials.Username.ShouldBe("guest1");
            settings.Credentials.Password.ShouldBe("guest1");
        }
        
        [Test]
        public void Test1()
        {
            var configProvider = new ConfigurationProvider();
            var provider = new BrokerClientConfigProvider(configProvider);
            var settings = provider.Init(x =>
            {
            });
            
            settings.BrokerUrl.ShouldBe("http://localhost:15672");
            settings.Credentials.ShouldNotBeNull();
            settings.Credentials.Username.ShouldBe("guest");
            settings.Credentials.Password.ShouldBe("guest");
        }
    }
}