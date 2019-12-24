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
namespace HareDu.Snapshotting.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SnapshotObjectRegistry :
        ISnapshotObjectRegistry
    {
        readonly IBrokerObjectFactory _factory;
        readonly Dictionary<string, object> _cache;

        public IDictionary<string, object> ObjectCache => _cache;

        public SnapshotObjectRegistry(IBrokerObjectFactory factory)
        {
            _factory = factory;
            _cache = new Dictionary<string, object>();
        }

        public void RegisterAll()
        {
            var types = GetType()
                .Assembly
                .GetTypes()
                .Where(x => typeof(ResourceSnapshot<>).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var type in types)
            {
                if (_cache.ContainsKey(type.FullName))
                    continue;
                
                RegisterInstance(type);
            }
        }

        public void Register(Type type)
        {
            if (_cache.ContainsKey(type.FullName))
                return;
            
            RegisterInstance(type);
        }

        public void Register<T>()
        {
            Type type = typeof(T);
            if (_cache.ContainsKey(type.FullName))
                return;
            
            RegisterInstance(type);
        }

        void RegisterInstance(Type type)
        {
            try
            {
                var instance = Activator.CreateInstance(type, _factory);
            
                _cache.Add(type.FullName, instance);
            }
            catch { }
        }
    }
}