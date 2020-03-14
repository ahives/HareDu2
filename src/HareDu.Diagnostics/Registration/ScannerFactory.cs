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
    using Core.Configuration;
    using Core.Extensions;
    using KnowledgeBase;
    using Probes;
    using Scans;
    using Snapshotting;

    /// <summary>
    /// This class should be instantiate once since construction can be expensive.
    /// </summary>
    public class ScannerFactory :
        IScannerFactory
    {
        readonly DiagnosticsConfig _config;
        readonly IKnowledgeBaseProvider _kb;
        readonly IDictionary<string, object> _cache;
        readonly IDictionary<string, DiagnosticProbe> _probeCache;
        readonly IList<IDisposable> _observers;

        public ScannerFactory(DiagnosticsConfig config, IKnowledgeBaseProvider kb)
        {
            _config = !config.IsNull() ? config : throw new HareDuDiagnosticsInitException();
            _kb = !kb.IsNull() ? kb : throw new HareDuDiagnosticsInitException();
            _cache = new Dictionary<string, object>();
            _probeCache = new Dictionary<string, DiagnosticProbe>();
            _observers = new List<IDisposable>();
            
            bool registeredProbes = TryRegisterAllProbes();
            if (!registeredProbes)
                throw new HareDuDiagnosticsInitException();
            
            bool registered = TryRegisterAll();
            if (!registered)
                throw new HareDuDiagnosticsInitException();
        }

        public bool TryGet<T>(out DiagnosticScan<T> scanner)
            where T : Snapshot
        {
            Type type = typeof(T);
            
            if (type.IsNull() || !_cache.ContainsKey(type.FullName))
            {
                scanner = new NoOpScanner<T>();
                return false;
            }
            
            scanner = (DiagnosticScan<T>) _cache[type.FullName];
            return true;
        }

        public void RegisterObservers(IReadOnlyList<IObserver<ProbeContext>> observers)
        {
            var probes = _probeCache.Values.ToList();
            
            for (int i = 0; i < observers.Count; i++)
            {
                if (observers[i] == null)
                    continue;
                
                for (int j = 0; j < probes.Count; j++)
                {
                    _observers.Add(probes[j].Subscribe(observers[i]));
                }
            }
        }

        public void RegisterObserver(IObserver<ProbeContext> observer)
        {
            if (observer == null)
                return;
            
            var probes = _probeCache.Values.ToList();
            
            for (int j = 0; j < probes.Count; j++)
            {
                _observers.Add(probes[j].Subscribe(observer));
            }
        }

        public bool RegisterProbe<T>(T probe)
            where T : DiagnosticProbe
        {
            if (probe.IsNull())
                return false;
            
            _probeCache.Add(typeof(T).FullName, probe);

            return _probeCache.ContainsKey(typeof(T).FullName);
        }

        public IReadOnlyList<string> GetAvailableProbes() => _probeCache.Keys.ToList();

        protected virtual bool TryRegisterAll()
        {
            var typeMap = GetTypeMap(GetType());
            bool registered = true;

            foreach (var type in typeMap)
            {
                registered = RegisterInstance(type.Value, type.Key) & registered;
            }

            if (!registered)
                _cache.Clear();

            return registered;
        }

        protected virtual bool RegisterInstance(Type type, string key)
        {
            try
            {
                var instance = CreateInstance(type);

                if (instance.IsNull())
                    return false;

                _cache.Add(key, instance);

                return _cache.ContainsKey(key);
            }
            catch
            {
                return false;
            }
        }

        protected virtual object CreateInstance(Type type)
        {
            var instance = Activator.CreateInstance(type, _probeCache.Values.ToList());

            return instance;
        }
        
        protected virtual IDictionary<string, Type> GetTypeMap(Type findType)
        {
            var types = findType
                .Assembly
                .GetTypes()
                .Where(x => x.IsClass && !x.IsGenericType)
                .ToList();
            var typeMap = new Dictionary<string, Type>();

            foreach (var type in types)
            {
                if (type.IsNull())
                    continue;

                if (!type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(DiagnosticScan<>)))
                    continue;

                var genericType = type
                    .GetInterfaces()
                    .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(DiagnosticScan<>));

                if (genericType.IsNull())
                    continue;

                string key = genericType.GetGenericArguments()[0].FullName;

                if (typeMap.ContainsKey(key))
                    continue;

                typeMap.Add(key, type);
            }

            return typeMap;
        }

        protected virtual bool TryRegisterAllProbes()
        {
            var typeMap = GetProbeTypeMap(GetType());
            bool registered = true;

            foreach (var type in typeMap)
            {
                registered = RegisterProbeInstance(type.Value, type.Key) & registered;
            }

            if (!registered)
                _probeCache.Clear();

            return registered;
        }

        protected virtual IDictionary<string, Type> GetProbeTypeMap(Type type)
        {
            var types = type
                .Assembly
                .GetTypes()
                .Where(x => typeof(DiagnosticProbe).IsAssignableFrom(x) && !x.IsInterface)
                .ToList();

            var typeMap = new Dictionary<string, Type>();

            for (int i = 0; i < types.Count; i++)
            {
                if (types[i].IsNull() || typeMap.ContainsKey(types[i].FullName))
                    continue;
                
                typeMap.Add(types[i].FullName, types[i]);
            }

            return typeMap;
        }

        protected virtual bool RegisterProbeInstance(Type type, string key)
        {
            try
            {
                var instance = CreateProbeInstance(type);

                if (instance.IsNull())
                    return false;

                _probeCache.Add(key, instance);

                return _probeCache.ContainsKey(key);
            }
            catch
            {
                return false;
            }
        }

        protected virtual DiagnosticProbe CreateProbeInstance(Type type)
        {
            var instance = type.GetConstructors()[0].GetParameters()[0].ParameterType == typeof(DiagnosticsConfig)
                           && type.GetConstructors()[0].GetParameters()[1].ParameterType == typeof(IKnowledgeBaseProvider)
                ? (DiagnosticProbe) Activator.CreateInstance(type, _config, _kb)
                : (DiagnosticProbe) Activator.CreateInstance(type, _kb);
            
            return instance;
        }
    }
}