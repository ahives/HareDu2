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
namespace HareDu.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Scanning;
    using Sensors;

    public class ComponentDiagnosticRegistrar :
        IDiagnosticsRegistrar
    {
        readonly List<Type> _types;
        readonly IDictionary<string, object> _cache;

        public IReadOnlyList<Type> Types => _types;
        public IDictionary<string, object> Cache => _cache;

        public ComponentDiagnosticRegistrar()
        {
            _cache = new Dictionary<string, object>();
            _types = GetTypes();
        }

        public void Register<T>(IReadOnlyList<IDiagnosticSensor> sensors)
        {
            Type type = typeof(T);
            
            _types.Add(type);
            
            Register(type, sensors);
        }

        public void RegisterAll(IReadOnlyList<IDiagnosticSensor> sensors)
        {
            foreach (var type in _types)
            {
                Register(type, sensors);
            }
        }

        List<Type> GetTypes() =>
            GetType()
                .Assembly
                .GetTypes()
                .Where(IsComponentDiagnostic)
                .ToList();

        void Register(Type type, IReadOnlyList<IDiagnosticSensor> sensors)
        {
            try
            {
                var instance = Activator.CreateInstance(type, sensors);
            
                _cache.Add(type.GenerateIdentifier(), instance);
            }
            catch { }
        }

        bool IsComponentDiagnostic(Type type)
            => type.IsClass
               && !type.IsGenericType
               && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IComponentDiagnostic<>));
    }
}