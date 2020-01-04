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
    using NUnit.Framework;

    [TestFixture]
    public class TopicPermissionsTest
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<HareDuModule>();

            _container = builder.Build();
        }

        [Test]
        public async Task Verify_can_get_all_topic_permissions()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .GetAll();
            
            foreach (var permission in result.Select(x => x.Data))
            {
                Console.WriteLine("VirtualHost: {0}", permission.VirtualHost);
                Console.WriteLine("Exchange: {0}", permission.Exchange);
                Console.WriteLine("Read: {0}", permission.Read);
                Console.WriteLine("Write: {0}", permission.Write);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }

            Console.WriteLine(result.ToJsonString());
        }
        
        [Test]
        public void Verify_can_filter_topic_permissions()
        {
            var result = _container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .GetAll()
                .Where(x => x.VirtualHost == "HareDu");
            
            foreach (var permission in result)
            {
                Console.WriteLine("VirtualHost: {0}", permission.VirtualHost);
                Console.WriteLine("Exchange: {0}", permission.Exchange);
                Console.WriteLine("Read: {0}", permission.Read);
                Console.WriteLine("Write: {0}", permission.Write);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test]
        public async Task Verify_can_create_user_permissions()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Create(x =>
                {
                    x.User("guest");
                    x.VirtualHost("HareDu");
                    x.Configure(c =>
                    {
                        c.OnExchange("E4");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                });

            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_delete_user_permissions()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Delete(x =>
                {
                    x.User("guest");
                    x.VirtualHost("HareDu7");
                });
            
            Console.WriteLine(result.ToJsonString());
        }
    }
}