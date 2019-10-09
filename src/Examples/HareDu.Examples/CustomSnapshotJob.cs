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
    using Quartz;
    using Snapshotting;
    using Snapshotting.Model;

    public class CustomSnapshotJob :
        IJob
    {
        readonly ResourceSnapshot<BrokerQueuesSnapshot> _resource;

        public CustomSnapshotJob(ISnapshotFactory factory)
        {
            _resource = factory.Snapshot<BrokerQueues>();//.RegisterObserver(new BrokerQueuesJsonExporter());
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Trigger Fired");
            _resource.Execute();

            var snapshot = _resource.Snapshots.MostRecent();
//            Console.WriteLine(_resource.Context.Snapshot.ToJsonString());
            File.WriteAllText($"/Users/albert/Documents/snapshots/snapshot_{snapshot.Identifier}.json", snapshot.ToJsonString());
        }
    }
}