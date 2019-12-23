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
namespace HareDu
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Core;
    using Core.Testing;
    using Registration;

    public class BrokerObjectFactory :
        IBrokerObjectFactory
    {
        readonly HttpClient _client;
        readonly IDictionary<string, object> _brokerObjectCache;

        public BrokerObjectFactory(HttpClient client, IDictionary<string, object> brokerObjectCache)
        {
            _brokerObjectCache = brokerObjectCache;
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public BrokerObjectFactory(HttpClient client, IBrokerObjectRegistration registrar)
        {
            _client = client;
            
            registrar.RegisterAll(client);

            _brokerObjectCache = registrar.Cache;
        }

        public BrokerObjectFactory(HttpClient client)
        {
            _client = client;
            
            IBrokerObjectRegistration registrar = new BrokerObjectRegistration();
            
            registrar.RegisterAll(client);

            _brokerObjectCache = registrar.Cache;
        }

        public T Object<T>()
            where T : BrokerObject
        {
            Type type = GetType()
                .Assembly
                .GetTypes()
                .FirstOrDefault(IsNotFakeType<T>);

            if (type == null)
                throw new HareDuBrokerObjectInitException($"Failed to find implementation class for interface {typeof(T)}");

            if (_brokerObjectCache.ContainsKey(type.FullName))
                return (T)_brokerObjectCache[type.FullName];
            
            var instance = (T)Activator.CreateInstance(type, _client);

            _brokerObjectCache.Add(type.FullName, instance);
            
            return instance;
        }

        public void CancelPendingRequest()
        {
            _client.CancelPendingRequests();
        }

        bool IsNotFakeType<T>(Type x)
            where T : BrokerObject
            => typeof(T).IsAssignableFrom(x)
               && !x.IsInterface
               && x.GetInterface(typeof(HareDuTestingFake).FullName) == null;
    }
}