namespace HareDu.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class QueueTests :
        HareDuTestBase
    {
        [Test]
        public void Test()
        {
            var result = Client
                .Factory<Queue>()
                .Create("Q1", "HareDu", x =>
                {
                    x.IsDurable();
                });
        }
    }
}