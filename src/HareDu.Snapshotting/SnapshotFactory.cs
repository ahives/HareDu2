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
namespace HareDu.Snapshotting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using HareDu.Registration;
    using Registration;

    public class SnapshotFactory :
        ISnapshotFactory
    {
        readonly IBrokerObjectFactory _factory;
        readonly IDictionary<string, object> _cache;

        public SnapshotFactory(IBrokerObjectFactory factory, ISnapshotObjectRegistrar registrar)
        {
            _factory = factory;
            _cache = registrar.ObjectCache;
        }

        public T Snapshot<T>()
            where T : HareDuSnapshot<Snapshot>
        {
            Type type = GetType()
                .Assembly
                .GetTypes()
                .FirstOrDefault(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface);

            if (type == null)
                throw new HareDuSnapshotInitException($"Failed to find implementation class for interface {typeof(T)}");

            string key = type.GetIdentifier();
            
            if (_cache.ContainsKey(key))
                return (T)_cache[key];

            var instance = Activator.CreateInstance(type, _factory);

            _cache.Add(key, instance);
            
            return (T)instance;
        }
    }
}