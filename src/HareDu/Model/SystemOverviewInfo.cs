// Copyright 2013-2020 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface SystemOverviewInfo
    {
        [JsonProperty("management_version")]
        string ManagementVersion { get; }

        [JsonProperty("rates_mode")]
        string RatesMode { get; }

        [JsonProperty("exchange_types")]
        IList<ExchangeType> ExchangeTypes { get; }

        [JsonProperty("rabbitmq_version")]
        string RabbitMqVersion { get; }

        [JsonProperty("cluster_name")]
        string ClusterName { get; }

        [JsonProperty("erlang_version")]
        string ErlangVersion { get; }

        [JsonProperty("erlang_full_version")]
        string ErlangFullVersion { get; }

        [JsonProperty("message_stats")]
        MessageStats MessageStats { get; }
        
        [JsonProperty("churn_rates")]
        ChurnRates ChurnRates { get; }

        [JsonProperty("queue_totals")]
        QueueStats QueueStats { get; }

        [JsonProperty("object_totals")]
        ClusterObjectTotals ObjectTotals { get; }

        [JsonProperty("statistics_db_event_queue")]
        long StatsDatabaseEventQueue { get; }

        [JsonProperty("node")]
        string Node { get; }

        [JsonProperty("listeners")]
        IList<Listener> Listeners { get; }

        [JsonProperty("contexts")]
        IList<NodeContext> Contexts { get; }
    }
}