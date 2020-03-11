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

    public class FakeHareDuSnapshot1Impl :
        SnapshotLens<FakeHareDuSnapshot1>
    {
        readonly Lazy<SnapshotHistory<FakeHareDuSnapshot1>> _timeline;
        readonly List<SnapshotResult<FakeHareDuSnapshot1>> _snapshots;

        public SnapshotHistory<FakeHareDuSnapshot1> History => _timeline.Value;

        public FakeHareDuSnapshot1Impl()
        {
            _snapshots = new List<SnapshotResult<FakeHareDuSnapshot1>>();
            _timeline = new Lazy<SnapshotHistory<FakeHareDuSnapshot1>>(() => new SnapshotHistoryImpl(_snapshots));
        }

        public SnapshotLens<FakeHareDuSnapshot1> TakeSnapshot(CancellationToken cancellationToken = default)
        {
            FakeHareDuSnapshot1 snapshot = new FakeHareDuSnapshotImpl();

            _snapshots.Add(new SnapshotResultImpl(snapshot));

            return this;
        }

        public SnapshotLens<FakeHareDuSnapshot1> TakeSnapshot(out SnapshotResult<FakeHareDuSnapshot1> result,
            CancellationToken cancellationToken = default)
        {
            var snapshot = new SnapshotResultImpl(new FakeHareDuSnapshotImpl());
            
            _snapshots.Add(snapshot);

            result = snapshot;
            return this;
        }
        
        public SnapshotLens<FakeHareDuSnapshot1> RegisterObserver(IObserver<SnapshotContext<FakeHareDuSnapshot1>> observer) => throw new NotImplementedException();

        public SnapshotLens<FakeHareDuSnapshot1> RegisterObservers(IReadOnlyList<IObserver<SnapshotContext<FakeHareDuSnapshot1>>> observers) => throw new NotImplementedException();

        
        class FakeHareDuSnapshotImpl :
            FakeHareDuSnapshot1
        {
        }


        class SnapshotHistoryImpl :
            SnapshotHistory<FakeHareDuSnapshot1>
        {
            public SnapshotHistoryImpl(List<SnapshotResult<FakeHareDuSnapshot1>> snapshots)
            {
                Results = snapshots;
            }

            public IReadOnlyList<SnapshotResult<FakeHareDuSnapshot1>> Results { get; }

            public void PurgeAll()
            {
                throw new NotImplementedException();
            }

            public void Purge<U>(SnapshotResult<U> result) where U : Snapshot
            {
                throw new NotImplementedException();
            }
        }


        class SnapshotResultImpl :
            SnapshotResult<FakeHareDuSnapshot1>
        {
            public SnapshotResultImpl(FakeHareDuSnapshot1 snapshot)
            {
                Identifier = NewId.NextGuid().ToString();
                Snapshot = snapshot;
                Timestamp = DateTimeOffset.Now;
            }

            public string Identifier { get; }
            public FakeHareDuSnapshot1 Snapshot { get; }
            public DateTimeOffset Timestamp { get; }
        }
    }
}