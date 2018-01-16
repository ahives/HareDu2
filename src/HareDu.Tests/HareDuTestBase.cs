﻿namespace HareDu.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class HareDuTestBase
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Client = HareDuClient.Initialize(x =>
            {
                x.ConnectTo("http://localhost:15672");
                x.UsingCredentials("guest", "guest");
            });
        }
        
        protected HareDuFactory Client { get; private set; }
    }
}