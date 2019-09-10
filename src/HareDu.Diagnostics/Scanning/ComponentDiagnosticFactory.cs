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
        readonly IReadOnlyList<IDiagnosticSensor> _sensors;
        readonly IDictionary<string, object> _diagnosticCache = new Dictionary<string, object>();
        readonly IList<IDisposable> _observers;

        public ComponentDiagnosticFactory(IReadOnlyList<IDiagnosticSensor> sensors)
        {
            _sensors = sensors;
            _observers = new List<IDisposable>();
            
            RegisterComponentDiagnostics(sensors);
        }

        public bool TryGet<T>(out IComponentDiagnostic<T> diagnostic)
            where T : Snapshot
        {
            Type type = GetType()
                .Assembly
                .GetTypes()
                .FirstOrDefault(x => typeof(IComponentDiagnostic<T>).IsAssignableFrom(x) && !x.IsInterface);

            if (type == null)
            {
                diagnostic = new DoNothingDiagnostic<T>();
                return false;
            }

            if (_diagnosticCache.ContainsKey(type.FullName))
            {
                string identifier = type.FullName.GenerateIdentifier();
                
                diagnostic = (IComponentDiagnostic<T>) _diagnosticCache[identifier];
                return true;
            }

            try
            {
                diagnostic = Activator.CreateInstance(type, _sensors) as IComponentDiagnostic<T>;

                // TODO: check to see if instance was created successfully
            
                _diagnosticCache.Add(diagnostic.Identifier, diagnostic);
                return true;
            }
            catch { }
            
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

        void RegisterComponentDiagnostics(IReadOnlyList<IDiagnosticSensor> sensors)
        {
            var types = GetType()
                .Assembly
                .GetTypes()
                .Where(x => typeof(IComponentDiagnostic<>).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type, sensors);
                
                _diagnosticCache.Add(type.FullName, instance);
            }
        }
    }
}