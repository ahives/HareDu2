namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using HareDu.Model;

    class InternalScopedParameterInfo :
        ScopedParameterInfo
    {
        public InternalScopedParameterInfo(ScopedParameterInfoImpl obj)
        {
            VirtualHost = obj.VirtualHost;
            Name = obj.Name;
            Component = obj.Component;
            Value = obj.Value;
        }

        public string VirtualHost { get; }
        public string Component { get; }
        public string Name { get; }
        public IDictionary<string, object> Value { get; }
    }
}