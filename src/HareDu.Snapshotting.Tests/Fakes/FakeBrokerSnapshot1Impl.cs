namespace HareDu.Snapshotting.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class FakeBrokerSnapshot1Impl :
        SnapshotLens<FakeHareDuSnapshot1>
    {
        public SnapshotHistory<FakeHareDuSnapshot1> History { get; }
        public Task<SnapshotResult<FakeHareDuSnapshot1>> TakeSnapshot(CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public SnapshotLens<FakeHareDuSnapshot1> RegisterObserver(IObserver<SnapshotContext<FakeHareDuSnapshot1>> observer) => throw new NotImplementedException();

        public SnapshotLens<FakeHareDuSnapshot1> RegisterObservers(IReadOnlyList<IObserver<SnapshotContext<FakeHareDuSnapshot1>>> observers) => throw new NotImplementedException();
    }
}