namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class GlobalParameterTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Verify_can_create_parameter()
        {
            Result<GlobalParameterInfo> result = await Client
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
        }
        
        [Test, Explicit]
        public async Task Test()
        {
            Result<GlobalParameterInfo> result = await Client
                .Factory<GlobalParameter>()
                .Delete(x => x.Parameter("Fred"));
        }
    }
}