﻿namespace HareDu.Tests
{
    using Core;
    using Core.Testing;
    using NUnit.Framework;

    [TestFixture]
    public class ResourceTestBase :
        IResourceTestHarness
    {
        readonly ResourceTestHarness _harness = new Harness();

        public IResourceFactory Client => _harness.Client;

        
        class Harness :
            ResourceTestHarness
        {
            protected override IResourceFactory InitializeClient()
            {
                return ResourceClient.Init(x =>
                {
                    x.ConnectTo("http://localhost:15672");
                    x.UsingCredentials("guest", "guest");
                });
            }
        }
    }
}