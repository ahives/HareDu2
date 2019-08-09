namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using Diagnostics;
    using NUnit.Framework;

    [TestFixture]
    public class DiagnosticReportGeneratorTests :
        SnapshotTestBase
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.Register(x => Client)
                .As<ISnapshotFactory>();

            builder.RegisterType<DiagnosticReportGenerator>()
                .As<IGenerateDiagnosticReport>();
            
//            builder.RegisterModule<MassTransitModule>();

            _container = builder.Build();
        }

        [Test]
        public async Task Test()
        {
            var connection = Client
                .Snapshot<RmqBrokerConnection>()
                .Execute();

            var generator = _container.Resolve<IGenerateDiagnosticReport>();

            var report = generator.Run<RmqBrokerConnection>();
            
            for (int i = 0; i < report.Results.Count; i++)
            {
                Console.WriteLine("Diagnostic => Channel: {0}, Status: {1}", report.Results[i].Identifier, report.Results[i].Status);
                
                if (report.Results[i].Status == DiagnosticStatus.Red)
                {
                    Console.WriteLine(report.Results[i].Reason);
                    Console.WriteLine(report.Results[i].Remediation);
                }
            }
        }
    }
}