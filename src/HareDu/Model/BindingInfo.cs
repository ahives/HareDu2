namespace HareDu.Model
{
    using System.Collections.Generic;

    public interface BindingInfo
    {
        string Source { get; }
        
        string VirtualHost { get; }
        
        string Destination { get; }
        
        string DestinationType { get; }
        
        string RoutingKey { get; }
        
        IDictionary<string, object> Arguments { get; }
        
        string PropertiesKey { get; }
    }
}