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
namespace HareDu.Analytics.Tests
{
    using System;
    using Analyzers;
    using Autofac;
    using AutofacIntegration;
    using Diagnostics;
    using Diagnostics.Analyzers;
    using Diagnostics.Scanning;
    using Fakes;
    using NUnit.Framework;
    using Snapshotting.Model;

    [TestFixture]
    public class QueueNoFlowReportAnalyzerTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<HareDuAnalyticsModule>();
            
            _container = builder.Build();
        }
        
        [Test]
        public void Test()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();
            IDiagnosticReportAnalyzerFactory factory = _container.Resolve<IDiagnosticReportAnalyzerFactory>();

            var summary = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot)
                .Analyze(factory, typeof(QueueNoFlowReportAnalyzer).GetIdentifier());
            
            for (int i = 0; i < summary.Count; i++)
            {
                Console.WriteLine(summary[i].Identifier);
                Console.WriteLine($"\t{summary[i].Green.Percentage}% green");
                Console.WriteLine($"\t{summary[i].Red.Percentage}% red");
                Console.WriteLine($"\t{summary[i].Yellow.Percentage}% yellow");
                Console.WriteLine($"\t{summary[i].Inconclusive.Percentage}% inconclusive");
            }
        }
    }
}