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
            Result result = await Client
                .Factory<GlobalParameter>()
                .CreateAsync(x =>
                {
                    x.Parameter("");
                    x.Configure(c =>
                    {
                        c.WithArguments(arg =>
                        {
                            arg.Set("arg1", "value1");
                        });
                    });
                });
        }
        
        [Test, Explicit]
        public async Task Test()
        {
            Result result = await Client
                .Factory<GlobalParameter>()
                .DeleteAsync(x => x.Parameter("Fred"));
        }
    }
}