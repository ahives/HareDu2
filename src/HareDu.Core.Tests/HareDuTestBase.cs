﻿namespace HareDu.Tests
{
    using Core;
    using Core.Testing;
    using NUnit.Framework;

    [TestFixture]
    public class HareDuTestBase :
        IHareDuTestHarness
    {
        readonly HareDuTestHarness _harness = new Harness();

        public HareDuFactory Client => _harness.Client;

        
        class Harness :
            HareDuTestHarness
        {
            protected override HareDuFactory InitializeClient()
            {
                return HareDuClient.Initialize(x =>
                {
                    x.ConnectTo("http://localhost:15672");
                    x.UsingCredentials("guest", "guest");
                });
            }
        }
    }
}