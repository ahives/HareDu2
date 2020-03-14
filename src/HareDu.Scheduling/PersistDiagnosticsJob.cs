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
    using Snapshotting.Registration;

    public class PersistDiagnosticsJob<T> :
        IJob
        where T : Snapshot
    {
        readonly ISnapshotFactory _factory;
        readonly IScanner _scanner;
        readonly IDiagnosticWriter _writer;
        readonly SnapshotLens<T> _lens;

        public PersistDiagnosticsJob(ISnapshotFactory factory, IScanner scanner, IDiagnosticWriter writer)
        {
            _factory = factory;
            _scanner = scanner;
            _writer = writer;
            _lens = _factory.Lens<T>();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _lens.TakeSnapshot(out var result);
            var scan = _scanner.Scan(result.Snapshot);

            _writer.TrySave(scan, $"diagnostics_{scan.Id}.json", context.JobDetail.JobDataMap["path"].ToString());
        }
    }
}