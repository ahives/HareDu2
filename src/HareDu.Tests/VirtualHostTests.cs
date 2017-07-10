namespace HareDu.Tests
{
    using System;
    using System.Net;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using Internal.Serialization;
    using Model;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    public class VirtualHostTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Verify_GetAll_works()
        {
            Result<IEnumerable<VirtualHost>> result = await Client
                .Factory<VirtualHostResource>(x => x.Credentials("guest", "guest"))
                .GetAll();

            foreach (var vhost in result.Data)
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public async Task Verify_Get_works()
        {
            Result<VirtualHost> result = await Client
                .Factory<VirtualHostResource>(x => x.Credentials("guest", "guest"))
                .Get("HareDu");

            Console.WriteLine("Name: {0}", result.Data.Name);
            Console.WriteLine("Tracing: {0}", result.Data.Tracing);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }

        [Test, Explicit]
        public async Task Verify_GetDefinition_works()
        {
            Result<VirtualHostDefinition> result = await Client
                .Factory<VirtualHostResource>(x => x.Credentials("guest", "guest"))
                .GetDefinition("HareDu");

            foreach (var exchange in result.Data.Exchanges)
            {
                Console.WriteLine("Name: {0}", exchange.Name);
                Console.WriteLine("Type: {0}", exchange.Type);
                Console.WriteLine("Durable: {0}", exchange.Durable);
                Console.WriteLine("AutoDelete: {0}", exchange.AutoDelete);
                Console.WriteLine("Internal: {0}", exchange.Internal);

                foreach (var argument in exchange.Arguments)
                {
                    Console.WriteLine("{0} : {1}", argument.Key, argument.Value);
                }
//                Console.WriteLine("Arguments: {0}", exchange.Arguments);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public async Task Verify_Create_works()
        {
            Result result = await Client
                .Factory<VirtualHostResource>(x => x.Credentials("guest", "guest"))
                .Create("HareDu3");

            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }

        [Test, Explicit]
        public async Task Verify_Delete_works()
        {
            Result result = await Client
                .Factory<VirtualHostResource>(x => x.Credentials("guest", "guest"))
                .Delete("HareDu2");

            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }

        [Test, Explicit]
        public async Task Verify_Exchange_works()
        {
            var args = new Dictionary<string, object> {{"arg1", 5}, {"arg2", true}, {"arg3", "Something"}};

            Result result = await Client
                .Factory<VirtualHostResource>(x => x.Credentials("guest", "guest"))
                .Factory<ExchangeResource>()
                .Create("E6", "HareDu", x =>
                {
                    x.IsDurable();
                    x.IsForInternalUse();
//                    x.UsingRoutingType("fanout");
//                    x.UsingRoutingType(r => r.Fanout());
                    x.WithArguments(args);
                });

            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }

        [Test]
        public void Test()
        {
            IDictionary<string, object> args = new Dictionary<string, object>
            {
                {"key1", 12},
                {"key2", true},
                {"key3", "value3"},
                {"key4", "value4"},
                {"key5", "value5"}
            };

//            string serialized = "{'key1':12,'key2':true,'key3':'value3','key4':'value4','key5':'value5'}";
//            Dictionary<string, object> args2 =
//                SerializerCache.Deserializer.Deserialize<Dictionary<string, object>>(new JsonTextReader(new StringReader(serialized)));
//
//            Assert.AreEqual(args, args2);
            using (StreamWriter sw = new StreamWriter("/users/albert/documents/git/test.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                SerializerCache.Serializer.Serialize(writer, args);
            }

        }
    }
}