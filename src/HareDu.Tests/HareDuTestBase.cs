namespace HareDu.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class HareDuTestBase
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Client = HareDuFactory.New(x =>
            {
                x.ConnectTo("http://localhost:15672");
                x.EnableLogging(s => s.Logger(""));
            });
        }
        
        protected HareDuClient Client { get; set; }
    }
}