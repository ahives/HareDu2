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
namespace HareDu.Scheduling
{
    using System.Threading.Tasks;
    using Quartz;
    using Snapshotting;
    using Snapshotting.Extensions;

    public class PersistSnapshotJob<T> :
        IJob
        where T : HareDuSnapshot<Snapshot>
    {
        readonly ISnapshotFactory _factory;
        readonly ISnapshotWriter _writer;

        public PersistSnapshotJob(ISnapshotFactory factory, ISnapshotWriter writer)
        {
            _factory = factory;
            _writer = writer;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var snapshot = _factory.Snapshot<T>().Execute();
            var result = snapshot.Timeline.MostRecent();

            _writer.TrySave(result, $"snapshot_{result.Identifier}.json", context.JobDetail.JobDataMap["path"].ToString());
        }
    }
}