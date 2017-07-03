namespace HareDu.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Internal.Json;
    using Model;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    public class VirtualHostTests //:
        //HareDuTestBase
    {
        [Test]
        public async Task Test()
        {
//            Result<IEnumerable<VirtualHost>> virtualHosts = await Client
//                .Factory<VirtualHostResource>(x => x.Credentials("guest", "guest"))
//                .GetAll();

            VirtualHost vhost = new VirtualHostImpl("HareDu", "none");

            using (StreamWriter sw = new StreamWriter("/users/albert/documents/git/test.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                SerializerCache.Serializer.Serialize(writer, vhost);
            }
        }

        class VirtualHostImpl :
            VirtualHost
        {
            public VirtualHostImpl(string name, string tracing)
            {
                Name = name;
                Tracing = tracing;
            }

            public string Name { get; }
            public string Tracing { get; }
        }
    }
}