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
namespace HareDu.Snapshotting.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using MassTransit;
    using Model;

    public class FakeCluster :
        Cluster
    {
        readonly Lazy<SnapshotTimeline<ClusterSnapshot>> _timeline;
        readonly List<SnapshotResult<ClusterSnapshot>> _snapshots;
        // public IReadOnlyList<SnapshotResult<ClusterSnapshot>> Timeline => _snapshots;

        public SnapshotTimeline<ClusterSnapshot> Timeline => _timeline.Value;

        public FakeCluster()
        {
            _snapshots = new List<SnapshotResult<ClusterSnapshot>>();
            _timeline = new Lazy<SnapshotTimeline<ClusterSnapshot>>(() => new SnapshotTimelineImpl(_snapshots));
        }

        public HareDuSnapshot<ClusterSnapshot> Execute(CancellationToken cancellationToken = default)
        {
            ClusterSnapshot snapshot = new FakeClusterSnapshot1();

            _snapshots.Add(new SnapshotResultImpl(snapshot));

            return this;
        }

        public HareDuSnapshot<ClusterSnapshot> RegisterObserver(IObserver<SnapshotResult<ClusterSnapshot>> observer) => throw new NotImplementedException();

        public HareDuSnapshot<ClusterSnapshot> RegisterObservers(IReadOnlyList<IObserver<SnapshotResult<ClusterSnapshot>>> observers) => throw new NotImplementedException();

        
        class SnapshotTimelineImpl :
            SnapshotTimeline<ClusterSnapshot>
        {
            public SnapshotTimelineImpl(List<SnapshotResult<ClusterSnapshot>> snapshots)
            {
                Results = snapshots;
            }

            public IReadOnlyList<SnapshotResult<ClusterSnapshot>> Results { get; }
            public void Flush()
            {
                throw new NotImplementedException();
            }
        }


        class SnapshotResultImpl :
            SnapshotResult<ClusterSnapshot>
        {
            public SnapshotResultImpl(ClusterSnapshot snapshot)
            {
                Identifier = NewId.NextGuid().ToString();
                Snapshot = snapshot;
                Timestamp = DateTimeOffset.Now;
            }

            public string Identifier { get; }
            public ClusterSnapshot Snapshot { get; }
            public DateTimeOffset Timestamp { get; }
        }
    }
}