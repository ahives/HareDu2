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
    using Core.Extensions;
    using Core.Testing;

    public class BrokerObjectRegistrar :
        IBrokerObjectRegistrar
    {
        readonly IBrokerObjectTypeFinder _finder;
        readonly IBrokerObjectInstanceCreator _creator;
        readonly IDictionary<string, object> _cache;

        public IDictionary<string, object> ObjectCache => _cache;

        public BrokerObjectRegistrar(IBrokerObjectTypeFinder finder, IBrokerObjectInstanceCreator creator)
        {
            _finder = finder;
            _creator = creator;
            _cache = new Dictionary<string, object>();
        }

        public void RegisterAll()
        {
            var types = _finder.GetTypes().ToList();
            bool registered = true;

            for (int i = 0; i < types.Count; i++)
            {
                if (types[i].GetInterface(typeof(HareDuTestingFake).FullName) != null || _cache.ContainsKey(types[i].GetIdentifier()))
                    continue;
                
                registered = RegisterInstance(types[i]) & registered;
            }

            if (!registered)
                _cache.Clear();
        }

        public void Register(Type type, HttpClient client)
        {
            if (_cache.ContainsKey(type.GetIdentifier()))
                return;
            
            RegisterInstance(type, client);
        }

        public void Register<T>(HttpClient client)
        {
            Type type = typeof(T);
            if (_cache.ContainsKey(type.GetIdentifier()))
                return;
            
            RegisterInstance(type, client);
        }

        public bool TryRegisterAll()
        {
            var types = _finder.GetTypes().ToList();
            bool registered = true;

            for (int i = 0; i < types.Count; i++)
            {
                if (types[i].GetInterface(typeof(HareDuTestingFake).FullName) != null || _cache.ContainsKey(types[i].GetIdentifier()))
                    continue;
                
                registered = RegisterInstance(types[i]) & registered;
            }

            if (!registered)
                _cache.Clear();

            return registered;
        }

        public bool TryRegister(Type type, HttpClient client)
        {
            if (_cache.ContainsKey(type.GetIdentifier()))
                return false;
            
            return RegisterInstance(type, client);
        }

        public bool TryRegister<T>(HttpClient client)
        {
            Type type = typeof(T);
            if (_cache.ContainsKey(type.GetIdentifier()))
                return false;
            
            return RegisterInstance(type, client);
        }

        protected virtual bool RegisterInstance(Type type, HttpClient client)
        {
            try
            {
                var instance = _creator.CreateInstance(type, client);

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

        protected virtual bool RegisterInstance(Type type)
        {
            try
            {
                var instance = _creator.CreateInstance(type);

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