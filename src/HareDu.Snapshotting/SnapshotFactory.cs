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
    using Core.Exceptions;
    using Model;

    public class SnapshotFactory :
        ISnapshotFactory
    {
        readonly IResourceFactory _factory;
        readonly IDictionary<string, object> _snapshotCache = new Dictionary<string, object>();

        public SnapshotFactory(IResourceFactory factory)
        {
            _factory = factory;
            
            RegisterSnapshots();
        }

        public U Snapshot<T, U>()
            where T : Snapshot
            where U : ComponentSnapshot<T>
        {
            Type type = GetType()
                .Assembly
                .GetTypes()
                .FirstOrDefault(x => typeof(U).IsAssignableFrom(x) && !x.IsInterface);

            if (type == null)
                throw new HareDuResourceInitException($"Failed to find implementation class for interface {typeof(U)}");

            if (_snapshotCache.ContainsKey(type.FullName))
                return (U)_snapshotCache[type.FullName];

            var instance = Activator.CreateInstance(type, _factory);

            _snapshotCache.Add(type.FullName, instance);
            
            return (U)instance;
        }

        void RegisterSnapshots()
        {
            var types = GetType()
                .Assembly
                .GetTypes()
                .Where(x => typeof(ComponentSnapshot<>).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type, _factory);
                
                _snapshotCache.Add(type.FullName, instance);
            }
        }
    }
}