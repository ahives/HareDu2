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
    using System.Linq;
    using Autofac;
    using AutofacIntegration;
    using Core.Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class ValueExtensionTests
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
        public void Verify_Where_works()
        {
            var vhosts = _container.Resolve<IRmqObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .Where(x => x.Name == "HareDu");

            foreach (var vhost in vhosts)
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public void Verify_Unwrap_works()
        {
            var vhosts = _container.Resolve<IRmqObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .Unfold();

            foreach (var vhost in vhosts.Data)
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public void Verify_Unwravel_works()
        {
            var vhosts = _container.Resolve<IRmqObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .Select(x => x.Data);

            foreach (var vhost in vhosts)
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public void Verify_Any_works()
        {
            bool found = _container.Resolve<IRmqObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .Any();
            
            Assert.IsTrue(found);
        }

        [Test, Explicit]
        public void Verify_Any_with_predicate_works()
        {
            bool found = _container.Resolve<IRmqObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .Any(x => x.Name == "HareDu");
            
            Assert.IsTrue(found);
        }
        
        [Test, Explicit]
        public void Verify_FirstOrDefault_works()
        {
            ExchangeInfo exchange = _container.Resolve<IRmqObjectFactory>()
                .Object<Exchange>()
                .GetAll()
                .Where(x => x.Name == "E2" && x.VirtualHost == "HareDu")
                .FirstOrDefault();
 
            Console.WriteLine("Name: {0}", exchange.Name);
            Console.WriteLine("AutoDelete: {0}", exchange.AutoDelete);
            Console.WriteLine("Internal: {0}", exchange.Internal);
            Console.WriteLine("Durable: {0}", exchange.Durable);
            Console.WriteLine("RoutingType: {0}", exchange.RoutingType);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }
    }
}