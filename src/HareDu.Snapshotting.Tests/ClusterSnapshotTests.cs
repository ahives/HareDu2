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
namespace HareDu.Snapshotting.Tests
{
    using System;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class ClusterSnapshotTests :
        SnapshotTestBase
    {
        [Test]
        public async Task Test()
        {
            var resource = Client.Resource<RmqCluster>();
            
//            resource.RegisterObserver(new DefaultClusterSnapshotConsoleLogger());

            resource.TakeSnapshot();

            var snapshot = resource.Snapshots[0].Select(x => x.Data);
            
            Console.WriteLine($"Cluster: {snapshot.ClusterName}");

//            var snapshot = Client
//                .Snapshot<RmqCluster>()
//                .RegisterObserver(new DefaultClusterSnapshotConsoleLogger())
//                .Take()
//                .Select(x => x.Data);
        }
    }
}