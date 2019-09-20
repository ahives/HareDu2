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
namespace HareDu.Snapshotting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Model;

    public class SnapshotFactory :
        ISnapshotFactory
    {
        readonly IResourceFactory _factory;
        readonly IDictionary<string, object> _cache = new Dictionary<string, object>();

        public SnapshotFactory(IResourceFactory factory)
        {
            _factory = factory;
            
            RegisterSnapshots();
        }

        public T Resource<T>()
            where T : ResourceSnapshot<Snapshot>
        {
            Type type = GetType()
                .Assembly
                .GetTypes()
                .FirstOrDefault(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface);

            if (type == null)
                throw new HareDuSnapshotInitException($"Failed to find implementation class for interface {typeof(T)}");

            if (_cache.ContainsKey(type.FullName))
                return (T)_cache[type.FullName];

            var instance = Activator.CreateInstance(type, _factory);

            _cache.Add(type.FullName, instance);
            
            return (T)instance;
        }

        void RegisterSnapshots()
        {
            var types = GetType()
                .Assembly
                .GetTypes()
                .Where(x => typeof(ResourceSnapshot<>).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type, _factory);
                
                _cache.Add(type.FullName, instance);
            }
        }
    }
}