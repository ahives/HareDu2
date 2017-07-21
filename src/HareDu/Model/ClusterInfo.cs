namespace HareDu.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface ClusterInfo
    {
        [JsonProperty("management_version")]
        string ManagementVersion { get; }

        [JsonProperty("rates_mode")]
        string RatesMode { get; }

        [JsonProperty("exchange_types")]
        IEnumerable<ExchangeType> ExchangeTypes { get; }

        [JsonProperty("rabbitmq_version")]
        string RabbitMqVersion { get; }

        [JsonProperty("cluster_name")]
        string ClusterName { get; }

        [JsonProperty("erlang_version")]
        string ErlangVerion { get; }

        [JsonProperty("erlang_full_version")]
        string ErlangFullVerion { get; }

        [JsonProperty("message_stats")]
        MessageStats MessageStats { get; }

        [JsonProperty("queue_totals")]
        QueueStats QueueStats { get; }

        [JsonProperty("object_totals")]
        ClusterObjectsSummary ClusterObjects { get; }

        [JsonProperty("statistics_db_event_queue")]
        long StatsDbEventQueue { get; }

        [JsonProperty("node")]
        string Node { get; }

        [JsonProperty("listeners")]
        IEnumerable<Listener> Listeners { get; }

        [JsonProperty("contexts")]
        IEnumerable<Context> Contexts { get; }
    }
}