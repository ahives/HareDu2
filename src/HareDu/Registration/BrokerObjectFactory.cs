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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using Core;
    using Core.Configuration;
    using Core.Extensions;

    public class BrokerObjectFactory :
        IBrokerObjectFactory
    {
        readonly HttpClient _client;
        readonly ConcurrentDictionary<string, object> _cache;

        public BrokerObjectFactory(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _cache = new ConcurrentDictionary<string, object>();
            
            if (!TryRegisterAll())
                throw new HareDuBrokerObjectInitException("Could not register broker objects.");
        }

        public BrokerObjectFactory(HareDuConfig config)
        {
            _client = GetClient(config);
            _cache = new ConcurrentDictionary<string, object>();
            
            if (!TryRegisterAll())
                throw new HareDuBrokerObjectInitException("Could not register broker objects.");
        }

        public T Object<T>()
            where T : BrokerObject
        {
            Type type = typeof(T);
            
            if (type == null)
                throw new HareDuBrokerObjectInitException($"Failed to find implementation class for interface {typeof(T)}");

            var typeMap = GetTypeMap(typeof(T));

            if (!typeMap.ContainsKey(type.FullName))
                return default;
            
            if (_cache.ContainsKey(type.FullName))
                return (T) _cache[type.FullName];
                
            bool registered = RegisterInstance(typeMap[type.FullName], type.FullName, _client);

            if (registered)
                return (T) _cache[type.FullName];

            return default;
        }

        public bool IsRegistered(string key) => _cache.ContainsKey(key);
        
        public IReadOnlyDictionary<string, object> GetObjects() => _cache;

        public void CancelPendingRequest()
        {
            _client.CancelPendingRequests();
        }

        public bool TryRegisterAll()
        {
            var typeMap = GetTypeMap(GetType());
            bool registered = true;

            foreach (var type in typeMap)
            {
                if (_cache.ContainsKey(type.Key))
                    continue;
                
                registered = RegisterInstance(type.Value, type.Key) & registered;
            }

            if (!registered)
                _cache.Clear();

            return registered;
        }

        protected virtual HttpClient GetClient(HareDuConfig config)
        {
            var uri = new Uri($"{config.Broker.Url}/");
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(config.Broker.Credentials.Username, config.Broker.Credentials.Password)
            };
            
            var client = new HttpClient(handler){BaseAddress = uri};
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (config.Broker.Timeout != TimeSpan.Zero)
                client.Timeout = config.Broker.Timeout;

            return client;
        }

        protected virtual bool RegisterInstance(Type type, string key, HttpClient client)
        {
            try
            {
                var instance = CreateInstance(type, client);

                if (instance.IsNull())
                    return false;

                return _cache.TryAdd(key, instance);
            }
            catch
            {
                return false;
            }
        }

        protected virtual bool RegisterInstance(Type type, string key)
        {
            try
            {
                var instance = CreateInstance(type);

                if (instance.IsNull())
                    return false;

                return _cache.TryAdd(key, instance);
            }
            catch
            {
                return false;
            }
        }
        
        protected virtual IDictionary<string, Type> GetTypeMap(Type findType)
        {
            var types = findType.Assembly.GetTypes();
            var interfaces = types
                .Where(x => typeof(BrokerObject).IsAssignableFrom(x) && x.IsInterface && !x.IsNull())
                .ToList();
            var typeMap = new Dictionary<string, Type>();

            for (int i = 0; i < interfaces.Count; i++)
            {
                var type = types.Find(x => interfaces[i].IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

                if (type.IsNull())
                    continue;
                
                typeMap.Add(interfaces[i].FullName, type);
            }

            return typeMap;
        }

        protected virtual object CreateInstance(Type type)
        {
            var instance = type.IsDerivedFrom(typeof(BaseBrokerObject))
                ? Activator.CreateInstance(type, _client)
                : Activator.CreateInstance(type);

            return instance;
        }

        protected virtual object CreateInstance(Type type, HttpClient client)
        {
            var instance = Activator.CreateInstance(type, client);

            return instance;
        }
    }
}