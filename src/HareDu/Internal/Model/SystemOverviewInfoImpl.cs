namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    class SystemOverviewInfoImpl
    {
        [JsonPropertyName("management_version")]
        public string ManagementVersion { get; set; }

        [JsonPropertyName("rates_mode")]
        public string RatesMode { get; set; }
        
        [JsonPropertyName("sample_retention_policies")]
        public SampleRetentionPoliciesImpl SampleRetentionPolicies { get; }

        [JsonPropertyName("exchange_types")]
        public IList<ExchangeTypeImpl> ExchangeTypes { get; set; }

        [JsonPropertyName("product_version")]
        public string ProductVersion { get; set; }

        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }

        [JsonPropertyName("rabbitmq_version")]
        public string RabbitMqVersion { get; set; }

        [JsonPropertyName("cluster_name")]
        public string ClusterName { get; set; }

        [JsonPropertyName("erlang_version")]
        public string ErlangVersion { get; set; }

        [JsonPropertyName("erlang_full_version")]
        public string ErlangFullVersion { get; set; }

        [JsonPropertyName("disable_stats")]
        public bool DisableStats { get; set; }

        [JsonPropertyName("enable_queue_totals")]
        public bool EnableQueueTotals { get; set; }
        
        [JsonPropertyName("message_stats")]
        public MessageStatsImpl MessageStats { get; set; }
        
        [JsonPropertyName("churn_rates")]
        public ChurnRatesImpl ChurnRates { get; set; }

        [JsonPropertyName("queue_totals")]
        public QueueStatsImpl QueueStats { get; set; }

        [JsonPropertyName("object_totals")]
        public ClusterObjectTotalsImpl ObjectTotals { get; set; }

        [JsonPropertyName("statistics_db_event_queue")]
        public ulong StatsDatabaseEventQueue { get; set; }

        [JsonPropertyName("node")]
        public string Node { get; set; }

        [JsonPropertyName("listeners")]
        public IList<ListenerImpl> Listeners { get; set; }

        [JsonPropertyName("contexts")]
        public IList<NodeContextImpl> Contexts { get; set; }
    }
}