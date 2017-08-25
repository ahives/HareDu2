namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class GlobalParameterTests :
        HareDuTestBase
    {
        public async Task Verify_can_create_parameter()
        {
            Result result = await Client
                .Factory<GlobalParameter>()
                .Create(x =>
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
        [Test]
        public async Task Test()
        {
            Result result = await Client
                .Factory<GlobalParameter>()
                .Delete(x => x.Parameter("Fred"));
        }
    }
}