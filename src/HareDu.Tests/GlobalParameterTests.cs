namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class GlobalParameterTests :
        HareDuTestBase
    {
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
            Console.WriteLine(result.ToJson());
        }
        
        [Test, Explicit]
        public async Task Verify_can_delete_parameter()
        {
            Result result = await Client
                .Factory<GlobalParameter>()
                .Delete(x => x.Parameter("Fred"));
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJson());
        }
    }
}