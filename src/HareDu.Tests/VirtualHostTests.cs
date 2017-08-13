namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Internal.Serialization;
    using Model;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    public class VirtualHostTests :
        HareDuTestBase
    {
        [Test]
        public async Task Verify_IsHealthy_working()
        {
            Result<VirtualHostHealthCheck> result = await Client
                .Factory<VirtualHost>()
                .IsHealthy("HareDu");

            Console.WriteLine("Status: {0}", result.Data.Status);
            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }
        
        [Test, Explicit]
        public async Task Verify_GetAll_works()
        {
            Result<IEnumerable<VirtualHostInfo>> result = await Client
                .Factory<VirtualHost>()
                .GetAll();

            Assert.IsTrue(result.HasValue());

            foreach (var vhost in result.Data)
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public async Task Verify_GetDefinition_works()
        {
            Result<VirtualHostDefinition> result = await Client
                .Factory<VirtualHost>()
                .GetDefinition("HareDu");

            foreach (var exchange in result.Data.ExchangeDefinitions)
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
                .Factory<VirtualHost>()
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
                .Factory<VirtualHost>()
                .Delete("HareDu2");

            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }

        public interface ParamTest
        {
            [JsonProperty("name")]
            string Name { get; }
            
            [JsonProperty("value")]
            IDictionary<string, object> Value { get; }
        }
        
        [Test]
        public void Test()
        {
//            IDictionary<string, object> args = new Dictionary<string, object>
//            {
//                {"key1", 12},
//                {"key2", true},
//                {"key3", "value3"},
//                {"key4", "value4"},
//                {"key5", "value5"}
//            };

            
//            string serialized = "{'key1':12,'key2':true,'key3':'value3','key4':'value4','key5':'value5'}";
//            Dictionary<string, object> args2 =
//                SerializerCache.Deserializer.Deserialize<Dictionary<string, object>>(new JsonTextReader(new StringReader(serialized)));
//
//            Assert.AreEqual(args, args2);

            IDictionary<string, object> args = new Dictionary<string, object>
            {
                {"guest", "/"},
                {"rabbit", "warren"}
            };

            ParamTest obj = new ParamTestImpl("user_vhost_mapping", args);
            using (StreamWriter sw = new StreamWriter("/users/albert/documents/git/test2.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                SerializerCache.Serializer.Serialize(writer, obj);
            }

        }

        class ParamTestImpl :
            ParamTest
        {
            public ParamTestImpl(string name, IDictionary<string, object> value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; }
            public IDictionary<string, object> Value { get; }
        }
    }
}