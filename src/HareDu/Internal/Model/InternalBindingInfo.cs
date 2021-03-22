namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using HareDu.Model;

    class InternalBindingInfo :
        BindingInfo
    {
        public InternalBindingInfo(BindingInfoImpl result)
        {
            Source = result.Source;
            VirtualHost = result.VirtualHost;
            Destination = result.Destination;
            DestinationType = result.DestinationType;
            RoutingKey = result.RoutingKey;
            Arguments = result.Arguments;
            PropertiesKey = result.PropertiesKey;
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