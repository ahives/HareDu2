// Copyright 2013-2019 Albert L. Hives
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
    using System.Threading.Tasks;
    using Autofac;
    using NUnit.Framework;
    using Observers;

    [TestFixture]
    public class ConnectionSnapshotTests :
        SnapshotTestBase
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.Register(x => Client)
                .As<ISnapshotFactory>();
            
//            builder.RegisterModule<MassTransitModule>();

            _container = builder.Build();
        }

        [Test]
        public async Task Test()
        {
            var connection = Client
                .Snapshot<BrokerConnection>()
                .RegisterObserver(new DefaultConnectivitySnapshotConsoleLogger())
                .Take();
        }

        [Test]
        public async Task Test2()
        {
            var connection = Client
                .Snapshot<BrokerConnection>()
                .RegisterObserver(new DefaultConnectivitySnapshotConsoleLogger())
                .Take();
            
//            Console.WriteLine(connection.ToJsonString());
        }

        [Test]
        public async Task Test4()
        {
            var connection = Client
                .Snapshot<BrokerConnection>()
                .Take();

//            var resource = Client.Snapshot<RmqConnection>();
//            var snapshot = resource.Get();
//            var data = snapshot.Select(x => x.Data);
//            var diagnosticResults = resource.RunDiagnostics(data).ToList();

//            for (int i = 0; i < diagnosticResults.Count; i++)
//            {
//                Console.WriteLine("Diagnostic => Channel: {0}, Status: {1}", diagnosticResults[i].Identifier, diagnosticResults[i].Status);
//            }
        }

        [Test]
        public async Task Test5()
        {
            var snapshot = Client
                .Snapshot<BrokerConnection>()
                .Take();

//            var snapshot = resource.Get();
//            var data = snapshot.Select(x => x.Data);
//            var diagnosticResults = resource.RunDiagnostics(data).ToList();

//            for (int i = 0; i < diagnosticResults.Count; i++)
//            {
//                Console.WriteLine("Diagnostic => Channel: {0}, Status: {1}", diagnosticResults[i].Identifier, diagnosticResults[i].Status);
//            }
        }
    }
}