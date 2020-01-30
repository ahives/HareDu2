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
namespace HareDu.Diagnostics.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using Probes;
    using Scanning;

    public class ComponentDiagnosticRegistrar :
        IComponentDiagnosticRegistrar
    {
        readonly IReadOnlyList<IDiagnosticProbe> _analyzers;
        readonly List<Type> _types;
        readonly IDictionary<string, object> _cache;

        public IReadOnlyList<Type> Types => _types;
        public IDictionary<string, object> ObjectCache => _cache;

        public ComponentDiagnosticRegistrar(IReadOnlyList<IDiagnosticProbe> analyzers)
        {
            _analyzers = analyzers;
            _cache = new Dictionary<string, object>();
            _types = GetTypes().ToList();
        }

        public ComponentDiagnosticRegistrar(IDiagnosticProbeRegistrar registrar)
        {
            _analyzers = registrar.ObjectCache.Values.ToList();
            _cache = new Dictionary<string, object>();
            _types = GetTypes().ToList();
        }

        public void RegisterAll()
        {
            for (int i = 0; i < _types.Count; i++)
            {
                if (_cache.ContainsKey(_types[i].GetIdentifier()))
                    continue;
                
                RegisterInstance(_types[i]);
            }
        }

        public void Register(Type type)
        {
            if (_cache.ContainsKey(type.GetIdentifier()))
                return;
            
            _types.Add(type);
            
            RegisterInstance(type);
        }

        public void Register<T>()
        {
            Type type = typeof(T);

            if (_cache.ContainsKey(type.GetIdentifier()))
                return;
            
            _types.Add(type);
            
            RegisterInstance(type);
        }

        IEnumerable<Type> GetTypes() =>
            GetType()
                .Assembly
                .GetTypes()
                .Where(IsComponentDiagnostic)
                .ToList();

        void RegisterInstance(Type type)
        {
            try
            {
                var instance = Activator.CreateInstance(type, _analyzers);
            
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