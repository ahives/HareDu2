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
namespace HareDu
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Core;
    using Core.Extensions;
    using Core.Testing;
    using Registration;

    public class BrokerObjectFactory :
        IBrokerObjectFactory
    {
        readonly HttpClient _client;
        readonly IBrokerObjectRegistrar _registrar;

        public BrokerObjectFactory(HttpClient client, IBrokerObjectRegistrar registrar)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _registrar = registrar;
            
            _registrar.RegisterAll();
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

            string key = type.GetIdentifier();
            
            if (_registrar.ObjectCache.ContainsKey(key))
                return (T)_registrar.ObjectCache[key];
            
            _registrar.Register(type, _client);
            
            return (T)_registrar.ObjectCache[key];
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