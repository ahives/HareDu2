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
    using Quartz;
    using Snapshotting;
    using Snapshotting.Model;

    public class CustomSnapshotJob<T> :
        IJob
        where T : ResourceSnapshot<Snapshot>
    {
        readonly T _resource;

        public CustomSnapshotJob(T resource)
        {
            _resource = resource;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _resource.Execute();
            Console.WriteLine("Snapshot Taken");

            bool persisted = _resource.Snapshots
                .MostRecent()
                .PersistJson("/Users/albert/Documents/snapshots");
        }
    }
}