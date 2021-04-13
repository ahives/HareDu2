namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using HareDu.Model;

    class InternalExchangeInfo :
        ExchangeInfo
    {
        public InternalExchangeInfo(ExchangeInfoImpl obj)
        {
            Name = obj.Name;
            VirtualHost = obj.VirtualHost;
            RoutingType = obj.RoutingType;
            Durable = obj.Durable;
            AutoDelete = obj.AutoDelete;
            Internal = obj.Internal;
            Arguments = obj.Arguments;
        }

        public string Name { get; }
        public string VirtualHost { get; }
        public ExchangeRoutingType RoutingType { get; }
        public bool Durable { get; }
        public bool AutoDelete { get; }
        public bool Internal { get; }
        public IDictionary<string, object> Arguments { get; }
    }
}