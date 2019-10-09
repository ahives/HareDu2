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

    public class BrokerQueuesDiagnostic :
        IComponentDiagnostic<BrokerQueuesSnapshot>
    {
        public string Identifier => GetType().GetIdentifier();

        readonly IReadOnlyList<IDiagnosticAnalyzer> _queueAnalyzers;
        readonly List<IDiagnosticAnalyzer> _exchangeAnalyzers;

        public BrokerQueuesDiagnostic(IReadOnlyList<IDiagnosticAnalyzer> sensors)
        {
            _queueAnalyzers = sensors.Where(IsQueueAnalyzer).ToList();
            _exchangeAnalyzers = sensors.Where(IsExchangeAnalyzer).ToList();
        }

        public IReadOnlyList<DiagnosticResult> Scan(BrokerQueuesSnapshot snapshot)
        {
            if (snapshot == null)
                return DiagnosticCache.EmptyDiagnosticResults;
            
            var results = new List<DiagnosticResult>();

            results.AddRange(_exchangeAnalyzers.Select(x => x.Execute(snapshot)));
            
            for (int i = 0; i < snapshot.Queues.Count; i++)
            {
                if (!snapshot.Queues[i].IsNull())
                    results.AddRange(_queueAnalyzers.Select(x => x.Execute(snapshot.Queues[i])));
            }

            return results;
        }

        bool IsExchangeAnalyzer(IDiagnosticAnalyzer analyzer) =>
            !analyzer.IsNull()
            && analyzer.Status == DiagnosticAnalyzerStatus.Online
            && analyzer.ComponentType == ComponentType.Exchange;

        bool IsQueueAnalyzer(IDiagnosticAnalyzer analyzer) =>
            !analyzer.IsNull()
            && analyzer.Status == DiagnosticAnalyzerStatus.Online
            && analyzer.ComponentType == ComponentType.Queue;
    }
}