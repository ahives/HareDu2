namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface SystemOverviewInfo
    {
        [JsonPropertyName("management_version")]
        string ManagementVersion { get; }

        [JsonPropertyName("rates_mode")]
        string RatesMode { get; }
        
        [JsonPropertyName("sample_retention_policies")]
        SampleRetentionPolicies SampleRetentionPolicies { get; }

        [JsonPropertyName("exchange_types")]
        IList<ExchangeType> ExchangeTypes { get; }

        [JsonPropertyName("product_version")]
        string ProductVersion { get; }

        [JsonPropertyName("product_name")]
        string ProductName { get; }

        [JsonPropertyName("rabbitmq_version")]
        string RabbitMqVersion { get; }

        [JsonPropertyName("cluster_name")]
        string ClusterName { get; }

        [JsonPropertyName("erlang_version")]
        string ErlangVersion { get; }

        [JsonPropertyName("erlang_full_version")]
        string ErlangFullVersion { get; }

        [JsonPropertyName("disable_stats")]
        bool DisableStats { get; }

        [JsonPropertyName("enable_queue_totals")]
        bool EnableQueueTotals { get; }
        
        [JsonPropertyName("message_stats")]
        MessageStats MessageStats { get; }
        
        [JsonPropertyName("churn_rates")]
        ChurnRates ChurnRates { get; }

        [JsonPropertyName("queue_totals")]
        QueueStats QueueStats { get; }

        [JsonPropertyName("object_totals")]
        ClusterObjectTotals ObjectTotals { get; }

        [JsonPropertyName("statistics_db_event_queue")]
        ulong StatsDatabaseEventQueue { get; }

        [JsonPropertyName("node")]
        string Node { get; }

        [JsonPropertyName("listeners")]
        IList<Listener> Listeners { get; }

        [JsonPropertyName("contexts")]
        IList<NodeContext> Contexts { get; }
    }
}