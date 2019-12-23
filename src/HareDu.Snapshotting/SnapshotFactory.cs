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
    using System.Net.Http;
    using Core;
    using Core.Configuration;
    using Registration;

    public class SnapshotFactory :
        ISnapshotFactory
    {
        readonly IBrokerObjectFactory _factory;
        readonly IDictionary<string, object> _snapshotCache;

        public SnapshotFactory(IBrokerObjectFactory factory, IDictionary<string, object> snapshotCache)
        {
            _factory = factory;
            _snapshotCache = snapshotCache;
        }

        public SnapshotFactory(IBrokerObjectFactory factory, ISnapshotRegistration registrar)
        {
            _factory = factory;

            registrar.RegisterAll(factory);
            
            _snapshotCache = registrar.Cache;
        }

        public SnapshotFactory(IBrokerObjectFactory factory)
        {
            _factory = factory;
            
            ISnapshotRegistration registrar = new SnapshotRegistration();
            
            registrar.RegisterAll(factory);

            _snapshotCache = registrar.Cache;
        }

        public SnapshotFactory(HttpClient client)
        {
            _factory = new BrokerObjectFactory(client);
            
            ISnapshotRegistration registrar = new SnapshotRegistration();
            
            registrar.RegisterAll(_factory);

            _snapshotCache = registrar.Cache;
        }

        public SnapshotFactory(Action<ClientConfigProvider> config)
        {
            var brokerConfigProvider = new BrokerConfigProvider(new ConfigurationProvider());
            var settings = brokerConfigProvider.Init(config);
            var comm = new BrokerConnectionClient();
            
            _factory = new BrokerObjectFactory(comm.Create(settings));
            
            ISnapshotRegistration registrar = new SnapshotRegistration();
            
            registrar.RegisterAll(_factory);

            _snapshotCache = registrar.Cache;
        }

        public SnapshotFactory()
        {
            var brokerConfigProvider = new BrokerConfigProvider(new ConfigurationProvider());

            brokerConfigProvider.TryGet(out var settings);

            var comm = new BrokerConnectionClient();
            
            _factory = new BrokerObjectFactory(comm.Create(settings));
            
            ISnapshotRegistration registrar = new SnapshotRegistration();
            
            registrar.RegisterAll(_factory);

            _snapshotCache = registrar.Cache;
        }

        public T Snapshot<T>()
            where T : ResourceSnapshot<Snapshot>
        {
            Type type = GetType()
                .Assembly
                .GetTypes()
                .FirstOrDefault(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface);

            if (type == null)
                throw new HareDuSnapshotInitException($"Failed to find implementation class for interface {typeof(T)}");

            if (_snapshotCache.ContainsKey(type.FullName))
                return (T)_snapshotCache[type.FullName];

            var instance = Activator.CreateInstance(type, _factory);

            _snapshotCache.Add(type.FullName, instance);
            
            return (T)instance;
        }
    }
}