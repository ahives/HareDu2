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
namespace HareDu.Diagnostics.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Analyzers;
    using Snapshotting;

    /// <summary>
    /// This class should be instantiate once since construction can be expensive.
    /// </summary>
    public class ComponentDiagnosticFactory :
        IComponentDiagnosticFactory
    {
        readonly IDictionary<string, object> _cache;
        readonly IReadOnlyList<IDiagnosticAnalyzer> _analyzers;
        readonly IList<IDisposable> _observers;
        readonly IEnumerable<Type> _types;

        public ComponentDiagnosticFactory(IDictionary<string, object> cache, IReadOnlyList<Type> types, IReadOnlyList<IDiagnosticAnalyzer> analyzers)
        {
            _cache = cache;
            _types = types;
            _analyzers = analyzers;
            _observers = new List<IDisposable>();
        }

        public bool TryGet<T>(out IComponentDiagnostic<T> diagnostic)
            where T : Snapshot
        {
            Type type = _types.FirstOrDefault(x => typeof(IComponentDiagnostic<T>).IsAssignableFrom(x) && !x.IsInterface);

            if (type == null)
            {
                diagnostic = new NoOpDiagnostic<T>();
                return false;
            }

            string identifier = type.GetIdentifier();
            
            if (_cache.ContainsKey(identifier))
            {
                diagnostic = (IComponentDiagnostic<T>) _cache[identifier];
                return true;
            }
            
            diagnostic = new NoOpDiagnostic<T>();
            return false;
        }

        public void RegisterObservers(IReadOnlyList<IObserver<DiagnosticAnalyzerContext>> observers)
        {
            for (int i = 0; i < observers.Count; i++)
            {
                if (observers[i] != null)
                {
                    for (int j = 0; j < _analyzers.Count; j++)
                    {
                        _observers.Add(_analyzers[j].Subscribe(observers[i]));
                    }
                }
            }
        }

        public void RegisterObserver(IObserver<DiagnosticAnalyzerContext> observer)
        {
            if (observer != null)
            {
                for (int j = 0; j < _analyzers.Count; j++)
                {
                    _observers.Add(_analyzers[j].Subscribe(observer));
                }
            }
        }
    }
}