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
namespace HareDu.Diagnostics.Reporting
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public class ConnectivityDiagnosticsRunner :
        IDiagnosticsRunner<ConnectivitySnapshot>
    {
        readonly IReadOnlyList<IDiagnostic> _diagnosticChecks;

        public ConnectivityDiagnosticsRunner(IReadOnlyList<IDiagnostic> diagnosticChecks)
        {
            _diagnosticChecks = diagnosticChecks;
        }

        public IReadOnlyList<DiagnosticResult> Execute(ConnectivitySnapshot snapshot)
        {
            var diagnosticResults = new List<DiagnosticResult>();
            
            for (int i = 0; i < snapshot.Connections.Count; i++)
            {
                diagnosticResults.AddRange(_diagnosticChecks
                    .Where(x => x.SnapshotType == SnapshotType.Connection)
                    .Select(x => x.Execute(snapshot.Connections[i])));

                for (int j = 0; j < snapshot.Connections[i].Channels.Count; j++)
                {
                    diagnosticResults.AddRange(_diagnosticChecks
                        .Where(x => x.SnapshotType == SnapshotType.Channel)
                        .Select(x => x.Execute(snapshot.Connections[i].Channels[j])));
                }
            }

            return diagnosticResults;
        }
    }
}