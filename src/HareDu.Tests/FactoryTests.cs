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
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using NUnit.Framework;

    [TestFixture]
    public class FactoryTests
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
        public async Task QueueResourceTest()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>().Object<Queue>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Queue).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task ExchangeResourceTest()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>().Object<Exchange>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Exchange).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task NodeResourceTest()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>().Object<Node>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Node).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task UserResourceTest()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>().Object<User>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(User).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task GlobalParameterResourceTest()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>().Object<GlobalParameter>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(GlobalParameter).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task UserPermissionsResourceTest()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>().Object<UserPermissions>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(UserPermissions).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task PolicyResourceTest()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>().Object<Policy>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Policy).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task BindingResourceTest()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>().Object<Binding>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Binding).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task ScopedParameterResourceTest()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>().Object<ScopedParameter>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(ScopedParameter).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task VirtualHostResourceTest()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>().Object<VirtualHost>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(VirtualHost).IsInstanceOfType(impl));
        }
    }
}