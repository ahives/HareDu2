namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class FactoryTests :
        HareDuTestBase
    {
        [Test]
        public async Task QueueResourceTest()
        {
            var impl = Client.Factory<Queue>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Queue).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task ExchangeResourceTest()
        {
            var impl = Client.Factory<Exchange>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Exchange).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task NodeResourceTest()
        {
            var impl = Client.Factory<Node>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Node).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task UserResourceTest()
        {
            var impl = Client.Factory<User>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(User).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task GlobalParameterResourceTest()
        {
            var impl = Client.Factory<GlobalParameter>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(GlobalParameter).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task UserPermissionsResourceTest()
        {
            var impl = Client.Factory<UserPermissions>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(UserPermissions).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task PolicyResourceTest()
        {
            var impl = Client.Factory<Policy>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Policy).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task BindingResourceTest()
        {
            var impl = Client.Factory<Binding>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(Binding).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task ScopedParameterResourceTest()
        {
            var impl = Client.Factory<ScopedParameter>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(ScopedParameter).IsInstanceOfType(impl));
        }
        
        [Test]
        public async Task VirtualHostResourceTest()
        {
            var impl = Client.Factory<VirtualHost>();

            Assert.IsNotNull(impl);
            Assert.IsTrue(typeof(VirtualHost).IsInstanceOfType(impl));
        }
    }
}