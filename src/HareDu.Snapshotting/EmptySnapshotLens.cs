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
namespace HareDu.Snapshotting
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class EmptySnapshotLens<T> :
        SnapshotLens<T>
        where T : Snapshot
    {
        public SnapshotHistory<T> History => new EmptySnapshotHistory<T>();

        public SnapshotResult<T> TakeSnapshot(CancellationToken cancellationToken = default) => new EmptySnapshotResult<T>();

        public SnapshotLens<T> RegisterObserver(IObserver<SnapshotContext<T>> observer) => this;

        public SnapshotLens<T> RegisterObservers(IReadOnlyList<IObserver<SnapshotContext<T>>> observers) => this;
    }
}