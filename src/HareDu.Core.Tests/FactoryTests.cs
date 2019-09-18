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
    using Core;
    using NUnit.Framework;

    [TestFixture]
    public class FactoryTests :
        ResourceTestBase
    {
        [Test]
        public async Task QueueResourceTest()
        {
            var impl = Client.Resource<Queue>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Queue).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task ExchangeResourceTest()
        {
            var impl = Client.Resource<Exchange>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Exchange).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task NodeResourceTest()
        {
            var impl = Client.Resource<Node>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Node).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task UserResourceTest()
        {
            var impl = Client.Resource<User>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(User).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task GlobalParameterResourceTest()
        {
            var impl = Client.Resource<GlobalParameter>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(GlobalParameter).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task UserPermissionsResourceTest()
        {
            var impl = Client.Resource<UserPermissions>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(UserPermissions).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task PolicyResourceTest()
        {
            var impl = Client.Resource<Policy>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Policy).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task BindingResourceTest()
        {
            var impl = Client.Resource<Binding>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Binding).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task ScopedParameterResourceTest()
        {
            var impl = Client.Resource<ScopedParameter>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(ScopedParameter).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task VirtualHostResourceTest()
        {
            var impl = Client.Resource<VirtualHost>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(VirtualHost).IsInstanceOfType(impl));
        }
    }
}