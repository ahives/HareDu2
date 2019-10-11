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
namespace HareDu.Diagnostics.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Analyzers;
    using Scanning;

    public class ComponentDiagnosticRegistration :
        IComponentDiagnosticRegistration
    {
        readonly List<Type> _types;
        readonly IDictionary<string, object> _cache;

        public IReadOnlyList<Type> Types => _types;
        public IDictionary<string, object> Cache => _cache;

        public ComponentDiagnosticRegistration()
        {
            _cache = new Dictionary<string, object>();
            _types = GetTypes();
        }

        public void Register<T>(IReadOnlyList<IDiagnosticAnalyzer> analyzers)
        {
            Type type = typeof(T);
            
            _types.Add(type);
            
            Register(type, analyzers);
        }

        public void RegisterAll(IReadOnlyList<IDiagnosticAnalyzer> analyzers)
        {
            foreach (var type in _types)
            {
                Register(type, analyzers);
            }
        }

        List<Type> GetTypes() =>
            GetType()
                .Assembly
                .GetTypes()
                .Where(IsComponentDiagnostic)
                .ToList();

        void Register(Type type, IReadOnlyList<IDiagnosticAnalyzer> analyzers)
        {
            try
            {
                var instance = Activator.CreateInstance(type, analyzers);
            
                _cache.Add(type.GetIdentifier(), instance);
            }
            catch { }
        }

        bool IsComponentDiagnostic(Type type)
            => type.IsClass
               && !type.IsGenericType
               && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IComponentDiagnostic<>));
    }
}