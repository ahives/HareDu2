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
                x.Logging(s =>
                {
                    s.Enable();
                    s.UseLogger("HareDuLogger");
                });
                x.UsingCredentials("guest", "guest");
                x.TransientRetry(s =>
                {
                    s.Enable();
                    s.RetryLimit(3);
                });
            });
        }
        
        protected HareDuClient Client { get; private set; }
    }
}