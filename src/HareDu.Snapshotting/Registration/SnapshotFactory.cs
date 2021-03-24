namespace HareDu.Snapshotting.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Configuration;
    using Core.Extensions;
    using Internal;

    public class SnapshotFactory :
        ISnapshotFactory
    {
        readonly HareDuConfig _config;
        readonly IBrokerObjectFactory _factory;
        readonly IDictionary<string, object> _cache;

        public SnapshotFactory(IBrokerObjectFactory factory)
        {
            _factory = factory;
            _cache = new Dictionary<string, object>();
            
            if (!TryRegisterAll())
                throw new HareDuSnapshotInitException("Could not register snapshot lenses.");
        }

        public SnapshotFactory(HareDuConfig config)
        {
            _config = config;
            _factory = new BrokerObjectFactory(_config);
            _cache = new Dictionary<string, object>();
            
            if (!TryRegisterAll())
                throw new HareDuSnapshotInitException("Could not register snapshot lenses.");
        }

        public SnapshotLens<T> Lens<T>()
            where T : Snapshot
        {
            Type type = typeof(T);
            
            if (type.IsNull())
                return new EmptySnapshotLens<T>();
            
            if (_cache.ContainsKey(type.FullName))
                return (SnapshotLens<T>) _cache[type.FullName];

            return new EmptySnapshotLens<T>();
        }

        public ISnapshotFactory Register<T>(SnapshotLens<T> lens)
            where T : Snapshot
        {
            Type type = typeof(T);

            if (_cache.ContainsKey(type.FullName))
                return this;
            
            _cache.Add(type.FullName, lens);

            return this;
        }

        protected virtual bool TryRegisterAll()
        {
            var typeMap = GetTypeMap(GetType());
            bool registered = true;

            foreach (var type in typeMap)
            {
                registered = RegisterInstance(type.Value, type.Key) & registered;
            }

            if (!registered)
                _cache.Clear();

            return registered;
        }

        protected virtual bool RegisterInstance(Type type, string key)
        {
            try
            {
                var instance = CreateInstance(type);

                if (instance.IsNull())
                    return false;

                _cache.Add(key, instance);

                return _cache.ContainsKey(key);
            }
            catch
            {
                return false;
            }
        }

        protected virtual object CreateInstance(Type type)
        {
            var instance = type.IsDerivedFrom(typeof(BaseSnapshotLens<>))
                ? Activator.CreateInstance(type, _factory)
                : Activator.CreateInstance(type);

            return instance;
        }
        
        protected virtual IDictionary<string, Type> GetTypeMap(Type findType)
        {
            var types = findType.Assembly.GetTypes();
            var interfaces = new Dictionary<string, Type>();

            foreach (var type in types)
            {
                if (!type.IsInterface || !type.InheritsFromInterface(typeof(Snapshot)))
                    continue;
                
                if (interfaces.ContainsKey(type.FullName))
                    continue;
                    
                interfaces.Add(type.FullName, type);
            }

            var typeMap = new Dictionary<string, Type>();

            foreach (var @interface in interfaces)
            {
                foreach (var type in types)
                {
                    if (type.IsInterface)
                        continue;
                    
                    if (type.GetInterfaces().Any(x => x == Type.GetType($"{typeof(SnapshotLens<>).FullName}[{@interface.Key}]")))
                        typeMap.Add(@interface.Key, type);
                }
            }

            return typeMap;
        }
    }
}