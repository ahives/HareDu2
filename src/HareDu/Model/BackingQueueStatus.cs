// Copyright 2013-2019 Albert L. Hives
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

    public interface BackingQueueStatus
    {
        [JsonProperty("mode")]
        string Mode { get; }
        
        [JsonProperty("q1")]
        long Q1 { get; }
        
        [JsonProperty("q2")]
        long Q2 { get; }
        
        [JsonProperty("delta")]
        IList<object> Delta { get; }
        
        [JsonProperty("q3")]
        long Q3 { get; }
        
        [JsonProperty("q4")]
        long Q4 { get; }
        
        [JsonProperty("len")]
        long Length { get; }
        
        [JsonProperty("target_ram_count")]
        string TargetTotalMessagesInRAM { get; }
        
        [JsonProperty("next_seq_id")]
        long NextSequenceId { get; }
        
        [JsonProperty("avg_ingress_rate")]
        decimal AvgIngressRate { get; }
        
        [JsonProperty("avg_egress_rate")]
        decimal AvgEgressRate { get; }
        
        [JsonProperty("avg_ack_ingress_rate")]
        decimal AvgAcknowledgementIngressRate { get; }
        
        [JsonProperty("avg_ack_egress_rate")]
        decimal AvgAcknowledgementEgressRate { get; }
    }
}