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
        public async Task Verify_Create_works()
        {
//            Result result = await Client
//                .Factory<VirtualHost>()
//                .Create("HareDu3");
            Result result = await Client
                .Factory<VirtualHost>()
                .Create(x =>
                {
                    x.Configure(c =>
                    {
                        c.Resource("HareDu5");
                        c.EnableTracing();
                    });
                });

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
                .Delete(x => x.Resource("HareDu2"));

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