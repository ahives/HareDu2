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
                x.ConnectTo("http://ahives-z620:15672");
                x.EnableLogging(s => s.Logger("HareDuLogger"));
                x.UsingCredentials("guest", "guest");
                x.EnableTransientRetry(s => s.RetryLimit(3));
            });
        }
        
        protected HareDuClient Client { get; private set; }
    }
}