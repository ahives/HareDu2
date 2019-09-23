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
namespace HareDu.Diagnostics.Scanning
{
    using System.Collections.Generic;
    using System.Linq;
    using Analyzers;
    using Core.Extensions;
    using Snapshotting.Model;

    public class BrokerConnectivityDiagnostic :
        IComponentDiagnostic<BrokerConnectivitySnapshot>
    {
        public string Identifier => GetType().GenerateIdentifier();

        readonly IReadOnlyList<IDiagnosticAnalyzer> _channelAnalyzers;
        readonly IReadOnlyList<IDiagnosticAnalyzer> _connectionAnalyzers;
        readonly IReadOnlyList<IDiagnosticAnalyzer> _connectivityAnalyzers;

        public BrokerConnectivityDiagnostic(IReadOnlyList<IDiagnosticAnalyzer> sensors)
        {
            _connectionAnalyzers = sensors.Where(IsConnectionThroughputAnalyzer).ToList();
            _channelAnalyzers = sensors.Where(IsChannelThroughputAnalyzer).ToList();
            _connectivityAnalyzers = sensors.Where(IsConnectivityAnalyzer).ToList();
        }

        public IReadOnlyList<DiagnosticResult> Scan(BrokerConnectivitySnapshot snapshot)
        {
            if (snapshot == null)
                return DiagnosticCache.EmptyDiagnosticResults;
            
            var results = new List<DiagnosticResult>();
            
            results.AddRange(_connectivityAnalyzers.Select(x => x.Execute(snapshot)));
            
            for (int i = 0; i < snapshot.Connections.Count; i++)
            {
                results.AddRange(_connectionAnalyzers.Select(x => x.Execute(snapshot.Connections[i])));

                for (int j = 0; j < snapshot.Connections[i].Channels.Count; j++)
                {
                    results.AddRange(_channelAnalyzers.Select(x => x.Execute(snapshot.Connections[i].Channels[j])));
                }
            }

            return results;
        }

        bool IsChannelThroughputAnalyzer(IDiagnosticAnalyzer analyzer) =>
            !analyzer.IsNull()
            && analyzer.Status == DiagnosticAnalyzerStatus.Online
            && analyzer.ComponentType == ComponentType.Channel
            && analyzer.Category != DiagnosticAnalyzerCategory.Connectivity;

        bool IsConnectionThroughputAnalyzer(IDiagnosticAnalyzer analyzer) =>
            !analyzer.IsNull()
            && analyzer.Status == DiagnosticAnalyzerStatus.Online
            && analyzer.ComponentType == ComponentType.Connection
            && analyzer.Category != DiagnosticAnalyzerCategory.Connectivity;

        bool IsConnectivityAnalyzer(IDiagnosticAnalyzer analyzer) =>
            !analyzer.IsNull()
            && analyzer.Status == DiagnosticAnalyzerStatus.Online
            && (analyzer.ComponentType == ComponentType.Connection || analyzer.ComponentType == ComponentType.Channel)
            && analyzer.Category == DiagnosticAnalyzerCategory.Connectivity;
    }
}