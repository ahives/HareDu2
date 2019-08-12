﻿// Copyright 2013-2019 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Snapshotting.Tests
{
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
//                    x.RegisterObserver(new DefaultConsoleLogger());
                });
            }
        }
    }
}