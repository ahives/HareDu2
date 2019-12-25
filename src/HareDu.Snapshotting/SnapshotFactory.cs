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
    using Core.Extensions;
    using Registration;

    public class SnapshotFactory :
        ISnapshotFactory
    {
        readonly IBrokerObjectFactory _factory;
        readonly IDictionary<string, object> _cache;

        public SnapshotFactory(IBrokerObjectFactory factory, IDictionary<string, object> snapshotObjectCache)
        {
            _factory = factory;
            _cache = snapshotObjectCache;
        }

        public SnapshotFactory(IBrokerObjectFactory factory, ISnapshotObjectRegistry registry)
        {
            _factory = factory;

            registry.RegisterAll();
            
            _cache = registry.ObjectCache;
        }

        public SnapshotFactory(IBrokerObjectFactory factory)
        {
            _factory = factory;
            
            var registry = new SnapshotObjectRegistry(factory);
            
            registry.RegisterAll();

            _cache = registry.ObjectCache;
        }

        public SnapshotFactory(HttpClient client)
        {
            _factory = new BrokerObjectFactory(client);
            
            var registry = new SnapshotObjectRegistry(_factory);
            
            registry.RegisterAll();

            _cache = registry.ObjectCache;
        }

        public SnapshotFactory(Action<ClientConfigProvider> config)
        {
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());
            var settings = configProvider.Init(config);
            var comm = new BrokerCommunication();
            
            _factory = new BrokerObjectFactory(comm.GetClient(settings));
            
            var registry = new SnapshotObjectRegistry(_factory);
            
            registry.RegisterAll();

            _cache = registry.ObjectCache;
        }

        public SnapshotFactory(IBrokerConfigProvider configProvider)
        {
            BrokerConfig settings;
            if (configProvider.IsNull())
            {
                var provider = new BrokerConfigProvider(new ConfigurationProvider());
                if (!provider.TryGet(out settings))
                    throw new HareDuClientConfigurationException();
            }
            else
            {
                configProvider.TryGet(out settings);
            }

            var comm = new BrokerCommunication();
            
            _factory = new BrokerObjectFactory(comm.GetClient(settings));
            
            var registry = new SnapshotObjectRegistry(_factory);
            
            registry.RegisterAll();

            _cache = registry.ObjectCache;
        }

        public SnapshotFactory()
        {
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);

            var comm = new BrokerCommunication();
            
            _factory = new BrokerObjectFactory(comm.GetClient(settings));
            
            var registry = new SnapshotObjectRegistry(_factory);
            
            registry.RegisterAll();

            _cache = registry.ObjectCache;
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

            if (_cache.ContainsKey(type.FullName))
                return (T)_cache[type.FullName];

            var instance = Activator.CreateInstance(type, _factory);

            _cache.Add(type.FullName, instance);
            
            return (T)instance;
        }
    }
}