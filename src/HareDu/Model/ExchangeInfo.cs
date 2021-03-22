namespace HareDu.Model
{
    using System.Collections.Generic;

    public interface ExchangeInfo
    {
        string Name { get; }
        
        string VirtualHost { get; }
        
        ExchangeRoutingType RoutingType { get; }
        
        bool Durable { get; }
        
        bool AutoDelete { get; }
        
        bool Internal { get; }
        
        IDictionary<string, object> Arguments { get; }
    }
}