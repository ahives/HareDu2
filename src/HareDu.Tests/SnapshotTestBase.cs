﻿namespace HareDu.Tests
{
    using Diagnostics.Observers;
    using NUnit.Framework;
    using Testing;

    [TestFixture]
    public class SnapshotTestBase :
        ISnapshotTestHarness
    {
        readonly SnapshotTestHarness _harness = new Harness();

        public ISnapshotFactory Client => _harness.Client;

        
        class Harness :
            SnapshotTestHarness
        {
            protected override ISnapshotFactory InitializeClient()
            {
                return SnapshotClient.Init(x =>
                {
                    x.ConnectTo("http://localhost:15672");
                    x.UsingCredentials("guest", "guest");
                    x.RegisterObserver(new DefaultConsoleLogger());
                });
            }
        }
    }
}