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
namespace HareDu.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Core.Exceptions;
    using HareDu.Diagnostics;

    public class SnapshotFactory :
        ISnapshotFactory
    {
        IDictionary<string, object> _snapshotCache;
        IResourceFactory _factory;
        static ISnapshotFactory _instance = null;
        static readonly object _gate = new object();
        readonly List<IObserver<DiagnosticContext>> _observers = new List<IObserver<DiagnosticContext>>();

        public static ISnapshotFactory Instance
        {
            get
            {
                lock (_gate)
                {
                    return _instance ?? (_instance = new SnapshotFactory());
                }
            }
        }

        public void Init(IResourceFactory factory)
        {
            _factory = factory;
            _snapshotCache = new Dictionary<string, object>();
            
            RegisterSnapshots();
        }

        public void Init(IResourceFactory factory, IList<IObserver<DiagnosticContext>> observers)
        {
            _factory = factory;

            for (int i = 0; i < observers.Count; i++)
            {
                if (!_observers.Contains(observers[i]))
                    _observers.Add(observers[i]);
            }

            _snapshotCache = new Dictionary<string, object>();
            
            RegisterSnapshots();
        }

        public T Snapshot<T>()
            where T : Snapshot
        {
            Type type = GetType()
                .Assembly
                .GetTypes()
                .FirstOrDefault(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface);

            if (type == null)
                throw new HareDuResourceInitException($"Failed to find implementation class for interface {typeof(T)}");

            if (_snapshotCache.ContainsKey(type.FullName))
                return (T)_snapshotCache[type.FullName];

            T instance = _observers.Any()
                ? (T) Activator.CreateInstance(type, _factory, _observers)
                : (T) Activator.CreateInstance(type, _factory);

            _snapshotCache.Add(type.FullName, instance);
            
            return instance;
        }

        void RegisterSnapshots()
        {
            var types = GetType()
                .Assembly
                .GetTypes()
                .Where(x => typeof(Snapshot).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type, _factory);
                
                _snapshotCache.Add(type.FullName, instance);
            }
        }
    }
}