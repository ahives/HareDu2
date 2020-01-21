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
namespace HareDu.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Core;
    using Core.Extensions;
    using Core.Testing;

    public class BrokerObjectRegistrar :
        IBrokerObjectRegistrar
    {
        readonly IDictionary<string, object> _cache;

        public IDictionary<string, object> ObjectCache => _cache;

        public BrokerObjectRegistrar()
        {
            _cache = new Dictionary<string, object>();
        }

        public void RegisterAll(HttpClient client)
        {
            var types = GetType()
                .Assembly
                .GetTypes()
                .Where(x => typeof(BrokerObject).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var type in types)
            {
                if (type.GetInterface(typeof(HareDuTestingFake).FullName) != null || _cache.ContainsKey(type.FullName))
                    continue;
                
                RegisterInstance(type, client);
            }
        }

        public void Register(Type type, HttpClient client)
        {
            if (_cache.ContainsKey(type.FullName))
                return;
            
            RegisterInstance(type, client);
        }

        public void Register<T>(HttpClient client)
        {
            Type type = typeof(T);
            if (_cache.ContainsKey(type.FullName))
                return;
            
            RegisterInstance(type, client);
        }

        public bool TryRegisterAll(HttpClient client)
        {
            if (client.IsNull())
                return false;
            
            var types = GetType()
                .Assembly
                .GetTypes()
                .Where(x => typeof(BrokerObject).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var type in types)
            {
                if (type.GetInterface(typeof(HareDuTestingFake).FullName) != null || _cache.ContainsKey(type.FullName))
                    continue;
                
                RegisterInstance(type, client);
            }

            return true;
        }

        public bool TryRegister(Type type, HttpClient client)
        {
            if (_cache.ContainsKey(type.FullName))
                return false;
            
            RegisterInstance(type, client);
            return true;
        }

        public bool TryRegister<T>(HttpClient client)
        {
            Type type = typeof(T);
            if (_cache.ContainsKey(type.FullName))
                return false;
            
            RegisterInstance(type, client);
            return true;
        }

        protected virtual void RegisterInstance(Type type, HttpClient client)
        {
            try
            {
                var instance = type.IsDerivedFrom(typeof(BaseBrokerObject))
                    ? Activator.CreateInstance(type, client)
                    : Activator.CreateInstance(type);

                if (instance.IsNull())
                    return;
                
                _cache.Add(type.FullName, instance);
            }
            catch { }
        }
    }
}