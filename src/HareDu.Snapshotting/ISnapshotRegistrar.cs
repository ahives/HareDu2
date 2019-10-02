namespace HareDu.Snapshotting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public interface ISnapshotRegistrar
    {
        IDictionary<string, object> Cache { get; }

        void RegisterAll(IBrokerObjectFactory factory);
    }

    public class SnapshotRegistrar : ISnapshotRegistrar
    {
        readonly Dictionary<string, object> _cache;

        public IDictionary<string, object> Cache => _cache;

        public SnapshotRegistrar()
        {
            _cache = new Dictionary<string, object>();
        }

        public void RegisterAll(IBrokerObjectFactory factory)
        {
            var types = GetType()
                .Assembly
                .GetTypes()
                .Where(x => typeof(ResourceSnapshot<>).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type, factory);
                
                _cache.Add(type.FullName, instance);
            }
        }
    }
}