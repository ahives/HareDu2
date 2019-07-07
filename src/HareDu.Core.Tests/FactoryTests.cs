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