namespace HareDu.Model
{
    using System.Collections.Generic;

    public interface SystemOverviewInfo
    {
        string ManagementVersion { get; }

        string RatesMode { get; }
        
        SampleRetentionPolicies SampleRetentionPolicies { get; }

        IList<ExchangeType> ExchangeTypes { get; }

        string ProductVersion { get; }

        string ProductName { get; }

        string RabbitMqVersion { get; }

        string ClusterName { get; }

        string ErlangVersion { get; }

        string ErlangFullVersion { get; }

        bool DisableStats { get; }

        bool EnableQueueTotals { get; }
        
        MessageStats MessageStats { get; }
        
        ChurnRates ChurnRates { get; }

        QueueStats QueueStats { get; }

        ClusterObjectTotals ObjectTotals { get; }

        ulong StatsDatabaseEventQueue { get; }

        string Node { get; }

        IList<Listener> Listeners { get; }

        IList<NodeContext> Contexts { get; }
    }
}