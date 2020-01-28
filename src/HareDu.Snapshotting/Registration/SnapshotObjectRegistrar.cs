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
namespace HareDu.Snapshotting.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using Core.Serialization;
    using Internal;

    public class SnapshotObjectRegistrar :
        ISnapshotObjectRegistrar
    {
        readonly IBrokerObjectFactory _factory;
        readonly Dictionary<string, object> _cache;

        public IDictionary<string, object> ObjectCache => _cache;

        public SnapshotObjectRegistrar(IBrokerObjectFactory factory)
        {
            _factory = factory;
            _cache = new Dictionary<string, object>();
        }

        public void RegisterAll()
        {
            bool registered = true;
            
            foreach (var type in GetTypes())
            {
                if (_cache.ContainsKey(type.GetIdentifier()))
                    continue;
                
                registered = RegisterInstance(type) & registered;
            }
            
            if (!registered)
                _cache.Clear();
        }

        public void Register(Type type)
        {
            if (_cache.ContainsKey(type.GetIdentifier()))
                return;
            
            RegisterInstance(type);
        }

        public void Register<T>()
        {
            Type type = typeof(T);
            if (_cache.ContainsKey(type.GetIdentifier()))
                return;
            
            RegisterInstance(type);
        }

        public bool TryRegisterAll()
        {
            bool registered = true;
            
            foreach (var type in GetTypes())
            {
                if (_cache.ContainsKey(type.GetIdentifier()))
                    continue;
                
                registered = RegisterInstance(type) & registered;
            }
            
            if (!registered)
                _cache.Clear();

            return registered;
        }

        public bool TryRegister(Type type)
        {
            if (_cache.ContainsKey(type.GetIdentifier()))
                return false;
            
            return RegisterInstance(type);
        }

        public bool TryRegister<T>()
        {
            Type type = typeof(T);
            if (_cache.ContainsKey(type.GetIdentifier()))
                return false;
            
            return RegisterInstance(type);
        }

        protected virtual IEnumerable<Type> GetTypes() =>
            from concrete in GetConcreteTypes()
            from @interface in GetInterfaces(concrete)
            where @interface.GetGenericTypeDefinition() == typeof(HareDuSnapshot<>)
            select concrete;

        protected virtual IEnumerable<Type> GetInterfaces(Type type)
        {
            return type.GetInterfaces().Where(x => x.IsGenericType);
        }

        protected virtual IEnumerable<Type> GetConcreteTypes()
        {
            return GetType()
                .Assembly
                .GetTypes()
                .Where(x => x.IsConcrete());
        }

        protected virtual bool RegisterInstance(Type type)
        {
            try
            {
                var instance = type.IsDerivedFrom(typeof(BaseSnapshot<>))
                    ? Activator.CreateInstance(type, _factory)
                    : Activator.CreateInstance(type);

                if (instance.IsNull())
                    return false;

                string key = type.GetIdentifier();
                
                _cache.Add(key, instance);
                
                return _cache.ContainsKey(key);
            }
            catch
            {
                return false;
            }
        }
    }
}