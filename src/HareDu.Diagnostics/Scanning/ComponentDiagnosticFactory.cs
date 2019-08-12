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
    using Core.Exceptions;
    using Sensors;

    /// <summary>
    /// This class should be instantiate once since construction can be expensive.
    /// </summary>
    public class ComponentDiagnosticFactory :
        IComponentDiagnosticFactory
    {
        readonly IReadOnlyList<IDiagnosticSensor> _sensors;
        readonly IDictionary<string, object> _sensorCache = new Dictionary<string, object>();

        public ComponentDiagnosticFactory(IReadOnlyList<IDiagnosticSensor> sensors)
        {
            _sensors = sensors;
            
            RegisterDiagnosticSensors(sensors);
        }

        public bool TryGet<T>(out IComponentDiagnostic<T> diagnostic)
        {
            Type type = GetType()
                .Assembly
                .GetTypes()
                .FirstOrDefault(x => typeof(IComponentDiagnostic<T>).IsAssignableFrom(x) && !x.IsInterface);

            if (type == null)
                throw new HareDuResourceInitException($"Failed to find implementation class for interface {typeof(T)}");

            if (_sensorCache.ContainsKey(type.FullName))
            {
                diagnostic = (IComponentDiagnostic<T>) _sensorCache[type.FullName];
                return true;
            }

            try
            {
                diagnostic = Activator.CreateInstance(type, _sensors) as IComponentDiagnostic<T>;

                // TODO: check to see if instance was created successfully
            
                _sensorCache.Add(type.FullName, diagnostic);
                return true;
            }
            catch (Exception e)
            {
            }
            
            diagnostic = new DoNothingDiagnostic<T>();
            return false;
        }

        void RegisterDiagnosticSensors(IReadOnlyList<IDiagnosticSensor> sensors)
        {
            var types = GetType()
                .Assembly
                .GetTypes()
                .Where(x => typeof(IComponentDiagnostic<>).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type, sensors);
                
                _sensorCache.Add(type.FullName, instance);
            }
        }
    }
}