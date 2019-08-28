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
namespace HareDu.Snapshotting.Observers
{
    using System;
    using Model;

    public class DefaultClusterSnapshotConsoleLogger :
        IObserver<SnapshotContext<ClusterSnapshot>>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(SnapshotContext<ClusterSnapshot> value)
        {
            Console.WriteLine($"Cluster Name: {value.Snapshot.ClusterName}");

            for (int i = 0; i < value.Snapshot.Nodes.Count; i++)
            {
                Console.WriteLine($"Node: {value.Snapshot.Nodes[i].Name}");
                Console.WriteLine($"Disk Capacity: {value.Snapshot.Nodes[i].IO.Disk.Capacity.Available}");
                Console.WriteLine($"Disk Reads: {value.Snapshot.Nodes[i].IO.Reads.Total}");
                Console.WriteLine($"Bytes Read: {value.Snapshot.Nodes[i].IO.Reads.Bytes.Total}");
                Console.WriteLine($"Disk Read Rate: {value.Snapshot.Nodes[i].IO.Reads.Rate}");
            }
        }
    }
}