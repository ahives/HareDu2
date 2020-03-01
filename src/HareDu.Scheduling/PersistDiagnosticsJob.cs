// Copyright 2013-2020 Albert L. Hives
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
namespace HareDu.Scheduling
{
    using System.Threading.Tasks;
    using Diagnostics;
    using Diagnostics.Persistence;
    using Quartz;
    using Snapshotting;
    using Snapshotting.Extensions;
    using Snapshotting.Registration;

    public class PersistDiagnosticsJob<T> :
        IJob
        where T : SnapshotLens<Snapshot>
    {
        readonly ISnapshotFactory _factory;
        readonly IDiagnosticScanner _scanner;
        readonly IDiagnosticWriter _writer;

        public PersistDiagnosticsJob(ISnapshotFactory factory, IDiagnosticScanner scanner, IDiagnosticWriter writer)
        {
            _factory = factory;
            _scanner = scanner;
            _writer = writer;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var snapshot = _factory.Lens<T>().TakeSnapshot();
            var result = _scanner.Scan(snapshot.History.MostRecent().Snapshot);

            _writer.TrySave(result, $"diagnostics_{result.Identifier}.json", context.JobDetail.JobDataMap["path"].ToString());
        }
    }
}