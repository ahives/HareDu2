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
namespace HareDu.Core.Model
{
    using Newtonsoft.Json;

    public interface MessageStats
    {
        [JsonProperty("publish")]
        long TotalMessagesPublished { get; }

        [JsonProperty("publish_details")]
        MessagesPublishedDetails MessagesPublishedDetails { get; }
        
        [JsonProperty("confirm")]
        long TotalMessagesConfirmed { get; }

        [JsonProperty("confirm_details")]
        MessagesConfirmedDetails MessagesConfirmedDetails { get; }
        
        [JsonProperty("return_unroutable")]
        long TotalUnroutableMessagesReturned { get; }

        [JsonProperty("return_unroutable_details")]
        UnroutableMessagesReturnedDetails UnroutableMessagesReturnedDetails { get; }
        
        [JsonProperty("disk_reads")]
        long TotalDiskReads { get; }

        [JsonProperty("disk_reads_details")]
        DiskReadDetails DiskReadDetails { get; }
        
        [JsonProperty("disk_writes")]
        long TotalDiskWrites { get; }

        [JsonProperty("disk_writes_details")]
        DiskWriteDetails DiskWriteDetails { get; }
        
        [JsonProperty("get")]
        long TotalMessageGets { get; }

        [JsonProperty("get_details")]
        MessageGetDetails MessageGetDetails { get; }
        
        [JsonProperty("get_no_ack")]
        long TotalMessageGetsWithoutAck { get; }

        [JsonProperty("get_no_ack_details")]
        MessageGetsWithoutAckDetails MessageGetsWithoutAckDetails { get; }
        
        [JsonProperty("deliver")]
        long TotalMessagesDelivered { get; }

        [JsonProperty("deliver_details")]
        MessageDeliveryDetails MessageDeliveryDetails { get; }
        
        [JsonProperty("deliver_no_ack")]
        long TotalMessageDeliveredWithoutAck { get; }

        [JsonProperty("deliver_no_ack_details")]
        MessagesDeliveredWithoutAckDetails MessagesDeliveredWithoutAckDetails { get; }
        
        [JsonProperty("redeliver")]
        long TotalMessagesRedelivered { get; }

        [JsonProperty("redeliver_details")]
        MessagesRedeliveredDetails MessagesRedeliveredDetails { get; }
        
        [JsonProperty("ack")]
        long TotalMessagesAcknowledged { get; }

        [JsonProperty("ack_details")]
        MessagesAcknowledgedDetails MessagesAcknowledgedDetails { get; }
        
        [JsonProperty("deliver_get")]
        long TotalMessageDeliveryGets { get; }

        [JsonProperty("deliver_get_details")]
        MessageDeliveryGetDetails MessageDeliveryGetDetails { get; }
    }

    public interface MessagesPublishedDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }

    public interface MessagesConfirmedDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }

    public interface UnroutableMessagesReturnedDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }

    public interface DiskReadDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }

    public interface DiskWriteDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }

    public interface MessageGetDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }

    public interface MessageGetsWithoutAckDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }

    public interface MessageDeliveryDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }

    public interface MessagesDeliveredWithoutAckDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }

    public interface MessagesRedeliveredDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }

    public interface MessagesAcknowledgedDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }

    public interface MessageDeliveryGetDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}