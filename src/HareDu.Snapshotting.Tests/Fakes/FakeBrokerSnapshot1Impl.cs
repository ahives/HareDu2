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

    public class FakeBrokerSnapshot1Impl : FakeBrokerSnapshot1
    {
        public SnapshotTimeline<FakeHareDuSnapshot1> Timeline { get; }
        public HareDuSnapshot<FakeHareDuSnapshot1> Execute(CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public HareDuSnapshot<FakeHareDuSnapshot1> RegisterObserver(IObserver<SnapshotResult<FakeHareDuSnapshot1>> observer) => throw new NotImplementedException();

        public HareDuSnapshot<FakeHareDuSnapshot1> RegisterObservers(IReadOnlyList<IObserver<SnapshotResult<FakeHareDuSnapshot1>>> observers) => throw new NotImplementedException();
    }
}