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
namespace HareDu.Analytics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using Diagnostics;
    using Diagnostics.Analyzers;
    using Diagnostics.Probes;

    public class ReportAnalyzerFactory :
        IReportAnalyzerFactory
    {
        readonly IDictionary<string, IReportAnalyzer> _analyzerCache;

        public ReportAnalyzerFactory()
        {
            _analyzerCache = new Dictionary<string, IReportAnalyzer>();
            
            bool registeredAnalyzers = TryRegisterAllReportAnalyzers();
            if (!registeredAnalyzers)
                throw new HareDuBrokerObjectInitException();
        }

        public bool TryGet(string key, out IReportAnalyzer analyzer)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                analyzer = DiagnosticReportAnalyzerCache.NoOpAnalyzer;
                return false;
            }

            if (_analyzerCache.ContainsKey(key))
            {
                analyzer = _analyzerCache[key];
                return true;
            }
            
            analyzer = DiagnosticReportAnalyzerCache.NoOpAnalyzer;
            return false;
        }

        public bool TryGet(Type type, out IReportAnalyzer analyzer)
        {
            string key = type.FullName;
            
            if (string.IsNullOrWhiteSpace(key))
            {
                analyzer = DiagnosticReportAnalyzerCache.NoOpAnalyzer;
                return false;
            }

            if (_analyzerCache.ContainsKey(key))
            {
                analyzer = _analyzerCache[key];
                return true;
            }
            
            analyzer = DiagnosticReportAnalyzerCache.NoOpAnalyzer;
            return false;
        }

        public bool TryGet<T>(out IReportAnalyzer analyzer)
            where T : IReportAnalyzer
        {
            string key = typeof(T).FullName;
            
            if (string.IsNullOrWhiteSpace(key))
            {
                analyzer = DiagnosticReportAnalyzerCache.NoOpAnalyzer;
                return false;
            }

            if (_analyzerCache.ContainsKey(key))
            {
                analyzer = _analyzerCache[key];
                return true;
            }
            
            analyzer = DiagnosticReportAnalyzerCache.NoOpAnalyzer;
            return false;
        }

        protected virtual bool RegisterReportAnalyzerInstance(Type type, string key)
        {
            try
            {
                var instance = CreateReportAnalyzerInstance(type);

                if (instance.IsNull())
                    return false;

                _analyzerCache.Add(key, instance);

                return _analyzerCache.ContainsKey(key);
            }
            catch
            {
                return false;
            }
        }

        protected virtual IReportAnalyzer CreateReportAnalyzerInstance(Type type)
        {
            var instance = (IReportAnalyzer) Activator.CreateInstance(type);
            
            return instance;
        }

        protected virtual bool TryRegisterAllReportAnalyzers()
        {
            var typeMap = GetReportAnalyzerTypeMap(GetType());
            bool registered = true;

            foreach (var type in typeMap)
            {
                registered = RegisterReportAnalyzerInstance(type.Value, type.Key) & registered;
            }

            if (!registered)
                _analyzerCache.Clear();

            return registered;
        }

        protected virtual IDictionary<string, Type> GetReportAnalyzerTypeMap(Type type)
        {
            var types = type
                .Assembly
                .GetTypes()
                .Where(x => typeof(IReportAnalyzer).IsAssignableFrom(x) && !x.IsInterface)
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