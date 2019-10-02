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
    using Core;
    using Model;

    abstract class BaseSnapshot<T> :
        IObservable<SnapshotContext<T>>
        where T : Snapshot
    {
        protected readonly IBrokerObjectFactory _factory;
        readonly List<IObserver<SnapshotContext<T>>> _observers;

        protected BaseSnapshot(IBrokerObjectFactory factory)
        {
            _factory = factory;
            _observers = new List<IObserver<SnapshotContext<T>>>();
        }

        protected virtual void NotifyObservers(T snapshot)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(new SnapshotContextImpl(snapshot));
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