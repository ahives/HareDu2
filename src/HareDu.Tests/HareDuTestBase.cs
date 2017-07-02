namespace HareDu.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class HareDuTestBase
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Client = HareDuFactory.New();
        }
        
        protected HareDuClient Client { get; set; }
    }
}