namespace HareDu.Snapshotting.Model
{
    using System.Collections.Generic;

    public interface VirtualHostSchemaSnapshot :
        Snapshot
    {
        string Node { get; }
        
        IReadOnlyList<VirtualHostSchema> VirtualHosts { get; }
        
        // IDictionary<string, IReadOnlyList<ExchangeSnapshot>> Schema { get; }
    }

    public interface VirtualHostSchema
    {
        string Name { get; }
        
        IReadOnlyList<ExchangeSnapshot> Definition { get; }
    }

    public interface ExchangeSnapshot
    {
        BindingDestination Source { get; }
        
        BindingDestination Destination { get; }
    }

    public interface BindingDestination
    {
        string Name { get; }
        
        BrokerObjectType SourceType { get; }
    }

    public enum BrokerObjectType
    {
        Exchange,
        Queue
    }
}