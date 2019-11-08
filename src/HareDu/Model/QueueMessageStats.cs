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
    using Newtonsoft.Json;

    public interface QueueMessageStats
    {
        [JsonProperty("publish")]
        ulong TotalMessagesPublished { get; }

        [JsonProperty("publish_details")]
        Rate MessagesPublishedDetails { get; }
        
        [JsonProperty("get")]
        ulong TotalMessageGets { get; }

        [JsonProperty("get_details")]
        Rate MessageGetDetails { get; }
        
        [JsonProperty("get_no_ack")]
        ulong TotalMessageGetsWithoutAck { get; }

        [JsonProperty("get_no_ack_details")]
        Rate MessageGetsWithoutAckDetails { get; }
        
        [JsonProperty("deliver")]
        ulong TotalMessagesDelivered { get; }

        [JsonProperty("deliver_details")]
        Rate MessageDeliveryDetails { get; }
        
        [JsonProperty("deliver_no_ack")]
        ulong TotalMessageDeliveredWithoutAck { get; }

        [JsonProperty("deliver_no_ack_details")]
        Rate MessagesDeliveredWithoutAckDetails { get; }
        
        [JsonProperty("deliver_get")]
        ulong TotalMessageDeliveryGets { get; }

        [JsonProperty("deliver_get_details")]
        Rate MessageDeliveryGetDetails { get; }
        
        [JsonProperty("redeliver")]
        ulong TotalMessagesRedelivered { get; }

        [JsonProperty("redeliver_details")]
        Rate MessagesRedeliveredDetails { get; }
        
        [JsonProperty("ack")]
        ulong TotalMessagesAcknowledged { get; }

        [JsonProperty("ack_details")]
        Rate MessagesAcknowledgedDetails { get; }
    }
}