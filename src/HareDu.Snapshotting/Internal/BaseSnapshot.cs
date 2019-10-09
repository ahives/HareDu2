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
namespace HareDu.Snapshotting.Internal
{
    using System;
    using System.Collections.Generic;
    using MassTransit;

    abstract class BaseSnapshot<T> :
        IObservable<SnapshotContext<T>>
        where T : Snapshot
    {
        protected readonly IBrokerObjectFactory _factory;
        protected readonly List<SnapshotContext<T>> _snapshots;
        
        readonly List<IObserver<SnapshotContext<T>>> _observers;

        protected BaseSnapshot(IBrokerObjectFactory factory)
        {
            _factory = factory;
            _observers = new List<IObserver<SnapshotContext<T>>>();
            _snapshots = new List<SnapshotContext<T>>();
        }

        protected virtual void NotifyObservers(T snapshot)
        {
            var context = new SnapshotContextImpl(snapshot);
            
            _snapshots.Add(context);
            
            foreach (var observer in _observers)
            {
                observer.OnNext(context);
            }
        }

        public IDisposable Subscribe(IObserver<SnapshotContext<T>> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new UnsubscribeObserver(_observers, observer);
        }

        
        class SnapshotContextImpl :
            SnapshotContext<T>
        {
            public SnapshotContextImpl(T snapshot)
            {
                Identifier = NewId.NextGuid().ToString();
                Snapshot = snapshot;
                Timestamp = DateTimeOffset.Now;
            }

            public string Identifier { get; }
            public T Snapshot { get; }
            public DateTimeOffset Timestamp { get; }
        }


        class UnsubscribeObserver :
            IDisposable
        {
            readonly List<IObserver<SnapshotContext<T>>> _observers;
            readonly IObserver<SnapshotContext<T>> _observer;

            public UnsubscribeObserver(List<IObserver<SnapshotContext<T>>> observers, IObserver<SnapshotContext<T>> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}