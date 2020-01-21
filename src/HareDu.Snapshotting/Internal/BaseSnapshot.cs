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
namespace HareDu.Snapshotting.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;

    public abstract class BaseSnapshot<T> :
        IObservable<SnapshotResult<T>>
        where T : Snapshot
    {
        protected readonly IBrokerObjectFactory _factory;
        protected readonly Lazy<SnapshotTimeline<T>> _timeline;
        protected readonly IDictionary<string, SnapshotResult<T>> _snapshots;
        
        readonly List<IObserver<SnapshotResult<T>>> _observers;

        protected BaseSnapshot(IBrokerObjectFactory factory)
        {
            _factory = factory;
            _observers = new List<IObserver<SnapshotResult<T>>>();
            _snapshots = new Dictionary<string, SnapshotResult<T>>();
            _timeline = new Lazy<SnapshotTimeline<T>>(() => new SnapshotTimelineImpl<T>(_snapshots));
        }

        public IDisposable Subscribe(IObserver<SnapshotResult<T>> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new UnsubscribeObserver(_observers, observer);
        }

        protected virtual void NotifyObservers(SnapshotResult<T> result)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(result);
            }
        }

        protected virtual void NotifyObserversOfError(HareDuSnapshotException e)
        {
            foreach (var observer in _observers)
            {
                observer.OnError(e);
            }
        }

        protected virtual void SaveSnapshot(string identifier, SnapshotResult<T> result)
        {
            if (result.IsNull())
                return;
            
            _snapshots.Add(identifier, result);
        }

        
        class SnapshotTimelineImpl<T> :
            SnapshotTimeline<T>
            where T : Snapshot
        {
            readonly IDictionary<string, SnapshotResult<T>> _snapshots;
            
            public IReadOnlyList<SnapshotResult<T>> Results => _snapshots.Values.ToList();

            public SnapshotTimelineImpl(IDictionary<string,SnapshotResult<T>> snapshots)
            {
                _snapshots = snapshots;
            }

            public void PurgeAll()
            {
                _snapshots.Clear();
            }

            public void Purge<U>(SnapshotResult<U> result)
                where U : Snapshot
            {
                _snapshots.Remove(result.Identifier);
            }
        }


        class UnsubscribeObserver :
            IDisposable
        {
            readonly List<IObserver<SnapshotResult<T>>> _observers;
            readonly IObserver<SnapshotResult<T>> _observer;

            public UnsubscribeObserver(List<IObserver<SnapshotResult<T>>> observers, IObserver<SnapshotResult<T>> observer)
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


        protected class SnapshotResultImpl :
            SnapshotResult<T>
        {
            public SnapshotResultImpl(string identifier, T snapshot)
            {
                Identifier = identifier;
                Snapshot = snapshot;
                Timestamp = DateTimeOffset.Now;
            }

            public string Identifier { get; }
            public T Snapshot { get; }
            public DateTimeOffset Timestamp { get; }
        }
    }
}