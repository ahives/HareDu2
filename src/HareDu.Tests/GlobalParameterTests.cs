namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class GlobalParameterTests :
        HareDuTestBase
    {
        [Test]
        public async Task Test()
        {
            Result result = await Client
                .Factory<GlobalParameter>()
                .Delete(x => x.Parameter("Fred"));
        }
    }
}