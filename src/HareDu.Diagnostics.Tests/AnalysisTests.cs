using NUnit.Framework;

namespace HareDu.Diagnostics.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autofac;
    using AutofacIntegration;
    using Diagnostics.Analyzers;
    using Diagnostics.Configuration;
    using Fakes;
    using KnowledgeBase;
    using Scanning;
    using Snapshotting.Model;

    [TestFixture]
    public class AnalysisTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<HareDuDiagnosticsModule>();
            
            _container = builder.Build();
        }
        
        [Test]
        public void Test()
        {
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot3();
            
            var report = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot);

            string identifier = typeof(ChannelThrottlingAnalyzer).GenerateIdentifier();

            var dictionary = new Dictionary<string, List<DiagnosticStatus>>();
            var filteredResults = report.Results.Where(x => x.AnalyzerIdentifier == identifier).ToList();
            
            foreach (var result in filteredResults)
            {
                if (dictionary.ContainsKey(result.ParentComponentIdentifier))
                    dictionary[result.ParentComponentIdentifier].Add(result.Status);
                else
                    dictionary.Add(result.ParentComponentIdentifier, new List<DiagnosticStatus> {result.Status});
            }

            var summaries = (from item in dictionary
                    let green =
                        new SummaryItemImpl(Convert.ToUInt32(item.Value.Count(x => x == DiagnosticStatus.Green)),
                            CalcPercentage(item.Value, DiagnosticStatus.Green))
                    let red =
                        new SummaryItemImpl(Convert.ToUInt32(item.Value.Count(x => x == DiagnosticStatus.Red)),
                            CalcPercentage(item.Value, DiagnosticStatus.Red))
                    let yellow =
                        new SummaryItemImpl(Convert.ToUInt32(item.Value.Count(x => x == DiagnosticStatus.Yellow)),
                            CalcPercentage(item.Value, DiagnosticStatus.Yellow))
                    let inconclusive =
                        new SummaryItemImpl(Convert.ToUInt32(item.Value.Count(x => x == DiagnosticStatus.Inconclusive)),
                            CalcPercentage(item.Value, DiagnosticStatus.Inconclusive))
                    select new SummaryImpl(item.Key, green, red, yellow, inconclusive))
                .Cast<Summary>()
                .ToList();

            for (int i = 0; i < summaries.Count; i++)
            {
                Console.WriteLine(summaries[i].Identifier);
                Console.WriteLine($"\t{summaries[i].Green.Percentage}% green");
                Console.WriteLine($"\t{summaries[i].Red.Percentage}% red");
                Console.WriteLine($"\t{summaries[i].Yellow.Percentage}% yellow");
                Console.WriteLine($"\t{summaries[i].Inconclusive.Percentage}% inconclusive");
            }
        }

        decimal CalcPercentage(List<DiagnosticStatus> results, DiagnosticStatus status)
        {
            decimal totalCount = Convert.ToDecimal(results.Count);
            decimal statusCount = Convert.ToDecimal(results.Count(x => x == status));

            return Convert.ToDecimal(statusCount / totalCount * 100);
        }
    }

    public interface Summary
    {
        string Identifier { get; }
        
        SummaryItem Green { get; }
        
        SummaryItem Red { get; }
        
        SummaryItem Yellow { get; }
        
        SummaryItem Inconclusive { get; }
    }

    class SummaryImpl :
        Summary
    {
        public SummaryImpl(string identifier, SummaryItem green, SummaryItem red, SummaryItem yellow, SummaryItem inconclusive)
        {
            Identifier = identifier;
            Green = green;
            Red = red;
            Yellow = yellow;
            Inconclusive = inconclusive;
        }

        public string Identifier { get; }
        public SummaryItem Green { get; }
        public SummaryItem Red { get; }
        public SummaryItem Yellow { get; }
        public SummaryItem Inconclusive { get; }
    }

    public interface SummaryItem
    {
        uint Total { get; }

        decimal Percentage { get; }
    }

    class SummaryItemImpl : SummaryItem
    {
        public SummaryItemImpl(uint total, decimal percentage)
        {
            Total = total;
            Percentage = percentage;
        }

        public uint Total { get; }
        public decimal Percentage { get; }
    }

    public class FakeBrokerConnectivitySnapshot3 : BrokerConnectivitySnapshot
    {
        public FakeBrokerConnectivitySnapshot3()
        {
            Connections = GetConnections().ToList();
            ChannelsClosed = new ChurnMetricsImpl(79, 5.5M);
            ChannelsCreated = new ChurnMetricsImpl(79, 5.5M);
            ConnectionsClosed = new ChurnMetricsImpl(79, 5.5M);
            ConnectionsCreated = new ChurnMetricsImpl(79, 5.5M);
        }

        IEnumerable<ConnectionSnapshot> GetConnections()
        {
            yield return new ConnectionSnapshotImpl("Connection (1)");
            yield return new ConnectionSnapshotImpl("Connection (2)");
            yield return new ConnectionSnapshotImpl("Connection (3)");
        }

        public ChurnMetrics ChannelsClosed { get; }
        public ChurnMetrics ChannelsCreated { get; }
        public ChurnMetrics ConnectionsClosed { get; }
        public ChurnMetrics ConnectionsCreated { get; }
        public IReadOnlyList<ConnectionSnapshot> Connections { get; }

        
        class ChurnMetricsImpl :
            ChurnMetrics
        {
            public ChurnMetricsImpl(ulong total, decimal rate)
            {
                Total = total;
                Rate = rate;
            }

            public ulong Total { get; }
            public decimal Rate { get; }
        }

        
        class ConnectionSnapshotImpl :
            ConnectionSnapshot
        {
            public ConnectionSnapshotImpl(string identifier)
            {
                Identifier = identifier;
                Channels = GetChannels(identifier).ToList();
            }

            IEnumerable<ChannelSnapshot> GetChannels(string identifier)
            {
                yield return new ChannelSnapshotImpl(32, 50, "Channel (1)", identifier);
                yield return new ChannelSnapshotImpl(3, 5, "Channel (2)", identifier);
                yield return new ChannelSnapshotImpl(23, 3, "Channel (3)", identifier);
                yield return new ChannelSnapshotImpl(6, 84, "Channel (4)", identifier);
                yield return new ChannelSnapshotImpl(9, 9, "Channel (5)", identifier);
                yield return new ChannelSnapshotImpl(9, 72, "Channel (6)", identifier);
                yield return new ChannelSnapshotImpl(23, 73, "Channel (7)", identifier);
                yield return new ChannelSnapshotImpl(42, 50, "Channel (8)", identifier);
                yield return new ChannelSnapshotImpl(21, 43, "Channel (9)", identifier);
                yield return new ChannelSnapshotImpl(32, 8, "Channel (10)", identifier);
            }

            public string Identifier { get; }
            public NetworkTrafficSnapshot NetworkTraffic { get; }
            public ulong ChannelLimit { get; }
            public string NodeIdentifier { get; }
            public string VirtualHost { get; }
            public ConnectionState State { get; }
            public IReadOnlyList<ChannelSnapshot> Channels { get; }


            class ChannelSnapshotImpl :
                ChannelSnapshot
            {
                public ChannelSnapshotImpl(uint prefetchCount, ulong unacknowledgedMessages, string identifier, string connectionIdentifier)
                {
                    PrefetchCount = prefetchCount;
                    UnacknowledgedMessages = unacknowledgedMessages;
                    Identifier = identifier;
                    ConnectionIdentifier = connectionIdentifier;
                }

                public uint PrefetchCount { get; }
                public ulong UncommittedAcknowledgements { get; }
                public ulong UncommittedMessages { get; }
                public ulong UnconfirmedMessages { get; }
                public ulong UnacknowledgedMessages { get; }
                public ulong Consumers { get; }
                public string Identifier { get; }
                public string ConnectionIdentifier { get; }
                public string Node { get; }
            }
        }
    }
}