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
            var impl = Client.Object<Queue>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Queue).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task ExchangeResourceTest()
        {
            var impl = Client.Object<Exchange>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Exchange).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task NodeResourceTest()
        {
            var impl = Client.Object<Node>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Node).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task UserResourceTest()
        {
            var impl = Client.Object<User>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(User).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task GlobalParameterResourceTest()
        {
            var impl = Client.Object<GlobalParameter>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(GlobalParameter).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task UserPermissionsResourceTest()
        {
            var impl = Client.Object<UserPermissions>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(UserPermissions).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task PolicyResourceTest()
        {
            var impl = Client.Object<Policy>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Policy).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task BindingResourceTest()
        {
            var impl = Client.Object<Binding>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Binding).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task ScopedParameterResourceTest()
        {
            var impl = Client.Object<ScopedParameter>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(ScopedParameter).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task VirtualHostResourceTest()
        {
            var impl = Client.Object<VirtualHost>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(VirtualHost).IsInstanceOfType(impl));
        }
    }
}