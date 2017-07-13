namespace HareDu.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class HareDuTestBase
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Client = HareDuFactory.Create(x =>
            {
                x.ConnectTo("http://localhost:15672");
                x.EnableLogging(s => s.Logger(""));
                x.UsingCredentials("guest", "guest");
            });
        }
        
        protected HareDuClient Client { get; private set; }
    }
}