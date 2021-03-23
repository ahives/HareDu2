namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using HareDu.Model;

    class InternalBindingInfo :
        BindingInfo
    {
        public InternalBindingInfo(BindingInfoImpl obj)
        {
            Source = obj.Source;
            VirtualHost = obj.VirtualHost;
            Destination = obj.Destination;
            DestinationType = obj.DestinationType;
            RoutingKey = obj.RoutingKey;
            Arguments = obj.Arguments;
            PropertiesKey = obj.PropertiesKey;
        }

        public string Source { get; }
        public string VirtualHost { get; }
        public string Destination { get; }
        public string DestinationType { get; }
        public string RoutingKey { get; }
        public IDictionary<string, object> Arguments { get; }
        public string PropertiesKey { get; }
    }
}