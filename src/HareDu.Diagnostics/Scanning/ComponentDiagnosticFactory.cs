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
    using Sensors;
    using Snapshotting;

    /// <summary>
    /// This class should be instantiate once since construction can be expensive.
    /// </summary>
    public class ComponentDiagnosticFactory :
        IComponentDiagnosticFactory
    {
        readonly IDiagnosticsRegistrar _diagnosticsRegistrar;
        readonly IDictionary<string, object> _diagnosticCache;
        readonly IReadOnlyList<IDiagnosticSensor> _sensors;
        readonly IList<IDisposable> _observers;
        readonly IEnumerable<Type> _types;

        public ComponentDiagnosticFactory(IDictionary<string, object> diagnosticCache, IReadOnlyList<Type> types, IReadOnlyList<IDiagnosticSensor> sensors)
        {
            _diagnosticCache = diagnosticCache;
            _types = types;
            _sensors = sensors;
            _observers = new List<IDisposable>();
        }

        public bool TryGet<T>(out IComponentDiagnostic<T> diagnostic)
            where T : Snapshot
        {
            Type type = _types.FirstOrDefault(x => typeof(IComponentDiagnostic<T>).IsAssignableFrom(x) && !x.IsInterface);

            if (type == null)
            {
                diagnostic = new DoNothingDiagnostic<T>();
                return false;
            }

            string identifier = type.FullName.GenerateIdentifier();
            
            if (_diagnosticCache.ContainsKey(identifier))
            {
                diagnostic = (IComponentDiagnostic<T>) _diagnosticCache[identifier];
                return true;
            }
            
            diagnostic = new DoNothingDiagnostic<T>();
            return false;
        }

        public void RegisterObservers(IReadOnlyList<IObserver<DiagnosticContext>> observers)
        {
            for (int i = 0; i < observers.Count; i++)
            {
                if (observers[i] != null)
                {
                    for (int j = 0; j < _sensors.Count; j++)
                    {
                        _observers.Add(_sensors[j].Subscribe(observers[i]));
                    }
                }
            }
        }

        public void RegisterObservers(IReadOnlyList<IObserver<DiagnosticSensorContext>> observers)
        {
            for (int i = 0; i < observers.Count; i++)
            {
                if (observers[i] != null)
                {
                    for (int j = 0; j < _sensors.Count; j++)
                    {
                        _observers.Add(_sensors[j].Subscribe(observers[i]));
                    }
                }
            }
        }

        public void RegisterObserver(IObserver<DiagnosticContext> observer)
        {
            if (observer != null)
            {
                for (int j = 0; j < _sensors.Count; j++)
                {
                    _observers.Add(_sensors[j].Subscribe(observer));
                }
            }
        }

        public void RegisterObserver(IObserver<DiagnosticSensorContext> observer)
        {
            if (observer != null)
            {
                for (int j = 0; j < _sensors.Count; j++)
                {
                    _observers.Add(_sensors[j].Subscribe(observer));
                }
            }
        }
    }
}