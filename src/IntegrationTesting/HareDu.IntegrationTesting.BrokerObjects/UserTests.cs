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
namespace HareDu.IntegrationTesting.BrokerObjects
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core.Extensions;
    using Extensions;
    using NUnit.Framework;
    using Registration;

    [TestFixture]
    public class UserTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            _container = new ContainerBuilder()
                .AddHareDuConfiguration($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .AddHareDu()
                .Build();
        }

        [Test]
        public async Task Verify_can_get_all_users()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<User>()
                .GetAll()
                .ScreenDump();
        }
        
        [Test]
        public async Task Verify_can_get_all_users_without_permissions()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<User>()
                .GetAllWithoutPermissions()
                .ScreenDump();
        }
        
        [Test]
        public async Task Verify_can_create()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<User>()
                .Create(x =>
                {
                    x.Username("testuser3");
                    x.Password("testuserpwd3");
                    x.PasswordHash("gkgfjjhfjh");
                    x.WithTags(t =>
                    {
                        t.Administrator();
                    });
                });
            
            Console.WriteLine(result.ToJsonString());
        }


        [Test]
        public async Task Verify_can_delete()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<User>()
                .Delete(x => x.User(""));
        }
    }
}