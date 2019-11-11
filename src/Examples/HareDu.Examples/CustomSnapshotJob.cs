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
namespace HareDu.Examples
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Core.Extensions;
    using Diagnostics;
    using Nest;
    using Quartz;
    using Snapshotting;
    using Snapshotting.Model;
    using Snapshot = Snapshotting.Snapshot;

    public class CustomSnapshotJob<T> :
        IJob
        where T : ResourceSnapshot<Snapshot>
    {
        readonly T _resource;
        readonly ElasticClient _client;

        public CustomSnapshotJob(T resource, ElasticClient client)
        {
            _resource = resource;
            _client = client;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _resource.Execute();

            var snapshot = _resource.Snapshots
                .MostRecent()
                .Cast<SnapshotContext<BrokerQueuesSnapshot>>();
            
//            bool persisted = snapshot.PersistJson("/Users/albert/Documents/snapshots");

            var response = _client.Index(snapshot, x => x.Index("haredu_snapshots"));
            if (response.Result == Result.Created)
                Console.WriteLine("Snapshot Recorded");
        }
    }
}