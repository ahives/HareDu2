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
namespace HareDu.IntegrationTesting.Diagnostics
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using HareDu.Diagnostics;
    using HareDu.Diagnostics.Scanning;
    using NUnit.Framework;
    using Snapshotting;

    [TestFixture]
    public class DiagnosticScannerTests :
        DiagnosticsTestBase
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.Register(x => Client)
                .As<ISnapshotFactory>();
            
            builder.RegisterModule<HareDuDiagnosticsModule>();

            _container = builder.Build();
        }

        [Test]
        public async Task Test()
        {
            var snapshot = Client
                .Snapshot<BrokerConnection>()
                .Take();

            var scanner = _container.Resolve<IDiagnosticScanner>();

            var report = scanner.Scan(snapshot.Select(x => x.Data));
            
            for (int i = 0; i < report.Results.Count; i++)
            {
                Console.WriteLine("Diagnostic => Channel: {0}, Status: {1}", report.Results[i].ComponentIdentifier, report.Results[i].Status);
                
                if (report.Results[i].Status == DiagnosticStatus.Red)
                {
                    Console.WriteLine(report.Results[i].KnowledgeBaseArticle.Reason);
                    Console.WriteLine(report.Results[i].KnowledgeBaseArticle.Remediation);
                }
            }
        }
    }
}