namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class GlobalParameterTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_global_parameters()
        {
            var result = await Client
                .Factory<GlobalParameter>()
                .GetAll();

            foreach (var parameter in result.Select(x => x.Data))
            {
                Console.WriteLine("Component: {0}", parameter.Component);
                Console.WriteLine("Name: {0}", parameter.Name);
                Console.WriteLine("Value: {0}", parameter.Value);
                Console.WriteLine("VirtualHost: {0}", parameter.VirtualHost);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
        
        [Test, Explicit]
        public async Task Verify_can_create_parameter()
        {
            Result result = await Client
                .Factory<GlobalParameter>()
                .Create(x =>
                {
                    x.Parameter("");
                    x.Configure(c =>
                    {
                        c.HasArguments(arg =>
                        {
                            arg.Set("arg1", "value1");
                        });
                    });
                });
             
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }
        
        [Test, Explicit]
        public async Task Verify_can_delete_parameter()
        {
            Result result = await Client
                .Factory<GlobalParameter>()
                .Delete(x => x.Parameter("Fred"));
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }
    }
}