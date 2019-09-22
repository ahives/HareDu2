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
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class ScopedParameterTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<HareDuModule>();

            _container = builder.Build();
        }

        [Test, Explicit]
        public async Task Should_be_able_to_get_all_scoped_parameters()
        {
            var result = await _container.Resolve<IRmqObjectFactory>()
                .Object<ScopedParameter>()
                .GetAll();

            foreach (var parameter in result.Select(x => x.Data))
            {
                Console.WriteLine("Component: {0}", parameter.Component);
                Console.WriteLine("Name: {0}", parameter.Name);
                Console.WriteLine("Value: {0}", parameter.Value);
                Console.WriteLine("VirtualHost: {0}", parameter.VirtualHost);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
        
        [Test, Explicit]
        public async Task Verify_can_create()
        {
            var result = await _container.Resolve<IRmqObjectFactory>()
                .Object<ScopedParameter>()
                .Create(x =>
                {
                    x.Parameter("test", "me");
                    x.Targeting(t =>
                    {
                        t.Component("federation");
                        t.VirtualHost("HareDu");
                    });
                });
            
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }

        [Test, Explicit]
        public async Task Test()
        {
            var result = await _container.Resolve<IRmqObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Parameter("");
                    x.Targeting(t =>
                    {
                        t.Component("federation");
                        t.VirtualHost("HareDu");
                    });
                });
        }
    }
}