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

    public class ScanAnalyzerFactory :
        IScanAnalyzerFactory
    {
        readonly IDictionary<string, IScanAnalyzer> _cache;

        public ScanAnalyzerFactory()
        {
            _cache = new Dictionary<string, IScanAnalyzer>();
            
            bool registered = TryRegisterAll();
            if (!registered)
                throw new HareDuBrokerObjectInitException();
        }

        public bool TryGet(string key, out IScanAnalyzer analyzer)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                analyzer = DiagnosticCache.NoOpAnalyzer;
                return false;
            }

            if (_cache.ContainsKey(key))
            {
                analyzer = _cache[key];
                return true;
            }
            
            analyzer = DiagnosticCache.NoOpAnalyzer;
            return false;
        }

        public bool TryGet(Type type, out IScanAnalyzer analyzer)
        {
            string key = type.FullName;
            
            if (string.IsNullOrWhiteSpace(key))
            {
                analyzer = DiagnosticCache.NoOpAnalyzer;
                return false;
            }

            if (_cache.ContainsKey(key))
            {
                analyzer = _cache[key];
                return true;
            }
            
            analyzer = DiagnosticCache.NoOpAnalyzer;
            return false;
        }

        public bool TryGet<T>(out IScanAnalyzer analyzer)
            where T : IScanAnalyzer
        {
            string key = typeof(T).FullName;
            
            if (string.IsNullOrWhiteSpace(key))
            {
                analyzer = DiagnosticCache.NoOpAnalyzer;
                return false;
            }

            if (_cache.ContainsKey(key))
            {
                analyzer = _cache[key];
                return true;
            }
            
            analyzer = DiagnosticCache.NoOpAnalyzer;
            return false;
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

        protected virtual IScanAnalyzer CreateInstance(Type type)
        {
            var instance = (IScanAnalyzer) Activator.CreateInstance(type);
            
            return instance;
        }

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

        protected virtual IDictionary<string, Type> GetTypeMap(Type type)
        {
            var types = type
                .Assembly
                .GetTypes()
                .Where(x => typeof(IScanAnalyzer).IsAssignableFrom(x) && !x.IsInterface)
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
    }
}