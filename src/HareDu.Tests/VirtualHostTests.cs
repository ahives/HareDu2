namespace HareDu.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class VirtualHostTests :
        HareDuTestBase
    {
        [Test]
        public async Task Test()
        {
            IEnumerable<VirtualHost> virtualHosts = await Client
                .Factory<VirtualHostResource>(x => x.Credentials("guest", "guest"))
                .GetAll();
        }
    }
}